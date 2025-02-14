using BibleExplanationControllers.Dtos.SubAdminDtos;
using BibleExplanationControllers.Models.User;

namespace BibleExplanationControllers.Mappers
{
    public static class SubAdminMapper
    {
        public static SubAdmin ToSubAdmin(this SubAdminCreateDto dto)
        {
            return new SubAdmin
            {
                UserName = dto.Username,
                CanChangeBooksData = dto.CanChangeBooksData
            };
        }

        public static SubAdminDetailsDto ToSubAdminDetailsDto(this SubAdmin subAdmin)
        {
            return new SubAdminDetailsDto
            {
                Id = subAdmin.Id,
                Username = subAdmin.UserName,
                CanChangeBooksData = subAdmin.CanChangeBooksData
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
