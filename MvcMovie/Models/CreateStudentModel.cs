using System; 
using System.ComponentModel.DataAnnotations; 
using System.ComponentModel.DataAnnotations.Schema; 
namespace MvcMovie.Models
{
    public class CreateStudentModel
    {             
        public int newSid { get; set; }
        public string newName { get; set; }
        public string newSex { get; set; }
    }
}