using Api.Conhecimento.Rag;

namespace Api.Conhecimento.Tests;

public class CosineSimilarityTests
{
    [Fact]
    public void VetoresIdenticos_RetornamUm()
    {
        var r = CosineSimilarity.Calcular([1f, 2f, 3f], [1f, 2f, 3f]);
        Assert.Equal(1f, r, 5);
    }

    [Fact]
    public void VetoresProporcionais_RetornamUm()
    {
        var r = CosineSimilarity.Calcular([1f, 2f, 3f], [2f, 4f, 6f]);
        Assert.Equal(1f, r, 5);
    }

    [Fact]
    public void VetoresOrtogonais_RetornamZero()
    {
        var r = CosineSimilarity.Calcular([1f, 0f], [0f, 1f]);
        Assert.Equal(0f, r, 5);
    }

    [Fact]
    public void VetoresOpostos_RetornamMenosUm()
    {
        var r = CosineSimilarity.Calcular([1f, 2f], [-1f, -2f]);
        Assert.Equal(-1f, r, 5);
    }

    [Fact]
    public void ValorConhecido_RetornaResultadoEsperado()
    {
        // (1*2 + 0*1) / (1 * sqrt(5)) = 2 / sqrt(5)
        var r = CosineSimilarity.Calcular([1f, 0f], [2f, 1f]);
        Assert.Equal(2f / MathF.Sqrt(5f), r, 5);
    }

    [Fact]
    public void VetorZerado_RetornaZero()
    {
        var r = CosineSimilarity.Calcular([0f, 0f], [1f, 2f]);
        Assert.Equal(0f, r);
    }

    [Fact]
    public void DimensoesDiferentes_LancaExcecao()
    {
        Assert.Throws<ArgumentException>(() => CosineSimilarity.Calcular([1f, 2f], [1f, 2f, 3f]));
    }

    [Fact]
    public void VectorStore_RetornaOsMaisSimilaresEmOrdem()
    {
        var store = new InMemoryVectorStore();
        store.Adicionar([
            new ChunkIndexado(new Chunk("a.md", "distante"), [0f, 1f]),
            new ChunkIndexado(new Chunk("b.md", "proximo"), [1f, 0.1f]),
            new ChunkIndexado(new Chunk("c.md", "medio"), [1f, 1f]),
            new ChunkIndexado(new Chunk("d.md", "identico"), [1f, 0f]),
        ]);

        var r = store.Buscar([1f, 0f], 3);

        Assert.Equal(["identico", "proximo", "medio"], r.Select(x => x.Chunk.Texto));
    }
}
