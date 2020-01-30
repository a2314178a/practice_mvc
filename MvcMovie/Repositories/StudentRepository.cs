using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using MvcMovie.Models;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;



namespace MvcMovie.Repositories
{
    public class StudentRepository : BaseRepository
    {
        //private DBContext _DbContext {get;}

        public StudentRepository(DBContext dbContext):base(dbContext)
        {           
            //this._DbContext = dbContext;
        }

        /*//添加
        public int Add(Student user)
        {
            int Count = 0;
            List<Student> result = null;
            using(_DbContext){
                var query = _DbContext.students.Where(u=>u.StudentID == user.StudentID);
                result = query.ToList();    //查詢帳號是否存在
                if(result.Count ==0){
                    _DbContext.students.Add(user);
                    Count = _DbContext.SaveChanges();
                }else{
                    Count = -1;
                }  
            }
            return Count;
        }

        //删除
        public int Delete(int id)
        {
            int Count = 0;
            using(_DbContext){
                var userFromContext = _DbContext.students.FirstOrDefault(u => u.Id == id);
                if(userFromContext != null)
                {    
                    _DbContext.students.Remove(userFromContext);
                    Count = _DbContext.SaveChanges();
                }
            }
            return Count;
        }

        //更新
        public int Update(Student user)
        {
            int Count = 0;
            using(_DbContext){

                var userFromContext = _DbContext.students.FirstOrDefault(u => u.Id == user.Id);
                if(userFromContext != null)
                {
                    userFromContext.StudentID = user.StudentID;
                    userFromContext.Name = user.Name;
                    userFromContext.Sex = user.Sex;
                    userFromContext.UpdateTime = user.UpdateTime;
                    Count = _DbContext.SaveChanges();
                }            
            }
            return Count;
        }

        public int UpdateDetail(Student user)
        {
            int Count = 0;
            using(_DbContext){

                var userFromContext = _DbContext.students.FirstOrDefault(u => u.Id == user.Id);
                if(userFromContext != null)
                {
                    userFromContext.StudentID = user.StudentID;
                    userFromContext.Name = user.Name;
                    userFromContext.Sex = user.Sex;
                    userFromContext.Birth = user.Birth;
                    userFromContext.Age = user.Age;
                    //userFromContext.SubjectID = user.SubjectID;
                    userFromContext.UpdateTime = user.UpdateTime;
                    Count = _DbContext.SaveChanges();
                }            
            }
            return Count;
        }*/



        public int StuDelSub(int studentID, int subjectID){
            int Count = 0;
            using(_DbContext){
                var context = _DbContext.stu_subs
                                .FirstOrDefault(u=> u.SubjectID == subjectID && u.StudentID == studentID);
                if(context != null)
                {    
                    _DbContext.stu_subs.Remove(context);
                    Count = _DbContext.SaveChanges();
                }
            }
            return Count;
        }

        public int AddSelSub(Stu_sub data){
            int Count = 0;
            try{
                _DbContext.stu_subs.Add(data);
                Count = _DbContext.SaveChanges();
            }
            catch(Exception e){
                return ((MySqlException)e.InnerException).Number;
            }  
            return Count;
        }


        public object GetVerifySubject(int accID){
            object result = null;
            using(_DbContext){
                var query = from a in _DbContext.subjects 
                            join b in _DbContext.subtimes on a.Id equals b.SubjectID
                            join c in _DbContext.stu_subs on a.Id equals c.SubjectID
                            join d in _DbContext.students on c.StudentID equals d.Id
                            where d.AccountID == accID && c.Status == 1
                            orderby a.SubjectName, b.WeekDay, b.StartTime
                            select new{
                                a.Id,a.SubjectName, 
                                b.WeekDay, b.StartTime, b.EndTime
                            };
                result = query.ToList();
            }
            return result;
        }

        public List<Student> Query(int? accountID)
        {
            List<Student> result = null;
            
            using(_DbContext){
                var query = _DbContext.students.Where(u=>u.AccountID == accountID);                                             
                result = query.ToList();       
            }
            return result; 
        }

        public List<SubTime> getAllSelSubjectTime (int stuID){
            List<SubTime> result = null;
            var query = from a in _DbContext.stu_subs
                        join b in _DbContext.subtimes on a.SubjectID equals b.SubjectID
                        where a.StudentID == stuID
                        select b;
            result = query.ToList();
            return result;
        }

