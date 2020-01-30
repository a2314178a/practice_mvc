using System; 
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; 
using System.ComponentModel.DataAnnotations.Schema; 
namespace MvcMovie.Models
{
    public class SubHomework
    {
        [Key]   //主key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//自增列
        public int Id { get; set; }
        [Required]
        public int SubjectID { get; set; }
        public string SubjectName { get; set; }
        public string HomeworkName { get; set; }
        public string HomeworkDetail { get; set; }
        public DateTime UploadDeadTime { get; set; }

        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}