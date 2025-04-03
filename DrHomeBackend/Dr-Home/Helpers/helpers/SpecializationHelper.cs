using Dr_Home.Data.Models;
using Dr_Home.UnitOfWork;

namespace Dr_Home.Helpers.helpers
{
    public class SpecializationHelper(IUnitOfWork _unitOfWork) : ISpecializationHelper
    {
        public async Task<ApiResponse<Specialization>> AddAsync(string name, CancellationToken cancellationToken = default)
        {
            var entity = new Specialization { Name = name };

            if (await _unitOfWork._specializationService.GetByName(name) != null)
                return new ApiResponse<Specialization> { Success = false, Message = "Already Added Before!!" };

            await _unitOfWork._specializationService.Add(entity, cancellationToken);
            await _unitOfWork.Complete(cancellationToken);


            return new ApiResponse<Specialization> { Success = true, Message = "Added Successfully", Data = entity };

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
