namespace dotnet_jwt_tokens.Db;

internal sealed class Heroi
{
    public string Nome { get; init; }
    public Poder[] Poderes { get; init; }
    
    public Heroi(string nome, Poder[] poderes)
    {
        Nome = nome;
        Poderes = poderes;
    }
}