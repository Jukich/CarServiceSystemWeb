using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CarServiceSystemWeb.EntityContext;

namespace CarServiceSystemWeb
{
    public class UserRepository
    {
        CarServiceContext context = new CarServiceContext();
        public UserProfile GetByUsernameAndPassword(UserProfile user)
        {
            return context.UserProfiles.Where(u => u.UserName == user.UserName & u.Password == user.Password).FirstOrDefault();
        }
    }
    public class UserApplication
    {
        UserRepository userRepo = new UserRepository();
        public UserProfile GetByUsernameAndPassword(UserProfile user)
        {
            return userRepo.GetByUsernameAndPassword(user);
        }
    }
}