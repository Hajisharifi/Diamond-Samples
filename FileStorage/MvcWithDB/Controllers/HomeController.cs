using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using Diamond.FileStorage;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace H2.Mvc_FileStorage.Controllers
{
    public class HomeController
        : Controller
    {
        //________________________________________________________________________

        public async Task<ActionResult> Index()
        {
            //const long SAMPLE_FILE_ID = 123;

            //using (var w = await FileStorage.Current.CreateTextAsync(SAMPLE_FILE_ID))
            //{
            //    w.WriteLine("Line1");
            //    w.WriteLine("Line2");
            //    w.WriteLine("Line3");
            //}

            //using (var w = await FileStorage.Current.AppendTextAsync(SAMPLE_FILE_ID))
            //{
            //    w.WriteLine("Line4");
            //    w.WriteLine("Line5");
            //}

            //using (var w = await FileStorage.Current.CreateTextAsync(SAMPLE_FILE_ID, "myMetaData"))
            //{
            //    w.WriteLine("Metadata attached to the main file");
            //}

            IList<Models.File> list;
            using (var da = new Models.Context())
                list = await da.Files.ToListAsync();

            return this.View(list);
        }
        //________________________________________________________________________

        public async Task<ActionResult> Download(long id)
        {
            var file = await FileStorage.Current.GetFileOptionAsync(id, null);
            if (file == null) return new HttpNotFoundResult();

            file.Merge(this.Request.CreateDownloadFileOption(id));
            //file.TransferSpeed = 50 * 1024; //50KB per second
            file.TransferResumable = true; //resumable download support
            //file...

            return new FileStorageResult(FileStorage.Current, file);
        }
        //________________________________________________________________________

        public ActionResult Upload()
        {
            return this.View();
        }
        //________________________________________________________________________

        [HttpPost]
        public async Task<ActionResult> Upload(System.Web.HttpPostedFileBase postfile)
        {
            var file = postfile.CreateUploadFileOption();
            await FileStorage.Current.UploadAsync(file, postfile.InputStream);
            return this.RedirectToAction(nameof(Index));
        }
        //________________________________________________________________________
    }
}