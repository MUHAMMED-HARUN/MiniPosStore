using DAL.EF.AppDBContext;
using DAL.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.IRepoServ
{
    public class CurrentUserRepo : ICurrentUserRepo
    {
        AppDBContext DBContext;
        public CurrentUserRepo(AppDBContext appDB)
        {
            DBContext = appDB;
        }
        public string GetFirstUserId()
        {
           return DBContext.Users.FirstOrDefault().Id;
        }
    }
}
