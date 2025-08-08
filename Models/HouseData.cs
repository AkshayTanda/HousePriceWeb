using Microsoft.ML.Data;

namespace HousePriceWeb.Models
{
    public class HouseData
    {
        [LoadColumn(0)] public float Rooms { get; set; }
        [LoadColumn(1)] public float Area  { get; set; }
        [LoadColumn(2)] public float Age   { get; set; }
        [LoadColumn(3)] public float Price { get; set; }
    }
}
