using Dr_Home.Data.Models;
using Dr_Home.File_Manager;
using Dr_Home.UnitOfWork;
using System.Numerics;

namespace Dr_Home.Helpers.helpers
{
    public class SpecializationHelper(IUnitOfWork _unitOfWork , IFileManager fileManager) : ISpecializationHelper
    {
        private readonly IFileManager _fileManager = fileManager;

        public async Task<ApiResponse<Specialization>> AddAsync(string name, CancellationToken cancellationToken = default)
        {
            var entity = new Specialization { Name = name };

            if (await _unitOfWork._specializationService.GetByName(name) != null)
                return new ApiResponse<Specialization> { Success = false, Message = "Already Added Before!!" };

            await _unitOfWork._specializationService.Add(entity, cancellationToken);
            await _unitOfWork.Complete(cancellationToken);


            return new ApiResponse<Specialization> { Success = true, Message = "Added Successfully", Data = entity };

        }

        public async Task<Result> updateAsync(int id , IFormFile? _pic , CancellationToken cancellationToken = default)
        {
            var entity = await _unitOfWork._specializationService.GetByIdAsync(id, cancellationToken);

            if (entity == null)
                return Result.Failure(new Error("specialization.NotFound", "Not Found", StatusCodes.Status404NotFound));
            if (entity.PicturePath!= null) { await _fileManager.Delete(entity.PicturePath); }
            if (_pic == null)
                entity.PicturePath = null;

            else entity.PicturePath = await _fileManager.Upload(_pic, cancellationToken);

            await _unitOfWork.Complete(cancellationToken);

            return Result.Success();
        }

        public async Task<ApiResponse<IEnumerable<Specialization>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var values = await _unitOfWork._specializationService.GetAll();

            if (!values.Any()) return new ApiResponse<IEnumerable<Specialization>> { Success = false, Message = "No Data", Data = values };

            return new ApiResponse<IEnumerable<Specialization>> { Success = true, Message = "Data Loaded Successfully", Data = values };
        }

        public async Task<ApiResponse<Specialization>> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _unitOfWork._specializationService.GetByIdAsync(id, cancellationToken);

            if (entity == null) return new ApiResponse<Specialization> { Success = false, Message = "Specialization Doesn`t exist" };

            await _unitOfWork._specializationService.DeleteAsync(entity, cancellationToken);
            await _unitOfWork.Complete(cancellationToken);

            return new ApiResponse<Specialization> { Success = true, Message = "Done!" };
        }
    }
}
