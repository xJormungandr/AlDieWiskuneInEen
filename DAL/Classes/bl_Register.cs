using System.Linq;
using DAL.Utility;

namespace DAL.Classes
{
    public class bl_Register
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public bool IsActive { get; set; }        

        public static long Register(bl_Register info)
        {
            //Validation
            var Exists = EmailExists(info.Email);

            if (Exists)
            {
                
            }

            if (info.FirstName == "" || info.LastName == "" || info.Email == "")
            {
                throw new System.Exception("Empty Fields Are Not Allowed");
            }

            //Crypto
            info.Password = Crypto.Encrypt(info.Password);
            info.ConfirmPassword = Crypto.Encrypt(info.ConfirmPassword);
            info.IsActive = false;

            if (info.Password != info.ConfirmPassword)
            {
                throw new System.Exception("Passwords Do Not Match");
            }


            //db add
            using (var metadata = new db_VoidConFormworkEntities())
            {

                var User = new db_Users
                {
                    Email = info.Email,
                    Password = info.Password,
                    FirstName = info.FirstName,
                    LastName = info.LastName,
                    isActive = info.IsActive
                    
                };

                metadata.db_Users.Add(User);

                metadata.SaveChanges();

                return User.userID;
            }
        }

        public static bool Login(string Username, string Password)
        {
            using (var metadata = new db_VoidConFormworkEntities())
            {
                //get user using LINQ
                var User =  metadata.db_Users.Where(r => r.Email == Username).FirstOrDefault();

                if(User == null)
                {
                    //fail
                }

                //Crypto

                var isAuth = true;

                if (isAuth)
                    return true;
                else
                    return false;

            }
        }

        public static bool EmailExists(string Email)
        {
            using(db_VoidConFormworkEntities emailCheck = new db_VoidConFormworkEntities())
            {
                var check = emailCheck.db_Users.Where(id => id.Email == Email).FirstOrDefault();
                return check != null;
            }
        }


        
    }
    
}

