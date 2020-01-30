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
using System.Security.Cryptography;
using System.Text;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Web;

namespace MvcMovie.Controllers
{  
    public class TeacherController : Controller
    {
        public TeacherRepository Repository { get; }
        private readonly string _folder;
        private readonly string upFolder = "UploadFolder";

        public TeacherController(TeacherRepository repository, IWebHostEnvironment env)
        {
            this.Repository = repository; 
            _folder = $@"{env.WebRootPath}";
        }

        public IActionResult MySubject(string page){
            if(!isLoginInfo()){
                return logOut();
            }
            ViewBag.showPage = page;
            ViewBag.Authority = 2;
            ViewBag.Auth = "Y";
            //var mySubjectID = Repository.QueryMySubject(HttpContext.Session.GetInt32("loginID"));
            return View("~/Views/Account/teacherPage.cshtml");
        }

        public IActionResult logOut(){ 
            ViewData["Auth"] = "N";
            HttpContext.Session.Clear();
            return View("~/Views/Account/Index.cshtml");
        }

        public IActionResult Index(){
            return View();
        }


        public object getSubMember(int subID){
            object studentData = Repository.QuerySubMember(subID);
            return studentData;
        }

        public List<Subject> getMySubject(){
            List<Subject> mySubData = Repository.QueryMySubject(HttpContext.Session.GetInt32("loginID"));
            return mySubData;
        }

        public object getMySubjectDetail(){
            object mySubData = Repository.getMySubDetail(HttpContext.Session.GetInt32("loginID"));
            return mySubData;
        }

        public int delSubjectTime(int subID, int timeID){
            if(!chkCurrentUser()){
                return -2;
            }  
            return Repository.DelSubjectTime(subID, timeID);
        }

        [HttpPost]
        public int createSubject(SubDetail data){
            if(!chkCurrentUser()){
                return -2;
            }
            data.TeacherID = (int)HttpContext.Session.GetInt32("loginID");
            data.CreateTime = DateTime.Now;   
            return Repository.CreateSubject(data); 
        }


         public int updateSubject(SubDetail data){
            data.UpdateTime = DateTime.Now;
            return Repository.UpdateSubject(data);
        }


        public IActionResult AddSubject(string page){
            if(!isLoginInfo()){
                return logOut();
            }
            ViewBag.showPage = page;
            ViewBag.Authority = 2;
            ViewBag.Auth = "Y";
            return View("~/Views/Account/teacherPage.cshtml");
        }

        public List<SubHomework> getSubHomework(int subID){
            return Repository.GetSubHomework(subID);
        }

        public int delHomework(int hwID){
            if(!chkCurrentUser()){
                return -2;
            }  
            return Repository.DelHomework(hwID);
        }
       
        public int homeworkCreate(SubHomework data){
            if(!chkCurrentUser()){
                return -2;
            } 
            data.CreateTime = DateTime.Now; 
            return Repository.HomeworkCreate(data);
        }

        public int updateHomework(SubHomework data){
            if(!chkCurrentUser()){
                return -2;
            } 
            data.UpdateTime = DateTime.Now; 
            return Repository.UpdateHomework(data);
        }

        public object getAllUploadFile(int hwID){
            return Repository.GetAllUploadFile(hwID);
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

        public int delFile(int fileID){
            var folderPath = Path.Combine(_folder, upFolder);
            return Repository.DelFile(fileID, folderPath);
        }



        public List<Student> query(){
            List<Student> studentData = Repository.Query(HttpContext.Session.GetInt32("loginID"));
            return studentData;
        }

        public List<Student> getStuDetail(int id){
            List<Student> studentData = Repository.QueryByID(id);
            return studentData;
        }

        public List<Student> queryFilter(string filterOption, string filterWord){            
            List<Student> studentData = Repository.query_filter(filterOption ,filterWord, HttpContext.Session.GetInt32("loginID"));
            return studentData;
        }

        public object subDetailByID(int subID, int timeID){
            return Repository.SubDetailByID(subID, timeID);
        }


        [HttpPost]
        public int isAgreeMember(Stu_sub data){ 
            if(!chkCurrentUser()){
                return -2;
            }
            DateTime updateTime = DateTime.Now;
            data.UpdateTime = updateTime;  
            data.VerifyAccID = (int)HttpContext.Session.GetInt32("loginID");         
            return Repository.IsAgreeMember(data);
        }

        [HttpPost]
        public int create([FromForm] Student tmp){   
            if(!chkCurrentUser()){
                return -2;
            }

            User newAccount = new User();
            newAccount.Account = tmp.StudentID.ToString();
            newAccount.Password = GetMD5((newAccount.Account+newAccount.Account));
            newAccount.UserName = tmp.Name;
            newAccount.Authority = 3; 
            newAccount.CreateTime = DateTime.Now;  
            int accResult = Repository.AddAccount(newAccount);
            if(accResult==1){
                tmp.AccountID = newAccount.Id;
                tmp.AttachID = (int)HttpContext.Session.GetInt32("loginID");
                tmp.CreateTime = DateTime.Now; 
                int userResult = Repository.AddUser(tmp);
                if(userResult ==1){
                    return 1;
                }else{
                    int delResult = Repository.DelAccount(newAccount.Id);
                    return userResult;
                }
            }else{
                return accResult; 
            }
        }

        [HttpPost]
        public int update([FromForm] Student tmp){ 
            if(!chkCurrentUser()){
                return -2;
            }
            tmp.UpdateTime = DateTime.Now;           
            return Repository.Update(tmp);
        }

        [HttpPost]
        public int del([FromForm] int id, int accountID){ 
            if(!chkCurrentUser()){
                return -2;
            }                   
            return Repository.Delete(id, accountID);
        }

        [HttpPost]
        public int updateDetail([FromForm] Student tmp){ 
            if(!chkCurrentUser()){
                return -2;
            }
            tmp.UpdateTime = DateTime.Now;          
            return Repository.UpdateDetail(tmp);
        }

        public bool chkCurrentUser(){
            string getTimeStamp = Repository.QueryTimeStamp(HttpContext.Session.GetInt32("loginID"));
            if(HttpContext.Session.GetString("loginTimeStamp") == getTimeStamp){
                return true;
            }else{
                return false;
            }
        }

        public static string GetMD5(string original) 
        { 
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider(); 
            byte[] b = md5.ComputeHash(Encoding.UTF8.GetBytes(original)); 
            return BitConverter.ToString(b).Replace("-", string.Empty); 
        }
    
        public bool isLoginInfo(){
            if(HttpContext.Session.GetInt32("loginID") == null || HttpContext.Session.GetInt32("loginAuthority") == null){
                return false;
            }else{
                return true;
            }
        }
    
    
    
    
    }

    

    
}
