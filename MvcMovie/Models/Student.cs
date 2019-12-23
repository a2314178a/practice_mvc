using System; 
using System.ComponentModel.DataAnnotations; 
using System.ComponentModel.DataAnnotations.Schema; 
namespace MvcMovie.Models
{
    public class Student
    {
        [Key]   //主key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//自增列
        public int Id { get; set; }
        [Required]
        public int StudentID { get; set; }
        public string Name { get; set; }
        [Required]
        public int AttachID { get; set; }
        public string Sex { get; set; }
        public int Age { get; set; }
        [DataType(DataType.Date)]
        public DateTime Birth { get; set; }
        public int AccountID { get; set; }
        public DateTime CreateTime {get; set;}
        public DateTime UpdateTime {get; set;}
    }
}