        public List<SubTime> getAddSubjectTime (int subID){
            List<SubTime> result = null;
            var query = _DbContext.subtimes.Where(t=>t.SubjectID == subID);
            result = query.ToList();
            return result;
        }

        public object GetThisSubHomework(int accID, int subID){
            object result = null;
            using(_DbContext){
                var query = from a in _DbContext.subhomeworks
                            join b in _DbContext.homeworkurls 
                            on new {A=a.Id, B=accID} equals new {A=b.SubHomeworkID, B=b.accountID} into tmp
                            from c in tmp.DefaultIfEmpty()
                            where a.SubjectID == subID
                            select new {
                                a.Id, a.HomeworkName, a.HomeworkDetail, a.UploadDeadTime,
                                urlID = c.Id, c.OriginalName
                            };
                result = query.ToList();
            }
            return result;
        }

        public object GetSubHwName(int hwID){
            object result = null;
            var query = from a in _DbContext.subhomeworks
                        where a.Id == hwID
                        select new{
                            a.SubjectName, a.HomeworkName
                        };
            result = query.ToList()[0];
            return result;
        }


        public int UploadFile(object data){  
            var formFile = getObjectValue("formFile", data);
            var _folder = getObjectValue("_folder", data);
            var accId = getObjectValue("accId", data);
            var hwID = getObjectValue("hwID", data);
            var subjectName = getObjectValue("SubjectName", getObjectValue("sub_hw_name", data));
            var homeworkName = getObjectValue("HomeworkName", getObjectValue("sub_hw_name", data));
            
            var uploadFolder = Path.Combine(_folder, subjectName, homeworkName);
            //uploadFolder = uploadFolder.Replace('\\','/');
            if (!Directory.Exists(uploadFolder)){
                Directory.CreateDirectory(uploadFolder);
            }
            var timeNow = DateTime.Now;
            var newName = timeNow.ToString("yyyyMMddHHmmss")+"_"+ accId +"_"+ (formFile.FileName);
            var uploadFilePath = Path.Combine(uploadFolder, newName);
            //uploadFilePath = uploadFilePath.Replace('\\','/');
            copyFileToFilePath(formFile, uploadFilePath);   //copy file to path

            var homeworkUrlLog = new HomeworkURL();
            homeworkUrlLog.accountID = accId;
            homeworkUrlLog.SubHomeworkID = hwID;
            homeworkUrlLog.URL = Path.Combine(subjectName, homeworkName, newName);
            //homeworkUrlLog.URL = (homeworkUrlLog.URL).Replace('\\','/');
            homeworkUrlLog.OriginalName = formFile.FileName;
            homeworkUrlLog.NewName = newName;
            homeworkUrlLog.CreateTime = timeNow;
            return addHomeworkUrlLog(homeworkUrlLog, _folder);
        }

        public void copyFileToFilePath(IFormFile file, string filePath){
            try{
                using (var fileStream = new FileStream(filePath, FileMode.Create)){
                    file.CopyTo(fileStream);
                }
            }catch(System.IO.IOException e){
                throw e;
            }
        }


        public int addHomeworkUrlLog(HomeworkURL data, string _folder){
            int Count = 0;
            var delPath= "";
            var context = _DbContext.homeworkurls
                            .FirstOrDefault(t=>t.accountID == data.accountID && t.SubHomeworkID == data.SubHomeworkID);
            try{
                if(context==null){
                    _DbContext.homeworkurls.Add(data);
                    Count = _DbContext.SaveChanges();
                }else{
                    delPath = Path.Combine(_folder, context.URL);
                    context.URL = data.URL;
                    context.OriginalName = data.OriginalName;
                    context.NewName = data.NewName;
                    context.UpdateTime = data.CreateTime;
                    Count = _DbContext.SaveChanges();
                }
            }
            catch(Exception e){
               throw (MySqlException)e.InnerException;
            } 
            delEntityFile(delPath); 
            return Count;
        }

        
        public dynamic getObjectValue(string key, object obj){
            return obj.GetType().GetProperty(key).GetValue(obj);
        }
       
    }
}