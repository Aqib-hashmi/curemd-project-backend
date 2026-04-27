using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StackOverFlowReplica.Context;
using System.Data;

namespace StackOverFlowReplica.DAL.Repositories
{
    public class AnswerRepository
    {
        private readonly AppDbContext _db;
        public AnswerRepository(AppDbContext db) { _db = db; }


        public object AddAnswer(string description, int userId, int questionId)
        {
            using var connection = _db.Database.GetDbConnection();
            using var command = connection.CreateCommand();
            command.CommandText = "AddAnswer";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@Description", description));
            command.Parameters.Add(new SqlParameter("@UserId", userId));
            command.Parameters.Add(new SqlParameter("@QuestionId", questionId));
            if (connection.State == ConnectionState.Closed) connection.Open();
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new
                {
                    answerId = Convert.ToInt32(reader["AnswerId"]),
                    description = reader["Description"]?.ToString(),
                    createdDate = reader["CreatedDate"],
                    isAccepted = Convert.ToBoolean(reader["IsAccepted"]),
                    voteCount = 0,
                    comments = new List<object>(),
                    owner = new
                    {
                        userId = Convert.ToInt32(reader["UserId"]),
                        name = reader["Name"]?.ToString(),
                        email = reader["Email"]?.ToString(),
                        bio = reader["Bio"]?.ToString()
                    }
                };
            }
            return null!;
        }

        public bool IsAnswerOwner(int answerId, int userId)
        {
            var connection = _db.Database.GetDbConnection();
            using var command = connection.CreateCommand();
            command.CommandText = "CheckAnswerOwner";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@AnswerId", answerId));
            command.Parameters.Add(new SqlParameter("@UserId", userId));
            if (connection.State == ConnectionState.Closed) connection.Open();
            var result = command.ExecuteScalar();
            connection.Close();
            return Convert.ToInt32(result) > 0;
        }

        public string EditAnswer(int answerId, string description, int userId, int RoleId)
        {
            using var connection = _db.Database.GetDbConnection();
            using var command = connection.CreateCommand();
            command.CommandText = "EditAnswer";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@AnswerId", answerId));
            command.Parameters.Add(new SqlParameter("@Description", description));
            command.Parameters.Add(new SqlParameter("@UserId", userId));
            command.Parameters.Add(new SqlParameter("@RoleId", RoleId));
            if (connection.State == ConnectionState.Closed) connection.Open();
            var result = command.ExecuteScalar();
            return result?.ToString() ?? "";
        }

        public string DeleteAnswer(int answerId, int userId,int RoleId)
        {
            using var connection = _db.Database.GetDbConnection();
            using var command = connection.CreateCommand();
            command.CommandText = "DeleteAnswer";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@AnswerId", answerId));
            command.Parameters.Add(new SqlParameter("@UserId", userId));
            command.Parameters.Add(new SqlParameter("@RoleId", RoleId));
            if (connection.State == ConnectionState.Closed) connection.Open();
            var result = command.ExecuteScalar();
            return result?.ToString() ?? "";
        }
    }
}
