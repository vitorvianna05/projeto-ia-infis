using Microsoft.Extensions.Options;

namespace Api.Conhecimento.Rag;

public interface IConhecimentoService
{
    Task<IReadOnlyList<ResultadoBusca>> BuscarAsync(string consulta, CancellationToken cancellationToken);
}

public class ConhecimentoService(
    IChunker chunker,
    IEmbeddingService embeddings,
    IVectorStore store,
    IOptions<RagOptions> options,
    ILogger<ConhecimentoService> logger) : IConhecimentoService
{
    private readonly SemaphoreSlim _indexacao = new(1, 1);
    private bool _indexado;

    public async Task<IReadOnlyList<ResultadoBusca>> BuscarAsync(string consulta, CancellationToken cancellationToken)
    {
        await GarantirIndexacaoAsync(cancellationToken);

        var vetores = await embeddings.GerarAsync([consulta], cancellationToken);
        return store.Buscar(vetores[0], options.Value.QuantidadeResultados);
    }

    private async Task GarantirIndexacaoAsync(CancellationToken cancellationToken)
    {
        if (_indexado)
        {
            return;
        }

        await _indexacao.WaitAsync(cancellationToken);
        try
        {
            if (_indexado)
            {
                return;
            }

            var pasta = Path.Combine(AppContext.BaseDirectory, options.Value.PastaDocumentos);
            var chunks = new List<Chunk>();

            if (Directory.Exists(pasta))
            {
                foreach (var arquivo in Directory.EnumerateFiles(pasta, "*.md", SearchOption.AllDirectories))
                {
                    var texto = await File.ReadAllTextAsync(arquivo, cancellationToken);
                    var nome = Path.GetRelativePath(pasta, arquivo);
                    chunks.AddRange(chunker.Dividir(texto).Select(t => new Chunk(nome, t)));
                }
            }
            else
            {
                logger.LogWarning("Pasta de documentos não encontrada: {Pasta}", pasta);
            }

            var vetores = await embeddings.GerarAsync(chunks.Select(c => c.Texto).ToList(), cancellationToken);
            store.Adicionar(chunks.Zip(vetores, (c, v) => new ChunkIndexado(c, v)));

            logger.LogInformation("{Quantidade} chunk(s) indexado(s) em memória.", store.Quantidade);
            _indexado = true;
        }
        finally
        {
            _indexacao.Release();
        }
    }
}
