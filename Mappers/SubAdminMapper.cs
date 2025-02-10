using BibleExplanationControllers.Dtos.SubAdminDtos;
using BibleExplanationControllers.Models.User;

namespace BibleExplanationControllers.Mappers
{
    public static class SubAdminMapper
    {
        public static SubAdmin ToSubAdmin(this SubAdminCreateDto dto, string adminId)
        {
            return new SubAdmin
            {
                UserName = dto.Username,
                Email = dto.Email,
                CanChangeBooksData = dto.CanChangeBooksData,
                AdminId = adminId
            };
        }

        public static SubAdminDetailsDto ToSubAdminDetailsDto(this SubAdmin subAdmin)
        {
            return new SubAdminDetailsDto
            {
                Id = subAdmin.Id,
                Username = subAdmin.UserName,
                Email = subAdmin.Email,
                CanChangeBooksData = subAdmin.CanChangeBooksData,
                AdminId = subAdmin.AdminId
            };
        }

        public static void UpdateSubAdminFromDto(this SubAdmin subAdmin, SubAdminUpdateDto dto)
        {
            if (!string.IsNullOrWhiteSpace(dto.Username))
            {
                subAdmin.UserName = dto.Username;
            }

            if (dto.CanChangeBooksData.HasValue)
            {
                subAdmin.CanChangeBooksData = dto.CanChangeBooksData.Value;
            }
        }
    }
}
