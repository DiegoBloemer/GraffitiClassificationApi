using Microsoft.EntityFrameworkCore;
using GraffitiClassificationApi.Api.Data;
using GraffitiClassificationApi.Api.Services;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

var builder = WebApplication.CreateBuilder(args);

// --- Serviços ---

// Registra os controllers tradicionais (sem Minimal APIs)
builder.Services.AddControllers();

var invariant = CultureInfo.InvariantCulture;
CultureInfo.DefaultThreadCurrentCulture = invariant;
CultureInfo.DefaultThreadCurrentUICulture = invariant;

// Configura o AppDbContext com o provider PostgreSQL (Npgsql),
// lendo a connection string "DefaultConnection" do appsettings.json
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registra o serviço de armazenamento MinIO
builder.Services.AddSingleton<IStorageService, MinioStorageService>();

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

// Política de CORS: em Development libera qualquer origem; em outros ambientes
// lê "AllowedOrigins" do appsettings (array de strings).
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTudo", policy =>
        {
        var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();

        if (builder.Environment.IsDevelopment() || allowedOrigins is null || allowedOrigins.Length == 0)
            policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        else
            policy.WithOrigins(allowedOrigins).AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(invariant),
    SupportedCultures = new[] { invariant },
    SupportedUICultures = new[] { invariant }
});

// Garante que o bucket do MinIO existe
using (var scope = app.Services.CreateScope())
{
    var storageService = scope.ServiceProvider.GetRequiredService<IStorageService>();
    await storageService.EnsureBucketExistsAsync();
}

// --- Pipeline de requisições ---

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("PermitirTudo");

app.UseAuthorization();

// Mapeia as rotas para os controllers
app.MapControllers();

app.Run();
