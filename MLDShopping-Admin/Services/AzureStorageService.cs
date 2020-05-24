using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace MLDShopping_Admin.Services
{
    public interface IAzureBlobService
    {
        Task<IEnumerable<Uri>> ListAsync(string containerName);
        Task<Uri> GetUriByNameAsync(string name, string containerName);
        Task UploadMultipleAsync(IFormFileCollection files, string containerName);
        Task<string> UploadSingleAsync(IFormFile file, string containerName);
        Task DeleteAsync(string fileUri, string name);
        Task DeleteAllAsync();
    }
    public class AzureBlobService : IAzureBlobService
    {
        private readonly IAzureBlobConnectionFactory _azureBlobConnectionFactory;
        public AzureBlobService(IAzureBlobConnectionFactory azureBlobConnectionFactory)
        {
            _azureBlobConnectionFactory = azureBlobConnectionFactory;
        }

        public Task DeleteAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(string fileUri, string name)
        {
            var blobContainer = await _azureBlobConnectionFactory.GetBlobContainer(name);
            var uri = new Uri(fileUri);
            string filename = Path.GetFileName(uri.LocalPath);
            var blob = blobContainer.GetBlockBlobReference(filename);
            await blob.DeleteIfExistsAsync();
        }

        public async Task<Uri> GetUriByNameAsync(string name, string containerName)
        {
            var blobContainer = await _azureBlobConnectionFactory.GetBlobContainer(containerName);
            var blob = blobContainer.GetBlobReference(name);

            return blob.Uri;
        }

        public async Task<IEnumerable<Uri>> ListAsync(string name)
        {
            // get a list of blobs
            var blobContainer = await _azureBlobConnectionFactory.GetBlobContainer(name);
            var blobList = new List<Uri>();
            // need to create a null continuation token
            BlobContinuationToken blobContinuationToken = null;
            do
            {
                var response = await blobContainer.ListBlobsSegmentedAsync(blobContinuationToken);
                foreach (var blob in response.Results)
                {
                    if (blob.GetType() == typeof(CloudBlockBlob))
                    {
                        blobList.Add(blob.Uri);
                    }
                }
                blobContinuationToken = response.ContinuationToken;
            } while (blobContinuationToken != null);
            return blobList;
        }

        public async Task UploadMultipleAsync(IFormFileCollection files, string containerName)
        {

            var blobContainer = await _azureBlobConnectionFactory.GetBlobContainer(containerName);

            for (int i = 0; i < files.Count; i++)
            {
                var blob = blobContainer.GetBlockBlobReference(GetRandomBlobName(files[i].FileName));
                using (var stream = files[i].OpenReadStream())
                {
                    await blob.UploadFromStreamAsync(stream);
                }
            }
        }
        public async Task<string> UploadSingleAsync(IFormFile file, string containerName)
        {
            var blobContainer = await _azureBlobConnectionFactory.GetBlobContainer(containerName);

            var blob = blobContainer.GetBlockBlobReference(GetRandomBlobName(file.FileName));
            using (var stream = file.OpenReadStream())
            {
                await blob.UploadFromStreamAsync(stream);

                return await Task.FromResult(blob.Name);
            }
        }
        private string GetRandomBlobName(string filename)
        {
            string ext = Path.GetExtension(filename);
            return string.Format("{0:10}_{1}{2}", DateTime.Now.Ticks, Guid.NewGuid(), ext);
        }

    }
}
