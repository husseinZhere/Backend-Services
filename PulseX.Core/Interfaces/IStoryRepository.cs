using PulseX.Core.Models;

namespace PulseX.Core.Interfaces
{
    public interface IStoryRepository
    {
        Task<Story?> GetByIdAsync(int id);
        Task<IEnumerable<Story>> GetAllAsync();
        Task<IEnumerable<Story>> GetPublishedAsync();
        Task<IEnumerable<Story>> GetByPatientIdAsync(int patientId);
        Task<Story> AddAsync(Story story);
        Task UpdateAsync(Story story);
        Task DeleteAsync(int id);
    }
}
