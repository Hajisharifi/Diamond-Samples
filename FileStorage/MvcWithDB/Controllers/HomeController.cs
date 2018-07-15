using System;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Diamond.FileStorage;

namespace H2.Mvc_FileStorage.Controllers
{
    public class HomeController
        : Controller
    {
        //________________________________________________________________________

        public ActionResult Index()
        {
            IList<Models.File> list;
            using (var da = new Models.Context())
                list = da.Files.ToList();

            return this.View(list);
        }
        //________________________________________________________________________

        public ActionResult Download(long id)
        {
            var file = FileStorage.Current.GetFileOption(id, null);
            if (file == null) return new HttpNotFoundResult();

            file.Merge(this.Request.CreateDownloadFileOption(id));
            file.TransferSpeed = 1024; //50KB per second
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
        public ActionResult Upload(System.Web.HttpPostedFileBase postfile)
        {
            var file = postfile.CreateUploadFileOption();
            FileStorage.Current.Upload(file, postfile.InputStream);
            return this.RedirectToAction(nameof(Index));
        }
        //________________________________________________________________________
    }
}