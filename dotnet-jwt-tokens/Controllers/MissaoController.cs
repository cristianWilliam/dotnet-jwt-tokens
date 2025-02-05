using dotnet_jwt_tokens.Autenticacao;
using dotnet_jwt_tokens.Db;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_jwt_tokens.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class MissaoController : ControllerBase
{
    private readonly ITokenManager _tokenManager;

    public MissaoController(ITokenManager tokenManager)
    {
        _tokenManager = tokenManager;
    }
    
    [HttpGet("hello-word")]
    public string HelloWord() => "Hello World!";

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        // Verificar se veio mesmo o heroi
        if (string.IsNullOrWhiteSpace(request.Heroi))
            return NotFound();
        
        // Tentar obter heroi
        var heroi = BancoHerois.Herois.FirstOrDefault(x => x.Nome == request.Heroi);

        if (heroi is null)
            return NotFound();
        
        // Gerar token para esse heroi
        var token = _tokenManager.GerarToken(heroi);
        var refreshToken = _tokenManager.GerarRefreshToken(heroi);
        
        return Ok(new LoginResponse(token, refreshToken));
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.RefreshToken))
            return BadRequest();
        
        var isValidTokenResult = await _tokenManager.ValidateTokenAsync(request.RefreshToken);

        if (!isValidTokenResult.isValid)
            return Unauthorized();

        var nomeHeroi = isValidTokenResult.nomeHeroi;
        var heroi = BancoHerois.Herois.FirstOrDefault(x => x.Nome == nomeHeroi);

        if (heroi is null)
            return Unauthorized();
        
        // Gerar tokens novamnete
        var token = _tokenManager.GerarToken(heroi);
        var refreshToken = _tokenManager.GerarRefreshToken(heroi);

        return Ok(new LoginResponse(token, refreshToken));
    }

    [Authorize]
    [HttpGet("somente-heroi")]
    public string SomenteHeroi() => "Voce é heroi";
    
    [Authorize(Roles = BancoHerois.PODE_VOAR)]
    [HttpGet("pode-voar")]
    public string PodeVoar() => "Missao completa";
    
    [Authorize(Roles = BancoHerois.SUPER_INTELIGENTE)]
    [HttpGet("investigar-crime")]
    public string InvestigarCrime() => "Missao completa";
}

public record LoginRequest(string Heroi);
public record RefreshTokenRequest(string RefreshToken);

public record LoginResponse(string Token, string RefreshToken);