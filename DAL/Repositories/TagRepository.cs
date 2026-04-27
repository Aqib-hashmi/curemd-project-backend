using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StackOverFlowReplica.Context;
using StackOverFlowReplica.Models.payloadModel;
using StackOverFlowReplica.StackOverFlowReplica.Models;
using System.Data;

namespace StackOverFlowReplica.DAL.Repositories
{
    public class TagRepository
    {
        private readonly AppDbContext _db;
        public TagRepository(AppDbContext db)
        {
            _db = db;
        }

        public List<Tag> GetAllTags()
        {
            var tags = new List<Tag>();
            var connection = _db.Database.GetDbConnection();

            try
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "GetAllTags";
                    command.CommandType = CommandType.StoredProcedure;

                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tags.Add(new Tag
                            {
                                TagId = reader["TagId"] != DBNull.Value ? Convert.ToInt32(reader["TagId"]) : 0,
                                TagName = reader["TagName"]?.ToString() ?? "",
                                Description = reader["Description"]?.ToString() ?? "",
                                CreatedDate = reader["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(reader["CreatedDate"]) : null,
                                UpdatedDate = reader["UpdatedDate"] != DBNull.Value ? Convert.ToDateTime(reader["UpdatedDate"]) : null,
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

            return tags;
        }

        public Tag CreateTag(Tag tag)
        {
            using (var connection = _db.Database.GetDbConnection())
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "CreateTag";
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@TagName", tag.TagName));
                    command.Parameters.Add(new SqlParameter("@Description", (object?)tag.Description ?? DBNull.Value));

                    var result = command.ExecuteScalar();

                    tag.TagId = Convert.ToInt32(result);

                    return tag;
                }
            }
        }

        public List<TagSuggestionDto> SearchTagSuggestions(string search)
        {
            var tags = new List<TagSuggestionDto>();

            using (var connection = _db.Database.GetDbConnection())
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SearchTagSuggestions";
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@Search", search));

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tags.Add(new TagSuggestionDto
                            {
                                TagId = reader["TagId"] != DBNull.Value ? Convert.ToInt32(reader["TagId"]) : 0,
                                TagName = reader["TagName"].ToString()?? ""
                            });
                        }
                    }
                }
            }

            return tags;
        }
    }
}
