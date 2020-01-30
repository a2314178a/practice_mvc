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
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Web;

namespace MvcMovie.Controllers
{  
    public class StudentController : Controller
    {
        public StudentRepository Repository { get; }
        private readonly string _folder;
        private readonly string upFolder = "UploadFolder";

        public StudentController(StudentRepository repository, IWebHostEnvironment env)
        {
            this.Repository = repository; 
            // 目錄為：wwwroot
            _folder = $@"{env.WebRootPath}";
        }



        public IActionResult Index(){
            return View();
        }

        public IActionResult MySubject(string page){
            if(!isLoginInfo()){
                return logOut();
            }
            ViewBag.showPage = page;
            ViewBag.Authority = 3;
            ViewBag.Auth = "Y";
            return View("~/Views/Account/studentPage.cshtml");
        }


        public List<Student> query(){
            List<Student> studentData = Repository.Query(HttpContext.Session.GetInt32("loginID"));
            return studentData;
        }

      

        public int stuDelSub(int studentID, int subjectID){
            if(!chkCurrentUser()){
                return -2;
            }
            return Repository.StuDelSub(studentID, subjectID);        
        }

        public int addSelSub(int studentID, int subjectID){
            if(!chkCurrentUser()){
                return -2;
            }
            if(chkHaveSameTime(studentID, subjectID)){
                 return -1;
            }
            Stu_sub data = new Stu_sub();
            data.StudentID = studentID;
            data.SubjectID = subjectID;
            data.Status = 0;
            data.CreateTime = DateTime.Now; 
            return Repository.AddSelSub(data);  
        }
       

        public object getVerifySubject(){
            return Repository.GetVerifySubject((int)HttpContext.Session.GetInt32("loginID"));
        }

        public bool chkHaveSameTime(int studentID, int subjectID){
            var hasSameTime = false;
            var selSub = new List<SubTime>();
            selSub = Repository.getAllSelSubjectTime(studentID);
            var addSub = new List<SubTime>();
            addSub = Repository.getAddSubjectTime(subjectID);
            foreach(SubTime sel in selSub)
            {
                foreach(SubTime add in addSub){
                    if(add.WeekDay == sel.WeekDay)
                    {
                        if(add.StartTime < sel.StartTime){
                            if(add.EndTime > sel.StartTime){
                                hasSameTime = true;
                            }
                        }else{
                            if(add.StartTime < sel.EndTime){
                                hasSameTime = true;
                            }
                        }
                    }
                }
            }
            return hasSameTime;
        }


        public object getThisSubHomework(int subID){
            int accID = (int)HttpContext.Session.GetInt32("loginID");
            return Repository.GetThisSubHomework(accID, subID);
        }

        public int uploadFile(UploadModel formData){
            var formFile = formData.StudentFile;
            var hwID = formData.SubHomeworkID;
            var accId = (int)HttpContext.Session.GetInt32("loginID");

            if (formFile == null || formFile.Length == 0 || !chkExtName(formFile) || hwID <= 0){
                return 0;
            }
            object sub_hw_name = Repository.GetSubHwName(hwID);

            var uploadDetail = new {
                sub_hw_name = sub_hw_name,
                _folder = Path.Combine(_folder, upFolder),
                formFile = formFile,
                hwID = hwID,
                accId = (int)HttpContext.Session.GetInt32("loginID"),
            };
            return Repository.UploadFile(uploadDetail);
        }

        public object downloadFile(int hwUrlID){
            HomeworkURL tmp = Repository.GetFileUrl(hwUrlID);
            //tmp.URL = HttpUtility.UrlEncode(tmp.URL, System.Text.Encoding.UTF8);
            //string abFileURL = Path.Combine(_folder, tmp.URL);
            //abFileURL = abFileURL.Replace('\\','/');
            string fileURL = Path.Combine(upFolder, tmp.URL);
            string originalName = tmp.OriginalName;
            var fileData = new{abFileURL= fileURL, originalName= originalName};
            return fileData;
        }









        public bool chkCurrentUser(){
            string getTimeStamp = Repository.QueryTimeStamp(HttpContext.Session.GetInt32("loginID"));
            if(HttpContext.Session.GetString("loginTimeStamp") == getTimeStamp){
                return true;
            }else{
                return false;
            }
        }

         public bool isLoginInfo(){
            if(HttpContext.Session.GetInt32("loginID") == null || HttpContext.Session.GetInt32("loginAuthority") == null){
                return false;
            }else{
                return true;
            }
        }

        public IActionResult logOut(){ 
            ViewData["Auth"] = "N";
            HttpContext.Session.Clear();
            return View("~/Views/Account/Index.cshtml");
        }

        public bool chkExtName(IFormFile file){
            string[] fileType = {"docx", "pptx", "pdf", "xlsx"};
            var fileName = file.FileName;
            var fileExt = fileName.Substring(fileName.LastIndexOf(".")+1);
            if(Array.IndexOf(fileType,fileExt) <0){
                return false;
            }
            return true;
        }
    }

    
}
