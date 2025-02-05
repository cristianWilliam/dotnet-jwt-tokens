using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace dotnet_jwt_tokens.Autenticacao;

internal static class TokenHelpers
{
    public static TokenValidationParameters GetTokenValidationParameters(IConfiguration configuration)
    {
        var tokenKey = Encoding.UTF8.GetBytes(configuration["JwtSettings:SecretKey"] ?? string.Empty);

        return new TokenValidationParameters()
        {
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            ValidateAudience = true,
            ValidAudience = configuration["JwtSettings:Audience"],
            ValidateIssuer = true,
            ValidIssuer = configuration["JwtSettings:Issuer"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(tokenKey),
        };
    }
}