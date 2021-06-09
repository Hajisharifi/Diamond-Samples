using System;
using Diamond.FileStorage;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace H2.Core_FileStorage.Models
{
    public class DatabaseProvider
        : IFileInterceptor
    {
        //________________________________________________________________________

        private readonly IServiceScopeFactory m_ServiceScopeFactory;
        //________________________________________________________________________

        public DatabaseProvider(IServiceScopeFactory scopeFactory)
        {
            this.m_ServiceScopeFactory = scopeFactory;
        }
        //________________________________________________________________________

        public ValueTask<FileOption> GetFileOptionAsync(long ID, string alternateDataStream)
        {
            var ret = new FileOption(ID, alternateDataStream);
            if (!string.IsNullOrEmpty(alternateDataStream)) return new ValueTask<FileOption>(ret);

            using var scope = this.m_ServiceScopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetService<Models.Context>();
            var row = db.Files.Find(ID);
            if (row is null) return default;
            ret.FileName = row.FileName;
            ret.FileSize = row.FileSize;
            ret.ContentType = row.ContentType;
            ret.DataTokens[FileOption.TOKEN_CREATIONTIME] = row.RegisterDate;

            return new ValueTask<FileOption>(ret);
        }
        //________________________________________________________________________

        public async ValueTask BeginUploadAsync(FileOption file)
        {
            if (!string.IsNullOrEmpty(file.AlternateDataStream)) return;
            //if (file.ID > 0) DB-UPDATE for change or reupload a file
            var row = new Models.File()
            {
                FileName = file.FileName,
                FileSize = file.FileSize,
                ContentType = file.ContentType,
                RegisterDate = DateTime.Now
            };

            using var scope = this.m_ServiceScopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetService<Models.Context>();
            db.Files.Add(row);
            await db.SaveChangesAsync();

            file.ID = row.FileId;
        }
        //________________________________________________________________________

        public async ValueTask EndDeleteAsync(FileOption file)
        {
            if (!string.IsNullOrEmpty(file.AlternateDataStream)) return;

            using var scope = this.m_ServiceScopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetService<Models.Context>();
            var row = db.Files.Find(file.ID);
            if (row is null) return;
            db.Files.Remove(row);
            await db.SaveChangesAsync();
        }
        //________________________________________________________________________

        public ValueTask EndUploadAsync(FileOption file) => default; //Always
        public ValueTask BeginDeleteAsync(FileOption file) => default;
        public ValueTask BeginDownloadAsync(FileOption file) => default;
        public ValueTask EndDownloadAsync(FileOption file) => default;
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