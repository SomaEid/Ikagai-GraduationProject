using Ikagai.Core;
using Ikagai.Dtos;
using Ikagai.Email;
using Ikagai.Services.BaseService;
using Ikagai.Services.EmailService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace Ikagai.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailServices _emailServices;
        private readonly IConfiguration _config;
        private readonly ApplicationDbContext _context;
        
        public AccountController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IEmailServices emailServices, IConfiguration config, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _emailServices = emailServices;
            _config = config;
            _context = context;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (ModelState.IsValid)
            {
                // Map User Data 
                var user = new ApplicationUser()
                {
                    Email = dto.Email,
                    UserName = dto.NationalId,
                    PhoneNumber = dto.PhoneNumber,
                };
                // Check Role Validation 
                var role = await _roleManager.FindByNameAsync(dto.Role);
                if (role is not null)
                {
                    //Create User With Hashed PassWord
                    var result = await _userManager.CreateAsync(user, dto.Password);
                    if (result.Succeeded)
                    {
                        // Assign Role
                        var resultRole = await _userManager.AddToRoleAsync(user, role.Name);

                        if (resultRole.Succeeded)
                        {
                            if (dto.Role.ToLower() == ("bloodbank").ToLower())
                            {
                                if (dto.StatusId is null || dto.StatusId is 0)
                                {
                                    return BadRequest(new { status = false, message = "InValid StatusId" });
                                }
                                if (dto.OpenHour is null)
                                {
                                    return BadRequest(new { status = false, message = "OpenHour Cannot be null" });
                                }
                                if (dto.ClosedHour is null)
                                {
                                    return BadRequest(new { status = false, message = "ClosedHour Cannot be null" });
                                }
                                if (dto.BloodBankName is null)
                                {
                                    return BadRequest(new { status = false, message = "BloodBank Name Cannot be null" });
                                }

                                var bloodBank = new BloodBank
                                {
                                    ApplicationUserId = user.Id,
                                    CityId = dto.CityId,
                                    GovernorateId = dto.GovernorateId,
                                    FirstName = dto.FirstName,
                                    LastName = dto.LastName,
                                    Location = dto.Location,
                                    StatusId = (byte)dto.StatusId,
                                    OpenHour = TimeOnly.FromTimeSpan((TimeSpan)dto.OpenHour),
                                    ClosedHour = TimeOnly.FromTimeSpan((TimeSpan)dto.ClosedHour),
                                    BloodBankName = dto.BloodBankName
                                };
                                await _context.BloodBanks.AddAsync(bloodBank);
                                await _context.SaveChangesAsync();

                            }
                            else if (dto.Role.ToLower() == "deliverycompany")
                            {
                                if (dto.OpenHour is null)
                                {
                                    return BadRequest(new { status = false, message = "OpenHour Cannot be null" });
                                }
                                if (dto.ClosedHour is null)
                                {
                                    return BadRequest(new { status = false, message = "ClosedHour Cannot be null" });
                                }
                                if (dto.CompanyName is null)
                                {
                                    return BadRequest(new { status = false, message = "Company Name Cannot be null" });
                                }
                                if (dto.CommercialRegister is null)
                                {
                                    return BadRequest(new { status = false, message = "Commercial Register Name Cannot be null" });
                                }
                                var deliveryCompany = new DeliveryCompany
                                {
                                    ApplicationUserId = user.Id,
                                    CityId = dto.CityId,
                                    GovernorateId = dto.GovernorateId,
                                    FirstName = dto.FirstName,
                                    LastName = dto.LastName,
                                    Location = dto.Location,
                                    OpenHour = TimeOnly.FromTimeSpan((TimeSpan)dto.OpenHour),
                                    ClosedHour = TimeOnly.FromTimeSpan((TimeSpan)dto.ClosedHour),
                                    DeliveryCompanyName = dto.CompanyName,
                                    CommercialRegister = dto.CommercialRegister
                                };
                                await _context.DeliveryCompanies.AddAsync(deliveryCompany);
                                await _context.SaveChangesAsync();
                            }
                            else if (dto.Role.ToLower() == "donor" || dto.Role.ToLower() == "customer"  || dto.Role.ToLower() == "admin")
                            {
                                if (dto.BloodId == 0 || dto.BloodId == null)
                                {
                                    return BadRequest(new { status = false, message = "BloodId Cannot be null or Zero" });
                                }
                                if (dto.Gender == null)
                                {
                                    return BadRequest(new { status = false, message = "Gender Cannot be null" });
                                }
                                if (dto.Role == null)
                                {
                                    return BadRequest(new { status = false, message = "RoleName Cannot be null" });
                                }

                                var person = new Person()
                                {
                                    ApplicationUserId = user.Id,
                                    CityId = dto.CityId,
                                    GovernorateId = dto.GovernorateId,
                                    FirstName = dto.FirstName,
                                    LastName = dto.LastName,
                                    Location = dto.Location,
                                    BirthDate = DateOnly.FromDateTime((DateTime)dto.BirthDate),
                                     BloodAndDerivativesId= (byte)dto.BloodId,
                                    Gender = (bool)dto.Gender,
                                    RoleName = dto.Role
                                };
                                await _context.People.AddAsync(person);
                                await _context.SaveChangesAsync();

                            }
                            else
                                return BadRequest(new
                                {
                                    message = "In Valid Role",
                                    status = false
                                });

                            // Send Email Confirmation Link 
                            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                            byte[] tokenGeneratedBytes = Encoding.UTF8.GetBytes(token);

                            var codeEncoded = WebEncoders.Base64UrlEncode(tokenGeneratedBytes);

                            var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account", new { token = codeEncoded, email = user.UserName }, Request.Scheme);

                            var message = new Message(new string[] { user.Email }, "Confirmation Link", confirmationLink);

                            await _emailServices.SendEmailAsync(message);

                            return Created(confirmationLink, new
                            {
                                codeEncoded,
                                message = "success",
                                status = true
                            });
                        }
                        else
                        {
                            foreach (var item in resultRole.Errors)
                            {
                                ModelState.AddModelError("", item.Description);
                            }
                            return BadRequest(new
                            {
                                message = "Error Happen",
                                status = false,
                                Errors = ModelState
                            });
                        }
                    }
                    else
                    {
                        foreach (var item in result.Errors)
                        {
                            ModelState.AddModelError("", item.Description);
                        }
                        return BadRequest(new
                        {
                            message = "Error Happen",
                            status = false,
                            Error = ModelState
                        });
                    }

                }
                return BadRequest(new
                {
                    message = "Invalid Role",
                    status = false,
                });
            }
            return BadRequest(new
            {
                message = "Error Happen",
                status = false,
                Error = ModelState
            });
        }

        [HttpGet]
        [Route("Confirm")]
        public async Task<IActionResult> ConfirmEmail(string token, string name)
        {
            var user = await _userManager.FindByNameAsync(name);

            if (user == null)
                return BadRequest(new
                {
                    message = "Error Happen In Token Or Name",
                    status = false
                });

            var codeDecodedBytes = WebEncoders.Base64UrlDecode(token);

            var codeDecoded = Encoding.UTF8.GetString(codeDecodedBytes);

            var result = await _userManager.ConfirmEmailAsync(user, codeDecoded);

            if (result.Succeeded)
                return Ok(new
                {
                    message = "success",
                    status = true
                });

            else
                return BadRequest(new
                {
                    message = "Error Happen",
                    status = false
                });
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(dto.NationalId);
                if (user is not null)
                {
                    // Check if Password is Valid
                    var isValidPassword = await _userManager.CheckPasswordAsync(user, dto.Password);

                    if (isValidPassword)
                    {
                        //Prepare Claims
                        var jwtClaims = new List<Claim>();
                        jwtClaims.Add(new Claim(ClaimTypes.Name, dto.NationalId));
                        jwtClaims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));

                        //Jti -> Token Id -> JwtSchema -> JwtRegisterClaimNames
                        jwtClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                        // Get User Roles
                        var roles = await _userManager.GetRolesAsync(user);
                        //Add Roles in Claims 
                        foreach (var item in roles)
                        {
                            jwtClaims.Add(new Claim(ClaimTypes.Role, item));
                        }

                        //Create Key For Token Signature 
                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]));
                        var credntials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        // Create Token
                        var jwt = new JwtSecurityToken(
                            issuer: _config["Jwt:Issuer"],
                            audience: _config["Jwt:Audience"],
                            claims: jwtClaims,
                            signingCredentials: credntials,
                            expires: DateTime.Now.AddDays(1)
                            );
                        return Ok(new
                        {
                            status = true,
                            Message = "Success",
                            Id = user.Id,
                            token = new JwtSecurityTokenHandler().WriteToken(jwt),
                            expiration = jwt.ValidTo,
                            UserType = (await _userManager.GetRolesAsync(user))[0]
                        });
                    }
                }
            }
            return BadRequest(new { status = false, message = "InValid UserName Or Password" });
        }

        [HttpGet]
        [Route("GetUser")]
        public async Task<IActionResult> GetUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            var person = await _context.People.Where(p => p.ApplicationUserId == user.Id).
                Select(p => new {Id = p.Id , Name = p.FirstName+" "+p.LastName , RoleName = p.RoleName , ApplicationUserId = p.ApplicationUserId}).FirstOrDefaultAsync();
            var bloodBank = await _context.BloodBanks.Where(p => p.ApplicationUserId == user.Id).
                Select(p => new { Id = p.Id, Name = p.BloodBankName, RoleName = "BloodBank", ApplicationUserId = p.ApplicationUserId }).FirstOrDefaultAsync();
            var deliveryCompany = await _context.DeliveryCompanies.Where(p => p.ApplicationUserId == user.Id).
                Select(p => new { Id = p.Id, Name = p.DeliveryCompanyName, RoleName ="DeliveryCompany", ApplicationUserId = p.ApplicationUserId }).FirstOrDefaultAsync();
            var AdminData = await _context.Users.Where(u => u.Id == id).FirstOrDefaultAsync();

            if (user != null)
            {
                if (person == null)
                {
                    if (bloodBank == null)
                    {
                        if (deliveryCompany == null)
                        {
                            if (AdminData == null)
                            {

                                return BadRequest("InValid PersonalData");
                            }
                            else
                            {
                                return Ok(new { status = true, message = "success", data = AdminData, UserType = (await _userManager.GetRolesAsync(user))[0] });
                            }
                        }
                        else
                        {
                            return Ok(new { status = true, message = "success", data = deliveryCompany, UserType = (await _userManager.GetRolesAsync(user))[0] });
                        }
                    }
                    else
                    {
                        return Ok(new { status = true, message = "success", data = bloodBank, UserType = (await _userManager.GetRolesAsync(user))[0] });
                    }
                }
                else
                {
                    return Ok(new { status = true, message = "success", data = person, UserType = (await _userManager.GetRolesAsync(user))[0] });
                }
            }
            else
                return BadRequest(new { status = true, message = "InValid Id" });

        }


        [HttpPost("forgetPassword")]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordDto dto)
        {
            if (ModelState.IsValid)
            {
                //Check if email is exsist
                var user = await _userManager.FindByNameAsync(dto.UserName);
                if (user is not null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                    byte[] tokenGeneratedBytes = Encoding.UTF8.GetBytes(token);

                    var codeEncoded = WebEncoders.Base64UrlEncode(tokenGeneratedBytes);

                    return Ok(new { status = true, Message = "Success", Token = codeEncoded });
                }
                return BadRequest(new { status = true, Message = "Invalid UserName" });
            }
            return BadRequest(new { status = true, Message = "Error Happen", data = ModelState });
        }

        //Forget Password
        [HttpPost("resetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(dto.UserName);
                if (user is not null)
                {
                    var bytes = WebEncoders.Base64UrlDecode(dto.Token);
                    var code = Encoding.UTF8.GetString(bytes);


                    var resetPasswordResult = await _userManager.ResetPasswordAsync(user, code, dto.Password);

                    if (resetPasswordResult.Succeeded)
                    {
                        return Ok(new { status = true, Message = resetPasswordResult.Succeeded });
                    }
                    else
                    {
                        foreach (var error in resetPasswordResult.Errors)
                        {
                            ModelState.AddModelError(error.Code, error.Description);
                        }
                        return BadRequest(new { status = true, Message = "Error Happen", Data = ModelState });
                    }
                }
                return BadRequest(new { status = true, Message = "Invalid UserName" });
            }
            return BadRequest(new { status = true, Message = "Error Happen", Data = ModelState });
        }

    }
}
