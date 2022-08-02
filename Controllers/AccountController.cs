using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using API.DTO;
using System.Security.Claims;

namespace API.Controllers
{
    public class AccountController : BaseController
    {
        public DataContext _context { get; }
        public AccountController(DataContext context)
        {
            this._context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AppUser>> Register(RegisterDto registerDto)
        {
            if (await IsExist(registerDto.UserName)) return BadRequest("Already Exists");


            using (var hmac = new HMACSHA512())
            {
                AppUser user = new AppUser
                {
                    Name = registerDto.UserName,
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                    PasswordSalt = hmac.Key,

                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return user;
            }
        }

        [HttpPost("Login")]
        public async Task<ActionResult<AppUser>> Login(LoginDto loginDto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Name == loginDto.UserName);
            if (user == null)
            {
                return Unauthorized();
            }

            //ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal();
            //ClaimsIdentity claimsIdentity = new ClaimsIdentity();
            //claimsPrincipal.AddIdentity(claimsIdentity);
            //SignIn(claimsPrincipal);


            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));


            ///Fisrt WAY
            //if (computedHash.SequenceEqual(user.PasswordHash))
            //{
            //    return user;
            //}


            ///OSTAD WAY
            for (int i = 0; i < computedHash.Length; i++)
            {
                
                if (computedHash[i] != user.PasswordHash[i])
                {
                    return Unauthorized("Invalid Password");
                }
            
            }
            
            return user;

        }


        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<bool> IsExist(string username)
        {

            return await _context.Users.AnyAsync(x => x.Name == username);

        }



        
        //[HttpPost("ageConverter")]
        //public async Task<int> AgeConverter(AgeConverterDto dateOfBirth)
        //{

        //    DateTime dateHolder = Convert.ToDateTime(dateOfBirth.DateOfBirth);


        //    int age = DateTime.Now.Year - dateOfBirth.DateOfBirth.Year;

        //    if (DateTime.Now.DayOfYear < dateOfBirth.DateOfBirth.DayOfYear)
        //    {
        //        age = age - 1;
        //    }


        //    return await Task.FromResult<int>(age);
        //}


        //[HttpPost("ageConverterSecond")]
        //public async Task<string> AgeConverterSecond(AgeConverterDto dateOfBirth)
        //{

        //    //DateTime dateHolder = Convert.ToDateTime(dateOfBirth.DateOfBirth);
        //    //string[] dateApart = dateHolder.ToString().Split("/");
        //    //var yearfromDate = dateApart[2].Split(" ");
        //    //var exactyear = yearfromDate.ElementAt(0);
        //    //var exactday = dateApart.ElementAt(0);
        //    //var exactmonth = dateApart.ElementAt(1);


        //    int year = DateTime.Now.Year - dateOfBirth.DateOfBirth.Year;

        //    int month = DateTime.Now.Month - dateOfBirth.DateOfBirth.Month;

        //    int Day = DateTime.Now.Day - dateOfBirth.DateOfBirth.Day;

        //    if (DateTime.Now.DayOfYear < dateOfBirth.DateOfBirth.DayOfYear)
        //    {
        //        year = year - 1;
        //    }
        //    return await Task.FromResult<string>($"{year}-{month}-{Day}");

        //    //$"{year}-{month}-{Day}";
        //}




    }
}
