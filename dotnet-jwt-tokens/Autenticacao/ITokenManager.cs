using dotnet_jwt_tokens.Db;

namespace dotnet_jwt_tokens.Autenticacao;

public interface ITokenManager
{
    string GerarToken(Heroi heroi);
    
    string GerarRefreshToken(Heroi heroi);
    
    Task<(bool isValid, string? nomeHeroi)> ValidateTokenAsync(string token); 
}