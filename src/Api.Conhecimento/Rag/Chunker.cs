namespace Api.Conhecimento.Rag;

public interface IChunker
{
    IReadOnlyList<string> Dividir(string texto);
}

public class ParagraphChunker(Microsoft.Extensions.Options.IOptions<RagOptions> options) : IChunker
{
    public IReadOnlyList<string> Dividir(string texto)
    {
        var maximo = options.Value.TamanhoMaximoChunk;
        var chunks = new List<string>();
        var atual = new System.Text.StringBuilder();

        var paragrafos = texto.Replace("\r\n", "\n")
            .Split("\n\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        foreach (var paragrafo in paragrafos)
        {
            if (atual.Length > 0 && atual.Length + paragrafo.Length + 2 > maximo)
            {
                chunks.Add(atual.ToString());
                atual.Clear();
            }

            if (paragrafo.Length > maximo)
            {
                for (var i = 0; i < paragrafo.Length; i += maximo)
                {
                    chunks.Add(paragrafo.Substring(i, Math.Min(maximo, paragrafo.Length - i)));
                }
                continue;
            }

            if (atual.Length > 0)
            {
                atual.Append("\n\n");
            }
            atual.Append(paragrafo);
        }

        if (atual.Length > 0)
        {
            chunks.Add(atual.ToString());
        }

        return chunks;
    }
}
