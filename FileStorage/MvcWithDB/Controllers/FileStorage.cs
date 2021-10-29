using System;
using Diamond.FileStorage;
using System.Threading.Tasks;

namespace H2.Mvc_FileStorage.Controllers
{
    public static class FileStorage
    {
        //________________________________________________________________________

        private static IStorageManager<long> m_Current;
        private static bool m_Initialized;
        private static object m_SyncLock = new object();
        //________________________________________________________________________

        /// <summary>
        /// singleton object
        /// </summary>
        public static IStorageManager<long> Current
        {
            get
            {
                if (m_Current == null)
                {
                    return System.Threading.LazyInitializer.EnsureInitialized(
                        ref m_Current,
                        ref m_Initialized,
                        ref m_SyncLock, () =>
                        {
                            var options = new StorageManagerOptions<long>(); ;
                            options.SetVirtualPath(@"~\App_Data\FileStorage");
                            options.FileInterceptor = new DatabaseProvider(); //Optional
                            //options.FilePathFactory = new Diamond.FileStorage.Infrastructure.V2Int64DecimalFilePathFactory(); //Support old FileStorage version
                            return new StorageManager<long>(options);
                        });
                }
                return m_Current;
            }
        }
    }
    //________________________________________________________________________

    public class DatabaseProvider
        : Diamond.FileStorage.IFileInterceptor<long>
    {
        public ValueTask<FileOption<long>> GetFileOptionAsync(long ID, string alternateDataStream)
        {
            var ret = FileOption<long>.Create(ID, alternateDataStream);
            if (!string.IsNullOrEmpty(alternateDataStream)) return new ValueTask<FileOption<long>>(ret);
            using (var da = new Models.Context())
            {
                var row = da.Files.Find(ID);
                if (row == null) return default;
                ret.FileName = row.FileName;
                ret.FileSize = row.FileSize;
                ret.ContentType = row.ContentType;
                ret.DataTokens[FileOption.TOKEN_CREATIONTIME] = row.RegisterDate;
            }
            return new ValueTask<FileOption<long>>(ret);
        }
        async ValueTask<FileOption> IFileOptionsProvider.GetFileOptionAsync(object ID, string AlternateDataStream)
        {
            return await this.GetFileOptionAsync(ID is null ? 0L : (long)ID, AlternateDataStream);
        }

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
            using (var da = new Models.Context())
            {
                da.Files.Add(row);
                await da.SaveChangesAsync();
            }
            file.ID = row.FileId;
        }

        public async ValueTask EndDeleteAsync(FileOption<long> file)
        {
            if (!string.IsNullOrEmpty(file.AlternateDataStream)) return;
            using (var da = new Models.Context())
            {
                var row = da.Files.Find(file.ID);
                if (row == null) return;
                da.Files.Remove(row);
                await da.SaveChangesAsync();
            }
        }

        public ValueTask EndUploadAsync(FileOption<long> file) { return default; } //Always
        public ValueTask BeginDeleteAsync(FileOption<long> file) { return default; }
        public ValueTask BeginDownloadAsync(FileOption<long> file) { return default; }
        public ValueTask EndDownloadAsync(FileOption<long> file) { return default; }
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