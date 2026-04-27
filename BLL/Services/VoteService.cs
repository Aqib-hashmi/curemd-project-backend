using StackOverFlowReplica.DAL.Repositories;
using StackOverFlowReplica.Models.payloadModel;

namespace StackOverFlowReplica.BLL.Services
{
    public class VoteService
    {

        private readonly IConfiguration _config;
        private readonly VoteRepository _VoteRepo;

        public VoteService(IConfiguration config, VoteRepository VoteRepo)
        {
            _config = config;
            _VoteRepo = VoteRepo;
        }

        public (bool success, string message) VoteQuestion(int userId, VoteQuestionDto dto)
        {
            if (dto.VoteValue != 1 && dto.VoteValue != -1)
                return (false, "VoteValue sirf 1 ya -1 ho sakta hai");

            var status = _VoteRepo.VoteQuestion(userId, dto.QuestionId, dto.VoteValue);
            return (true, status);
        }

        public (bool success, string message) VoteAnswer(int userId, VoteAnswerDto dto)
        {
            if (dto.VoteValue != 1 && dto.VoteValue != -1)
                return (false, "VoteValue sirf 1 ya -1 ho sakta hai");

            var status = _VoteRepo.VoteAnswer(userId, dto.AnswerId,dto.QuestionId, dto.VoteValue);
            return (true, status);
        }
    }
}
