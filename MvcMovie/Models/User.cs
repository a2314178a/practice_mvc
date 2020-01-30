using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations; 
using System.ComponentModel.DataAnnotations.Schema; 
namespace MvcMovie.Models
{
    public class User
    {
        [Key]   //主key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//自增列
        public int Id { get; set; }
        [Required]
        public string Account { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string UserName { get; set; }
        public int Authority { get; set; } 
        public DateTime CreateTime {get; set;}
        public DateTime UpdateTime {get; set;}
        public string loginTime {get; set;}
        //public int DetailID { get; set; }
    
    }
}