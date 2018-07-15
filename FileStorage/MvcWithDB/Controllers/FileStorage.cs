using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Diamond.FileStorage;

namespace H2.Mvc_FileStorage.Controllers
{
    public class FileStorage
        : Diamond.FileStorage.StorageManager
    {
        //________________________________________________________________________

        private FileStorage()
        {
            //((Diamond.FileStorage.LocalFileSystemProvider)this.FileSystem).UseDecimalPathBuilder = false; //compatibility with old version 04.2013.10.0
            ((Diamond.FileStorage.LocalFileSystemProvider)this.FileSystem).SetVirtualPath(@"~\App_Data\FileStorage");
            this.Interceptor = new DatabaseProvider(); //Optional
        }
        //________________________________________________________________________

        /// <summary>
        /// singleton object
        /// </summary>
        private static FileStorage m_Current;
        public static FileStorage Current
        {
            get
            {
                if (m_Current == null)
                    System.Threading.Interlocked.CompareExchange(ref m_Current, new FileStorage(), null);
                return m_Current;
            }
        }
    }
    //________________________________________________________________________

    public class DatabaseProvider
        : Diamond.FileStorage.IFileInterceptor
    {
        public FileOption GetFileOption(long ID, string alternateDataStream)
        {
            var ret = new FileOption(ID, alternateDataStream);
            if (!string.IsNullOrEmpty(alternateDataStream)) return ret;
            using (var da = new Models.Context())
            {
                var row = da.Files.Find(ID);
                if (row == null) return null;
                ret.FileName = row.FileName;
                ret.FileSize = row.FileSize;
                ret.ContentType = row.ContentType;
                ret.DataTokens[FileOption.TOKEN_CREATIONTIME] = row.RegisterDate;
            }
            return ret;
        }

        public void BeginUpload(FileOption file)
        {
            if (!string.IsNullOrEmpty(file.AlternateDataStream)) return;
            var row = new Models.File()
            {
                FileName = file.FileName,
                FileSize = file.FileSize,
                ContentType = file.ContentType,
                RegisterDate = DateTime.Now
            };
            using (var da = new Models.Context())
            {
                da.Files.Add(row);
                da.SaveChanges();
            }
            file.ID = row.FileId;
        }

        public void EndDelete(FileOption file)
        {
            if (!string.IsNullOrEmpty(file.AlternateDataStream)) return;
            using (var da = new Models.Context())
            {
                var row = da.Files.Find(file.ID);
                if (row == null) return;
                da.Files.Remove(row);
                da.SaveChanges();
            }
        }

        public void EndUpload(FileOption file) { }
        public void BeginDelete(FileOption file) { }
        public void BeginDownload(FileOption file) { }
        public void EndDownload(FileOption file) { }
    }
    //________________________________________________________________________

    /*public class MyUploadHttpModule
        : UploadHttpModule
    {
        public MyUploadHttpModule()
        {
            this.AllowHttpMethods = new[] { "POST" };
            this.StorageManager = FileStorage.Current;
        }
        //override OnBeginUpload...
        //override OnEndUpload...
    }*/
}