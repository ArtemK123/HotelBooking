using System;
using System.IO;
using System.Threading.Tasks;
using HotelBooking.Contracts.Domain.Repositories;
using HotelBooking.Contracts.Dto.Data;
using HotelBooking.Contracts.Dto.Models.Account;
using HotelBooking.Contracts.Dto.Models.Images;
using HotelBooking.Contracts.Services;
using HotelBooking.Services.Interfaces;

namespace HotelBooking.Services
{
    internal class ImageService : IImageService
    {
        private readonly ISessionHandler sessionHandler;
        private readonly IClientRepository clientRepository;

        private readonly string accountImageStorage = "Accounts";
        private readonly string webFilesFolder = "wwwroot";

        public ImageService(ISessionHandler sessionHandler, IClientRepository clientRepository)
        {
            this.sessionHandler = sessionHandler;
            this.clientRepository = clientRepository;
        }

        public async Task<UpdateImageResponseModel> UpdateProfileImageAsync(UpdateProfileImageModel updateProfileImageModel)
        {
            var response = new UpdateImageResponseModel { IsSuccessful = false, Message = "Error while updating" };
            ClientData client = await sessionHandler.GetClientAsync(updateProfileImageModel.Token);
            if (client == null)
            {
                response.Message = "Invalid javascript web token - access denied";
                return response;
            }

            try
            {
                string clientImageFolderAbsolutePath = Path.Combine(Directory.GetCurrentDirectory(), webFilesFolder, accountImageStorage, client.Id.ToString());
                if (!Directory.Exists(clientImageFolderAbsolutePath))
                {
                    Directory.CreateDirectory(clientImageFolderAbsolutePath);
                }

                string newImageRelativePath = Path.Combine(accountImageStorage, client.Id.ToString(), updateProfileImageModel.Image.FileName);
                string newImageAbsolutePath = Path.Combine(clientImageFolderAbsolutePath, updateProfileImageModel.Image.FileName);
                if (updateProfileImageModel.Image.Length <= 0)
                {
                    response.Message = "Error while loading file";
                    return response;
                }
                using (var stream = new FileStream(newImageAbsolutePath, FileMode.Create))
                {
                    await updateProfileImageModel.Image.CopyToAsync(stream);
                }

                string oldImageAbsolutePath = Path.Combine(Directory.GetCurrentDirectory(), webFilesFolder, client.PhotoPath);
                if (File.Exists(oldImageAbsolutePath) && (oldImageAbsolutePath != newImageAbsolutePath))
                {
                    File.Delete(oldImageAbsolutePath);
                }
                client.PhotoPath = newImageRelativePath;
                await clientRepository.UpdateAsync(client);
                response.IsSuccessful = true;
                response.Message = newImageRelativePath;
                return response;
            }
            catch (Exception)
            {
                return response;
            }
        }
    }
}