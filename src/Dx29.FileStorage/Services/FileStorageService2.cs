using System;
using System.IO;
using System.Threading.Tasks;

namespace Dx29.Services
{
    public class FileStorageService2
    {
        public FileStorageService2(BlobStorage blobStorage)
        {
            BlobStorage = blobStorage;
        }

        public BlobStorage BlobStorage { get; }

        public async Task UploadFileAsync(string userId, string caseId, string filePath, Stream stream)
        {
            string container = userId;
            string path = $"{caseId}/{filePath}";
            await BlobStorage.UploadStreamAsync(container, path, stream);
        }

        public async Task<Stream> DownloadFileAsync(string userId, string caseId, string filePath)
        {
            string container = userId;
            string path = $"{caseId}/{filePath}";
            return await BlobStorage.DownloadStreamAsync(container, path);
        }

        public async Task DeleteFileAsync(string userId, string caseId, string filePath)
        {
            string container = userId;
            string path = $"{caseId}/{filePath}";
            await BlobStorage.DeleteBlobAsync(container, path);
        }

        public async Task MoveFileAsync(string userId, string sourcePath, string targetPath)
        {
            string container = userId;
            await BlobStorage.MoveBlobAsync(container, sourcePath, targetPath);
        }
        public async Task MoveFileAsync(string sourceUserId, string targetUserId, string sourcePath, string targetPath)
        {
            string sourceContainer = sourceUserId;
            string targetContainer = targetUserId;
            await BlobStorage.MoveBlobAsync(sourceContainer, targetContainer, sourcePath, targetPath);
        }

        public async Task CopyFileAsync(string userId, string sourcePath, string targetPath)
        {
            string container = userId;
            await BlobStorage.CopyBlobAsync(container, sourcePath, targetPath);
        }
        public async Task CopyFileAsync(string sourceUserId, string targetUserId, string sourcePath, string targetPath)
        {
            string sourceContainer = sourceUserId;
            string targetContainer = targetUserId;
            await BlobStorage.CopyBlobAsync(sourceContainer, targetContainer, sourcePath, targetPath);
        }

        public string CreateFileShare(string userId, string caseId, string filePath)
        {
            string container = userId;
            string path = $"{caseId}/{filePath}";
            return BlobStorage.CreateBlobShare(container, path, seconds: 5 * 60);
        }
    }
}
