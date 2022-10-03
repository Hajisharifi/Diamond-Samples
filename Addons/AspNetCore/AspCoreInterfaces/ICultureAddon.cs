using System;

namespace AspCoreAddons.Interfaces
{
    public interface ICultureAddon
        : Diamond.Addons.IAddon
    {
        System.Globalization.Calendar Calendar { get; }
        System.Globalization.CultureInfo Culture { get; }
        DateTime? Parse(string text);
        string ToString(DateTime? value, string format);
    }
}
