# tcc-backend-lanchonete-api
API REST em ASP.NET para gestão multicanal de rede de lanchonetes, com pedidos por APP/TOTEM/BALCÃO/PICKUP/WEB, controle de estoque por unidade, pagamento mock, autenticação JWT e documentação Swagger.


## Pré-requisitos

Antes de executar o projeto, garanta que o ambiente possui as seguintes dependências mínimas instaladas:

- .NET SDK 8.0 ou superior
- SQL Server (Express, Developer ou superior) 
- Entity Framework Core CLI (dotnet-ef)

### Verificar versões

dotnet --version
dotnet ef --version

### Instalar EF CLI (caso não tenha)

dotnet tool install --global dotnet-ef

## Execucao local

1. Configurar conexao com banco em appsettings.json:


{
	"ConnectionStrings": {
		"TCC": "Server=localhost,1433;Database=TCC_Lanchonete;User Id=sa;Password=SuaSenhaForte123!;TrustServerCertificate=True"
	}
}


2. Restaurar e compilar:

dotnet restore "tcc-backend-lanchonete-api/tcc-backend-lanchonete-api.sln"
dotnet build "tcc-backend-lanchonete-api/tcc-backend-lanchonete-api.sln"


3. Aplicar migrations no banco:

dotnet ef database update --project "tcc-backend-lanchonete-api/TCC.Infra.Data/TCC.Infra.Data.csproj" --startup-project "tcc-backend-lanchonete-api/tcc-backend-lanchonete-api/tcc-backend-lanchonete-api.csproj" --context "ApplicationDbContext"

4. Subir a API:

dotnet run --project "tcc-backend-lanchonete-api/tcc-backend-lanchonete-api/tcc-backend-lanchonete-api.csproj"

### Massa inicial criada pela migration

Ao aplicar a migration (`dotnet ef database update`), o banco ja sobe com dados minimos para uso e testes:

- Unidade inicial: `UN001 - Unidade Centro` (ativa)
- Canais habilitados na unidade: APP, TOTEM, BALCAO, PICKUP e WEB
- Produtos iniciais:
	- X-Burger
	- Batata Frita
	- Refrigerante Lata
- Cardapio inicial: os 3 produtos vinculados a `UN001`
- Estoque inicial da unidade:
	- X-Burger: 100
	- Batata Frita: 120
	- Refrigerante Lata: 150

Usuarios pre-cadastrados para testes:

- `joao.silva@email.com` (CLIENTE)
- `atendente@test.com` (ATENDENTE)
- `gerente@test.com` (GERENTE)

Senha padrao dos usuarios: `senha123`.

5. Abrir Swagger em https://localhost:7000/swagger.

## Colecao Postman (testes minimos)

Arquivo da colecao:

- `tcc-backend-lanchonete-api/tests/TCC-Testes.postman_collection.json`

### Importacao

1. No Postman, clique em `Import`.
2. Selecione o arquivo `TCC-Testes.postman_collection.json`.
3. Abra a colecao importada.

### Variaveis da colecao

Preencha antes de executar:

- baseUrl (padrao: https://localhost:7000)
- login_cliente_email
- login_cliente_senha
- login_atendente_email
- login_atendente_senha
- login_gerente_email
- login_gerente_senha

Variaveis preenchidas automaticamente pela propria colecao durante a execucao:

- token
- token_cliente
- token_cliente_ou_admin
- token_atendente_ou_gerente_ou_admin
- token_cozinha_ou_gerente_ou_atendente
- token_gerente_ou_admin
- unidadeId
- unidadeCodigo
- pedidoId

## Autenticacao na colecao

A colecao executa os logins automaticamente no inicio:

- 00 - Autenticacao cliente
- 00.1 - Autenticacao atendente
- 00.2 - Autenticacao gerente

Esses requests chamam POST /api/Login e salvam os tokens nas variaveis da colecao.

## Ordem de execucao dos testes

Execute na ordem ja definida na colecao (00, 00.1, 00.2 e depois 01 a 10).

Os cenarios 03, 05, 08 e 10 sao negativos e devem retornar erro (4xx).
Os demais sao cenarios positivos.
