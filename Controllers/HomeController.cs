using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.ML;
using HousePriceWeb.Models;
using Microsoft.ML;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HousePriceWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ModelMetrics _metrics;
        private readonly PredictionEnginePool<HouseData, HousePricePrediction> _predEngine;
        private readonly MLContext _mlContext;
        private readonly ITransformer _model;

        public HomeController(
            ModelMetrics metrics,
            PredictionEnginePool<HouseData, HousePricePrediction> predEngine,
            MLContext mlContext,
            ITransformer model)
        {
            _metrics = metrics;
            _predEngine = predEngine;
            _mlContext = mlContext;
            _model = model;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new HomeViewModel { R2 = _metrics.R2, RMSE = _metrics.RMSE });
        }

        [HttpPost]
        public async Task<IActionResult> Index(HomeViewModel vm)
        {
            // Always show metrics
            vm.R2 = _metrics.R2;
            vm.RMSE = _metrics.RMSE;
            vm.PredictedPrice = null;

            // If a CSV was uploaded, do batch scoring
            if (vm.BatchFile != null && vm.BatchFile.Length > 0)
            {
                var inputs = new List<HouseData>();
                using var reader = new StreamReader(vm.BatchFile.OpenReadStream());
                await reader.ReadLineAsync(); // skip header
                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();
                    if (line != null)
                    {
                        var parts = line.Split(',', System.StringSplitOptions.TrimEntries);
                        if (parts.Length >= 3 &&
                            float.TryParse(parts[0], out var r) &&
                            float.TryParse(parts[1], out var a) &&
                            float.TryParse(parts[2], out var age))
                        {
                            inputs.Add(new HouseData { Rooms = r, Area = a, Age = age });
                        }
                    }
                }
                // Run the pipeline in batch
                var batchDv = _mlContext.Data.LoadFromEnumerable(inputs);
                var scored = _model.Transform(batchDv);
                var results = _mlContext.Data.CreateEnumerable<HouseBatchResult>(scored, reuseRowObject: false)
                                .ToList();
                vm.BatchResults = results;
            }
            else if (ModelState.IsValid) // no file → single prediction
            {
                var input = new HouseData { Rooms = vm.Rooms, Area = vm.Area, Age = vm.Age };
                var pred = _predEngine.Predict("HousePriceModel_Best", input);
                vm.PredictedPrice = pred.Price;
            }

            return View(vm);
        }
        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }

    }
}
