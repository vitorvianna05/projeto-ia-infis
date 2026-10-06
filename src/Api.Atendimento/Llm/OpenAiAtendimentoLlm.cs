using System.Text.Json;
using Microsoft.Extensions.Options;
using OpenAI.Chat;

namespace Api.Atendimento.Llm;

public class OpenAiAtendimentoLlm : IAtendimentoLlm
{
    private const string EsquemaResposta = """
        {
          "type": "object",
          "properties": {
            "resposta": {
              "type": "string",
              "description": "Resposta à pergunta, baseada exclusivamente no contexto."
            },
            "categoria": {
              "type": "string",
              "description": "Categoria curta do assunto da pergunta (ex.: RH, TI, Institucional). Use 'Não identificado' se não houver dados suficientes."
            },
            "fontes": {
              "type": "array",
              "items": { "type": "string" },
              "description": "Nomes dos arquivos (valor de 'fonte:' no contexto) efetivamente usados na resposta. Lista vazia se não houver dados suficientes."
            }
          },
          "required": ["resposta", "categoria", "fontes"],
          "additionalProperties": false
        }
        """;

    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    private readonly ChatClient _client;
    private readonly string _modelo;
    private readonly ILogger<OpenAiAtendimentoLlm> _logger;

    public OpenAiAtendimentoLlm(IOptions<OpenAiOptions> options, ILogger<OpenAiAtendimentoLlm> logger)
    {
        _modelo = options.Value.Modelo;
        _client = new ChatClient(_modelo, options.Value.ApiKey);
        _logger = logger;
    }

    public async Task<RespostaLlm> ResponderAsync(string contexto, string pergunta, CancellationToken cancellationToken)
    {
        var completionOptions = new ChatCompletionOptions
        {
            ResponseFormat = ChatResponseFormat.CreateJsonSchemaFormat(
                jsonSchemaFormatName: "resposta_atendimento",
                jsonSchema: BinaryData.FromString(EsquemaResposta),
                jsonSchemaIsStrict: true)
        };

        ChatMessage[] mensagens = [new UserChatMessage(PromptAtendimento.Montar(contexto, pergunta))];
        var completion = (await _client.CompleteChatAsync(mensagens, completionOptions, cancellationToken)).Value;

        var json = completion.Content[0].Text;
        var estruturada = JsonSerializer.Deserialize<SaidaEstruturada>(json, JsonOptions)
            ?? throw new InvalidOperationException("A OpenAI retornou uma resposta estruturada vazia.");

        _logger.LogInformation("Resposta gerada pelo modelo {Modelo}.", completion.Model);
        return new RespostaLlm(estruturada.Resposta, estruturada.Categoria, estruturada.Fontes, completion.Model);
    }

    private record SaidaEstruturada(string Resposta, string Categoria, string[] Fontes);
}
