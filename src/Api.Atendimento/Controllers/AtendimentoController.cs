using Api.Atendimento.Atendimento;
using Microsoft.AspNetCore.Mvc;

namespace Api.Atendimento.Controllers;

[ApiController]
[Route("api/atendimento")]
public class AtendimentoController(IAtendimentoService atendimento) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<RespostaAtendimento>> Perguntar(
        [FromBody] PerguntaRequest request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Pergunta))
        {
            return BadRequest("A pergunta é obrigatória.");
        }

        return Ok(await atendimento.ResponderAsync(request.Pergunta, cancellationToken));
    }
}
