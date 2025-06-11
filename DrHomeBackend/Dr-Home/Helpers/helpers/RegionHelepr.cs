
using Dr_Home.UnitOfWork;
using System.Text.Json;

namespace Dr_Home.Helpers.helpers
{
    public class RegionHelepr(IWebHostEnvironment environment) : IRegionHelper
    {
        private readonly IWebHostEnvironment _environment = environment;
        public async Task<ApiResponse<IEnumerable<Region>>> GetCityRegionsAsync(int CityId , string lang)
        {
            var filePath = Path.Combine(_environment.ContentRootPath, "Data", "regions.json");

            if (!System.IO.File.Exists(filePath))
                return new ApiResponse<IEnumerable<Region>>
                {
                    Success = false,
                    Message = "There Is No Data",

                };

            var json = await System.IO.File.ReadAllTextAsync(filePath);
            var regions = JsonSerializer.Deserialize<List<region>>(json);

            var result = regions!.Where(r => int.Parse(r.governorate_id) == CityId).Select(
                x => new Region
                {
                    Id = x.id,
                    CityId = int.Parse(x.governorate_id),
                    Name = (lang == "ar") ? x.city_name_ar : x.city_name_en
                }
                );

            return new ApiResponse<IEnumerable<Region>> { Success = true, Message = "Regions Loaded Successfully", Data = result };

        }

        public async Task<ApiResponse<IEnumerable<Region>>> GetAllRegionsAsync(string lang)
        {
            var filePath = Path.Combine(_environment.ContentRootPath, "Data", "regions.json");

            if (!System.IO.File.Exists(filePath))
                return new ApiResponse<IEnumerable<Region>>
                {
                    Success = false,
                    Message = "There Is No Data",

                };

            var json = await System.IO.File.ReadAllTextAsync(filePath);
            var regions = JsonSerializer.Deserialize<List<region>>(json);

            var result = regions!.Select(r => new Region
            {
                Id = r.id,
                CityId = int.Parse(r.governorate_id),
                Name = (lang == "ar") ? r.city_name_ar : r.city_name_en

            });

            return new ApiResponse<IEnumerable<Region>> { Success = true, Message = "Regions Loaded Successfully", Data = result };

        }
    }
}
