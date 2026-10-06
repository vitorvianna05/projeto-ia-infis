using Api.Atendimento.Llm;
using Api.Atendimento.Mcp;

namespace Api.Atendimento.Atendimento;

public interface IAtendimentoService
{
    Task<RespostaAtendimento> ResponderAsync(string pergunta, CancellationToken cancellationToken);
}

public class AtendimentoService(IConhecimentoClient conhecimento, IAtendimentoLlm llm) : IAtendimentoService
{
    public async Task<RespostaAtendimento> ResponderAsync(string pergunta, CancellationToken cancellationToken)
    {
        var contexto = await conhecimento.BuscarConhecimentoAsync(pergunta, cancellationToken);
        var resposta = await llm.ResponderAsync(contexto, pergunta, cancellationToken);

        // Tokens e custo serão preenchidos na Etapa 6; o fallback (Gemini) na Etapa 7.
        return new RespostaAtendimento(
            resposta.Resposta,
            resposta.Categoria,
            resposta.Fontes,
            resposta.Modelo,
            Fallback: false,
            TokensEntrada: 0,
            TokensSaida: 0,
            CustoEstimado: 0m);
    }
}
