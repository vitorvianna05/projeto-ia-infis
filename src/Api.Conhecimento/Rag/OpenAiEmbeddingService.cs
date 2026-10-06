using Microsoft.Extensions.Options;
using OpenAI.Embeddings;

namespace Api.Conhecimento.Rag;

public class OpenAiEmbeddingService(IOptions<OpenAiOptions> options) : IEmbeddingService
{
    private readonly EmbeddingClient _client = new(options.Value.ModeloEmbedding, options.Value.ApiKey);

    public async Task<IReadOnlyList<float[]>> GerarAsync(IReadOnlyList<string> textos, CancellationToken cancellationToken)
    {
        if (textos.Count == 0)
        {
            return [];
        }

        var resposta = await _client.GenerateEmbeddingsAsync(textos, cancellationToken: cancellationToken);
        return resposta.Value.OrderBy(e => e.Index).Select(e => e.ToFloats().ToArray()).ToList();
    }
}
