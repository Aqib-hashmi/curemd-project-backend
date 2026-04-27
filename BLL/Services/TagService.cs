using Microsoft.EntityFrameworkCore;
using StackOverFlowReplica.DAL.Repositories;
using StackOverFlowReplica.Models.payloadModel;
using StackOverFlowReplica.StackOverFlowReplica.Models;

namespace StackOverFlowReplica.BLL.Services
{
    public class TagService
    {
        private readonly IConfiguration _config;
        private readonly TagRepository _tagRepo;

        public TagService(IConfiguration config, TagRepository tagRepo)
        {
            _config = config;
            _tagRepo = tagRepo;
        }


        public List<Tag> GetAllTags()
        {
            var tags = _tagRepo.GetAllTags();

            return tags.Select(t => new Tag
            {
                TagId = t.TagId,
                TagName = t.TagName,
                Description = t.Description,
                CreatedDate = t.CreatedDate,
                UpdatedDate = t.UpdatedDate,
            }).ToList();
        }

        public Tag CreateTag(CreateTag dto)
        {
            var name = dto.TagName.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(name))
                throw new Exception("Tag name is required");

            if (name.Length < 2 || name.Length > 30)
                throw new Exception("Tag length must be between 2 and 30 characters");

            // duplicate check
            var exists = _tagRepo.GetAllTags()
                .Any(t => t.TagName.ToLower() == name);

            if (exists)
                throw new Exception("Tag already exists");

            var tag = new Tag
            {
                TagName = name,
                Description = dto.Description,
                CreatedDate = DateTime.UtcNow
            };

            // repository call
            var createdTag = _tagRepo.CreateTag(tag);

            return createdTag;
        }
        public List<TagSuggestionDto> SearchTagSuggestions(string search)
        {
            return _tagRepo.SearchTagSuggestions(search);
        }
    }
}