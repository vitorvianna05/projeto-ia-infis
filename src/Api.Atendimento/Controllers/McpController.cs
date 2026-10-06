using Api.Atendimento.Mcp;
using Microsoft.AspNetCore.Mvc;

namespace Api.Atendimento.Controllers;

[ApiController]
[Route("api/mcp")]
public class McpController(IConhecimentoClient conhecimento) : ControllerBase
{
    [HttpGet("ferramentas")]
    public async Task<ActionResult<IReadOnlyList<FerramentaMcp>>> ListarFerramentas(CancellationToken cancellationToken)
        => Ok(await conhecimento.ListarFerramentasAsync(cancellationToken));

    [HttpGet("buscar")]
    public async Task<ActionResult<string>> Buscar([FromQuery] string consulta, CancellationToken cancellationToken)
        => Ok(await conhecimento.BuscarConhecimentoAsync(consulta, cancellationToken));
}
