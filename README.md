\# InventoryHub API



API REST para controle de estoque desenvolvida com \*\*C#\*\*, \*\*ASP.NET Core\*\*, \*\*Entity Framework Core\*\* e \*\*SQLite\*\*.



Projeto de portfólio focado em autenticação JWT, API REST e regras de negócio aplicadas à logística.



\---



\## Funcionalidades



\- Cadastro de usuários

\- Login com JWT

\- Rota protegida

\- CRUD de produtos

\- Controle de SKU único

\- Entrada de estoque

\- Saída de estoque

\- Validação de estoque insuficiente

\- Histórico de movimentações

\- Consulta de baixo estoque



\---



\## Tecnologias



\- C#

\- ASP.NET Core Web API

\- Entity Framework Core

\- SQLite

\- JWT Authentication

\- Swagger



\---



\## Como rodar o projeto



```bash

dotnet restore

dotnet ef database update

dotnet run



Acesse: http://localhost:5000/swagger



Autenticação

1:Registrar usuário

2:Fazer login

3:Copiar token

4:Usar no Swagger: Bearer seu\_token\_aqui



Endpoints principais



Auth

°POST /api/Auth/register

°POST /api/Auth/login



Products

°GET /api/Products

°POST /api/Products

°1PUT /api/Products/{id}

°DELETE /api/Products/{id}



Stock

°POST /api/StockMovements/entry

°POST /api/StockMovements/exit



Regras de negócio

°1SKU único

°Não permite saída maior que o estoque

°Atualização automática do saldo

°Controle de estoque mínimo



Autor



Iohana Allen



https://github.com/iohanaallen

