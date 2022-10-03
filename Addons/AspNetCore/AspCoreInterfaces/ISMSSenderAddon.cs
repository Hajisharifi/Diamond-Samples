using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AspCoreAddons.Interfaces
{
    public interface ISMSSenderAddon
        : Diamond.Addons.IAddon
    {
        bool IsEnabled { get; }
        Task<bool[]> Send(string message, ICollection<string> toPhones);
    }
}
