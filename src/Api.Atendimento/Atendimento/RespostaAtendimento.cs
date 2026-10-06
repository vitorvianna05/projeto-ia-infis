namespace Api.Atendimento.Atendimento;

public record RespostaAtendimento(
    string Resposta,
    string Categoria,
    string[] Fontes,
    string Modelo,
    bool Fallback,
    int TokensEntrada,
    int TokensSaida,
    decimal CustoEstimado);

public record PerguntaRequest(string Pergunta);
