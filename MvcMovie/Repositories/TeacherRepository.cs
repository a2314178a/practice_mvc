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
    public class TeacherRepository : BaseRepository
    {
       // private DBContext _DbContext {get;}

        public TeacherRepository(DBContext dbContext):base(dbContext)
        {           
            //this._DbContext = dbContext;
        }

        //添加
        public int AddUser(Student user)
        {
            int Count = 0;
            List<Student> result = null;
            try{
                var query = _DbContext.students.Where(u=>u.StudentID == user.StudentID);
                result = query.ToList();    //查詢帳號是否存在
                if(result.Count ==0){  
                    _DbContext.students.Add(user);
                    Count = _DbContext.SaveChanges();
                }else{
                    Count = -1;
                }    
            }catch(Exception e){
                return ((MySqlException)e.InnerException).Number;
            }
            return Count;
        }

        public int AddAccount(User user)
        {
            int Count = 0;
            List<User> result = null;
            try{
                var query = _DbContext.users.Where(u=>u.Account == user.Account);
                result = query.ToList();    //查詢帳號是否存在
                if(result.Count ==0){
                    _DbContext.users.Add(user);
                    Count = _DbContext.SaveChanges();
                }else{
                    Count = -1;
                }  
            }catch(Exception e){
                return ((MySqlException)e.InnerException).Number;
            }  
            return Count;
        }

        public int DelAccount(int id)
        {
            int Count = 0;
            var userFromContext = _DbContext.users.FirstOrDefault(u => u.Id == id);
            if(userFromContext != null)
            {    
                _DbContext.users.Remove(userFromContext);
                Count = _DbContext.SaveChanges();
            }
            return Count;
        }

        //删除
        public int Delete(int id, int accountID)
        {
            int Count = 0;
            using(_DbContext){
                var userFromContext = _DbContext.students.FirstOrDefault(u => u.Id == id);
                if(userFromContext != null){    
                    _DbContext.students.Remove(userFromContext);
                    Count = _DbContext.SaveChanges();
                }
                if(Count==1){
                    var userFromContext2 = _DbContext.users.FirstOrDefault(u => u.Id == accountID);
                    if(userFromContext != null)
                    {    
                        _DbContext.users.Remove(userFromContext2);
                        Count = _DbContext.SaveChanges();
                    }
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
                    userFromContext.UpdateTime = user.UpdateTime;
                    Count = _DbContext.SaveChanges();
                }            
            }
            return Count;
        }

        public int IsAgreeMember(Stu_sub data){
            int Count = 0;
            using(_DbContext){              
                var context = _DbContext.stu_subs.FirstOrDefault(
                                u => u.StudentID == data.StudentID && u.SubjectID == data.SubjectID);                
                if(context != null)
                {
                    context.Status = data.Status;
                    context.VerifyAccID = data.VerifyAccID;
                    context.UpdateTime = data.UpdateTime;
                    Count = _DbContext.SaveChanges();      
                }            
            }
            return Count;
        }



        public List<Student> Query(int? attachID)
        {
            List<Student> result = null;
            
            using(_DbContext){
                //var query = _DbContext.students.FromSqlRaw("select * from students");
                var query = _DbContext.students.Where(u=>u.AttachID == attachID)
                                                .OrderBy(u=>u.StudentID);
                result = query.ToList();       
            }
            return result; 
        }

        public List<Student> QueryByID(int id)
        {
            List<Student> result = null;            
            using(_DbContext){
                var query = _DbContext.students.Where(u=>u.Id == id);
                result = query.ToList();   
            }
            return result; 
        }

        public List<Student> query_filter(string filterOption, string filterWord, int? attachID)
        {
            List<Student> result = null;
   
            using(_DbContext){
                IQueryable<Student> query = null;
                switch(filterOption){
                    case "sid": query = _DbContext.students.FromSqlRaw(
                                            $"select * from students where AttachID = {attachID} And StudentID LIKE '%{filterWord}%' Order By StudentID"
                                        );                                      
                                break;
                    
                    case "name":query = _DbContext.students
                                            .Where(u=>u.Name.Contains(filterWord) && u.AttachID == attachID)
                                            .OrderBy(u=>u.StudentID);
                                break;
                    
                    case "sex": query = _DbContext.students
                                            .Where(u=>u.Sex==int.Parse(filterWord) && u.AttachID ==attachID)
                                            .OrderBy(u=>u.StudentID);
                                break;
                }               
                result = query.ToList();       
            }
            return result; 
        }

        
        public object SubDetailByID(int subID, int timeID)
        {
            object result = null;            
            using(_DbContext){
                var query = from a in _DbContext.subjects 
                        join b in _DbContext.subtimes on a.Id equals b.SubjectID
                        where a.Id == subID && b.Id == timeID
                        select new{
                            a.SubjectName, a.MaxPeople,
                            b.WeekDay, b.StartTime, b.EndTime  
                        };   
                result = query.ToList();
            }
            return result; 
        }


        public object QuerySubMember(int subID)
        {
            object result = null;         
            using(_DbContext){

                var query = from a in _DbContext.students
                            join b in _DbContext.stu_subs on a.Id equals b.StudentID
                where b.SubjectID == subID 
                select new
                {
                    a.Id, a.StudentID, a.Name, a.Sex,
                    b.Status
                };
                //"COUNT(DISTINCT if(stu_subs.`Status`=1, 1, NULL)) AS accept "
                result = query.ToList();    
            }
            return result; 
        }  

        public List<Subject> QueryMySubject (int? teacherID){
            List<Subject> result = null;
            using(_DbContext){
                var query = _DbContext.subjects.Where(u=>u.TeacherID == teacherID).OrderBy(u=>u.SubjectName);
                result = query.ToList();       
            }
            return result; 
        }

        public object getMySubDetail (int? teacherID){
            object result = null;
            using(_DbContext){
                var query = from a in _DbContext.subjects
                            join b in _DbContext.subtimes on a.Id equals b.SubjectID
                            where a.TeacherID == teacherID
                            orderby a.SubjectName, a.Id ,b.WeekDay, b.StartTime
                            select new{
                                a.Id, a.SubjectName, a.TeacherID, a.MaxPeople,
                                timeID = b.Id, b.SubjectID, b.WeekDay, b.StartTime, b.EndTime               
                            };
                result = query.ToList();     
            }
            return result; 
        }

        public List<SubHomework> GetSubHomework(int subID){
            List<SubHomework> result = null;
            var query = _DbContext.subhomeworks.Where(t=>t.SubjectID == subID);
            result = query.ToList();
            return result;
        }

        public int DelHomework(int hwID){
            int count = 0;
            using(_DbContext){
                var context = _DbContext.subhomeworks.FirstOrDefault(t=>t.Id == hwID);
                if(context != null){
                    _DbContext.subhomeworks.Remove(context);
                    count = _DbContext.SaveChanges();
                }
            }
            return count;
        }

        public int HomeworkCreate(SubHomework data){
            int Count = 0;
            using(_DbContext){
                try{
                    _DbContext.subhomeworks.Add(data);
                    Count = _DbContext.SaveChanges();  
                }catch(Exception e){
                    return ((MySqlException)e.InnerException).Number;
                }
            }
            return Count;
        }

        public int UpdateHomework(SubHomework data)
        {
            int Count = 0;
            using(_DbContext){
                var context = _DbContext.subhomeworks.FirstOrDefault(u => u.Id == data.Id);
                if(context != null)
                {
                    context.HomeworkName = data.HomeworkName;
                    context.HomeworkDetail = data.HomeworkDetail;
                    context.UploadDeadTime = data.UploadDeadTime;
                    context.UpdateTime = data.UpdateTime;
                    Count = _DbContext.SaveChanges();
                }            
            }
            return Count;
        }

        public object GetAllUploadFile(int hwID){
            object result = null;
            var query = from a in _DbContext.homeworkurls
                        join b in _DbContext.students on a.accountID equals b.AccountID
                        where a.SubHomeworkID == hwID
                        select new{
                            a.Id, a.OriginalName,
                            b.StudentID, b.Name
                        };
            result = query.ToList();
            return result;
        }

        public int DelFile(int fileID, string folderPath){
            int count = 0;
            string delPath = "";
            using(_DbContext){
                var filecontext = _DbContext.homeworkurls.FirstOrDefault(u=>u.Id == fileID);
                if(filecontext != null){
                    delPath = Path.Combine(folderPath, filecontext.URL);
                    _DbContext.homeworkurls.Remove(filecontext);
                    count = _DbContext.SaveChanges();
                }
            }
            delEntityFile(delPath);
            return count;
        }


        public int DelSubjectTime(int subID, int timeID){
            int Count = 0;
            using(_DbContext){
                var timecontext = _DbContext.subtimes.FirstOrDefault(u=>u.Id == timeID);
                if(timecontext != null){
                    _DbContext.subtimes.Remove(timecontext);
                    Count = _DbContext.SaveChanges();
                }
                var query = _DbContext.subtimes.Where(u=>u.SubjectID == subID).ToList(); 
                if(query.Count ==0){
                    var subcontext = _DbContext.subjects.FirstOrDefault(u=>u.Id == subID);
                    if(timecontext != null){
                        _DbContext.subjects.Remove(subcontext);
                        _DbContext.SaveChanges();
                        var subTimeText = _DbContext.stu_subs.Where(u=>u.SubjectID == subID);
                        _DbContext.stu_subs.RemoveRange(subTimeText);
                        _DbContext.SaveChanges();
                    }
                }
            }
            return Count;
        }

        public int CreateSubject(SubDetail data){
            int Count = 0;
            Subject newSub = new Subject();
            newSub.SubjectName = data.SubjectName;
            newSub.MaxPeople = data.MaxPeople;
            newSub.TeacherID = data.TeacherID;
            newSub.CreateTime = data.CreateTime;
            try{
                _DbContext.subjects.Add(newSub);
                Count = _DbContext.SaveChanges();
                if(Count == 1){
                    foreach(TimeDetail detail in data.SubTimeDetail){
                        var subtime = new SubTime();
                        subtime.SubjectID = newSub.Id;
                        subtime.WeekDay = detail.WeekDay;
                        subtime.StartTime = detail.StartTime;
                        subtime.EndTime = detail.EndTime;
                        subtime.CreateTime = newSub.CreateTime;
                        _DbContext.subtimes.Add(subtime);
                        Count = _DbContext.SaveChanges();
                    }
                }        
            }catch(Exception e){
                return ((MySqlException)e.InnerException).Number;
            }
            return Count;
        }

        public int UpdateSubject(SubDetail data){
            int Count = 0, subCount = 0, timeCount = 0;
            Subject newSub = new Subject();
            newSub.SubjectName = data.SubjectName;
            newSub.MaxPeople = data.MaxPeople;
            newSub.TeacherID = data.TeacherID;
            newSub.CreateTime = data.CreateTime;
            try{
                var subcontext = _DbContext.subjects.FirstOrDefault(u=>u.Id == data.Id);
                if(subcontext != null){
                    subcontext.SubjectName = data.SubjectName;
                    subcontext.MaxPeople = data.MaxPeople;
                    subcontext.UpdateTime = data.UpdateTime;
                    subCount = _DbContext.SaveChanges();
                }
                var timecontext = _DbContext.subtimes.FirstOrDefault(t=>t.Id == data.SubTimeDetail[0].Id);
                if(timecontext != null){
                    timecontext.WeekDay = data.SubTimeDetail[0].WeekDay;
                    timecontext.StartTime = data.SubTimeDetail[0].StartTime;
                    timecontext.EndTime = data.SubTimeDetail[0].EndTime;
                    timecontext.UpdateTime = data.UpdateTime;
                    timeCount = _DbContext.SaveChanges();
                }             
            }catch(Exception e){
                return ((MySqlException)e.InnerException).Number;
            }
            if(subCount>0 || timeCount>0)Count = 1;
            return Count;
        }
    }
}