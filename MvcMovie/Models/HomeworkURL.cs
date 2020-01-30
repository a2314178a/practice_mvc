using System; 
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; 
using System.ComponentModel.DataAnnotations.Schema; 
namespace MvcMovie.Models
{
    public class HomeworkURL
    {
        [Key]   //主key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//自增列
        public int Id { get; set; }
        [Required]
        public int SubHomeworkID { get; set; }  
        public int accountID {get; set;}
        public string URL { get; set; }
        public string OriginalName {get; set;}
        public string NewName {get; set;}

        public DateTime CreateTime {get; set;}
        public DateTime UpdateTime {get; set;}
    }
}