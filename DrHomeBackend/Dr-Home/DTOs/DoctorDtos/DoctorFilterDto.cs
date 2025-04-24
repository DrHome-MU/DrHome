namespace Dr_Home.DTOs.DoctorDtos
{
    public record DoctorFilterDto
    (
        string? FullName , 
        string? city , 
        string? region ,
        int? SpecializationId
    );
}
