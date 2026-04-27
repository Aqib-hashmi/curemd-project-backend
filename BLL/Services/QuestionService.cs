using StackOverFlowReplica.DAL.Repositories;
using StackOverFlowReplica.Models.payloadModel;
using StackOverFlowReplica.StackOverFlowReplica.Models;

namespace StackOverFlowReplica.BLL.Services
{
    public class QuestionService
    {
        private readonly IConfiguration _config;
        private readonly QuestionRepository _questionRepo;

        public QuestionService(IConfiguration config, QuestionRepository questionRepo)
        {
            _config = config;
            _questionRepo = questionRepo;
        }
         public int CreateQuestion(CreateQuestion dto, int userId)
            {
                string tagIds = string.Join(",", dto.TagIds);

                return _questionRepo.CreateQuestion(
                    dto.Title,
                    dto.Description,
                    userId,
                    tagIds
                );
            }


        public List<Question> GetAllQuestions(int? userId, int pageNumber, int pageSize,string? tagName)
        {
            return _questionRepo.GetAllQuestions(userId, pageNumber, pageSize,tagName);
        }

        public QuestionDetailDto GetQuestionDetail(int questionId)
        {
            return _questionRepo.GetQuestionDetail(questionId);
        }

        public (bool success, string status) AddQuestionView(int questionId, int userId)
        {
            var status = _questionRepo.AddQuestionView(questionId, userId);
            return (true, status);
        }


        public (bool success, string message) UpdateQuestion(int questionId, UpdateQuestionDto dto, int currentUserId, string currentUserRole)
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
                return (false, "Title required hai");

            if (string.IsNullOrWhiteSpace(dto.Description))
                return (false, "Description required hai");

            if (dto.TagIds == null || dto.TagIds.Count == 0)
                return (false, "Kam se kam ek tag select karo");

            bool isOwner = _questionRepo.IsQuestionOwner(questionId, currentUserId);
            bool isAdmin = currentUserRole == "Admin" || currentUserRole == "1";

            if (!isOwner && !isAdmin)
                return (false, "Unauthorized");

            bool result = _questionRepo.UpdateQuestion(questionId, dto);

            return result
                ? (true, "Question updated successfully")
                : (false, "Update failed");
        }

        public (bool success, string message) deleteQuestion(int questionId, int currentUserId, string currentUserRole)
        {

            bool isOwner = _questionRepo.IsQuestionOwner(questionId, currentUserId);
            bool isAdmin = currentUserRole == "Admin" || currentUserRole == "1";

            if (!isOwner && !isAdmin)
                return (false, "Unauthorized");

            bool result = _questionRepo.deleteQuestion(questionId,currentUserId);

            return result
                ? (true, "Question deleted successfully")
                : (false, "delete failed");
        }
        //public Question? GetQuestionById(int questionId)
        //{
        //    return _questionRepo.GetUserById(questionId);
        //}

        // BLL/Services/AuthService.cs
        //public bool UpdateUser(User user)
        //{
        //    // Optional: check if user exists first
        //    var existingUser = _repo.GetUserById(user.UserId);
        //    if (existingUser == null)
        //        return false;

        //    return _repo.UpdateUser(user);
        //}

    }

}
