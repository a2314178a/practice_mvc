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
    public class BaseRepository
    {
        protected DBContext _DbContext {get;set;}

        public BaseRepository(DBContext dbContext)
        {
            this._DbContext = dbContext;
        }

        public string QueryTimeStamp(int? id){
            string result = "";
            var query = _DbContext.users.Where(u=>u.Id == id).Select(u => u.loginTime);
            result = query.ToList()[0];                                                                             
            return result; 
        }

        public HomeworkURL GetFileUrl(int hwUrlID){
            HomeworkURL selCol = null;
            var query = (from a in _DbContext.homeworkurls
                        where a.Id == hwUrlID
                        select new HomeworkURL {
                            URL= a.URL, OriginalName= a.OriginalName
                        }).FirstOrDefault();
            selCol = query;
            return selCol;
        }

        public void delEntityFile(string delPath){
            if(System.IO.File.Exists(delPath)){
                try{
                    System.IO.File.Delete(delPath);
                }
                catch (Exception e){}//System.IO.IOException e                  
            }
        }
    }
}