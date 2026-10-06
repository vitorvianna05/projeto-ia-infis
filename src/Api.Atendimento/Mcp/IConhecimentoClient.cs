namespace Api.Atendimento.Mcp;

public record FerramentaMcp(string Nome, string? Descricao);

public interface IConhecimentoClient
{
    Task<IReadOnlyList<FerramentaMcp>> ListarFerramentasAsync(CancellationToken cancellationToken);

    Task<string> BuscarConhecimentoAsync(string consulta, CancellationToken cancellationToken);
}
