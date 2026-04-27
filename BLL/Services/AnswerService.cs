using StackOverFlowReplica.DAL.Repositories;
using StackOverFlowReplica.Models.payloadModel;

namespace StackOverFlowReplica.BLL.Services
{
    public class AnswerService
    {
        private readonly IConfiguration _config;
        private readonly AnswerRepository _AnsRepo;

        public AnswerService(IConfiguration config, AnswerRepository AnsRepo)
        {
            _config = config;
            _AnsRepo = AnsRepo;
        }

        public (bool success, string message, object? data) AddAnswer( AddAnswerDto dto, int userId)
        {
            if (string.IsNullOrWhiteSpace(dto.Description))
                return (false, "Answer empty nahi ho sakta", null);
            var result = _AnsRepo.AddAnswer(dto.Description, userId, dto.QuestionId);
            return (true, "Answer added", result);
        }

        public (bool success, string message) EditAnswer(int answerId, EditAnswerDto dto, int userId,int RoleId)
        {
            if (string.IsNullOrWhiteSpace(dto.Description))
            {
                return (false, "Answer empty nahi ho sakta");
            }
            bool isOwner = _AnsRepo.IsAnswerOwner(answerId, userId);
            bool isAdmin = RoleId == 1;
            if (!isOwner && !isAdmin)
                return (false, "Unauthorized");
            var result = _AnsRepo.EditAnswer(answerId, dto.Description, userId, RoleId);
            if (result != "updated")
            {
                return (false, "Update failed");
            }
            return (true, "Answer updated");
        }

        public (bool success, string message) DeleteAnswer(
            int answerId, int userId,int RoleId)
        {
            var result = _AnsRepo.DeleteAnswer(answerId, userId, RoleId);
            if (result == "unauthorized")
            {
                return (false, "Unauthorized ya answer exist nahi");
            }
            return (true, "Answer deleted");
        }
    }
}
