using System;

namespace AspCoreAddons.Interfaces
{
    public interface IAreaAddon
        : Diamond.Addons.IAddon
    {
        string Area { get; }
    }
}
