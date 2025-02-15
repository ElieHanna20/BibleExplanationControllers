using BibleExplanationControllers.Dtos.WorkerDtos;
using BibleExplanationControllers.Models.User;
using System;

namespace BibleExplanationControllers.Mappers
{
    public static class WorkerMapper
    {
        public static Worker ToWorker(this WorkerCreateDto dto, Guid subAdminId)
        {
            return new Worker
            {
                Id = Guid.NewGuid(),
                Username = dto.Username,
                SubAdminId = subAdminId,
                CanChangeBooksData = dto.CanChangeBooksData ?? false,
                PasswordHash = string.Empty // Will be set in the controller after hashing
            };
        }

        public static WorkerDetailsDto ToWorkerResponseDto(this Worker worker)
        {
            return new WorkerDetailsDto
            {
                Id = worker.Id, // Now a Guid
                Username = worker.Username,
                CanChangeBooksData = worker.CanChangeBooksData
            };
        }

        public static void UpdateWorkerFromDto(this Worker worker, WorkerUpdateDto dto)
        {
            if (!string.IsNullOrWhiteSpace(dto.Username))
            {
                worker.Username = dto.Username;
            }

            if (dto.CanChangeBooksData.HasValue)
            {
                worker.CanChangeBooksData = dto.CanChangeBooksData.Value;
            }
            // Password is handled separately in the controller
        }
    }
}
