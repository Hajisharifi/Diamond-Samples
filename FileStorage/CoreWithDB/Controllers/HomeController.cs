using System;
using System.Linq;
using Diamond.FileStorage;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace H2.Core_FileStorage.Controllers
{
    public class HomeController
        : Controller
    {
        //________________________________________________________________________

        private readonly Models.Context m_DbContext;
        private readonly IStorageManager<long> m_StorageManager;
        //________________________________________________________________________

        public HomeController(IStorageManager<long> storageManager, Models.Context dbContext)
        {
            this.m_DbContext = dbContext;
            this.m_StorageManager = storageManager;
        }
        //________________________________________________________________________

        public async Task<ActionResult> Index()
        {
            //const long SAMPLE_FILE_ID = 123;

            //using (var w = await this.m_StorageManager.CreateTextAsync(SAMPLE_FILE_ID))
            //{
            //    w.WriteLine("Line1");
            //    w.WriteLine("Line2");
            //    w.WriteLine("Line3");
            //}

            //using (var w = await this.m_StorageManager.AppendTextAsync(SAMPLE_FILE_ID))
            //{
            //    w.WriteLine("Line4");
            //    w.WriteLine("Line5");
            //}

            //using (var w = await this.m_StorageManager.CreateTextAsync(SAMPLE_FILE_ID, "myMetaData"))
            //{
            //    w.WriteLine("Metadata attached to the main file");
            //}

            var list = await this.m_DbContext.Files.ToListAsync();
            return this.View(list);
        }
        //________________________________________________________________________

        public async Task<ActionResult> Download(long id)
        {
            var file = await this.m_StorageManager.GetFileOptionAsync(this.Request, id, null);
            if (file is null) return this.NotFound();

            //file.TransferSpeed = 50 * 1024; //50KB per second
            file.TransferResumable = true; //resumable download support
            //file...

            return new FileStorageResult(this.m_StorageManager, file);
        }
        //________________________________________________________________________

        public ActionResult Upload()
        {
            return this.View();
        }
        //________________________________________________________________________

        [HttpPost]
        public async Task<ActionResult> Upload(IFormFile postFile)
        {
            var file = this.m_StorageManager.CreateUploadFileOption(postFile);
            using var stream = postFile?.OpenReadStream();
            await this.m_StorageManager.UploadAsync(file, stream);

            return this.RedirectToAction(nameof(Index));
        }
        //________________________________________________________________________
    }
}
