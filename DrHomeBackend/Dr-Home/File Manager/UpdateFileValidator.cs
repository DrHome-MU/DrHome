using Dr_Home.DTOs.DoctorDtos;
using Dr_Home.Settings;
using FluentValidation;

namespace Dr_Home.File_Manager
{
    public class UpdateFileValidator:AbstractValidator<UpdateDoctorDto>
    {
        public UpdateFileValidator()
        {
            //Pic Size
            RuleFor(x => x.PersonalPic)
                .Must((request, context) => request.PersonalPic.Length <= FileSettings.MaximumSizeInBytes)
                .WithMessage($"Max File Size Is {FileSettings.MaximumSizeInBytes}")
                .When(x => x.PersonalPic is not null);
            //Pic Squence

            RuleFor(x => x.PersonalPic)
                .Must((request, context) =>
                {
                    BinaryReader binary = new(request.PersonalPic.OpenReadStream());
                    var bytes = binary.ReadBytes(2); 

                    var fileSequenceHex = BitConverter.ToString(bytes);

                    foreach(var signeture in FileSettings.BlockedSignatures)
                    {
                        if (signeture.Equals(fileSequenceHex, StringComparison.OrdinalIgnoreCase))
                            return false;
                    }
                    return true;
                })
                .WithMessage("File Content Is Not Allowed")
                .When(x => x.PersonalPic is not null);

            //Pic Extension

            RuleFor(x => x.PersonalPic)
                .Must((request, context) =>
                {
                    var extension = Path.GetExtension(request.PersonalPic.FileName.ToLower());

                    return FileSettings.AllowedExtensions.Contains(extension);
                })
                .WithMessage("Extension Is Not Allowed")
                .When(x => x.PersonalPic is not null); 

        }
    }
}
