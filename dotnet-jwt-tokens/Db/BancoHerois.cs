namespace dotnet_jwt_tokens.Db;

internal static class BancoHerois
{
    public static Heroi[] Herois = ObterHerois();

    private static Heroi[] ObterHerois()
    {
        var batman = new Heroi("Batman", [SuperInteligente()]);
        var superMan = new Heroi("Super-Man", [PodeVoar()]);
        var flash = new Heroi("Flash", [SuperVelocidade()]);
        
        return [batman, superMan, flash];
    }

    private static Poder PodeVoar() => new(PODE_VOAR);
    private static Poder SuperVelocidade() => new(SUPER_VELOCIDADE);
    private static Poder SuperInteligente() => new(SUPER_INTELIGENTE);
    
    public const string SUPER_INTELIGENTE = "Super-Inteligente";
    public const string PODE_VOAR = "pode-voar";
    public const string SUPER_VELOCIDADE = "Super-Velocidade";
}