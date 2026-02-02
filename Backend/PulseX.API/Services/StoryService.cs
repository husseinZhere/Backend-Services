using AutoMapper;
using PulseX.Core.DTOs.Story;
using PulseX.Core.Interfaces;
using PulseX.Core.Models;

namespace PulseX.API.Services
{
    public class StoryService
    {
        private readonly IStoryRepository _storyRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IActivityLogRepository _activityLogRepository;
        private readonly IMapper _mapper;

        public StoryService(
            IStoryRepository storyRepository,
            IPatientRepository patientRepository,
            IActivityLogRepository activityLogRepository,
            IMapper mapper)
        {
            _storyRepository = storyRepository;
            _patientRepository = patientRepository;
            _activityLogRepository = activityLogRepository;
            _mapper = mapper;
        }

        public async Task<StoryDto> CreateStoryAsync(int patientId, CreateStoryDto dto)
        {
            var patient = await _patientRepository.GetByIdAsync(patientId);
            if (patient == null)
            {
                throw new Exception("Patient not found");
            }

            var story = _mapper.Map<Story>(dto);
            story.PatientId = patientId;

            await _storyRepository.AddAsync(story);

            await _activityLogRepository.AddAsync(new ActivityLog
            {
                UserId = patient.UserId,
                Action = "Create Story",
                EntityType = "Story",
                EntityId = story.Id,
                Details = $"Story '{story.Title}' created"
            });

            var result = await _storyRepository.GetByIdAsync(story.Id);
            return _mapper.Map<StoryDto>(result);
        }

        public async Task<IEnumerable<StoryDto>> GetPublishedStoriesAsync()
        {
            var stories = await _storyRepository.GetPublishedAsync();
            return _mapper.Map<IEnumerable<StoryDto>>(stories);
        }

        public async Task<IEnumerable<StoryDto>> GetPatientStoriesAsync(int patientId)
        {
            var stories = await _storyRepository.GetByPatientIdAsync(patientId);
            return _mapper.Map<IEnumerable<StoryDto>>(stories);
        }

        public async Task<IEnumerable<StoryDto>> GetAllStoriesAsync()
        {
            var stories = await _storyRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<StoryDto>>(stories);
        }

        public async Task<StoryDto> PublishStoryAsync(int storyId, int adminUserId)
        {
            var story = await _storyRepository.GetByIdAsync(storyId);
            if (story == null)
            {
                throw new Exception("Story not found");
            }

            story.IsPublished = true;
            story.IsHidden = false;
            story.PublishedAt = DateTime.UtcNow;

            await _storyRepository.UpdateAsync(story);

            await _activityLogRepository.AddAsync(new ActivityLog
            {
                UserId = adminUserId,
                Action = "Publish Story",
                EntityType = "Story",
                EntityId = story.Id,
                Details = $"Story '{story.Title}' published"
            });

            var result = await _storyRepository.GetByIdAsync(storyId);
            return _mapper.Map<StoryDto>(result);
        }

        public async Task<StoryDto> HideStoryAsync(int storyId, int adminUserId)
        {
            var story = await _storyRepository.GetByIdAsync(storyId);
            if (story == null)
            {
                throw new Exception("Story not found");
            }

            story.IsHidden = true;

            await _storyRepository.UpdateAsync(story);

            await _activityLogRepository.AddAsync(new ActivityLog
            {
                UserId = adminUserId,
                Action = "Hide Story",
                EntityType = "Story",
                EntityId = story.Id,
                Details = $"Story '{story.Title}' hidden"
            });

            var result = await _storyRepository.GetByIdAsync(storyId);
            return _mapper.Map<StoryDto>(result);
        }

        public async Task DeleteStoryAsync(int storyId, int adminUserId)
        {
            var story = await _storyRepository.GetByIdAsync(storyId);
            if (story == null)
            {
                throw new Exception("Story not found");
            }

            await _activityLogRepository.AddAsync(new ActivityLog
            {
                UserId = adminUserId,
                Action = "Delete Story",
                EntityType = "Story",
                EntityId = story.Id,
                Details = $"Story '{story.Title}' deleted"
            });

            await _storyRepository.DeleteAsync(storyId);
        }
    }
}
