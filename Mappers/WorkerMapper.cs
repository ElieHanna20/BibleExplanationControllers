using BibleExplanationControllers.Dtos.WorkerDtos;
using BibleExplanationControllers.Models.User;

namespace BibleExplanationControllers.Mappers
{
    public static class WorkerMapper
    {
        public static Worker ToWorker(this WorkerCreateDto dto, string subAdminId)
        {
            return new Worker
            {
                UserName = dto.Username,
                SubAdminId = subAdminId,
                CanChangeBooksData = dto.CanChangeBooksData ?? false
            };
        }


        public static WorkerDetailsDto ToWorkerResponseDto(this Worker worker)
        {
            return new WorkerDetailsDto
            {
                Id = worker.Id,
                Username = worker.UserName,
                CanChangeBooksData = worker.CanChangeBooksData
            };
        }

        public static void UpdateWorkerFromDto(this Worker worker, WorkerUpdateDto dto)
        {
            if (!string.IsNullOrWhiteSpace(dto.Username))
            {
                worker.UserName = dto.Username;
            }

            if (dto.CanChangeBooksData.HasValue)
            {
                worker.CanChangeBooksData = dto.CanChangeBooksData.Value;
            }
        }
    }
}
