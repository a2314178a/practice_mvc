using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations; 
using System.ComponentModel.DataAnnotations.Schema; 
namespace MvcMovie.Models
{
    public class Stu_sub
    {
        [Key]   //主key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//自增列
        public int Id { get; set; }
        public int StudentID { get; set; }
        public int SubjectID { get; set; }
        public int Status { get; set; }
        public int VerifyAccID { get; set; }
        public DateTime CreateTime {get; set;}
        public DateTime UpdateTime {get; set;}
    }
}