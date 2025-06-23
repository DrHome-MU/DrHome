
using Dr_Home.DTOs.RegionDtos;
using Dr_Home.UnitOfWork;
using System.Text.Json;

namespace Dr_Home.Helpers.helpers
{
    public class CityHelper(IWebHostEnvironment environment) : ICityHelper
    {
        private readonly IWebHostEnvironment _environment = environment;


        public async Task<ApiResponse<IEnumerable<City>>> GetAllAsync(string lang)
        {
            var filePath = Path.Combine(_environment.ContentRootPath, "Data", "governorate.json");

            if (!System.IO.File.Exists(filePath))
                return new ApiResponse<IEnumerable<City>>
                {
                    Success = false,
                    Message = "There Is No Data",

                };

            var json = await System.IO.File.ReadAllTextAsync(filePath);
            var governorates = JsonSerializer.Deserialize<List<Governorate>>(json);


            var citiess = governorates!.Select(g => new City
            {
                Id = g.Id,
                Name = (lang == "ar") ?  g.Governorate_Name_Ar : g.Governorate_Name_En
            });
          

            return new ApiResponse<IEnumerable<City>>
            {
                Success = true,
                Message = "Cities Loaded Successfully",
                Data = citiess
            };
        }
    }
}
