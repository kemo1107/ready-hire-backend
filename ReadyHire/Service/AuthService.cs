using ReadyHire.Helper;
using ReadyHire.Models.Authentication;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Web;

namespace ReadyHire.Service
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly emailsettings  emailSettings;


        private readonly Jwt _jwt;
        public AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<Jwt> jwt, IOptions<emailsettings> options)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            emailSettings = options.Value;
            _jwt = jwt.Value;
        }

        //registration
        public async Task<AuthModel> RegisterAsync(RegisterModel model)
        {
            if (await _userManager.FindByEmailAsync(model.Email) != null)
                return new AuthModel { Message="Email is already registerd",IsAuthenticated=false };

            if (await _userManager.FindByNameAsync(model.UserName) is not null)
                return new AuthModel { Message = "User Name is already used" };

            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
            };

            var resut = await _userManager.CreateAsync(user,model.Password);
            if (!resut.Succeeded) 
            {
                var errors= string.Empty;
                foreach (var error in resut.Errors) 
                {
                    errors += $"{error.Description}";
                }

                return new AuthModel { Message = errors };

            }
            await _userManager.AddToRoleAsync(user, "User");

            var jwtSecurityToken = await CreateJwtToken(user);
            return new AuthModel
            {
                Email=user.Email,
                Expireson=jwtSecurityToken.ValidTo,
                IsAuthenticated=true,
                Roles=new List<string> {"User"},
                Token=new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                UserName=user.UserName,
                UserId=user.Id
            };
        }

        //login
        public async Task<AuthModel> LoginAsync(LoginModel model) 
        {
            var authmodel = new AuthModel();
            var user= await _userManager.FindByEmailAsync(model.Email);
            if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password)) 
            {
                authmodel.Message = "Email or Password is InCorrect";
                return authmodel;
            }

            var jwtSecurityToken = await CreateJwtToken(user);
            var roleslist = await _userManager.GetRolesAsync(user);

            authmodel.IsAuthenticated = true;
            authmodel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authmodel.Email = user.Email;
            authmodel.UserName = user.UserName;
            authmodel.Expireson = jwtSecurityToken.ValidTo;
            authmodel.Roles = roleslist.ToList();
            authmodel.UserId = user.Id;
            authmodel.UserProfileId = user.UserProfileId;           // ✅ أضف دي
            authmodel.CompanyProfileId = user.CompanyProfilesId;    // ✅ وأضف دي


            return authmodel;
        }

        //addroles
        public async Task<string> AddrolesAsync(AddRoleModel model)
        {
            
            var user = await _userManager.FindByIdAsync(model.userid);

            if (user is null || !await _roleManager.RoleExistsAsync(model.role))
                return "Invalid Userid or role";


            if (await _userManager.IsInRoleAsync(user, model.role))
                return "user already assigned to this role";

            var result = await _userManager.AddToRoleAsync(user, model.role);

            return result.Succeeded ? string.Empty : "something wrong";
        }

        //sendrequest

        public async Task SendEmailAsync(mailrequest mailrequest, string? token = null)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(emailSettings.Email);
            email.To.Add(MailboxAddress.Parse(mailrequest.ToEmail));
            email.Subject = mailrequest.Subject;

            var builder = new BodyBuilder();

            if (!string.IsNullOrEmpty(token))
            {
                builder.TextBody = $"Your authentication token is: {token}";
            }
            else
            {
                builder.HtmlBody = mailrequest.Body;
            }

            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect(emailSettings.Host, emailSettings.Port, SecureSocketOptions.SslOnConnect);
            smtp.Authenticate(emailSettings.Email, emailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }


        //send token

         public async Task<string> SendTokenToEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return "Email is not registered";

            // 🔹 استخدم Password Reset Token بدلاً من JWT Token
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            token = HttpUtility.UrlEncode(token); // ✅ تشفير التوكين عند الإرسال

            var mailRequest = new mailrequest
            {
                ToEmail = email,
                Subject = "Your Authentication Token",
                Body = $"Use this token to reset your password: {token}"
            };

            await SendEmailAsync(mailRequest, token);
            return "Token sent to your email";
        }


        //reset password
        public async Task<string> ResetPasswordAsync(resetpassword model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null)
                return "Invalid email or token";

            var decodedToken = HttpUtility.UrlDecode(model.Token); 

            var result = await _userManager.ResetPasswordAsync(user, decodedToken, model.NewPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return $"Error: {errors}";
            }

            return "Password has been Update successfully";
        }




        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }
    }
}
