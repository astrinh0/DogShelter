namespace Crosscutting.Services
{
    internal class CalculationService : ICalculationService
    {
        public decimal GetAverageHeight(string height)
        {
            if (string.IsNullOrWhiteSpace(height)) return 0;

            var allHeight = height.Split(' ');

            return (Decimal.Parse(allHeight[2]) + Decimal.Parse(allHeight[0])) / 2;
        }
    }
}