using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MvcMovie.Models;
using MvcMovie.Repositories;
using System.Text.Json;

namespace MvcMovie.Controllers
{  
    public class CURDController : Controller
    {
        public DbSqlRepository Repository { get; }

        public CURDController(DbSqlRepository repository)
        {
            this.Repository = repository;
        }

        public IActionResult Index(){
            return View();
        }


        public List<Student> query(){
            List<Student> studentData = Repository.Query();
            return studentData;
        }

        public List<Student> query_option(string filterOption, string filterWord){            
            List<Student> studentData = Repository.query_option(filterOption ,filterWord);
            return studentData;
        }

        [HttpPost]
        public int  create([FromForm] Student tmp){   
            return Repository.Add(tmp); 
        }

        [HttpPost]
        public int update([FromForm] Student tmp){           
            return Repository.Update(tmp);
        }

        [HttpPost]
        public int del([FromForm] int id){                    
            return Repository.Delete(id);
        }
    }

    
}
