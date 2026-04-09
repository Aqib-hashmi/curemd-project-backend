namespace StackOverFlowReplica.Models.payloadModel
{
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

    public class UpdateQuestionDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }



}
