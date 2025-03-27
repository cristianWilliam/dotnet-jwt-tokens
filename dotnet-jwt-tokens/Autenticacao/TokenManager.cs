using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using dotnet_jwt_tokens.Db;
using Microsoft.IdentityModel.Tokens;

namespace dotnet_jwt_tokens.Autenticacao;

internal sealed class TokenManager : ITokenManager
{
    private readonly IConfiguration _configuration;

    public TokenManager(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GerarToken(Heroi heroi)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8
            .GetBytes(jwtSettings["SecretKey"] ?? string.Empty));
        
        // Informações contidas no token
        var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Sub, heroi.Nome),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };
        
        foreach (var poder in heroi.Poderes)
        {
            claims.Add(new Claim(ClaimTypes.Role, poder.Descricao));
        }
        
        // Tempo de expiração.
        var tempoExpiracaoInMinutes = jwtSettings.GetValue<int>("ExpirationTimeInMinutes");
        
        // Montando token
        var token = new JwtSecurityToken(
            issuer: jwtSettings.GetValue<string>("Issuer"), // Quem emite
            audience: jwtSettings.GetValue<string>("Audience"), // Quem consume
            claims: claims,
            expires: DateTime.UtcNow.AddSeconds(5),
            signingCredentials: new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256));
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GerarRefreshToken(Heroi heroi)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8
            .GetBytes(jwtSettings["SecretKey"] ?? string.Empty));
        
        // Informações contidas no token
        var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Sub, heroi.Nome),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };
        
        // Tempo de expiração.
        var tempoExpiracaoInMinutes = jwtSettings.GetValue<int>("RefreshExpirationTimeInMinutes");
        
        // Montando token
        var token = new JwtSecurityToken(
            issuer: jwtSettings.GetValue<string>("Issuer"), // Quem emite
            audience: jwtSettings.GetValue<string>("Audience"), // Quem consume
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(tempoExpiracaoInMinutes),
            signingCredentials: new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256));
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<(bool isValid, string? nomeHeroi)> ValidateTokenAsync(string token)
    {
        if(string.IsNullOrWhiteSpace(token))
            return (false, null);
        
        var tokenParameters = TokenHelpers.GetTokenValidationParameters(_configuration);
        var validTokenResult = await new JwtSecurityTokenHandler().ValidateTokenAsync(token, tokenParameters);
        
        if (!validTokenResult.IsValid)
            return (false, null);
        
        var userName = validTokenResult
            .Claims.FirstOrDefault(c => c.Key == ClaimTypes.NameIdentifier).Value as string;

        return (true, userName);
    }
}