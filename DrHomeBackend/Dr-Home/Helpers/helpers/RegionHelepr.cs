
using Dr_Home.UnitOfWork;

namespace Dr_Home.Helpers.helpers
{
    public class RegionHelepr(IUnitOfWork _unitOfWork) : IRegionHelper
    {
        public async Task<ApiResponse<Region>> AddRegionAsync(AddRegionDto dto, CancellationToken cancellationToken = default)
        {
            var region = new Region
            {
                Name = dto.name,
                CityId = dto.CityId
            };

            if (await _unitOfWork._regionService.IsRegionExistedAsync(dto,cancellationToken) == true)
                return new ApiResponse<Region>() { Success = false, Message = "This Region Added With This City before!!" };

            await _unitOfWork._regionService.AddAsync(region, cancellationToken);
            await _unitOfWork.Complete(cancellationToken);

            return new ApiResponse<Region> { Success = true, Message = "Added Successfully", Data = region };
        }

       

        public async Task<ApiResponse<IEnumerable<Region>>> GetCityRegionsAsync(int CityId, CancellationToken cancellationToken = default)
        {
            var regions = await _unitOfWork._regionService.GetCityRegionsAsync(CityId, cancellationToken);

            if (!regions.Any()) return new ApiResponse<IEnumerable<Region>>
            {
                Success = false,
                Message = "There Is No Regions For This City",
                Data = regions
            };


            return new ApiResponse<IEnumerable<Region>> { Success = true, Message = "Regions Loaded Successfully", Data = regions };

        }

        public async Task<ApiResponse<IEnumerable<Region>>> GetAllRegionsAsync(CancellationToken cancellationToken = default)
        {
            var regions = await _unitOfWork._regionService.GetAllAsync(cancellationToken);

            if (!regions.Any())
                return new ApiResponse<IEnumerable<Region>> { Success = false, Message = "There Is No Regions", Data = regions };

            return new ApiResponse<IEnumerable<Region>> { Success = true, Message = "Regions Loaded Successffully", Data = regions };
        }
    }
}
