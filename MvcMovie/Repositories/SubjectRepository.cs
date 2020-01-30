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



namespace MvcMovie.Repositories
{
    public class SubjectRepository : BaseRepository
    {
        //private DBContext _DbContext {get;}

        public SubjectRepository(DBContext dbContext):base(dbContext)
        {           
            //this._DbContext = dbContext;
        }

        public object Query()
        {
            object result = null;      
            using(_DbContext){
                var query = from a in _DbContext.subjects
                            join b in _DbContext.subtimes on a.Id equals b.SubjectID
                            orderby a.SubjectName, b.WeekDay, b.StartTime
                            select new{
                                a.Id, a.SubjectName, a.TeacherID, a.MaxPeople,
                                timeID = b.Id, b.SubjectID, b.WeekDay, b.StartTime, b.EndTime               
                            };
                result = query.ToList();
                /*var query = _DbContext.subjects.OrderBy(u=>u.SubjectName)//20191231
                                .ThenBy(u=>u.GroupTag).ThenBy(u=>u.WeekDay).ThenBy(u=>u.StartTime);
                result = query.ToList();*/
            }
            return result; 
        }

        
        public object GetStuSelSub(int id)
        {
            object result = null;            
            using(_DbContext){
                var query = from a in _DbContext.subjects
                join b in _DbContext.stu_subs on a.Id equals b.SubjectID
                join c in _DbContext.subtimes on b.SubjectID equals c.SubjectID
                where b.StudentID == id
                orderby a.SubjectName, a.Id, c.WeekDay, c.StartTime 
                select new
                {
                    a.Id, a.SubjectName,
                    c.WeekDay, c.StartTime, c.EndTime,
                    status = b.Status == 1 ? "(已接受)":b.Status == 2? "(被拒絕)":"(待審核)",            
                };
                result = query.ToList();
            }
            return result; 
        } 

        public object GetAllSubject(){
            object result = null;
            using(_DbContext){
                var query = from a in _DbContext.subjects
                            join b in _DbContext.subtimes on a.Id equals b.SubjectID
                            orderby a.SubjectName, a.Id, b.WeekDay, b.StartTime
                            select new {
                                a.Id, a.SubjectName,
                                b.WeekDay, b.StartTime, b.EndTime
                            };
                result = query.ToList();
            }
            return result;
        }
    }
}