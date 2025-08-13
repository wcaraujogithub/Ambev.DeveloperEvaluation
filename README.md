# 🛡️  Developer Evaluation - Sales API

API desenvolvida com foco em boas práticas de arquitetura, testes, validações e extensibilidade.

---

## ✅ Funcionalidades

- ✅ Criação, listagem, atualização e exclusão de vendas  
- ✅ Validações robustas com FluentValidation  
- ✅ Aplicação do padrão CQRS com MediatR  
- ✅ Logs estruturados com Serilog  
- ✅ Idempotência nas rotas críticas (via header `Idempotency-Key`)  
- ✅ Publicação de eventos de domínio (`SaleCreatedEvent`) via `IMediator` (simulado por log)

---

## 🧰 Tecnologias e Skills Aplicadas
- **[.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- **.NET Core**, C#
- **Entity Framework Core  (Postgres)**
- **CQRS**, **MediatR**, **FluentValidation**, **AutoMapper**
- **Clean Architecture**
- **xUnit**, **Testes de Integração**
- **Segurança com JWT e Roles**
- **Docker**, **PostgreSQL**
- **Git Avançado**, GitHub Flow
- **Serilog**
- **xUnit + NSubstitute + FluentAssertions + Bogus**


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
`GET /api/sale?page=1&_size=10&_order=customer asc`

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

## Documentação Técnica - Projeto [Developer Evaluation - Sales API]

### Visão Geral
Sistema voltado para [descrever o propósito do projeto].

### 📦 Arquitetura
- Camadas: Application, Domain, Infrastructure
- Padrões: CQRS, Clean Architecture
- Comunicação: REST + Event Driven (eventualmente assíncrono)

### 🔐 Segurança
- Controle de acesso por Role (`"Admin"`).
- Middleware de validação JWT + Role.

### ⚙️ Funcionalidades Implementadas
- Criação de Venda (CreateSaleCommand)
- Eventos: `SaleCreatedEvent`
- Repositórios e testes
- Idempotência com `IdempotentAttribute`

### 🧪 Testes
- Unitários com cobertura dos principais fluxos de negócio.
- Integração com repositórios e banco em memória.

### 🚀 Próximos passos sugeridos
- Introdução de mensageria real (RabbitMQ ou Kafka)
- Monitoramento via logs estruturados e observabilidade

## Resumo Técnico - [Wesley]

Atuando no projeto [Developer Evaluation], contribuí com decisões estratégicas, implementação de funcionalidades críticas e estruturação da base do sistema, demonstrando visão de arquitetura, boas práticas e foco em escalabilidade e qualidade.

Assumi responsabilidades além de um desenvolvedor comum, contribuindo diretamente com decisões de arquitetura, melhorias estruturais e implementação de práticas que aumentaram a confiabilidade, testabilidade e escalabilidade do sistema.


### 🧠 Principais Contribuições

✅ **Implementação de Idempotência**
- Implementado header `Idempotency-Key` nas requisições POST.
- Criado `IdempotentCommandHandler` e testes relacionados.
- Evita duplicação de operações críticas.

✅ **Arquitetura Orientada a Eventos**
- Criação do `SaleCreatedEvent` e `SaleCreatedEventHandler`.
- Início de uma estrutura para adoção futura de mensageria (RabbitMQ, Kafka).
- Segue princípios de arquitetura reativa e desacoplamento.

✅ **Aplicação de Clean Architecture + CQRS**
- Uso de comandos e handlers separados por responsabilidade.
- Estrutura clara: Application, Domain, Infrastructure, API.

✅ **Cobertura de Testes Automatizados**
- Criação de testes unitários para Handlers, Validadores, AutoMapper.
- Testes de integração com repositório.
- Aumento da cobertura geral e confiabilidade.

✅ **Segurança e Autorização**
- Validação de roles com destaque para `"Admin"`.
- Controle de acesso por meio de JWT + política de role baseada em claims.

✅ **Git Avançado e Boas Práticas**
- Rebases interativos, squash de commits, push limpo.
- Histórico limpo e sem ruído.


**Skills aplicadas**: .NET Core, CQRS, MediatR, FluentValidation, AutoMapper, REST, SOLID, Git avançado, segurança, testes (xUnit), Docker, integração contínua.


## 👨‍💻 Desenvolvedor

  
🔗 [LinkedIn]https://www.linkedin.com/in/wcaraujoanalistasistema/  
📧 wcaraujo@hotmail.com.br - (62) 99104-6911
