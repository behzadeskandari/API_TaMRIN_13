using API.Data;
using API.DTO;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;

namespace API.Controllers
{

    public class UserController : BaseController
    {
        private readonly DataContext _context;

        public UserController(DataContext context)
        {
            _context = context;
        }


        [HttpGet]
        public ActionResult<IEnumerable<AppUser>> GetUsers()
        {
            
            return _context.Users.ToList();
        }

        [HttpGet("{Id}")]
        public ActionResult<AppUser> getUserById(int Id)
        {
            return _context.Users.Find(Id);
            
            //return _context.Users.First(x => x.Id == Id);
            //return _context.Users.Any(x => x.Id == Id); //Wrong
        }

        [HttpPost("SaveDateOfBirth")]
        public async Task<ActionResult<AppUser>> SaveDateOfBirth(DateOfBirthDTO dateOfBirthDTO)
        {
            //DateOfBirthResponseDTO ageDto = new DateOfBirthResponseDTO();
            AppUser appUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == dateOfBirthDTO.Id);

            appUser.DateofBirth = dateOfBirthDTO.DateofBirth;

            _context.Users.Update(appUser);
            await _context.SaveChangesAsync();
            return appUser;

        }

        [HttpPost("SaveTelephoneNumber")]
        public async Task<ActionResult<AppUser>> SaveTelephoneNumber(TelePhoneDTO telePhoneDTO)
        {
            //DateOfBirthResponseDTO ageDto = new DateOfBirthResponseDTO();
            
            AppUser appUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == telePhoneDTO.Id);

            appUser.Telephone = telePhoneDTO.Telephone;

            //int variable = 0;
            

            if (telePhoneDTO.Telephone.ToString().Length == 8 && IsAllDigits(telePhoneDTO.Telephone.ToString()))///&& telePhoneDTO.Telephone == typeof(variable)
            {
                _context.Users.Update(appUser);
                await _context.SaveChangesAsync();
            }else
            {
                BadRequest();
            }

            return appUser;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public bool IsAllDigits(string name)
        {
            return name.All(char.IsDigit);
        }

        [HttpPost("showAgeFromDateofBirth")]
        public async Task<int> showAgeFromDateofBirth(AgeConverterDto dateOfBirth)
        {
            
            AppUser User = await _context.Users.FindAsync(dateOfBirth.Id);

            
            int age = DateTime.Now.Year - User.DateofBirth.Year;

            if (DateTime.Now.DayOfYear < User.DateofBirth.DayOfYear)
            {
                age = age - 1;
            }


            return await Task.FromResult<int>(age);
        }


    }
}
