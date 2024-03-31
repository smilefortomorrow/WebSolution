using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Data;
using System.Threading.Tasks;
using WorkFlowSystem.Entities;
using WorkFlowSystem.ViewModels;


namespace WorkFlowSystem.BLL
{
    public class UploadFileService
    {
        private string connectionString = "DefaultEndpointsProtocol=https;AccountName=WorkFlowwebapp;AccountKey=nv/Jgs3+WKOnjE5ORTthUw9umu9QchLj7T03JIr7Lq+h8OzFyM/AalE/Wov0+l8nfmhoMf7ph0ME+AStRwWWvg==;EndpointSuffix=core.windows.net";
        private string containerName = "WorkFlowstorage";

        public async Task<string> UploadFileAsync(IBrowserFile file)
        {
            try
            {
                var container = new BlobContainerClient(connectionString, containerName);
                await container.CreateIfNotExistsAsync();

                var blob = container.GetBlobClient(file.Name);
                await using var stream = file.OpenReadStream(maxAllowedSize: 2147483648); // 2 GB limit
                await blob.UploadAsync(stream, overwrite: true);

                return blob.Uri.ToString(); // Return the URI of the uploaded blob
            }
            catch (Exception ex)
            {
                throw new Exception($"Error uploading file: {ex.Message}", ex);
            }
        }
    }
}
