using System.ComponentModel.DataAnnotations;

namespace Api.Atendimento.Mcp;

public class ConhecimentoMcpOptions
{
    public const string SectionName = "Conhecimento:Mcp";

    [Required]
    public Uri Endpoint { get; set; } = default!;
}
