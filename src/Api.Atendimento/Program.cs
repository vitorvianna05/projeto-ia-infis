using Api.Atendimento.Atendimento;
using Api.Atendimento.Llm;
using Api.Atendimento.Mcp;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOptions<ConhecimentoMcpOptions>()
    .BindConfiguration(ConhecimentoMcpOptions.SectionName)
    .ValidateDataAnnotations()
    .ValidateOnStart();
builder.Services.AddHttpClient<IConhecimentoClient, ConhecimentoMcpClient>();

builder.Services.AddOptions<OpenAiOptions>()
    .BindConfiguration(OpenAiOptions.SectionName)
    .ValidateDataAnnotations()
    .ValidateOnStart();
builder.Services.AddSingleton<IAtendimentoLlm, OpenAiAtendimentoLlm>();
builder.Services.AddScoped<IAtendimentoService, AtendimentoService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();


