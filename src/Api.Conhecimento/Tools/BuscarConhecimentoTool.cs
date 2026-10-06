using System.ComponentModel;
using System.Text;
using Api.Conhecimento.Rag;
using ModelContextProtocol.Server;

namespace Api.Conhecimento.Tools;

[McpServerToolType]
public class BuscarConhecimentoTool(IConhecimentoService conhecimento)
{
    [McpServerTool(Name = "BuscarConhecimento")]
    [Description("Busca conhecimento corporativo relevante para a consulta informada.")]
    public async Task<string> BuscarConhecimentoAsync(
        [Description("Pergunta ou termos da busca.")] string consulta,
        CancellationToken cancellationToken)
    {
        var resultados = await conhecimento.BuscarAsync(consulta, cancellationToken);
        if (resultados.Count == 0)
        {
            return "Nenhum conhecimento encontrado.";
        }

        var saida = new StringBuilder();
        foreach (var r in resultados)
        {
            saida.AppendLine($"[fonte: {r.Chunk.Fonte} | similaridade: {r.Similaridade:F3}]");
            saida.AppendLine(r.Chunk.Texto);
            saida.AppendLine("---");
        }
        return saida.ToString().TrimEnd();
    }
}
