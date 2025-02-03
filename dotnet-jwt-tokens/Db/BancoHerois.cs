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

    private static Poder PodeVoar() => new("pode-voar");
    private static Poder SuperVelocidade() => new("super-velocidade");
    private static Poder SuperInteligente() => new("super-inteligente");
}