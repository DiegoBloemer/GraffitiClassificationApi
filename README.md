# 🎯 Graffiti Classification API

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-16-336791?logo=postgresql)](https://www.postgresql.org/)
[![Entity Framework](https://img.shields.io/badge/EF%20Core-8.0-512BD4)](https://docs.microsoft.com/ef/core/)
[![Swagger](https://img.shields.io/badge/Swagger-OpenAPI-85EA2D?logo=swagger)](https://swagger.io/)
[![License](https://img.shields.io/badge/license-Academic-blue)](LICENSE)

> API REST para mapeamento e classificação de pichações relacionadas à demarcação territorial de facções criminosas.

## 📋 Índice

- [Sobre](#-sobre)
- [Funcionalidades](#-funcionalidades)
- [Tecnologias](#-tecnologias)
- [Arquitetura](#-arquitetura)
- [Instalação](#-instalação)
- [Uso](#-uso)
- [API Endpoints](#-api-endpoints)
- [Modelo de Dados](#-modelo-de-dados)
- [Documentação](#-documentação)
- [Contribuindo](#-contribuindo)

## 🎯 Sobre

Sistema backend desenvolvido em .NET 8 para gerenciamento de ocorrências de pichações vinculadas a facções criminosas. Permite cadastro, classificação por nível de ameaça, upload de imagens e registro de localização geográfica completa.

**Desenvolvido para:** Universidade Federal de Santa Catarina (UFSC)  
**Disciplina:** Desenvolvimento Web  
**Ano:** 2026

## ✨ Funcionalidades

### Gestão de Facções
- ✅ CRUD completo de facções criminosas
- ✅ Validação de dados obrigatórios
- ✅ Proteção contra exclusão com pichações vinculadas
- ✅ Busca por ID

### Gestão de Pichações
- ✅ CRUD completo de pichações
- ✅ Upload de imagens para MinIO (JPEG, PNG, GIF, WebP)
- ✅ Validação de extensão e tamanho de imagem (até 10 MB)
- ✅ Classificação por nível de ameaça (Low, Medium, High)
- ✅ Registro de localização geográfica (endereço + coordenadas)
- ✅ Vinculação obrigatória a facção
- ✅ Data de registro automática (UTC)
- ✅ Atualização completa via JSON (inclui endereço e data de registro)
- ✅ Exclusão em cascata de localização
- ✅ Exclusão automática de imagem do MinIO

### Dashboard e Estatísticas
- ✅ Resumo geral do sistema (totais e nível predominante)
- ✅ Estatísticas por facção (para gráfico de pizza)
- ✅ Estatísticas por estado (para mapa de calor)
- ✅ Estatísticas por estado e facção (para gráfico empilhado)
- ✅ Agregações otimizadas no banco de dados
- ✅ Endpoints prontos para Recharts

### Recursos Técnicos
- ✅ API REST com padrões HTTP semânticos
- ✅ Documentação automática via Swagger/OpenAPI
- ✅ Upload de arquivos via multipart/form-data
- ✅ Object Storage com MinIO (compatível com S3)
- ✅ DTOs para evitar referências circulares
- ✅ Eager loading de relacionamentos
- ✅ Validação de modelo com Data Annotations
- ✅ CORS configurado por ambiente (AllowedOrigins fora de Development)
- ✅ Service Layer para lógica de negócio
- ✅ Dependency Injection nativa do .NET

## 🚀 Tecnologias

### Core
- **.NET 8** - Framework web moderno
- **ASP.NET Core** - API REST
- **C# 12** - Linguagem de programação

### Banco de Dados
- **PostgreSQL 16** - Banco relacional
- **Entity Framework Core 8.0** - ORM
- **Npgsql 8.0** - Provider PostgreSQL

### Armazenamento
- **MinIO** - Object Storage (compatível com S3)
- **Minio SDK 6.0.3** - Cliente .NET para MinIO

### Documentação
- **Swagger/OpenAPI** - Documentação interativa
- **XML Comments** - Documentação de código

### Ferramentas
- **Docker** - Containerização (PostgreSQL, pgAdmin, MinIO)
- **EF Core Migrations** - Versionamento do banco

## 🏗️ Arquitetura

```
GraffitiClassificationApi/
├── Controllers/              # Camada de Apresentação
│   ├── GangsController.cs   # Endpoints de facções
│   ├── GraffitisController.cs # Endpoints de pichações
│   └── DashboardController.cs # Endpoints de estatísticas
├── Models/                   # Camada de Domínio
│   ├── Gang.cs              # Entidade Facção
│   ├── Graffiti.cs          # Entidade Pichação
│   └── GraffitiLocation.cs  # Entidade Localização
├── DTOs/                     # Data Transfer Objects
│   ├── GraffitiCreateDto.cs # DTO para criação
│   ├── GraffitiUpdateDto.cs # DTO para atualização
│   ├── GraffitiResponseDto.cs # DTO para resposta
│   ├── DashboardSummaryDto.cs # DTO para resumo
│   ├── ChartDataDto.cs      # DTO para gráficos simples
│   └── StackedChartDataDto.cs # DTO para gráfico empilhado
├── Services/                 # Camada de Serviços
│   ├── IStorageService.cs   # Interface de armazenamento
│   └── MinioStorageService.cs # Implementação MinIO
├── Data/                     # Camada de Dados
│   └── AppDbContext.cs      # Contexto EF Core
├── Migrations/               # Migrations do banco
└── Program.cs                # Configuração da aplicação
```

### Padrões Utilizados
- **Repository Pattern** (via EF Core)
- **Service Layer Pattern** (IStorageService)
- **DTO Pattern** (separação de concerns)
- **Dependency Injection** (injeção nativa do .NET)
- **Async/Await** (operações assíncronas)

## 📦 Instalação

### Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [PostgreSQL 16](https://www.postgresql.org/download/) ou [Docker](https://www.docker.com/)
- [Git](https://git-scm.com/)

### Passo a Passo

1. **Clone o repositório**
```bash
git clone <url-do-repositorio>
cd ProjetoFinal/GraffitiClassificationApi
```

2. **Configure o banco de dados**

**Opção A: Docker (Recomendado)**
```bash
# Sobe PostgreSQL, pgAdmin e MinIO
docker-compose up -d
```

**Opção B: PostgreSQL Local**
```bash
# Crie o banco manualmente
createdb grafiti_classification_db

# Atualize a connection string em appsettings.json
# Instale MinIO separadamente ou use Docker apenas para MinIO
```

3. **Restaure as dependências**
```bash
dotnet restore
```

4. **Aplique as migrations**
```bash
dotnet ef database update
```

5. **Execute a aplicação**
```bash
dotnet run
```

A API estará disponível em:
- **API**: http://localhost:5219
- **Swagger**: http://localhost:5219/swagger
- **MinIO Console**: http://localhost:9001 (minioadmin / minioadmin123)
- **pgAdmin**: http://localhost:8080 (admin@admin.com / admin123)

## 🎮 Uso

### Swagger UI

Acesse http://localhost:5219/swagger para:
- Visualizar todos os endpoints
- Testar requisições interativamente
- Ver schemas de request/response
- Consultar códigos de status

### Exemplo: Criar Facção

```bash
curl -X POST "http://localhost:5219/api/gangs" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Primeiro Comando da Capital",
    "acronym": "PCC",
    "origin": "São Paulo"
  }'
```

### Exemplo: Criar Pichação com Imagem

```bash
curl -X POST "http://localhost:5219/api/graffitis" \
  -F "visualDescription=Pichação com símbolo da facção" \
  -F "threatLevel=High" \
  -F "gangId=1" \
  -F "street=Rua das Flores, 123" \
  -F "neighborhood=Centro" \
  -F "city=Florianópolis" \
  -F "state=SC" \
  -F "lat=-27.5954" \
  -F "lon=-48.5480" \
  -F "image=@/path/to/image.jpg"
```

### Exemplo: Atualizar Pichação (JSON)

```bash
curl -X PUT "http://localhost:5219/api/graffitis/1" \
  -H "Content-Type: application/json" \
  -d '{
    "id": 1,
    "visualDescription": "Pichação atualizada",
    "threatLevel": "Medium",
    "gangId": 1,
    "registeredAt": "2026-05-31T10:30:00Z",
    "street": "Rua das Flores, 123",
    "neighborhood": "Centro",
    "city": "Florianópolis",
    "state": "SC",
    "lat": -27.5954,
    "lon": -48.5480
  }'
```

## 📡 API Endpoints

### Facções (Gangs)

| Método | Endpoint | Descrição | Status |
|--------|----------|-----------|--------|
| GET | `/api/gangs` | Lista todas as facções | 200 |
| GET | `/api/gangs/{id}` | Busca facção por ID | 200, 404 |
| POST | `/api/gangs` | Cria nova facção | 201, 400 |
| PUT | `/api/gangs/{id}` | Atualiza facção | 200, 400, 404 |
| DELETE | `/api/gangs/{id}` | Exclui facção | 204, 404, 409 |

### Pichações (Graffitis)

**POST** usa `multipart/form-data` (suporta upload opcional de imagem). **PUT** usa JSON.

| Método | Endpoint | Descrição | Status |
|--------|----------|-----------|--------|
| GET | `/api/graffitis` | Lista todas as pichações | 200 |
| GET | `/api/graffitis/{id}` | Busca pichação por ID | 200, 404 |
| POST | `/api/graffitis` | Cria nova pichação | 201, 400 |
| PUT | `/api/graffitis/{id}` | Atualiza pichação | 200, 400, 404 |
| DELETE | `/api/graffitis/{id}` | Exclui pichação | 204, 404 |

### Dashboard (Estatísticas)

| Método | Endpoint | Descrição | Status |
|--------|----------|-----------|--------|
| GET | `/api/dashboard/summary` | Resumo geral do sistema | 200 |
| GET | `/api/dashboard/graffitis-by-gang` | Pichações por facção | 200 |
| GET | `/api/dashboard/graffitis-by-state` | Pichações por estado | 200 |
| GET | `/api/dashboard/graffitis-by-gang-and-state` | Pichações por estado e facção | 200 |

### Códigos de Status

- **200 OK** - Requisição bem-sucedida
- **201 Created** - Recurso criado com sucesso
- **204 No Content** - Exclusão bem-sucedida
- **400 Bad Request** - Dados inválidos
- **404 Not Found** - Recurso não encontrado
- **409 Conflict** - Violação de regra de negócio

## 🗄️ Modelo de Dados

### Gang (Facção)
```json
{
  "id": 1,
  "name": "Primeiro Comando da Capital",
  "acronym": "PCC",
  "origin": "São Paulo"
}
```

### Graffiti (Pichação)
```json
{
  "id": 1,
  "registeredAt": "2025-01-15T10:30:00Z",
  "visualDescription": "Pichação com símbolo da facção",
  "threatLevel": "High",
  "gangId": 1,
  "imagePath": "http://localhost:9000/graffiti-images/occurrences/abc123.jpg"
}
```

### GraffitiLocation (Localização)
```json
{
  "id": 1,
  "street": "Rua das Flores, 123",
  "neighborhood": "Centro",
  "city": "Florianópolis",
  "state": "SC",
  "lat": -27.5954,
  "lon": -48.5480,
  "graffitiId": 1
}
```

### Relacionamentos

```
Gang (1) ──────< (N) Graffiti (1) ────── (1) GraffitiLocation
```

- **Gang → Graffiti**: 1:N (DeleteBehavior.Restrict)
- **Graffiti → GraffitiLocation**: 1:1 (DeleteBehavior.Cascade)

## 📚 Documentação


### Swagger/OpenAPI
- Acesse `/swagger` para documentação interativa
- Schemas de request/response
- Códigos de status documentados
- Teste de endpoints

### Migrations
```bash
# Criar nova migration
dotnet ef migrations add NomeDaMigration

# Aplicar migrations
dotnet ef database update

# Reverter migration
dotnet ef database update PreviousMigrationName

# Remover última migration
dotnet ef migrations remove
```

## 🔧 Configuração

### appsettings.json

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=grafiti_classification_db;Username=postgres;Password=postgres123"
  },
  "MinIO": {
    "Endpoint": "localhost:9000",
    "AccessKey": "minioadmin",
    "SecretKey": "minioadmin123",
    "BucketName": "graffiti-images",
    "UseSSL": false
  }
}
```

### docker-compose.yml

O projeto inclui um `docker-compose.yml` que sobe:
- **PostgreSQL 16** (porta 5432)
- **pgAdmin** (porta 8080)
- **MinIO** (portas 9000 e 9001)

```bash
docker-compose up -d
```

### CORS

Em `Development`, permite qualquer origem. Em outros ambientes, tenta ler `AllowedOrigins` do appsettings:
```csharp
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
```

**⚠️ Para produção:** Restrinja a origens específicas.

## 🧪 Testes

### Testar com Swagger
1. Acesse http://localhost:5219/swagger
2. Expanda o endpoint desejado
3. Clique em "Try it out"
4. Preencha os parâmetros
5. Execute

### Testar com cURL
Veja exemplos na seção [Uso](#-uso).

## 🛡️ Segurança

### Implementado
- ✅ Validação de modelo
- ✅ Proteção contra SQL Injection (EF Core)
- ✅ CORS configurado
- ✅ Validação de extensão e tamanho de upload

### Recomendações para Produção
- 🔒 Implementar autenticação JWT
- 🔒 Restringir CORS a origens específicas
- 🔒 HTTPS obrigatório
- 🔒 Validação de tipo de arquivo
- 🔒 Limite de tamanho de upload
- 🔒 Rate limiting
- 🔒 Logging de auditoria

## 📈 Performance

### Otimizações
- Eager loading com `Include()` para evitar N+1 queries
- Async/Await em todas as operações de I/O
- DTOs para reduzir payload
- Connection pooling (Npgsql)
- Índice único em FK 1:1
- Object Storage (MinIO) para imagens
- Acesso direto via HTTP (sem middleware .NET)

### Melhorias Futuras
- Paginação em listagens
- Cache com Redis
- CDN para imagens (integração MinIO + CloudFront)
- Compressão de resposta (Gzip/Brotli)
- Thumbnails automáticos

## 🤝 Contribuindo

1. Fork o projeto
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

## 📝 Convenções de Código

- **Classes**: PascalCase
- **Métodos**: PascalCase
- **Propriedades**: PascalCase
- **Variáveis**: camelCase
- **Tabelas**: snake_case
- **Colunas**: snake_case

## 🐛 Troubleshooting

### Erro: "Cannot connect to database"
```bash
# Verifique se o PostgreSQL está rodando
docker ps  # Se usando Docker

# Teste a conexão
psql -h localhost -U postgres -d grafiti_classification_db
```

### Erro: "Failed to connect to MinIO"
```bash
# Verifique se MinIO está rodando
docker ps | findstr minio

# Ver logs
docker logs minio-graffiti-storage

# Reiniciar
docker-compose restart minio
```

### Erro: "Pending migrations"
```bash
dotnet ef database update
```

### Erro: "Port already in use"
```bash
# Altere a porta em Properties/launchSettings.json
# Ou mate o processo usando a porta
# Ou pare containers antigos: docker stop $(docker ps -aq)
```

## 📊 Estatísticas do Projeto

- **Linhas de Código**: ~1.800
- **Controllers**: 3
- **Endpoints**: 14
- **Modelos**: 3
- **DTOs**: 6
- **Services**: 2 (Interface + Implementação)
- **Migrations**: 4
- **Dependências**: 5
- **Containers Docker**: 3 (PostgreSQL, pgAdmin, MinIO)

## 📄 Licença

Este projeto foi desenvolvido para fins acadêmicos na UFSC.

## 👨‍💻 Autor

**Diego**  
Universidade Federal de Santa Catarina (UFSC)  
Desenvolvimento Web - 2026

## 🙏 Agradecimentos

- UFSC - Universidade Federal de Santa Catarina
- Professor: Matheus Venos da Silva Cataneo
- Comunidade .NET

## 🔗 Links Úteis

- [Documentação .NET](https://docs.microsoft.com/dotnet)
- [Entity Framework Core](https://docs.microsoft.com/ef/core)
- [PostgreSQL](https://www.postgresql.org/docs)
- [Swagger](https://swagger.io/docs)
- [ASP.NET Core](https://docs.microsoft.com/aspnet/core)
- [MinIO Documentation](https://min.io/docs/minio/linux/index.html)
- [MinIO .NET SDK](https://github.com/minio/minio-dotnet)

## 📞 Suporte

Para dúvidas e suporte:
1. Verifique o Swagger em `/swagger`
2. Abra uma issue no repositório

---

**Desenvolvido com usando .NET 8, Entity Framework Core, PostgreSQL e MinIO**

**Status**: Em desenvolvimento

**Última Atualização**: Maio de 2026
