namespace Api.Conhecimento.Rag;

public interface IEmbeddingService
{
    Task<IReadOnlyList<float[]>> GerarAsync(IReadOnlyList<string> textos, CancellationToken cancellationToken);
}
