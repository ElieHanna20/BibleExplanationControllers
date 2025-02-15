using BibleExplanationControllers.Dtos.SubAdminDtos;
using BibleExplanationControllers.Models.User;
using System;

namespace BibleExplanationControllers.Mappers
{
    public static class SubAdminMapper
    {
        public static SubAdmin ToSubAdmin(this SubAdminCreateDto dto, Guid adminId)
        {
            return new SubAdmin
            {
                Id = Guid.NewGuid(), // Generate a new GUID for the SubAdmin's Id
                Username = dto.Username,
                AdminId = adminId, // Assign the Admin's Id
                CanChangeBooksData = dto.CanChangeBooksData,
                PasswordHash = string.Empty // Placeholder; will be set after hashing
            };
        }

        public static SubAdminDetailsDto ToSubAdminDetailsDto(this SubAdmin subAdmin)
        {
            return new SubAdminDetailsDto
            {
                Id = subAdmin.Id, // Now both are Guid, assignment is direct
                Username = subAdmin.Username,
                CanChangeBooksData = subAdmin.CanChangeBooksData
            };
        }

        public static void UpdateSubAdminFromDto(this SubAdmin subAdmin, SubAdminUpdateDto dto)
        {
            if (!string.IsNullOrWhiteSpace(dto.Username))
            {
                subAdmin.Username = dto.Username;
            }

            if (dto.CanChangeBooksData.HasValue)
            {
                subAdmin.CanChangeBooksData = dto.CanChangeBooksData.Value;
            }
            // Password is handled separately in the controller
        }
    }
}
