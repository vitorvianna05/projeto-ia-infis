namespace Api.Atendimento.Llm;

public static class PromptAtendimento
{
    private const string Modelo = """
        Você é um agente corporativo.

        Responda utilizando exclusivamente
        o contexto fornecido.

        Caso o contexto não contenha a informação,
        informe que não encontrou dados suficientes.

        CONTEXTO:

        {contexto}

        PERGUNTA:

        {pergunta}
        """;

    public static string Montar(string contexto, string pergunta)
        => Modelo.Replace("{contexto}", contexto).Replace("{pergunta}", pergunta);
}
