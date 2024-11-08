using CommUnity.BackEnd.Helpers;
using CommUnity.BackEnd.UnitsOfWork.Implementations;
using CommUnity.BackEnd.UnitsOfWork.Interfaces;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Responses;
using CommUnity.Shared.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CommUnity.BackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IUsersUnitOfWork _usersUnitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IFileStorage _fileStorage;
        private readonly IMailHelper _mailHelper;
        private readonly IResidentialUnitUnitOfWork _residentialUnitUnitOfWork;
        private readonly IApartmentsUnitOfWork _apartmentsUnitOfWork;
        private readonly string _container;

        public AccountsController(
            IUsersUnitOfWork usersUnitOfWork,
            IConfiguration configuration,
            IFileStorage fileStorage,
            IMailHelper mailHelper,
            IResidentialUnitUnitOfWork residentialUnitUnitOfWork,
            IApartmentsUnitOfWork apartmentsUnitOfWork
        )
        {
            _usersUnitOfWork = usersUnitOfWork;
            _configuration = configuration;
            _fileStorage = fileStorage;
            _mailHelper = mailHelper;
            _container = "users";
            _residentialUnitUnitOfWork = residentialUnitUnitOfWork;
            _apartmentsUnitOfWork = apartmentsUnitOfWork;
        }

        [HttpPost("ResendToken")]
        public async Task<IActionResult> ResedTokenAsync([FromBody] EmailDTO model)
        {
            var user = await _usersUnitOfWork.GetUserAsync(model.Email);
            if (user == null)
            {
                return NotFound();
            }

            var response = await SendConfirmationEmailAsync(user);
            if (response.WasSuccess)
            {
                return NoContent();
            }

            return BadRequest(response.Message);
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmailAsync(string userId, string token)
        {
            token = token.Replace(" ", "+");
            var user = await _usersUnitOfWork.GetUserAsync(new Guid(userId));
            if (user == null)
            {
                return NotFound();
            }

            var result = await _usersUnitOfWork.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.FirstOrDefault());
            }

            return NoContent();
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PutAsync(User user)
        {
            try
            {
                var currentUser = await _usersUnitOfWork.GetUserAsync(User.Identity!.Name!);
                if (currentUser == null)
                {
                    return NotFound();
                }

                if (!string.IsNullOrEmpty(user.Photo))
                {
                    var photoUser = Convert.FromBase64String(user.Photo);
                    user.Photo = await _fileStorage.SaveFileAsync(photoUser, ".jpg", _container);
                }

                currentUser.Document = user.Document;
                currentUser.FirstName = user.FirstName;
                currentUser.LastName = user.LastName;
                currentUser.Address = user.Address;
                currentUser.PhoneNumber = user.PhoneNumber;
                currentUser.Photo = !string.IsNullOrEmpty(user.Photo) && user.Photo != currentUser.Photo ? user.Photo : currentUser.Photo;
                currentUser.CityId = user.CityId;
                currentUser.ResidentialUnitId = user.ResidentialUnitId;
                currentUser.ApartmentId = user.ApartmentId;
                currentUser.UserType = user.UserType;

                var result = await _usersUnitOfWork.UpdateUserAsync(currentUser);
                if (result.Succeeded)
                {
                    return Ok(BuildToken(currentUser));
                }

                return BadRequest(result.Errors.FirstOrDefault());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("RecoverPassword")]
        public async Task<IActionResult> RecoverPasswordAsync([FromBody] EmailDTO model)
        {
            var user = await _usersUnitOfWork.GetUserAsync(model.Email);
            if (user == null)
            {
                return NotFound();
            }

            var myToken = await _usersUnitOfWork.GeneratePasswordResetTokenAsync(user);
            var tokenLink = Url.Action("ResetPassword", "accounts", new
            {
                userid = user.Id,
                token = myToken
            }, HttpContext.Request.Scheme, _configuration["Url Frontend"]);

            var emailBody = $@"
            <div style='font-family: Arial, sans-serif; padding: 20px; background-color: #f5f5f5;'>
                <div style='max-width: 600px; margin: auto; background: #fff; border-radius: 8px; box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);'>
                    <div style='background-color: #8019fb; color: white; padding: 10px; border-radius: 8px 8px 0 0;'>
                        <h1 style='margin: 0;'>CommUnity - Recuperación de contraseña</h1>
                    </div>
                    <div style='padding: 20px;'>
                        <h2 style='color: #8019fb;'>Recupere su contraseña</h2>
                        <p style='font-size: 16px;'>Hemos recibido una solicitud para restablecer su contraseña. Para proceder, haga clic en el enlace a continuación:</p>
                        <div style='text-align: center; margin: 20px 0;'>
                            <a href='{tokenLink}' style='background-color: #8019fb; color: white; padding: 10px 20px; border-radius: 5px; text-decoration: none; font-weight: bold; display: inline-block;'>Recuperar Contraseña</a>
                        </div>
                        <p style='font-size: 16px;'>Si usted no solicitó esta acción, puede ignorar este correo electrónico.</p>
                        <p style='font-size: 14px; color: #777;'>Este enlace expirará en 24 horas por motivos de seguridad.</p>
                    </div>
                </div>
            </div>";

            var response = _mailHelper.SendMail(user.FullName, user.Email!, "CommUnity - Recuperación de contraseña", emailBody);

            if (response.WasSuccess)
            {
                return NoContent();
            }

            return BadRequest(response.Message);
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordDTO model)
        {
            var user = await _usersUnitOfWork.GetUserAsync(model.Email);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _usersUnitOfWork.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {
                return NoContent();
            }

            return BadRequest(result.Errors.FirstOrDefault()!.Description);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _usersUnitOfWork.GetUserAsync(User.Identity!.Name!));
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] UserDTO model)
        {
            User user = model;
            if (!string.IsNullOrEmpty(model.Photo))
            {
                var photoUser = Convert.FromBase64String(model.Photo);
                model.Photo = await _fileStorage.SaveFileAsync(photoUser, ".jpg", _container);
            }

            var result = await _usersUnitOfWork.AddUserAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _usersUnitOfWork.AddUserToRoleAsync(user, user.UserType.ToString());
                if (user.UserType == UserType.AdminResidentialUnit || user.UserType == UserType.Worker)
                {
                    var response = await SendConfirmationAdminEmailAsync(model);
                    if (response.WasSuccess)
                    {
                        return NoContent();
                    }

                    return BadRequest(response.Message);
                }
                else
                {
                    var response = await SendConfirmationEmailAsync(user);
                    if (response.WasSuccess)
                    {
                        return NoContent();
                    }

                    return BadRequest(response.Message);
                }
            }

            return BadRequest(result.Errors.FirstOrDefault());
        }

        private async Task<ActionResponse<string>> SendConfirmationEmailAsync(User user)
        {
            var myToken = await _usersUnitOfWork.GenerateEmailConfirmationTokenAsync(user);
            var tokenLink = Url.Action("ConfirmEmail", "accounts", new
            {
                userid = user.Id,
                token = myToken
            }, HttpContext.Request.Scheme, _configuration["Url Frontend"]);

            var residentialUnit = await _residentialUnitUnitOfWork.GetAsync((int)user.ResidentialUnitId!);
            var apartment = await _apartmentsUnitOfWork.GetAsync((int)user.ApartmentId!);
            var admin = await _usersUnitOfWork.GetAdminResidentialUnit((int)user.ResidentialUnitId!);

            var emailBody = $@"
            <div style='font-family: Arial, sans-serif; padding: 20px; background-color: #f5f5f5;'>
                <div style='max-width: 600px; margin: auto; background: #fff; border-radius: 8px; box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);'>
                    <div style='background-color: #8019fb; color: white; padding: 10px; border-radius: 8px 8px 0 0;'>
                        <h1 style='margin: 0;'>CommUnity - Confirmación de cuenta</h1>
                    </div>
                    <div style='padding: 20px;'>
                        <h2 style='color: #8019fb;'>Nuevo usuario registrado en {residentialUnit.Result!.Name}</h2>
                        <p style='font-size: 16px;'>Se ha registrado un nuevo usuario en tu unidad con los siguientes datos:</p>
                        <ul style='font-size: 16px; line-height: 1.5; color: #333;'>
                            <li><strong>Nombre:</strong> {user.FirstName} {user.LastName}</li>
                            <li><strong>Documento:</strong> {user.Document}</li>
                            <li><strong>Correo:</strong> {user.Email}</li>
                            <li><strong>Teléfono:</strong> {user.PhoneNumber}</li>
                            <li><strong>Apartamento:</strong> {apartment.Result!.Number}</li>
                        </ul>
                        <p style='font-size: 16px;'>Para habilitar el usuario, haga clic en el siguiente enlace:</p>
                        <div style='text-align: center; margin: 20px 0;'>
                            <a href='{tokenLink}' style='background-color: #8019fb; color: white; padding: 10px 20px; border-radius: 5px; text-decoration: none; font-weight: bold; display: inline-block;'>Confirmar Email</a>
                        </div>
                        <p style='font-size: 14px; color: #777;'>Si no estás seguro de esta operación, contacta con el usuario directamente o ignora este correo.</p>
                    </div>
                </div>
            </div>";

            return _mailHelper.SendMail(admin.Result!.FullName, admin.Result!.Email!, "CummUnity - Confirmación de cuenta", emailBody);

        }

        private async Task<ActionResponse<string>> SendConfirmationAdminEmailAsync(UserDTO user)
        {
            var myToken = await _usersUnitOfWork.GenerateEmailConfirmationTokenAsync(user);
            var tokenLink = Url.Action("ConfirmEmail", "accounts", new
            {
                userid = user.Id,
                token = myToken
            }, HttpContext.Request.Scheme, _configuration["Url Frontend"]);

            string type;
            if (user.UserType == UserType.AdminResidentialUnit)
            {
                type = "Administrador";
            }
            else
            {
                type = "Trabajador";
            }

            var residentialUnit = await _residentialUnitUnitOfWork.GetAsync((int)user.ResidentialUnitId!);

            var emailBody = $@"
            <div style='font-family: Arial, sans-serif; padding: 20px; background-color: #f5f5f5;'>
                <div style='max-width: 600px; margin: auto; background: #fff; border-radius: 8px; box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);'>
                    <div style='background-color: #8019fb; color: white; padding: 10px; border-radius: 8px 8px 0 0;'>
                        <h1 style='margin: 0;'>CommUnity - Confirmación de cuenta</h1>
                    </div>
                    <div style='padding: 20px;'>
                        <p style='font-size: 16px; color: #333;'>Para habilitar el acceso a su cuenta de <strong>{type}</strong> en la unidad <strong>{residentialUnit.Result?.Name}</strong>, haga clic en el siguiente enlace:</p>
                        <div style='text-align: center; margin: 20px 0;'>
                            <a href='{tokenLink}' style='background-color: #8019fb; color: white; padding: 10px 20px; border-radius: 5px; text-decoration: none; font-weight: bold; display: inline-block;'>Confirmar Email</a>
                        </div>
                        <p style='font-size: 16px;'>Para acceder a su cuenta, utilice los siguientes datos:</p>
                        <ul style='font-size: 16px; line-height: 1.5; color: #333;'>
                            <li><strong>Email:</strong> {user.Email}</li>
                            <li><strong>Contraseña:</strong> {user.Password}</li>
                        </ul>
                        <p style='font-size: 14px; color: #777;'>Por motivos de seguridad, se recomienda cambiar su contraseña tras el primer inicio de sesión.</p>
                    </div>
                </div>
            </div>";

            return _mailHelper.SendMail(user.FullName, user.Email!, "CummUnity - Confirmación de cuenta", emailBody);

        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDTO model)
        {
            var result = await _usersUnitOfWork.LoginAsync(model);
            if (result.Succeeded)
            {
                var user = await _usersUnitOfWork.GetUserAsync(model.Email);
                return Ok(BuildToken(user));
            }

            if (result.IsLockedOut)
            {
                return BadRequest("Ha superado el máximo número de intentos, su cuenta está bloqueada, intente de nuevo en 5 minutos.");
            }

            if (result.IsNotAllowed)
            {
                return BadRequest("El usuario no ha sido habilitado, debes de seguir las instrucciones del correo enviado para poder habilitar el usuario.");
            }

            return BadRequest("Email o contraseña incorrectos.");
        }

        [HttpPost("changePassword")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ChangePasswordAsync(ChangePasswordDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _usersUnitOfWork.GetUserAsync(User.Identity!.Name!);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _usersUnitOfWork.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.FirstOrDefault()!.Description);
            }

            return NoContent();
        }

        private TokenDTO BuildToken(User user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.Email!),
                new(ClaimTypes.Role, user.UserType.ToString()),
                new("Document", user.Document),
                new("FirstName", user.FirstName),
                new("LastName", user.LastName),
                new("Address", user.Address),
                new("Photo", user.Photo ?? string.Empty),
                new("CityId", user.CityId.ToString()),
                new("ApartmentId", user.ApartmentId.ToString()!)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwtKey"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddDays(30);
            var token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: expiration,
                signingCredentials: credentials);

            return new TokenDTO
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }
    }
}