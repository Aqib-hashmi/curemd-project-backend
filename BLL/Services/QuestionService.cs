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

        public int CreateQuestion(Question q)
        {
            if (q == null)
            {
                throw new ArgumentException("Invalid request.");
            }

            if (string.IsNullOrWhiteSpace(q.Title))
            {
                throw new ArgumentException("Title is required.");
            }

            if (string.IsNullOrWhiteSpace(q.Description))
            {

                throw new ArgumentException("Description is required.");
            }

            if (q.Title.Length > 200)
            {

                throw new ArgumentException("Title cannot exceed 200 characters.");
            }

            if (q.Description.Length > 2000)
            {

                throw new ArgumentException("Description cannot exceed 2000 characters.");
            }

            if (q.UserId <= 0)
            {

                throw new ArgumentException("Invalid UserId.");
            }

            return _questionRepo.CreateQuestion(q);
        }


        public List<Question> GetAllQuestions()
        {
            var questions = _questionRepo.GetAllQuestions();

            return questions.Select(q => new Question
            {
                QuestionId = q.QuestionId,
                Title = q.Title,
                Description = q.Description,
                Views = q.Views,
                UserId = q.UserId,
                CreatedDate = q.CreatedDate,
                UpdatedDate = q.UpdatedDate,
                Owner = new User
                {
                    UserId = q.Owner.UserId,
                    Name = q.Owner.Name,
                    Email = q.Owner.Email,
                    Password = q.Owner.Password,
                    RoleId = q.Owner.RoleId,
                    Bio = q.Owner.Bio,
                    isActive = q.Owner.isActive
                }
            }).ToList();
        }


        public Question? GetQuestionById(int questionId)
        {
            return _questionRepo.GetUserById(questionId);
        }

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
