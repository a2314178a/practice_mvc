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
        public string Age { get; set; }
        public string Birth { get; set; }
        public string Subject { get; set; }
    }
}