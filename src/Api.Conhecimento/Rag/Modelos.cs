namespace Api.Conhecimento.Rag;

public record Chunk(string Fonte, string Texto);

public record ChunkIndexado(Chunk Chunk, float[] Embedding);

public record ResultadoBusca(Chunk Chunk, float Similaridade);
