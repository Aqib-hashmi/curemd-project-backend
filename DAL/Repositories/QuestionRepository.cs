using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StackOverFlowReplica.Context;
using StackOverFlowReplica.Models.payloadModel;
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

        //public int CreateQuestion(Question q)
        //{
        //    using var connection = _db.Database.GetDbConnection();
        //    using var command = connection.CreateCommand();

        //    command.CommandText = "CreateQuestion";
        //    command.CommandType = CommandType.StoredProcedure;

        //    command.Parameters.Add(new SqlParameter("@Title", q.Title));
        //    command.Parameters.Add(new SqlParameter("@Description", q.Description));
        //    command.Parameters.Add(new SqlParameter("@UserId", q.UserId));

        //    if (connection.State == ConnectionState.Closed)
        //        connection.Open();

        //    var result = command.ExecuteScalar();
        //    return Convert.ToInt32(result);
        //}
        //public void AddQuestionTag(int questionId, int tagId)
        //{
        //    var qt = new QuestionTag
        //    {
        //        QuestionId = questionId,
        //        TagId = tagId
        //    };
        //}

        public int CreateQuestion(string title, string description, int userId, string tagIds)
        {
            var connection = _db.Database.GetDbConnection();
            using var command = connection.CreateCommand();

            command.CommandText = "sp_CreateQuestionWithTags";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter("@Title", title));
            command.Parameters.Add(new SqlParameter("@Description", description));
            command.Parameters.Add(new SqlParameter("@UserId", userId));
            command.Parameters.Add(new SqlParameter("@TagIds", tagIds));

            connection.Open();

            var result = command.ExecuteScalar();

            connection.Close();

            return Convert.ToInt32(result);
        }

        //public List<Question> GetAllQuestions()
        //{
        //    var dict = new Dictionary<int, Question>();

        //    var connection = _db.Database.GetDbConnection();

        //    using var command = connection.CreateCommand();
        //    command.CommandText = "GetAllQuestionsWithOwnerAndTags";
        //    command.CommandType = CommandType.StoredProcedure;

        //    if (connection.State == ConnectionState.Closed)
        //        connection.Open();

        //    using var reader = command.ExecuteReader();

        //    while (reader.Read())
        //    {
        //        int qId = Convert.ToInt32(reader["QuestionId"]);

        //        if (!dict.ContainsKey(qId))
        //        {
        //            dict[qId] = new Question
        //            {
        //                QuestionId = qId,
        //                Title = reader["Title"]?.ToString() ?? "",
        //                Description = reader["Description"]?.ToString() ?? "",
        //                UserId = Convert.ToInt32(reader["UserId"]),
        //                ViewCount = reader["Views"] != DBNull.Value ? Convert.ToInt32(reader["Views"]) : 0,
        //                VoteCount = reader["VoteCount"] != DBNull.Value ? Convert.ToInt32(reader["VoteCount"]) : 0,
        //                AnswerCount = reader["AnswerCount"] != DBNull.Value ? Convert.ToInt32(reader["AnswerCount"]) : 0,
        //                CreatedDate = reader["CreatedDate"] as DateTime?,
        //                UpdatedDate = reader["UpdatedDate"] as DateTime?,

        //                Owner = new User
        //                {
        //                    UserId = Convert.ToInt32(reader["OwnerUserId"]),
        //                    Name = reader["OwnerName"]?.ToString() ?? ""   ,
        //                    Email = reader["OwnerEmail"]?.ToString() ?? "",
        //                    RoleId = Convert.ToInt32(reader["OwnerRoleId"]),
        //                    Bio = reader["OwnerBio"]?.ToString() ?? "",
        //                    isActive = Convert.ToBoolean(reader["OwnerIsActive"])
        //                },

        //                Tags = new List<Tag>()
        //            };
        //        }

        //        // 🔥 ADD TAGS
        //        if (reader["TagId"] != DBNull.Value)
        //        {
        //            dict[qId].Tags.Add(new Tag
        //            {
        //                TagId = reader["TagId"] != DBNull.Value ? Convert.ToInt32(reader["TagId"]) : 0,
        //                TagName = reader["TagName"]?.ToString() ?? "",
        //                Description = reader["TagDescription"]?.ToString() ?? ""    
        //            });
        //        }
        //    }

        //    return dict.Values.ToList();
        //}

        // tagIds = null → sab questions
        // tagIds = "1,2,3" → filtered questions
        public List<Question> GetAllQuestions(int? userId, int pageNumber, int pageSize,string? tagName)
        {
            var dict = new Dictionary<int, Question>();

              using var connection = _db.Database.GetDbConnection();
              var command = connection.CreateCommand();

            command.CommandText = "GetAllQuestionsWithOwnerAndTags";
            command.CommandType = CommandType.StoredProcedure;

            var param1 = command.CreateParameter(); param1.ParameterName = "@UserId";
            param1.Value = userId;
            command.Parameters.Add(param1);

            var param2 = command.CreateParameter(); param2.ParameterName = "@PageNumber";
            param2.Value = pageNumber;
            command.Parameters.Add(param2);

            var param3 = command.CreateParameter();
            param3.ParameterName = "@PageSize";
            param3.Value = pageSize;
            command.Parameters.Add(param3);

            var param4 = command.CreateParameter();
            param4.ParameterName="@tagName";
            param4.Value = (object?)tagName ?? DBNull.Value;
            command.Parameters.Add(param4);

            if (connection.State != ConnectionState.Open)
                connection.Open();

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                int qId = Convert.ToInt32(reader["QuestionId"]);

                if (!dict.ContainsKey(qId))
                {
                    dict[qId] = new Question
                    {
                        QuestionId = qId,
                        Title = reader["Title"]?.ToString() ?? "",
                        Description = reader["Description"]?.ToString() ?? "",
                        UserId = Convert.ToInt32(reader["UserId"]),
                        ViewCount = reader["Views"] != DBNull.Value ? Convert.ToInt32(reader["Views"]) : 0,
                        VoteCount = reader["VoteCount"] != DBNull.Value ? Convert.ToInt32(reader["VoteCount"]) : 0,
                        AnswerCount = reader["AnswerCount"] != DBNull.Value ? Convert.ToInt32(reader["AnswerCount"]) : 0,
                        CreatedDate = reader["CreatedDate"] as DateTime?,
                        UpdatedDate = reader["UpdatedDate"] as DateTime?,
                        Owner = new User
                        {
                            UserId = Convert.ToInt32(reader["OwnerUserId"]),
                            Name = reader["OwnerName"]?.ToString() ?? "",
                            Email = reader["OwnerEmail"]?.ToString() ?? "",
                            RoleId = Convert.ToInt32(reader["OwnerRoleId"]),
                            Bio = reader["OwnerBio"]?.ToString() ?? "",
                            isActive = Convert.ToBoolean(reader["OwnerIsActive"])
                        },
                        Tags = new List<Tag>()
                    };
                }

                if (reader["TagId"] != DBNull.Value)
                {
                    int tagId = Convert.ToInt32(reader["TagId"]);

                    if (!dict[qId].Tags.Any(t => t.TagId == tagId))
                    {
                        dict[qId].Tags.Add(new Tag
                        {
                            TagId = tagId,
                            TagName = reader["TagName"].ToString()?? "",
                            Description = reader["TagDescription"].ToString()??""
                        });
                    }
                }
            }

            return dict.Values.ToList();
        }

        //public List<Tag> GetUserTags(int userId)
        //{
        //    var tags = new List<Tag>();

        //   using var connection = _db.Database.GetDbConnection();
        //   using var command = connection.CreateCommand();

        //    command.CommandText = "GetUserTags";
        //    command.CommandType = CommandType.StoredProcedure;

        //    command.Parameters.Add(new SqlParameter("@UserId", userId));

        //    if (connection.State != ConnectionState.Open)
        //        connection.Open();

        //    var reader = command.ExecuteReader();

        //    while (reader.Read())
        //    {
        //        tags.Add(new Tag
        //        {
        //            TagId = Convert.ToInt32(reader["TagId"]),
        //            TagName = reader["TagName"]?.ToString()?? "",
        //        });
        //    }

        //    return tags;
        //}


        public string AddQuestionView(int questionId, int userId)
        {
            using var connection = _db.Database.GetDbConnection();
            using var command = connection.CreateCommand();

            command.CommandText = "AddQuestionView";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter("@QuestionId", questionId));
            command.Parameters.Add(new SqlParameter("@UserId", userId));

            if (connection.State == ConnectionState.Closed)
                connection.Open();

            var result = command.ExecuteScalar();
            return result?.ToString() ?? "added";
        }

        public QuestionDetailDto GetQuestionDetail(int questionId)
        {
            var question = new QuestionDetailDto
            {
                Tags = new List<TagDto>(),
                Answers = new List<AnswerDto>(),
                Comments = new List<CommentDto>()
            };

            var connection = _db.Database.GetDbConnection();
            var command = connection.CreateCommand();


            command.CommandText = "GetQuestionDetail";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@QuestionId", questionId));

            if (connection.State == ConnectionState.Closed)
                connection.Open();

            using var reader = command.ExecuteReader();

            // =====================================================
            // 1️⃣ QUESTION
            // =====================================================
            if (reader.Read())
            {
                question.QuestionId = Convert.ToInt32(reader["QuestionId"]);
                question.Title = reader["Title"]?.ToString() ?? "";
                question.Description = reader["Description"]?.ToString() ?? "";
                question.CreateDate = reader["CreatedDate"] as DateTime?;
                question.Views = Convert.ToInt32(reader["Views"]);
                question.VoteCount = Convert.ToInt32(reader["VoteCount"]);
                question.AnswerCount = Convert.ToInt32(reader["AnswerCount"]);

                question.Owner = new UserDto
                {
                    UserId = Convert.ToInt32(reader["OwnerUserId"]),
                    Name = reader["OwnerName"]?.ToString() ?? "",
                    Email = reader["OwnerEmail"]?.ToString() ?? "",
                    Bio = reader["OwnerBio"]?.ToString() ?? ""
                };
            }

            // =====================================================
            // 2️⃣ QUESTION COMMENTS
            // =====================================================
            reader.NextResult();

            while (reader.Read())
            {
                question.Comments.Add(new CommentDto
                {
                    CommentId = Convert.ToInt32(reader["CommentId"]),
                    Content = reader["Content"]?.ToString() ?? "",
                    CreatedDate = reader["CreatedDate"] as DateTime?,

                    Owner = new UserDto
                    {
                        UserId = Convert.ToInt32(reader["UserId"]),
                        Name = reader["Name"]?.ToString() ?? "",
                        Email = reader["Email"]?.ToString() ?? "",
                        Bio = reader["Bio"]?.ToString() ?? ""
                    }
                });
            }

            // =====================================================
            // 3️⃣ TAGS
            // =====================================================
            reader.NextResult();

            while (reader.Read())
            {
                question.Tags.Add(new TagDto
                {
                    TagId = Convert.ToInt32(reader["TagId"]),
                    TagName = reader["TagName"]?.ToString() ?? "",
                    Description = reader["Description"]?.ToString() ?? ""
                });
            }

            // =====================================================
            // 4️⃣ ANSWERS
            // =====================================================
            reader.NextResult();

            while (reader.Read())
            {
                question.Answers.Add(new AnswerDto
                {
                    AnswerId = Convert.ToInt32(reader["AnswerId"]),
                    Description = reader["Description"]?.ToString() ?? "",
                    CreatedDate = reader["CreatedDate"] as DateTime?,
                    VoteCount = reader["VoteCount"] != DBNull.Value? Convert.ToInt32(reader["VoteCount"]) : 0,

                    Owner = new UserDto
                    {
                        UserId = Convert.ToInt32(reader["UserId"]),
                        Name = reader["Name"]?.ToString() ?? "",
                        Email = reader["Email"]?.ToString() ?? "",
                        Bio = reader["Bio"]?.ToString() ?? ""
                    },

                    Comments = new List<CommentDto>() // ⭐ IMPORTANT
                });
            }

            // =====================================================
            // 5️⃣ ANSWER COMMENTS
            // =====================================================
            reader.NextResult();

            while (reader.Read())
            {
                var answerId = Convert.ToInt32(reader["AnswerId"]);

                var answer = question.Answers.FirstOrDefault(a => a.AnswerId == answerId);

                if (answer != null)
                {
                    answer.Comments.Add(new CommentDto
                    {
                        CommentId = Convert.ToInt32(reader["CommentId"]),
                        Content = reader["Content"]?.ToString() ?? "",
                        CreatedDate = reader["CreatedDate"] as DateTime?,

                        Owner = new UserDto
                        {
                            UserId = Convert.ToInt32(reader["UserId"]),
                            Name = reader["Name"]?.ToString() ?? "",
                            Email = reader["Email"]?.ToString() ?? "",
                            Bio = reader["Bio"]?.ToString() ?? ""
                        }
                    });
                }
            }

            return question;
        }



        public bool UpdateQuestion(int questionId, UpdateQuestionDto dto)
        {
            var connection = _db.Database.GetDbConnection();
            using var command = connection.CreateCommand();

            command.CommandText = "UpdateQuestion";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter("@QuestionId", questionId));
            command.Parameters.Add(new SqlParameter("@Title", dto.Title));
            command.Parameters.Add(new SqlParameter("@Description", dto.Description));
            command.Parameters.Add(new SqlParameter("@TagIds", string.Join(",", dto.TagIds)));

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();

            return true;
        }

        public bool IsQuestionOwner(int questionId, int userId)
        {
            var connection = _db.Database.GetDbConnection();
            using var command = connection.CreateCommand();

            command.CommandText = "CheckQuestionOwner";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter("@QuestionId", questionId));
            command.Parameters.Add(new SqlParameter("@UserId", userId));

            connection.Open();
            var result = command.ExecuteScalar();
            connection.Close();

            return Convert.ToInt32(result) > 0;
        }

        public bool deleteQuestion(int questionId,int userId)
        {
            var connection = _db.Database.GetDbConnection();
            using var command = connection.CreateCommand();

            command.CommandText = "deleteQuestion";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter("@QuestionId", questionId));
            command.Parameters.Add(new SqlParameter("@UserId", userId));

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();

            return true;
        }


        //public List<QuestionDto> SearchQuestionsByTag(int tagId)
        //{
        //    var list = new List<QuestionDto>();

        //    using (var connection = _db.Database.GetDbConnection())
        //    {
        //        connection.Open();

        //        using (var command = connection.CreateCommand())
        //        {
        //            command.CommandText = "SearchQuestionsByTag";
        //            command.CommandType = CommandType.StoredProcedure;

        //            command.Parameters.Add(new SqlParameter("@TagId", tagId));

        //            using (var reader = command.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    list.Add(new QuestionDto
        //                    {
        //                        QuestionId = Convert.ToInt32(reader["QuestionId"]),
        //                        Title = reader["Title"].ToString(),
        //                        Description = reader["Description"].ToString(),
        //                        VoteCount = Convert.ToInt32(reader["VoteCount"]),
        //                        Views = Convert.ToInt32(reader["Views"])
        //                    });
        //                }
        //            }
        //        }
        //    }

        //    return list;
        //}


        //public bool IsOwnerOrAdmin(int questionId, int userId, string role)
        //    {
        //        if (role == "Admin") return true;

        //        using (SqlConnection con = new SqlConnection(_connectionString))
        //        {
        //            con.Open();

        //            SqlCommand cmd = new SqlCommand("SP_CheckQuestionOwner", con);
        //            cmd.CommandType = CommandType.StoredProcedure;

        //            cmd.Parameters.AddWithValue("@QuestionId", questionId);
        //            cmd.Parameters.AddWithValue("@UserId", userId);

        //            int count = (int)cmd.ExecuteScalar();
        //            return count > 0;
        //        }
        //    }

        //public Question? GetUserById(int questionId)
        //{
        //    var connection = _db.Database.GetDbConnection();

        //    using (var command = connection.CreateCommand())
        //    {
        //        command.CommandText = "GetQuestionById"; // SP in DB
        //        command.CommandType = CommandType.StoredProcedure;

        //        command.Parameters.Add(new SqlParameter("@QuestionId", questionId));

        //        if (connection.State == ConnectionState.Closed)
        //            connection.Open();

        //        using (var reader = command.ExecuteReader())
        //        {
        //            if (reader.Read())
        //            {
        //                return new Question
        //                {
        //                    QuestionId = reader["QuestionId"] != DBNull.Value ? Convert.ToInt32(reader["QuestionId"]) : 0,
        //                    Title = reader["Title"].ToString() ?? "",
        //                    Description = reader["Description"].ToString() ?? "",
        //                    Views = reader["Views"] != DBNull.Value ? Convert.ToInt32(reader["Views"]) : 0,
        //                    UserId = reader["UserId"] != DBNull.Value ? Convert.ToInt32(reader["UserId"]) : 0,
        //                    UpdatedDate = reader["UpdatedDate"] != DBNull.Value ? Convert.ToDateTime(reader["UpdatedDate"]) : DateTime.MinValue
        //                };
        //            }
        //        }
        //        return null;

        //    }
        //}
    }
}
