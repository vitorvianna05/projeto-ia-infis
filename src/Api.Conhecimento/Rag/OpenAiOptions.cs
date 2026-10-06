using System.ComponentModel.DataAnnotations;

namespace Api.Conhecimento.Rag;

public class OpenAiOptions
{
    public const string SectionName = "OpenAI";

    // Fornecida por User Secrets ou variável de ambiente (OpenAI__ApiKey); nunca em arquivo versionado.
    [Required]
    public string ApiKey { get; set; } = default!;

    [Required]
    public string ModeloEmbedding { get; set; } = "text-embedding-3-small";
}
