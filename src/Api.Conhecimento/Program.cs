using Api.Conhecimento.Rag;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOptions<RagOptions>().BindConfiguration(RagOptions.SectionName);
builder.Services.AddOptions<OpenAiOptions>()
    .BindConfiguration(OpenAiOptions.SectionName)
    .ValidateDataAnnotations()
    .ValidateOnStart();
builder.Services.AddSingleton<IChunker, ParagraphChunker>();
builder.Services.AddSingleton<IEmbeddingService, OpenAiEmbeddingService>();
builder.Services.AddSingleton<IVectorStore, InMemoryVectorStore>();
builder.Services.AddSingleton<IConhecimentoService, ConhecimentoService>();

builder.Services.AddMcpServer()
    .WithHttpTransport()
    .WithToolsFromAssembly();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapMcp("/mcp");

app.Run();


