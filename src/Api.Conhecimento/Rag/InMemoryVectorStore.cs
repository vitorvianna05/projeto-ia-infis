namespace Api.Conhecimento.Rag;

public interface IVectorStore
{
    int Quantidade { get; }

    void Adicionar(IEnumerable<ChunkIndexado> itens);

    IReadOnlyList<ResultadoBusca> Buscar(float[] consulta, int quantidade);
}

public class InMemoryVectorStore : IVectorStore
{
    private readonly List<ChunkIndexado> _itens = [];
    private readonly Lock _lock = new();

    public int Quantidade
    {
        get { lock (_lock) { return _itens.Count; } }
    }

    public void Adicionar(IEnumerable<ChunkIndexado> itens)
    {
        lock (_lock)
        {
            _itens.AddRange(itens);
        }
    }

    public IReadOnlyList<ResultadoBusca> Buscar(float[] consulta, int quantidade)
    {
        ChunkIndexado[] copia;
        lock (_lock)
        {
            copia = [.. _itens];
        }

        return copia
            .Select(i => new ResultadoBusca(i.Chunk, CosineSimilarity.Calcular(consulta, i.Embedding)))
            .OrderByDescending(r => r.Similaridade)
            .Take(quantidade)
            .ToList();
    }
}
