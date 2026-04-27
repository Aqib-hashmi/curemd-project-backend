using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StackOverFlowReplica.Context;
using StackOverFlowReplica.Models.payloadModel;
using System.Data;

namespace StackOverFlowReplica.DAL.Repositories
{
    public class SearchRepository
    {
        private readonly AppDbContext _db;
        public SearchRepository(AppDbContext db)
        {
            _db = db;
        }


        public List<TagSearchDto> GetTagSuggestions(string searchText, int? userId = null)
        {
            var tags = new List<TagSearchDto>();

            using var connection = _db.Database.GetDbConnection();
            using var command = connection.CreateCommand();

            command.CommandText = "SearchTagSuggestions";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@SearchText", searchText));
            command.Parameters.Add(new SqlParameter("@UserId",(object?)userId ?? DBNull.Value));

            if (connection.State == ConnectionState.Closed)
                connection.Open();

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var sourceType = reader["SourceType"]?.ToString() ?? "tag";

                if (sourceType == "question")
                {
                    // Question title suggestion
                    tags.Add(new TagSearchDto
                    {
                        SourceType = "question",
                        QuestionId = reader["QuestionId"] == DBNull.Value? null : Convert.ToInt32(reader["QuestionId"]),
                        QuestionTitle = reader["QuestionTitle"]?.ToString(),
                        IsInHistory = false
                    });
                }
                else
                {
                    // Tag suggestion (existing same)
                    tags.Add(new TagSearchDto
                    {
                        TagId = reader["TagId"] == DBNull.Value? 0 : Convert.ToInt32(reader["TagId"]),
                        TagName = reader["TagName"]?.ToString() ?? "",
                        Description = reader["Description"]?.ToString() ?? "",
                        QuestionCount = reader["QuestionCount"] == DBNull.Value? 0 : Convert.ToInt32(reader["QuestionCount"]),
                        IsInHistory = Convert.ToInt32(reader["IsInHistory"]) == 1,
                        LastSearched = reader["LastSearched"] as DateTime?,
                        SourceType = "tag"
                    });
                }
            }
            return tags;
        }

        public List<TagSearchDto> GetUserSearchHistory(int userId)
        {
            var tags = new List<TagSearchDto>();

            using var connection = _db.Database.GetDbConnection();
            using var command = connection.CreateCommand();

            command.CommandText = "GetUserSearchHistory";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@UserId", userId));

            if (connection.State == ConnectionState.Closed)
                connection.Open();

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                tags.Add(new TagSearchDto
                {
                    TagId = Convert.ToInt32(reader["TagId"]),
                    TagName = reader["TagName"]?.ToString() ?? "",
                    Description = reader["Description"]?.ToString() ?? "",
                    QuestionCount = Convert.ToInt32(reader["QuestionCount"]),
                    IsInHistory = true,
                    LastSearched = reader["LastSearched"] as DateTime?
                });
            }

            return tags;
        }

        public void SaveSearchHistory(int userId, int tagId)
        {
            using var connection = _db.Database.GetDbConnection();
            using var command = connection.CreateCommand();

            command.CommandText = "SaveSearchHistory";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@UserId", userId));
            command.Parameters.Add(new SqlParameter("@TagId", tagId));

            if (connection.State == ConnectionState.Closed)
                connection.Open();

            command.ExecuteNonQuery();
        }
    }
}
