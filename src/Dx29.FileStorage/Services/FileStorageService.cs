using System;
using System.IO;
using System.Threading.Tasks;

namespace Dx29.Services
{
    public class FileStorageService
    {
        public FileStorageService(BlobStorage blobStorage)
        {
            BlobStorage = blobStorage;
        }

        public BlobStorage BlobStorage { get; }

        public async Task UploadFileAsync(string userId, string caseId, string resourceId, string name, Stream stream)
        {
            string container = userId;
            string path = $"{caseId}/{resourceId}/{name}";
            await BlobStorage.UploadStreamAsync(container, path, stream);
        }

        public async Task<Stream> DownloadFileAsync(string userId, string caseId, string resourceId, string name)
        {
            string container = userId;
            string path = $"{caseId}/{resourceId}/{name}";
            return await BlobStorage.DownloadStreamAsync(container, path);
        }

        public async Task DeleteFileAsync(string userId, string caseId, string resourceId, string name)
        {
            string container = userId;
            string path = $"{caseId}/{resourceId}/{name}";
            await BlobStorage.DeleteBlobAsync(container, path);
        }

        public async Task CopyFileAsync(string userId, string source, string target, string resourceId, string name)
        {
            string container = userId;
            string sourcePath = $"{source}/{resourceId}/{name}";
            string targetPath = $"{target}/{resourceId}/{name}";
            await BlobStorage.CopyBlobAsync(container, sourcePath, targetPath);
        }

        public async Task MoveFileAsync(string userId, string source, string target, string resourceId, string name)
        {
            string container = userId;
            string sourcePath = $"{source}/{resourceId}/{name}";
            string targetPath = $"{target}/{resourceId}/{name}";
            await BlobStorage.MoveBlobAsync(container, sourcePath, targetPath);
        }

        public string CreateFileShare(string userId, string caseId, string resourceId, string name)
        {
            string container = userId;
            string path = $"{caseId}/{resourceId}/{name}";
            return BlobStorage.CreateBlobShare(container, path, seconds: 5 * 60);
        }
    }
}
