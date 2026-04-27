using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StackOverFlowReplica.Context;
using System.Data;

namespace StackOverFlowReplica.DAL.Repositories
{
    // Repositories/CommentRepository.cs
    public class CommentRepository
    {
        private readonly AppDbContext _db;
        public CommentRepository(AppDbContext db) 
        {
            _db = db; 
        }

        public bool IsAnswerCommentOwner(int commentId, int userId)
        {
            var connection = _db.Database.GetDbConnection();
            using var command = connection.CreateCommand();
            command.CommandText = "SP_CheckAnswerCommentOwner";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@CommentId", commentId));
            command.Parameters.Add(new SqlParameter("@UserId", userId));
            if (connection.State == ConnectionState.Closed) connection.Open();
            var result = command.ExecuteScalar();
            connection.Close();
            return Convert.ToInt32(result) > 0;
        }

        public bool IsQuestionCommentOwner(int commentId, int userId)
        {
            var connection = _db.Database.GetDbConnection();
            using var command = connection.CreateCommand();
            command.CommandText = "SP_CheckQuestionCommentOwner";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@CommentId", commentId));
            command.Parameters.Add(new SqlParameter("@UserId", userId));
            if (connection.State == ConnectionState.Closed) connection.Open();
            var result = command.ExecuteScalar();
            connection.Close();
            return Convert.ToInt32(result) > 0;
        }

        public object AddQuestionComment(string content, int userId, int questionId)
        {
            using var connection = _db.Database.GetDbConnection();
            using var command = connection.CreateCommand();
            command.CommandText = "AddQuestionComment";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@Content", content));
            command.Parameters.Add(new SqlParameter("@UserId", userId));
            command.Parameters.Add(new SqlParameter("@QuestionId", questionId));

            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new
                {
                    commentId = Convert.ToInt32(reader["CommentId"]),
                    content = reader["Content"]?.ToString(),
                    createdDate = reader["CreatedDate"],
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

        public string EditQuestionComment(int commentId, string content, int userId,int RoleId)
        {
            using var connection = _db.Database.GetDbConnection();
            using var command = connection.CreateCommand();
            command.CommandText = "EditQuestionComment";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@CommentId", commentId));
            command.Parameters.Add(new SqlParameter("@Content", content));
            command.Parameters.Add(new SqlParameter("@UserId", userId));
            command.Parameters.Add(new SqlParameter("@RoleId", RoleId));

            if (connection.State == ConnectionState.Closed) connection.Open();
            var result = command.ExecuteScalar();
            return result?.ToString() ?? "";
        }

        public string DeleteQuestionComment(int commentId, int userId, int RoleId)
        {
            using var connection = _db.Database.GetDbConnection();
            using var command = connection.CreateCommand();
            command.CommandText = "DeleteQuestionComment";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@CommentId", commentId));
            command.Parameters.Add(new SqlParameter("@UserId", userId));
            command.Parameters.Add(new SqlParameter("@RoleId", RoleId));
            if (connection.State == ConnectionState.Closed) connection.Open();
            var result = command.ExecuteScalar();
            return result?.ToString() ?? "";
        }

        public object AddAnswerComment(string content, int userId, int answerId)
        {
            using var connection = _db.Database.GetDbConnection();
            using var command = connection.CreateCommand();
            command.CommandText = "AddAnswerComment";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@Content", content));
            command.Parameters.Add(new SqlParameter("@UserId", userId));
            command.Parameters.Add(new SqlParameter("@AnswerId", answerId));
            if (connection.State == ConnectionState.Closed) connection.Open();
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new
                {
                    commentId = Convert.ToInt32(reader["CommentId"]),
                    content = reader["Content"]?.ToString(),
                    createdDate = reader["CreatedDate"],
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

        public string EditAnswerComment(int commentId, string content, int userId,int RoleId)
        {
            using var connection = _db.Database.GetDbConnection();
            using var command = connection.CreateCommand();
            command.CommandText = "SP_EditAnswerComment";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@CommentId", commentId));
            command.Parameters.Add(new SqlParameter("@Content", content));
            command.Parameters.Add(new SqlParameter("@UserId", userId));
            command.Parameters.Add(new SqlParameter("@RoleId", RoleId));
            if (connection.State == ConnectionState.Closed) connection.Open();
            var result = command.ExecuteScalar();
            return result?.ToString() ?? "";
        }

        public string DeleteAnswerComment(int commentId, int userId,int RoleId)
        {
            using var connection = _db.Database.GetDbConnection();
            using var command = connection.CreateCommand();
            command.CommandText = "SP_DeleteAnswerComment";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@CommentId", commentId));
            command.Parameters.Add(new SqlParameter("@UserId", userId));
            command.Parameters.Add(new SqlParameter("@RoleId", RoleId));
            if (connection.State == ConnectionState.Closed) connection.Open();
            var result = command.ExecuteScalar();
            return result?.ToString() ?? "";
        }
    }
}
