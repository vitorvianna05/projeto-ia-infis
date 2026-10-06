namespace Api.Atendimento.Llm;

public record RespostaLlm(string Resposta, string Categoria, string[] Fontes, string Modelo);

public interface IAtendimentoLlm
{
    Task<RespostaLlm> ResponderAsync(string contexto, string pergunta, CancellationToken cancellationToken);
}
