using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CodeTale.Models
{
    public class Algorithm
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AlgorithmId { get; set; }

        [Required]
        public string AlgorithmName { get; set; }
        public string AlgorithmDetails { get; set; }

        [DataType(DataType.MultilineText)]
        public string Code { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}