using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StackOverFlowReplica.Context;
using StackOverFlowReplica.StackOverFlowReplica.Models;
using System.Data;

namespace StackOverFlowReplica.DAL.Repositories
{
    public class QuestionRepository
    {
        private readonly AppDbContext _db;
        public QuestionRepository(AppDbContext db)
        {
            _db = db;
        }

        public int CreateQuestion(Question q)
        {
            using var connection = _db.Database.GetDbConnection();
            using var command = connection.CreateCommand();

            command.CommandText = "CreateQuestion";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter("@Title", q.Title));
            command.Parameters.Add(new SqlParameter("@Description", q.Description));
            command.Parameters.Add(new SqlParameter("@UserId", q.UserId));

            if (connection.State == ConnectionState.Closed)
                connection.Open();

            var result = command.ExecuteScalar();
            return Convert.ToInt32(result);
        }

        public List<Question> GetAllQuestions()
        {
            var questions = new List<Question>();
            var connection = _db.Database.GetDbConnection();

            try
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "GetAllQuestionsWithOwner";
                    command.CommandType = CommandType.StoredProcedure;

                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            questions.Add(new Question
                            {
                                QuestionId = reader["QuestionId"] != DBNull.Value ? Convert.ToInt32(reader["QuestionId"]) : 0,
                                Title = reader["Title"]?.ToString() ?? "",
                                Description = reader["Description"]?.ToString() ?? "",
                                UserId = reader["UserId"] != DBNull.Value ? Convert.ToInt32(reader["UserId"]) : 0,
                                Views = reader["Views"] != DBNull.Value ? Convert.ToInt32(reader["Views"]) : 0,
                                CreatedDate = reader["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(reader["CreatedDate"]) : null,
                                UpdatedDate = reader["UpdatedDate"] != DBNull.Value ? Convert.ToDateTime(reader["UpdatedDate"]) : null,

                                // 🔥 Owner mapping
                                Owner = new User
                                {
                                    UserId = reader["OwnerUserId"] != DBNull.Value ? Convert.ToInt32(reader["OwnerUserId"]) : 0,
                                    Name = reader["OwnerName"]?.ToString() ?? "",
                                    Email = reader["OwnerEmail"]?.ToString() ?? "",
                                    RoleId = reader["OwnerRoleId"] != DBNull.Value ? Convert.ToInt32(reader["OwnerRoleId"]) : 0,
                                    Bio = reader["OwnerBio"]?.ToString() ?? "",
                                    isActive = reader["OwnerIsActive"] != DBNull.Value && Convert.ToBoolean(reader["OwnerIsActive"])
                                }
                            });
                        }
                    }
                }
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }

            return questions;
        }

        public Question? GetUserById(int questionId)
        {
            var connection = _db.Database.GetDbConnection();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "GetQuestionById"; // SP in DB
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@QuestionId", questionId));

                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Question
                        {
                            QuestionId = reader["QuestionId"] != DBNull.Value ? Convert.ToInt32(reader["QuestionId"]) : 0,
                            Title = reader["Title"].ToString() ?? "",
                            Description = reader["Description"].ToString() ?? "",
                            Views = reader["Views"] != DBNull.Value ? Convert.ToInt32(reader["Views"]) : 0,
                            UserId = reader["UserId"] != DBNull.Value ? Convert.ToInt32(reader["UserId"]) : 0,
                            UpdatedDate = reader["UpdatedDate"] != DBNull.Value ? Convert.ToDateTime(reader["UpdatedDate"]) : DateTime.MinValue
                        };
                    }
                }
                return null;

            }
        }
    }
}
