using System;
using Diamond.FileStorage;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace H2.Core_FileStorage.Models
{
    public class DatabaseProvider
        : IFileInterceptor<long>
    {
        //________________________________________________________________________

        private readonly IServiceScopeFactory m_ServiceScopeFactory;
        //________________________________________________________________________

        public DatabaseProvider(IServiceScopeFactory scopeFactory)
        {
            this.m_ServiceScopeFactory = scopeFactory;
        }
        //________________________________________________________________________

        public ValueTask<FileOption<long>> GetFileOptionAsync(long ID, string alternateDataStream)
        {
            var ret = FileOption.Create(ID, alternateDataStream);
            if (!string.IsNullOrEmpty(alternateDataStream)) return new ValueTask<FileOption<long>>(ret);

            using var scope = this.m_ServiceScopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetService<Models.Context>();
            var row = db.Files.Find(ID);
            if (row is null) return default;
            ret.FileName = row.FileName;
            ret.FileSize = row.FileSize;
            ret.ContentType = row.ContentType;
            ret.DataTokens[FileOption.TOKEN_CREATIONTIME] = row.RegisterDate;

            return new ValueTask<FileOption<long>>(ret);
        }
        async ValueTask<FileOption> IFileOptionsProvider.GetFileOptionAsync(object ID, string AlternateDataStream)
        {
            return await this.GetFileOptionAsync(ID is null ? 0L : (long)ID, AlternateDataStream);
        }
        //________________________________________________________________________

        public async ValueTask BeginUploadAsync(FileOption<long> file)
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

        public async ValueTask EndDeleteAsync(FileOption<long> file)
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

        public ValueTask EndUploadAsync(FileOption<long> file) => default; //Always
        public ValueTask BeginDeleteAsync(FileOption<long> file) => default;
        public ValueTask BeginDownloadAsync(FileOption<long> file) => default;
        public ValueTask EndDownloadAsync(FileOption<long> file) => default;
    }
    //________________________________________________________________________
}