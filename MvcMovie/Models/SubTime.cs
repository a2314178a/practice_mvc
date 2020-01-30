using System; 
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; 
using System.ComponentModel.DataAnnotations.Schema; 
namespace MvcMovie.Models
{
    public class SubTime
    {
        [Key]   //主key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//自增列
        public int Id { get; set; }
        [Required]
        public int SubjectID { get; set; }
        public string WeekDay { get; set; }
        [Column(TypeName="time")]
        public TimeSpan StartTime { get; set; }
        [Column(TypeName="time")]
        public TimeSpan EndTime { get; set; }
        public DateTime CreateTime {get; set;}
        public DateTime UpdateTime {get; set;}
    }
}