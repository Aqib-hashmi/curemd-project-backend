using StackOverFlowReplica.DAL.Repositories;
using StackOverFlowReplica.Models.payloadModel;

namespace StackOverFlowReplica.BLL.Services
{
    public class SearchService
    {
        private readonly IConfiguration _config;
        private readonly SearchRepository _searchRepo;

        public SearchService(IConfiguration config, SearchRepository searchRepo)
        {
            _config = config;
            _searchRepo = searchRepo;
        }

        public List<TagSearchDto> GetTagSuggestions(string searchText, int? userId = null)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return new List<TagSearchDto>();

            return _searchRepo.GetTagSuggestions(searchText, userId);
        }

        public List<TagSearchDto> GetUserSearchHistory(int userId)
        {
            return _searchRepo.GetUserSearchHistory(userId);
        }

        public void SaveSearchHistory(int userId, int tagId)
        {
            _searchRepo.SaveSearchHistory(userId, tagId);
        }

    }
}
