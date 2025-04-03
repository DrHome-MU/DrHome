
using Dr_Home.UnitOfWork;

namespace Dr_Home.Helpers.helpers
{
    public class CityHelper(IUnitOfWork _unitOfWork) : ICityHelper
    {
        public async Task<ApiResponse<City>> AddAsync(string name, CancellationToken cancellationToken = default)
        {
            var city  = new City { Name = name };

            if (await _unitOfWork._cityService.GetByNameAsync(name) != null)
                return new ApiResponse<City> { Success = false, Message = "Already Added!!" };

            await _unitOfWork._cityService.AddAsync(city , cancellationToken);  
            await _unitOfWork.Complete(cancellationToken);

            return new ApiResponse<City> {Success = true, Message ="Done!" , Data = city};

        }

        public async Task<ApiResponse<IEnumerable<City>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var values = await _unitOfWork._cityService.GetAllAsync(cancellationToken);

            if (!values.Any())
                return new ApiResponse<IEnumerable<City>> { Success = false  , Message = "There Is No Data" , Data = values};

            return new ApiResponse<IEnumerable<City>> {Success = true , Message = "Cities Loaded Successfully" , Data = values };
        }
    }
}
