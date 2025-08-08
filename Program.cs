using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.Extensions.ML;
using HousePriceWeb.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. Create MLContext
var mlContext = new MLContext(seed: 0);

// 2. Load raw data to compute metrics
var dataPath = "Models/housing.csv";
var data = mlContext.Data.LoadFromTextFile<HouseData>(
    path: dataPath,
    hasHeader: true,
    separatorChar: ',');

// 3. Recreate the training pipeline (same as you used for training)
var pipeline = mlContext.Transforms
    .Concatenate("Features",
        nameof(HouseData.Rooms),
        nameof(HouseData.Area),
        nameof(HouseData.Age))
    .Append(mlContext.Transforms.NormalizeMinMax("Features"))
    .Append(mlContext.Regression.Trainers.FastTree(
        new Microsoft.ML.Trainers.FastTree.FastTreeRegressionTrainer.Options
        {
            LabelColumnName = "Price",
            FeatureColumnName = "Features",
            NumberOfLeaves = 20,
            NumberOfTrees = 100,
            LearningRate = 0.05f,
            MinimumExampleCountPerLeaf = 2
        }));

// 4. Compute 5-fold cross-validation metrics at startup
var cvResults = mlContext.Regression.CrossValidate(
    data: data,
    estimator: pipeline,
    numberOfFolds: 5,
    labelColumnName: "Price");
var avgR2   = cvResults.Average(r => r.Metrics.RSquared);
var avgRmse = cvResults.Average(r => r.Metrics.RootMeanSquaredError);

// 5. Load your tuned, persisted model
string modelFilePath = "Models/HousePriceModel_Best.zip";
ITransformer trainedModel = mlContext.Model.Load(modelFilePath, out DataViewSchema modelSchema);

// 6. Register services for DI
builder.Services.AddSingleton(mlContext);
builder.Services.AddSingleton<ITransformer>(trainedModel);
builder.Services.AddSingleton(new ModelMetrics
{
    R2   = avgR2,
    RMSE = avgRmse
});
builder.Services.AddPredictionEnginePool<HouseData, HousePricePrediction>()
    .FromFile(
        modelName: "HousePriceModel_Best",
        filePath: modelFilePath,
        watchForChanges: true);

// 7. Add MVC services
builder.Services.AddControllersWithViews();

var app = builder.Build();

// 8. Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseStaticFiles();
app.UseRouting();
app.MapDefaultControllerRoute();

// 9. Run the web app
app.Run();
