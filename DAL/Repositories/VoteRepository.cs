using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StackOverFlowReplica.Context;
using System.Data;

namespace StackOverFlowReplica.DAL.Repositories
{
    public class VoteRepository
    {
        private readonly AppDbContext _db;

        public VoteRepository(AppDbContext db)
        {
            _db = db;
        }

        public string VoteQuestion(int userId, int questionId, int voteValue)
        {
            using var connection = _db.Database.GetDbConnection();
            using var command = connection.CreateCommand();

            command.CommandText = "VoteQuestion";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter("@UserId", userId));
            command.Parameters.Add(new SqlParameter("@QuestionId", questionId));
            command.Parameters.Add(new SqlParameter("@VoteValue", voteValue));

            if (connection.State == ConnectionState.Closed)
                connection.Open();

            var result = command.ExecuteScalar();
            return result?.ToString() ?? "added";
        }
        public string VoteAnswer(int userId, int answerId,int QuestionId, int voteValue)
        {
            using var connection = _db.Database.GetDbConnection();
            using var command = connection.CreateCommand();

            command.CommandText = "VoteAnswer";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter("@UserId", userId));
            command.Parameters.Add(new SqlParameter("@AnswerId", answerId));
            command.Parameters.Add(new SqlParameter("@QuestionId",QuestionId));
            command.Parameters.Add(new SqlParameter("@VoteValue", voteValue));

            if (connection.State == ConnectionState.Closed)
                connection.Open();

            var result = command.ExecuteScalar();
            return result?.ToString() ?? "added";
        }
    }
}
