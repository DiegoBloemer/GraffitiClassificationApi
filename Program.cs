using Microsoft.EntityFrameworkCore;
using GraffitiClassificationApi.Api.Data;

var builder = WebApplication.CreateBuilder(args);

// --- Serviços ---

// Registra os controllers tradicionais (sem Minimal APIs)
builder.Services.AddControllers();

// Configura o AppDbContext com o provider PostgreSQL (Npgsql),
// lendo a connection string "DefaultConnection" do appsettings.json
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configura o Swagger para documentação automática da API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "GraffitiClassification API",
        Version = "v1",
        Description = "API para mapeamento e classificação de pichações ligadas à demarcação de território de facções criminosas."
    });

    // Inclui os comentários XML gerados pelo compilador na documentação do Swagger
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

// Política de CORS "PermitirTudo": libera qualquer origem, método e cabeçalho
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTudo", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

var app = builder.Build();

// --- Pipeline de requisições ---

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("PermitirTudo");

// Habilita o serviço de arquivos estáticos a partir de wwwroot/
// Necessário para que as imagens salvas em wwwroot/imagens sejam acessíveis via URL
app.UseStaticFiles();

app.UseAuthorization();

// Mapeia as rotas para os controllers
app.MapControllers();

app.Run();
