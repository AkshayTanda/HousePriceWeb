
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.ML.Data;
using System.ComponentModel.DataAnnotations;

namespace HousePriceWeb.Models
{
    public class HouseBatchResult
    {
        public float Rooms { get; set; }
        public float Area  { get; set; }
        public float Age   { get; set; }

        [ColumnName("Score")]
        public float PredictedPrice { get; set; } 
    }

    public class HomeViewModel
    {
        // existing single‐input props…
        public double R2 { get; set; }
        public double RMSE { get; set; }

        [Required, Range(1, 100, ErrorMessage = "Rooms must be 1–100")]
        public float Rooms { get; set; }

        [Required, Range(100, 10000, ErrorMessage = "Area must be 100–10000 sqft")]
        public float Area  { get; set; }

        [Required, Range(0, 200, ErrorMessage = "Age must be 0–200 years")]
        public float Age   { get; set; }

        public float? PredictedPrice { get; set; }

        // new for batch
        public IFormFile? BatchFile { get; set; }
        public List<HouseBatchResult> BatchResults { get; set; } = new List<HouseBatchResult>();
    }
}
