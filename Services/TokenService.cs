﻿namespace IAutor.Api.Services;

public interface ITokenService
{
    string GenerateToken(User token);
}

public sealed class TokenService(IOptions<Security> security) : ITokenService
{
    public string GenerateToken(User user)
    {
        List<Claim> claims =
        [
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.ToString()),
            new(ClaimTypes.Email, user.Email),
            new("type", user.Type.ToString())
        ];

        List<string> roles = ["Get", "Create", "Update"];
        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        if (user.Type == UserTypeEnum.Admin || user.Type == UserTypeEnum.Operator)
            claims.Add(new Claim(ClaimTypes.Role, "Delete"));

        if (user.Type == UserTypeEnum.Admin)
        {
            claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            claims.Add(new Claim(ClaimTypes.Role, "GetIncomes"));
        }

        if (user.Type == UserTypeEnum.Influencer)
        {
            claims.Add(new Claim(ClaimTypes.Role, "Influencer"));
            claims.Add(new Claim(ClaimTypes.Role, "GetIncomes"));
        }

        if (user.Type == UserTypeEnum.Agent)
        {
            claims.Add(new Claim(ClaimTypes.Role, "Agent"));
            claims.Add(new Claim(ClaimTypes.Role, "GetIncomes"));
        }

        var token = new JwtSecurityToken(
            claims: claims,
            signingCredentials: new(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(security.Value.Token)), SecurityAlgorithms.HmacSha256),
            audience: security.Value.Audience,
            issuer: security.Value.Issuer,
            expires: DateTime.Now.AddMinutes(security.Value.ExpirationTimeInMinutes)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

public static class TokenExtension
{
    public static long GetUserIdFromToken(HttpContext context)
    {
        var value = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        return long.Parse(!string.IsNullOrEmpty(value) ? value : "0");
    }

    public static string GetUserNameFromToken(HttpContext context)
    {
        var value = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        if (!string.IsNullOrEmpty(value) && value.Length > 50) value = value[..50];
        return value!;
    }

    public static string GetUserEmailFromToken(HttpContext context) => context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)!.Value;
}