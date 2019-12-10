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
    public class DbSqlRepository
    {
        private DBContext _DbContext {get;}

        public DbSqlRepository(DBContext dbContext)
        {
            //在构造函数中注入DbContext
            this._DbContext = dbContext;
        }

        //添加
        public int Add(Student user)
        {
            int Count = 0;
            using(_DbContext){
                _DbContext.Students.Add(user);
                Count = _DbContext.SaveChanges();
            }
            return Count;
        }

        //删除
        public int Delete(int id)
        {
            int Count = 0;
            using(_DbContext){
                var userFromContext = _DbContext.Students.FirstOrDefault(u => u.Id == id);
                if(userFromContext != null)
                {    
                    _DbContext.Students.Remove(userFromContext);
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

                var userFromContext = _DbContext.Students.FirstOrDefault(u => u.Id == user.Id);
                if(userFromContext != null)
                {
                    userFromContext.StudentID = user.StudentID;
                    userFromContext.Name = user.Name;
                    userFromContext.Sex = user.Sex;
                    Count = _DbContext.SaveChanges();
                }            
            }
            return Count;
        }

        public List<Student> Query()
        {
            List<Student> result = null;
            using(_DbContext){
                //var query = _DbContext.Students.FromSqlRaw("select * from students");
                var query = _DbContext.Students.OrderBy(u=>u.StudentID);
                result = query.ToList();       
            }
            return result; 
        }

        public List<Student> query_option(string filterOption,string filterWord)
        {
            List<Student> result = null;
   
            using(_DbContext){
                IQueryable<Student> query = null;
                string text = "select * from students where StudentId LIKE '%"+filterWord+"%'";
                Console.WriteLine(text);
                switch(filterOption){
                    case "sid": query = _DbContext.Students.FromSqlRaw(
                                        "select * from students where StudentId LIKE '%"+filterWord+"%' Order By StudentID");
                                break;
                    
                    case "name":query = _DbContext.Students
                                            .Where(u=>u.Name.Contains(filterWord))
                                            .OrderBy(u=>u.StudentID);
                                break;
                    
                    case "sex": query = _DbContext.Students
                                            .Where(u=>u.Sex.Contains(filterWord))
                                            .OrderBy(u=>u.StudentID);
                                break;
                }               
                result = query.ToList();       
            }
            return result; 
        }
    }
}