namespace Api.Conhecimento.Rag;

public class RagOptions
{
    public const string SectionName = "Rag";

    public string PastaDocumentos { get; set; } = "Documentos";

    public int TamanhoMaximoChunk { get; set; } = 800;

    public int QuantidadeResultados { get; set; } = 3;
}
