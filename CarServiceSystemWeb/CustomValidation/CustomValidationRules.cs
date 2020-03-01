using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using CarServiceSystemWeb.EntityContext;
using System.Reflection;

namespace CarServiceSystemWeb.CustomValidation
{

    public class DuplicateValidation : ValidationAttribute
    {
        public string PropName { get; set; }

        public DuplicateValidation(string propName)
        {
            this.PropName = propName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            using (var context = new CarServiceSystemWeb.EntityContext.CarServiceContext())
            {
                if (PropName == "UserProfile")
                {
                    foreach(UserProfile usp in context.UserProfiles)
                    {
                        if (usp.UserName == value.ToString())
                        {
                            return new ValidationResult(ErrorMessage = "This username is already in use!");
                        }
                    }
                }

                    if (PropName == "Car")
                {
                    Car us = (Car)validationContext.ObjectInstance;
                    Car realUser = null;
                    if (us.Id != 0)
                    {
                        // throw new Exception(us.Id.ToString());
                        realUser = context.Cars.Where(i => i.Id == us.Id).First();
                        context.Entry<Car>(realUser).Reload();
                    }
                    foreach (var user in context.Cars)
                    {
                        PropertyInfo property = typeof(Car).GetProperty(validationContext.MemberName);
                        var val = property.GetValue(user, null);
                        if (us.Id != 0)
                        {
                            var realVall = property.GetValue(realUser, null);
                            if (realVall.ToString() == val.ToString())
                            {
                                // throw new Exception(val.ToString());
                                continue;
                            }
                        }
                        if (val.ToString() == value.ToString())
                        {
                            return new ValidationResult(ErrorMessage = "This " + validationContext.MemberName + " is already in use!");
                        }
                    }
                }
                else if(PropName == "User")
                {
                    User us = (User)validationContext.ObjectInstance;
                    User realUser = null;
                    if (us.Id != 0)
                    {
                        // throw new Exception(us.Id.ToString());
                        realUser = context.Users.Where(i => i.Id == us.Id).First();
                        context.Entry<User>(realUser).Reload();
                    }
                    foreach (var user in context.Users)
                    {
                        PropertyInfo property = typeof(User).GetProperty(validationContext.MemberName);
                        var val = property.GetValue(user, null);
                        if (us.Id != 0)
                        {
                            var realVall = property.GetValue(realUser, null);
                            if (realVall.ToString() == val.ToString())
                            {
                                // throw new Exception(val.ToString());
                                continue;
                            }
                        }
                        if (val.ToString() == value.ToString())
                        {
                            return new ValidationResult(ErrorMessage = "This " + validationContext.MemberName + " is already in use!");
                        }
                    }
                }

            }


            return ValidationResult.Success;
        }
    }
}