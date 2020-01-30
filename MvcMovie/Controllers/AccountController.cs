using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MvcMovie.Models;
using MvcMovie.Repositories;
using System.Text.Json;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace MvcMovie.Controllers
{  
    public class AccountController : Controller
    {
        public AccountRepository Repository { get; }

        public AccountController(AccountRepository repository)
        {
            this.Repository = repository;
        }

        public IActionResult Index(){ 
            if(isLoginInfo()){
                return selectPage();
            }else{
                return View();
            }
        }

        public IActionResult logOut(){ 
            ViewData["Auth"] = "N";
            HttpContext.Session.Clear();
            return View("Index");
        }

        public IActionResult selectPage(){
            if(!isLoginInfo()){
                return logOut();
            }
            ViewData["Title"] = HttpContext.Session.GetString("loginName");
            ViewBag.Auth = "Y";
            ViewBag.Authority = HttpContext.Session.GetInt32("loginAuthority");
            switch(HttpContext.Session.GetInt32("loginAuthority")){
                case 1: return View("masterPage");
                case 2: return View("teacherPage");
                case 3: return View("studentPage");
                default: return View("Index");
            }
        }

        public IActionResult addAccountForm(){
            if(!isLoginInfo()){
                return logOut();
            }
            ViewBag.Action = "add";
            ViewBag.mainText = "新增人員";
            return View("subPage/addUpdateForm");      
        }
        
        public IActionResult updateAccountForm(int id){
            if(!isLoginInfo()){
                return logOut();
            }
            List<User> userData = Repository.query_optionId(id);
            if(userData.Count>0){
                ViewBag.Action = "update";
                ViewBag.mainText = "編輯人員";
                ViewBag.Id = userData[0].Id;
                ViewBag.Account = userData[0].Account;
                ViewBag.UserName = userData[0].UserName;
                ViewBag.Authority = userData[0].Authority;
            }
            return View("subPage/addUpdateForm");      
        }

        public IActionResult Subject(string page){
            if(!isLoginInfo()){
                return logOut();
            }
            ViewBag.showPage = page;
            ViewBag.Auth = "Y";
            return View("masterPage");
        }

        public bool isLoginInfo(){
            if(HttpContext.Session.GetInt32("loginID") == null || HttpContext.Session.GetInt32("loginAuthority") == null){
                return false;
            }else{
                return true;
            }
        }






        public List<User> query(){
            List<User> userData = Repository.Query();
            return userData;
        }
  
        public List<Subject> querySubjectByID(int id){
            List<Subject> subjectData = Repository.QuerySubjectByID(id);
            return subjectData;
        }


        public List<User> query_filter(string filterOption, string filterWord){            
            List<User> userData = Repository.query_filter(filterOption ,filterWord);
            return userData;
        }

        public List<User> getTeacherName(){
            int Authority = 2;
            List<User> userData = Repository.GetTeacherName(Authority);
            return userData;
        }



        [HttpPost]
        public string login(string account, string password){ 
            var md5password = GetMD5((account+password));
            List<User> userData = Repository.query_checkUser(account, md5password);
            if(userData.Count==1){
                DateTime nowTime= DateTime.Now;
                DateTime Jan1st1970 = new DateTime  (1970, 1, 1, 8, 0, 0, DateTimeKind.Utc);
                long timeStamp = Convert.ToInt64((nowTime - Jan1st1970).TotalMilliseconds);
                //DateTime timeStampToDateTime = Jan1st1970.AddMilliseconds(timeStamp);

                int count = Repository.UpdateTimeStamp(userData[0].Id, timeStamp.ToString(), nowTime);
                
                HttpContext.Session.SetString("loginTimeStamp",timeStamp.ToString());
                HttpContext.Session.SetInt32("loginID",userData[0].Id);
                HttpContext.Session.SetInt32("loginAuthority",userData[0].Authority);
                HttpContext.Session.SetString("loginName",userData[0].UserName.ToString());              
                switch(userData[0].Authority){
                    case 1: return "1"; //master
                    case 2: return "2"; //teacher
                    case 3: return "3"; //student
                    default: return "0";
                }
            }    
            else
                return "0";
        }
            
        [HttpPost]
        public int  create([FromForm] User tmp){  
            if(!chkCurrentUser()){
                return -2;
            }
            tmp.Password = GetMD5((tmp.Account+tmp.Password));
            tmp.CreateTime = DateTime.Now;  
            return Repository.Add(tmp); //-2:mulUserlongin -1:already account, 0:add fail, 1:add success
        }

        [HttpPost]
        public int del([FromForm] int id){  
            if(!chkCurrentUser()){
                return -2;
            }                  
            return Repository.Delete(id);
        }

        [HttpPost]
        public int update([FromForm] User tmp){
            if(!chkCurrentUser()){
                return -2;
            } 
            if(tmp.Password != null){
                tmp.Password = GetMD5((tmp.Account+tmp.Password));
            }
            tmp.UpdateTime = DateTime.Now;
            return Repository.Update(tmp);
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

        [HttpPost]
        public int delSubject([FromForm] int id){  
            if(!chkCurrentUser()){
                return -2;
            }                  
            return Repository.DeleteSubject(id);
        }
        
        [HttpPost]
        public int  createSubject([FromForm] Subject tmp){  
            if(!chkCurrentUser()){
                return -2;
            }
            tmp.CreateTime = DateTime.Now;  
            return Repository.AddSubject(tmp); 
        }
        
        [HttpPost]
        public int updateSubject([FromForm] Subject tmp){
            if(!chkCurrentUser()){
                return -2;
            } 
            tmp.UpdateTime = DateTime.Now;
            return Repository.UpdateSubject(tmp);
        }
        
        
        
        /*
            [HttpPost]
            public ActionResult Login(FormCollection post)
            {
                string account = post["account"];
                string password = post["password"];

                //驗證密碼
                if(db.CheckUserData(account, password))
                {
                    Response.Redirect("~/Home/Home");
                    return new EmptyResult();
                }
                else
                {
                    ViewBag.Msg = "登入失敗...";
                    return View();
                }
            }
        */


        

        

        
    }

    
}
