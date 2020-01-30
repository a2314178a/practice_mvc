using System; 
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; 
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace MvcMovie.Models
{
    public class UploadModel
    {
        public IFormFile StudentFile { get; set; }
        public int SubHomeworkID { get; set; }
    }
}