# 🛡️ Ambev Developer Evaluation - Sales API

API desenvolvida como parte do processo seletivo da Ambev Tech, com foco em boas práticas de arquitetura, testes, validações e extensibilidade.

---

## ✅ Funcionalidades

- ✅ Criação, listagem, atualização e exclusão de vendas  
- ✅ Validações robustas com FluentValidation  
- ✅ Aplicação do padrão CQRS com MediatR  
- ✅ Logs estruturados com Serilog  
- ✅ Idempotência nas rotas críticas (via header `Idempotency-Key`)  
- ✅ Publicação de eventos de domínio (`SaleCreatedEvent`) via `IMediator` (simulado por log)

---

## 🚀 Tecnologias Utilizadas

- [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- ASP.NET Core Web API
- Entity Framework Core (InMemory)
- MediatR
- AutoMapper
- FluentValidation
- Serilog
- xUnit + NSubstitute + FluentAssertions + Bogus

---

## ⚙️ Como Executar o Projeto

### 1. Clone o repositório:
```bash
git clone https://github.com/seu-usuario/nome-do-repositorio.git
cd nome-do-repositorio
```

### 2. Execute a aplicação:
```bash
cd src/Ambev.DeveloperEvaluation.API
dotnet run
```

Acesse a documentação interativa em:  
📍 `https://localhost:5001/swagger`

---

## 🧪 Como Rodar os Testes

```bash
cd tests/Ambev.DeveloperEvaluation.Unit
dotnet test
```

---

## 📫 Requisições Testáveis

### ➕ Criar Venda
`POST /api/sale`  
Header obrigatório:
```
Idempotency-Key: 03d8e1b7-7c6b-4f21-9506-6c64bbabf5c2
```

### 📃 Listar Vendas
`GET /api/sale?page=1&pageSize=10`

### ✏️ Atualizar Venda
`PUT /api/sale/{id}`

### ❌ Remover Venda
`DELETE /api/sale/{id}`

---

## 📋 Observações Técnicas

- Os eventos são apenas simulados com `ILogger` para simular a publicação (`SaleCreatedEvent`).
- A persistência está em memória para simplificar a avaliação.
- Commits seguem padrão semântico (`feat:`, `fix:`, `test:`, etc).
- Projeto pronto para extensão com banco real e message broker.

---

## 👨‍💻 Desenvolvedor

**Seu Nome**  
🔗 [LinkedIn](https://www.linkedin.com/in/wcaraujoanalistasistema/)  
📧 wcaraujo@hotmail.com.br - (62) 99104-6911
