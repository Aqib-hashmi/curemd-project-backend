using StackOverFlowReplica.Models.payloadModel;
using System.ComponentModel.DataAnnotations;

namespace StackOverFlowReplica.StackOverFlowReplica.Models
{
    public class User
    {
        public int UserId { get; set; }// PK
        public required string Name { get; set; }    
        public required string Email { get; set; }
       public  string? Password { get; set; }      
        public int RoleId { get; set; }
        public required string Bio { get; set; }          
        public bool isActive { get; set; }        
        public int isActiveBy { get; set; }      
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }

    public class ChangeUserStatusRequest
    {
        public int AdminId { get; set; }
        public int TargetUserId { get; set; }
        public bool IsActive { get; set; }
    }

    public class Question
    {
        public int QuestionId { get; set; }     
        public required  string Title { get; set; } 
        public required string Description { get; set; }
        public int UserId { get; set; } 
        public int Views { get; set; }  
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public User? Owner { get; set; }
    }
}
