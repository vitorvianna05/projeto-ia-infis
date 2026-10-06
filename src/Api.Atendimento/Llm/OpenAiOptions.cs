using System.ComponentModel.DataAnnotations;

namespace Api.Atendimento.Llm;

public class OpenAiOptions
{
    public const string SectionName = "OpenAI";

    // Fornecida por User Secrets ou variável de ambiente (OpenAI__ApiKey); nunca em arquivo versionado.
    [Required]
    public string ApiKey { get; set; } = default!;

    [Required]
    public string Modelo { get; set; } = "gpt-4o-mini";
}
