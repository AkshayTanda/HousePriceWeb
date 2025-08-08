using Microsoft.ML.Data;

namespace HousePriceWeb.Models
{
    public class HousePricePrediction
    {
        [ColumnName("Score")] public float Price { get; set; }
    }
}
