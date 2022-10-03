using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AspCoreAddons.Interfaces
{
    public interface IExportAddon<T>
        : Diamond.Addons.IAddon
    {
        string Title { get; }
        Task<IActionResult> Download(T data);
    }
}
