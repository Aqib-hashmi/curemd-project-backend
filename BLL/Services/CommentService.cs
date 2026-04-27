using StackOverFlowReplica.DAL.Repositories;
using StackOverFlowReplica.Models.payloadModel;

namespace StackOverFlowReplica.BLL.Services
{
    public class CommentService
    {
        private readonly IConfiguration _config;
        private readonly CommentRepository _comntRepo;

        public CommentService(IConfiguration config, CommentRepository comntRepo)
        {
            _config = config;
            _comntRepo = comntRepo;
        }

        public (bool success, string message, object? data) AddQuestionComment(
            AddQuestionCommentDto dto, int userId)
        {
            if (string.IsNullOrWhiteSpace(dto.Content))
                return (false, "Comment empty nahi ho sakta", null);
            if (dto.Content.Length > 600)
                return (false, "Comment 600 characters se zyada nahi ho sakta", null);
            var result = _comntRepo.AddQuestionComment(dto.Content, userId, dto.QuestionId);
            return (true, "Comment added", result);
        }

        public (bool success, string message) EditQuestionComment(
            int commentId, EditCommentDto dto, int userId,int RoleId)
        {
            if (string.IsNullOrWhiteSpace(dto.Content))
                return (false, "Comment empty nahi ho sakta");
            var result = _comntRepo.EditQuestionComment(commentId, dto.Content, userId, RoleId);
            if (result == "") return (false, "Unauthorized ya comment exist nahi");
            return (true, "Comment updated");
        }

        public (bool success, string message) DeleteQuestionComment(
            int commentId, int userId, int RoleId)
        {
            var result = _comntRepo.DeleteQuestionComment(commentId, userId, RoleId);
            if (result == "") return (false, "Unauthorized ya comment exist nahi");
            return (true, "Comment deleted");
        }

        public (bool success, string message, object? data) AddAnswerComment(
            AddAnswerCommentDto dto, int userId)
        {
            if (string.IsNullOrWhiteSpace(dto.Content))
                return (false, "Comment empty nahi ho sakta", null);
            if (dto.Content.Length > 600)
                return (false, "Comment 600 characters se zyada nahi ho sakta", null);
            var result = _comntRepo.AddAnswerComment(dto.Content, userId, dto.AnswerId);
            return (true, "Comment added", result);
        }

        public (bool success, string message) EditAnswerComment(
            int commentId, EditCommentDto dto, int userId ,int RoleId)
        {
            if (string.IsNullOrWhiteSpace(dto.Content))
                return (false, "Comment empty nahi ho sakta");
            var result = _comntRepo.EditAnswerComment(commentId, dto.Content, userId, RoleId);
            if (result == "") return (false, "Unauthorized ya comment exist nahi");
            return (true, "Comment updated");
        }

        public (bool success, string message) DeleteAnswerComment(
            int commentId, int userId, int RoleId)
        {
            var result = _comntRepo.DeleteAnswerComment(commentId, userId, RoleId);
            if (result == "") return (false, "Unauthorized ya comment exist nahi");
            return (true, "Comment deleted");
        }
    }
}
