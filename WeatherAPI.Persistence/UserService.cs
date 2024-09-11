using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WeatherAPI.Application;

namespace WeatherAPI.Persistence
{
    public class UserService(ApplicationDbContext context, UserManager<IdentityUser> userManager) : IUserService
    {
        public string Login(string username, string password)
        {
            var user = context.Users.SingleOrDefault(u => u.UserName == username);

            if (user is null)
                return "User not found!";

            var result = userManager.CheckPasswordAsync(user, password).Result;
            if (!result)
                return "Invalid password!";

            List<Claim> userRoles = userManager.GetRolesAsync(user).Result.Select(role => new Claim(ClaimTypes.Role, role)).ToList();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SecurityKeyFromConfigAndIHaveToMakeItEvenLongerToWorkProperly"));
            var jwt = new JwtSecurityToken(
                audience: "Users",
                issuer: "WeatherApp",
                claims: userRoles,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256),
                expires: DateTime.Now.AddMinutes(30)
            );
            string token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return "Bearer " + token;
        }
    }
}
