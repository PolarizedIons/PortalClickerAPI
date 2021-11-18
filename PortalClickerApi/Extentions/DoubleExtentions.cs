using System;

namespace PortalClickerApi.Extentions
{
    public static class DoubleExtentions
    {
        public static double FloorTo(this double value, uint amount)
        {
            var decimals = Math.Pow(10, amount);
            return Math.Floor(value * decimals) / decimals;
        }
    }
}
