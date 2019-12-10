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

            #region
            /*
            using (var connection = _DbContext.Database.GetDbConnection())
            {
                connection.Open();
                var command = connection.CreateCommand() as MySqlCommand;
                command.CommandText = "INSERT INTO students(StudentID,Name,Sex) VALUES(@sid,@name,@sex)";
                command.Parameters.Add(new MySqlParameter()
                {
                    ParameterName = "@sid",
                    DbType = DbType.Int32,
                    Value = user.newSid
                });
                command.Parameters.Add(new MySqlParameter()
                {
                    ParameterName = "@name",
                    DbType = DbType.Int32,
                    Value = user.newName
                });
                command.Parameters.Add(new MySqlParameter()
                {
                    ParameterName = "@sex",
                    DbType = DbType.String,
                    Value = user.newSex
                });
                var count = command.ExecuteNonQuery();
               
                //获取插入时产生的自增列Id并赋值给user.Id使用
                //user.Id = (int)command.LastInsertedId;
                return count;
            }
            */
            #endregion
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
            /*using (var connection = _DbContext.Database.GetDbConnection())
            {
                connection.Open();
                var command = connection.CreateCommand() as MySqlCommand;
                command.CommandText = "Delete From students Where Id = @id";
                command.Parameters.Add(new MySqlParameter()
                {
                    ParameterName = "@id",
                    DbType = DbType.Int32,
                    Value = id
                });
                var count = command.ExecuteNonQuery();
               
                return count;
            }*/
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
            /*
            using (var connection = _DbContext.Database.GetDbConnection())
            {
                connection.Open();
                var command = connection.CreateCommand() as MySqlCommand;
                command.CommandText = "Update students set StudentID=@sid, Name=@name, Sex=@sex Where Id=@id ";
                command.Parameters.Add(new MySqlParameter()
                {
                    ParameterName = "@sid",
                    DbType = DbType.Int32,
                    Value = user.StudentID
                });
                command.Parameters.Add(new MySqlParameter()
                {
                    ParameterName = "@name",
                    DbType = DbType.Int32,
                    Value = user.Name
                });
                command.Parameters.Add(new MySqlParameter()
                {
                    ParameterName = "@sex",
                    DbType = DbType.String,
                    Value = user.Sex
                });
                command.Parameters.Add(new MySqlParameter()
                {
                    ParameterName = "@id",
                    DbType = DbType.Int32,
                    Value = user.Id
                });
                var count = command.ExecuteNonQuery();
                
                return count;
            }*/
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