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
using Microsoft.AspNetCore.Http;

namespace MvcMovie.Controllers
{  
    public class SubjectController : Controller
    {
        public SubjectRepository Repository { get; }

        public SubjectController(SubjectRepository repository)
        {
            this.Repository = repository; 
        }

        public IActionResult Index(){
            return View();
        }



        public object query(){
            object subjectData = Repository.Query();
            return subjectData;
        }

        public object getAllSubject(){
            object subjectData = Repository.GetAllSubject();
            return subjectData;
        }

        

        public object getStuSelSub(int id){
            object subjectData = Repository.GetStuSelSub(id);
            return subjectData;
        }



        public bool chkCurrentUser(){
            string getTimeStamp = Repository.QueryTimeStamp(HttpContext.Session.GetInt32("loginID"));
            if(HttpContext.Session.GetString("loginTimeStamp") == getTimeStamp){
                return true;
            }else{
                return false;
            }
        }
    }

    
}
