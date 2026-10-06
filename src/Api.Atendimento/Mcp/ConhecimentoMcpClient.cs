using Microsoft.Extensions.Options;
using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol;

namespace Api.Atendimento.Mcp;

public class ConhecimentoMcpClient(
    IOptions<ConhecimentoMcpOptions> options,
    HttpClient httpClient,
    ILoggerFactory loggerFactory) : IConhecimentoClient
{
    private const string NomeFerramentaBusca = "BuscarConhecimento";

    private readonly ILogger _logger = loggerFactory.CreateLogger<ConhecimentoMcpClient>();

    public async Task<IReadOnlyList<FerramentaMcp>> ListarFerramentasAsync(CancellationToken cancellationToken)
    {
        await using var client = await CriarClienteAsync(cancellationToken);
        var ferramentas = await client.ListToolsAsync(cancellationToken: cancellationToken);

        _logger.LogInformation("{Quantidade} ferramenta(s) MCP descoberta(s).", ferramentas.Count);
        return ferramentas.Select(f => new FerramentaMcp(f.Name, f.Description)).ToList();
    }

    public async Task<string> BuscarConhecimentoAsync(string consulta, CancellationToken cancellationToken)
    {
        await using var client = await CriarClienteAsync(cancellationToken);
        var resultado = await client.CallToolAsync(
            NomeFerramentaBusca,
            new Dictionary<string, object?> { ["consulta"] = consulta },
            cancellationToken: cancellationToken);

        if (resultado.IsError == true)
        {
            throw new InvalidOperationException($"A ferramenta MCP '{NomeFerramentaBusca}' retornou erro.");
        }

        return string.Concat(resultado.Content.OfType<TextContentBlock>().Select(c => c.Text));
    }

    private Task<McpClient> CriarClienteAsync(CancellationToken cancellationToken)
    {
        var transport = new HttpClientTransport(
            new HttpClientTransportOptions { Endpoint = options.Value.Endpoint },
            httpClient,
            loggerFactory,
            ownsHttpClient: false);

        return McpClient.CreateAsync(transport, loggerFactory: loggerFactory, cancellationToken: cancellationToken);
    }
}
