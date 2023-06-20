using Microsoft.IdentityModel.Tokens;
using NewsApp.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace NewsApp.MiddleWare;
public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task Invoke(HttpContext context, IUserServices userService)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token != null)
            await AttachUserToContext(context, userService, token);
        await _next(context);
    }

    private static async Task AttachUserToContext(HttpContext context, IUserServices userService, string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("SECRETSECRETSECRETSECRETSECRETSECRETSECRETSECRETSECRETSECRET");
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = jwtToken.Claims.First(x => x.Type == "id").Value;

            context.Items["UserId"] = userId;
            context.Items["User"] = await userService.GetUser();

        }
        catch
        {

        }
    }

}