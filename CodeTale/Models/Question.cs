using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CodeTale.Models
{
    public class Question
    {
        [Key]
        public int QuestionId { get; set; }
        public string QuestionStatement { get; set; }

        

        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        public int? UserID { get; set; }
        [ForeignKey("UserID")]
        public virtual User User { get; set; }



        public ICollection<Answer> Answers { get; set; }
    }
}