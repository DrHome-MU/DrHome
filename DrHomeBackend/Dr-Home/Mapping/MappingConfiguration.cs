using Dr_Home.Data.Models;
using Dr_Home.DTOs.DoctorDtos;
using Mapster;

namespace Dr_Home.Mapping
{
    public class MappingConfiguration : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<AddDoctorDto, Doctor>()
                .Ignore(dest => dest.HashPassword)
                .Ignore(dest => dest.role);
                

           // config.NewConfig<UpdateDoctorDto, Doctor>() .Ignore(src => src.P);
        }
    }
}
