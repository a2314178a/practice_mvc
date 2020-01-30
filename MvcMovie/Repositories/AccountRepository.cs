using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using MvcMovie.Models;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;




namespace MvcMovie.Repositories
{
    public class AccountRepository : BaseRepository
    {
        //private DBContext _DbContext {get;}

        public AccountRepository(DBContext dbContext):base(dbContext)
        {
            //this._DbContext = dbContext;
        }

        //添加
        public int Add(User user)
        {
            int Count = 0;
            List<User> result = null;
            using(_DbContext)
            {
                try{
                    var query = _DbContext.users.Where(u=>u.Account == user.Account);
                    result = query.ToList();    //查詢帳號是否存在
                    if(result.Count ==0){
                        _DbContext.users.Add(user);
                        Count = _DbContext.SaveChanges();
                    }
                    else{
                        Count = -1;
                    }
                }
                catch(Exception e){          
                    return ((MySqlException)e.InnerException).Number;
                }    
            }
            return Count;    //-1:already account, 0:add fail, 1:add success
        }

        //删除
        public int Delete(int id)
        {
            int Count = 0;
            using(_DbContext)
            {
                try{
                    var userFromContext = _DbContext.users.FirstOrDefault(u => u.Id == id);
                    if(userFromContext != null){    
                        _DbContext.users.Remove(userFromContext);
                        Count = _DbContext.SaveChanges();
                    }
                }
                catch(Exception e){          
                    return ((MySqlException)e.InnerException).Number;
                }
            }
            return Count;
        }

        //更新
        public int Update(User user)
        {
            int Count = 0;
            User updateDate = user; 
            using(_DbContext)
            { 
                try{
                    var userFromContext = _DbContext.users.FirstOrDefault(u => u.Id == updateDate.Id);
                    if(userFromContext != null)
                    {
                        if(updateDate.Password != null){
                            userFromContext.Password = updateDate.Password;
                        }
                        userFromContext.UserName = updateDate.UserName;
                        userFromContext.Authority = updateDate.Authority;
                        userFromContext.UpdateTime = updateDate.UpdateTime;
                        Count = _DbContext.SaveChanges();
                    }
                }catch(Exception e){          
                    return ((MySqlException)e.InnerException).Number;
                }
            }
            return Count;
        }

        public int UpdateTimeStamp(int id, string timeStamp, DateTime updateTime)
        {
            int Count = 0;
            using(_DbContext)
            {
                try{
                    var userFromContext = _DbContext.users.FirstOrDefault(u => u.Id == id);  
                    userFromContext.loginTime = timeStamp;
                    userFromContext.UpdateTime = updateTime;
                    Count = _DbContext.SaveChanges();         
                }
                catch(Exception e){          
                    return ((MySqlException)e.InnerException).Number;
                } 
            }
            return Count;
        }


        public int DeleteSubject(int id)
        {
            int Count = 0;
            using(_DbContext){
                try{
                    var userFromContext = _DbContext.subjects.FirstOrDefault(u => u.Id == id);
                    if(userFromContext != null){    
                        _DbContext.subjects.Remove(userFromContext);
                        Count = _DbContext.SaveChanges();
                        
                        var subContext = _DbContext.stu_subs.Where(u => u.SubjectID == id);
                        _DbContext.stu_subs.RemoveRange(subContext);
                        _DbContext.SaveChanges();
                    }
                }
                catch(Exception e){          
                    return ((MySqlException)e.InnerException).Number;
                }  
            }
            return Count;
        }

        public int AddSubject(Subject subject)
        {
            int Count = 0;
            using(_DbContext)
            {       
                try{
                     _DbContext.subjects.Add(subject);
                    Count = _DbContext.SaveChanges();
                }    
                catch(Exception e){          
                    return ((MySqlException)e.InnerException).Number;
                }          
            }
            return Count;    //0:add fail, 1:add success
        }


        public int UpdateSubject(Subject subject)
        {
            int Count = 0;
            using(_DbContext)
            { 
                var maxPeopleOrTeacherChange = false;
                try{         
                    var userFromContext = _DbContext.subjects.FirstOrDefault(u => u.Id == subject.Id);
                    if(userFromContext != null)
                    {
                        if(userFromContext.MaxPeople != subject.MaxPeople || userFromContext.TeacherID != subject.TeacherID){
                            maxPeopleOrTeacherChange = true;
                        }
                        userFromContext.SubjectName = subject.SubjectName;
                        userFromContext.MaxPeople = subject.MaxPeople;
                        //userFromContext.WeekDay = subject.WeekDay;
                        //userFromContext.StartTime = subject.StartTime;    //20191231
                        //userFromContext.EndTime = subject.EndTime;
                        userFromContext.UpdateTime = subject.UpdateTime;
                        Count = _DbContext.SaveChanges();
                    }
                    /*if(maxPeopleOrTeacherChange){
                        var subContext = _DbContext.subjects.Where(u=>u.GroupTag == userFromContext.GroupTag).ToList();
                        if(subContext.Count >1){
                            foreach(Subject data in subContext){
                                data.TeacherID = subject.TeacherID;
                                data.MaxPeople = subject.MaxPeople;
                                _DbContext.SaveChanges();
                            }
                        }
                    }*/
                }catch(Exception e){          
                    return ((MySqlException)e.InnerException).Number;
                }
            }
            return Count;
        }

        //查詢
        public List<User> Query()
        {
            List<User> result = null;
            using(_DbContext){
                //var query = _DbContext.students.FromSqlRaw("select * from students");
                var query = _DbContext.users;
                result = query.ToList();       
            }
            return result; 
        }

        public List<Subject> QuerySubjectByID(int id)
        {
            List<Subject> result = null;
            using(_DbContext){              
                var query = _DbContext.subjects.Where(t=>t.Id == id);
                result = query.ToList();       
            }
            return result; 
        }

        public List<User> GetTeacherName(int level){
            List<User> result = null;
            using(_DbContext){              
                var query = _DbContext.users.Where(u=>u.Authority == level)
                                .Select(u=> new User{Id = u.Id,UserName = u.UserName});
                result = query.ToList();       
            }
            return result; 
        }



        public List<User> query_checkUser(string account, string password)
        {
            List<User> result = null;
            var query = _DbContext.users.Where(u=>u.Account == account && u.Password == password);
            result = query.ToList();                                                                                
            return result; 
        }

        public List<User> query_optionId(int id)
        {
            List<User> result = null;
            using(_DbContext){
               var query = _DbContext.users.Where(u=>u.Id == id);
               result = query.ToList();                             
            }                                                
            return result; 
        }

        public List<User> query_filter(string filterOption,string filterWord)
        {
            List<User> result = null;
            using(_DbContext){
                IQueryable<User> query = null;
                switch(filterOption){
                    case "account": query = _DbContext.users
                                            .Where(u=>u.Account.Contains(filterWord))
                                            .OrderBy(u=>u.Id);
                                break;
                    
                    case "userName":query = _DbContext.users
                                            .Where(u=>u.UserName.Contains(filterWord))
                                            .OrderBy(u=>u.Id);
                                break;
                    
                    case "authority": query = _DbContext.users
                                            .Where(u=>u.Authority == Convert.ToInt16(filterWord))
                                            .OrderBy(u=>u.Id);
                                break;
                }               
                result = query.ToList();                               
            }                                                
            return result; 
        }
    }
}