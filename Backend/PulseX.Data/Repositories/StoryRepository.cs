using Microsoft.EntityFrameworkCore;
using PulseX.Core.Interfaces;
using PulseX.Core.Models;

namespace PulseX.Data.Repositories
{
    public class StoryRepository : IStoryRepository
    {
        private readonly ApplicationDbContext _context;

        public StoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Story?> GetByIdAsync(int id)
        {
            return await _context.Stories
                .Include(s => s.Patient).ThenInclude(p => p.User)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Story>> GetAllAsync()
        {
            return await _context.Stories
                .Include(s => s.Patient).ThenInclude(p => p.User)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Story>> GetPublishedAsync()
        {
            return await _context.Stories
                .Include(s => s.Patient).ThenInclude(p => p.User)
                .Where(s => s.IsPublished && !s.IsHidden)
                .OrderByDescending(s => s.PublishedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Story>> GetByPatientIdAsync(int patientId)
        {
            return await _context.Stories
                .Include(s => s.Patient).ThenInclude(p => p.User)
                .Where(s => s.PatientId == patientId)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
        }

        public async Task<Story> AddAsync(Story story)
        {
            await _context.Stories.AddAsync(story);
            await _context.SaveChangesAsync();
            return story;
        }

        public async Task UpdateAsync(Story story)
        {
            story.UpdatedAt = DateTime.UtcNow;
            _context.Stories.Update(story);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var story = await _context.Stories.FindAsync(id);
            if (story != null)
            {
                _context.Stories.Remove(story);
                await _context.SaveChangesAsync();
            }
        }
    }
}
