using System;

namespace PortalClickerApi.Models
{
    [Flags]
    public enum UpgradeMultiplierType
    {
        AddClick = 1 << 0,
        Click = 1 << 1,
        ItemPrice = 1 << 2,
        ItemPortals = 1 << 3,
    }
}
