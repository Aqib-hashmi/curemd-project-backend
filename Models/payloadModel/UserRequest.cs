using StackOverFlowReplica.StackOverFlowReplica.Models;

namespace StackOverFlowReplica.Models.payloadModel
{
    public class UpdateUserDto
    {
        public int UserId { get; set; }
        public required string Name { get; set; }
        public required string Bio { get; set; }
    }
    public class UserRequest
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

    public class RegisterRequest
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Bio { get; set; }

    }

    public class CreateQuestion
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required List<int> TagIds { get; set; }
    }

    public class QuestionResponseDTO
    {
        public int QuestionId { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public int Views { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public QuestionOwnerDTO? Owner { get; set; }
    }

    public class QuestionOwnerDTO
    {
        public int UserId { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }

        public required string Password { get; set; }
        public int RoleId { get; set; }
        public required string Bio { get; set; }
        public bool isActive { get; set; }
    }

    public class QuestionDetailDto
    {
        public int QuestionId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }

        public int Views { get; set; }
        public int VoteCount { get; set; }
        public int AnswerCount { get; set; }
        public DateTime? CreateDate { get; set; }

        public VoteDto? Votes { get; set; }

        public UserDto? Owner { get; set; }

        public List<CommentDto> Comments { get; set; } = new();

        public List<TagDto> Tags { get; set; } = new();

        public List<AnswerDto> Answers { get; set; } = new();
    }
    public class UserDto
    {
        public int UserId { get; set; }
        public required string Name { get; set; }
        public string? Email { get; set; }
        public string? Bio { get; set; }
    }
    public class TagDto
    {
        public int TagId { get; set; }
        public required string TagName { get; set; }
        public required string Description { get; set; }
    }
    public class AnswerDto
    {
        public int AnswerId { get; set; }

        public required string Description { get; set; }

        public DateTime? CreatedDate { get; set; }
        public int VoteCount { set; get; }

        public UserDto? Owner { get; set; }

        public List<CommentDto> Comments { get; set; } = new();

        public VoteDto? Votes { get; set; }
    }
    public class CommentDto
    {
        public int CommentId { get; set; }

        public required string Content { get; set; }

        public DateTime? CreatedDate { get; set; }

        public UserDto? Owner { get; set; }
    }
    public class VoteDto
    {
        public int VoteCount { get; set; }
        public bool IsUpVotedByUser { get; set; }
        public bool IsDownVotedByUser { get; set; }
    }


    public class UpdateQuestionDto
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required List<int> TagIds { get; set; }
    }

    // DTOs/VoteDto.cs
    public class VoteQuestionDto
    {
        public int QuestionId { get; set; }
        public int VoteValue { get; set; }  // 1 ya -1
    }

    public class VoteAnswerDto
    {
        public int AnswerId { get; set; }
        public int QuestionId { get; set; }
        public int VoteValue { get; set; }  // 1 ya -1
    }
    public class AddViewDto
    {
        public int QuestionId { get; set; }
    }

    public class CreateTag
    {
        public required string TagName { get; set; }
        public string? Description { get; set; }
    }
    public class TagSuggestionDto
    {
        public int TagId { get; set; }
        public string? TagName { get; set; }
    }


    public class TagSearchDto
    {
        public int TagId { get; set; }
        public  string? TagName { get; set; }
        public string? Description { get; set; }
        public int QuestionCount { get; set; }
        public bool IsInHistory { get; set; }
        public DateTime? LastSearched { get; set; }

        public string SourceType { get; set; } = "tag";  // "tag" | "question"
        public int? QuestionId { get; set; }
        public string? QuestionTitle { get; set; }
    }

    // DTOs/SaveSearchHistoryDto.cs
    public class SaveSearchHistoryDto
    {
        public int TagId { get; set; }
    }

    // DTOs/CommentDto.cs
    public class AddQuestionCommentDto
    {
        public required string Content { get; set; }
        public int QuestionId { get; set; }
    }

    public class AddAnswerCommentDto
    {
        public required string Content { get; set; }
        public int AnswerId { get; set; }
    }

    public class EditCommentDto
    {
        public required string Content { get; set; }
    }

    // DTOs/AnswerDto.cs
    public class AddAnswerDto
    {
        public required string Description { get; set; }
        public int QuestionId { get; set; }
    }

    public class EditAnswerDto
    {
        public required string Description { get; set; }
    }


}
