using System; 
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; 
using System.ComponentModel.DataAnnotations.Schema; 
namespace MvcMovie.Models
{
    public class Subject
    {
        [Key]   //主key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//自增列
        public int Id { get; set; }
        [Required]
        public string SubjectName { get; set; }
        public int TeacherID { get; set; }
        public int MaxPeople { get; set; }
        public DateTime CreateTime {get; set;}
        public DateTime UpdateTime {get; set;}
    }
}