using DAL;
using DAL.Classes;
using System.Linq;
using System.Web.Http;
using VoidConFormwork.Models.API.Register;


namespace VoidConFormwork.ApiControllers
{
    public class RegisterApiController : ApiController
    {
        [HttpPost]
        public object Index(Register info)
        {
            //validation
            if (!ModelState.IsValid)
                return new { isSucces = false, error = "Invalid data!" };

            //add to db
            var id = bl_Register.Register(new bl_Register
            {
                Email = info.Email,
                Password = info.Password,
                FirstName = info.FirstName,
                LastName = info.LastName,
                ConfirmPassword = info.ConfirmPassword  

            });

            return new { isSucces = true, id= id };
            
        }
        

    }
}
