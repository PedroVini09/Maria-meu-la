# Pastoral da Juventude

![CI](https://github.com/PedroVini09/Pastoral_Juventude/actions/workflows/ci.yml/badge.svg)
![.NET](https://img.shields.io/badge/.NET-10-512BD4?logo=dotnet)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-MVC-512BD4?logo=dotnet)
![MySQL](https://img.shields.io/badge/MySQL-EF_Core-4479A1?logo=mysql&logoColor=white)

Sistema web da **Pastoral da Juventude** (Paróquia Sagrado Coração de Jesus – Campina Grande/PB) para divulgar as missões da pastoral, receber inscrições e mensagens de contato, com uma **área administrativa** protegida para a equipe gerenciar todo o conteúdo do site.

> Projeto desenvolvido de forma voluntária para a pastoral.

<!-- Adicione prints em docs/ e descomente:
![Página inicial](docs/home.png)
![Painel administrativo](docs/admin.png)
-->

---

## Funcionalidades

### Site público
- Página inicial com as missões ativas, exibidas na ordem definida pelo painel admin
- Páginas de cada missão: **Maria em Meu Lar**, **Retiro Quaresmal**, **Segue-me Jovem**, **Semana da Juventude** e **Terço da Juventude**
- Programação e galeria de fotos
- **Formulário de inscrição** nas missões, com validação dos dados
- **Formulário de contato** com envio de e-mail via SMTP

### Área administrativa (`/Admin`)
- Login de administrador com **JWT armazenado em cookie** (`HttpOnly`, `Secure`, `SameSite=Strict`)
- Acesso restrito por perfil (`[Authorize(Roles = "Admin")]`)
- Gerenciamento de missões: criar, editar, desativar e abrir/fechar inscrições
- Edição do conteúdo das páginas das missões (história, seções e itens de cada seção)
- Controle de inscrições: listar, filtrar por missão, ver detalhes, editar, excluir e cadastrar manualmente

---

## Tecnologias

| Camada | Tecnologias |
|---|---|
| Back-end | C#, .NET 10, ASP.NET Core MVC |
| Persistência | Entity Framework Core, Pomelo (MySQL) |
| Banco de dados | MySQL |
| Segurança | JWT Bearer, `PasswordHasher` (ASP.NET Core Identity), proteção anti-CSRF |
| E-mail | SMTP (`System.Net.Mail`) |
| Front-end | Razor Views, HTML, CSS, JavaScript, Bootstrap |
| Testes | xUnit (40 testes de validação dos modelos) |
| Infra | Docker, GitHub Actions |

---

## Arquitetura

```
MariaEmMeuLar/
├── Controllers/     # Home (site público), Auth (login/logout), Admin (painel)
├── Data/            # AppDbContext (EF Core) e AdminSeeder (admin inicial)
├── Models/          # Entidades do domínio e ViewModels
├── Services/        # EmailService e JwtService, com interfaces para injeção de dependência
├── Views/           # Razor Views (site, admin e autenticação com layouts separados)
└── wwwroot/         # CSS, JavaScript e imagens

MariaEmMeuLar.Tests/ # Testes unitários com xUnit
```

**Decisões de projeto**
- **Injeção de dependência** para serviços (`IEmailService`, `IJwtService`, `IPasswordHasher`), o que facilita testes e substituição de implementações.
- **ViewModels** separados das entidades, para o formulário receber apenas os campos permitidos.
- **Segredos fora do código:** senhas, chave JWT e credenciais de e-mail vêm de variáveis de ambiente / `.env`, nunca do `appsettings.json`.
- O sistema **falha ao iniciar** se a configuração do JWT estiver ausente, em vez de rodar inseguro.
- Senhas de administrador armazenadas apenas como **hash**, com re-hash automático quando o algoritmo é atualizado.

---

## Como executar

### Pré-requisitos
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- MySQL 8+

### 1. Clonar
```bash
git clone https://github.com/PedroVini09/Pastoral_Juventude.git
cd Pastoral_Juventude/MariaEmMeuLar
```

### 2. Configurar variáveis de ambiente
Copie o arquivo de exemplo e preencha com seus dados:
```bash
cp .env.example .env
```

Além das configurações de e-mail do `.env.example`, defina:
```env
ConnectionStrings__DefaultConnection=server=localhost;port=3306;database=pastoral_juventude_db;user=SEU_USUARIO;password=SUA_SENHA;
Jwt__Key=uma-chave-secreta-com-pelo-menos-32-caracteres

# Admin criado automaticamente no primeiro start em ambiente de desenvolvimento
AdminSeed__Nome=Administrador
AdminSeed__Email=admin@exemplo.com
AdminSeed__Password=SenhaForte@123
```

### 3. Banco de dados
Crie o banco `pastoral_juventude_db` no MySQL. <!-- TODO: descrever como as tabelas são criadas (script SQL ou migrations) -->

### 4. Rodar
```bash
dotnet run
```
Acesse `https://localhost:7041` (site) e `https://localhost:7041/Auth/Login` (painel admin).

### Testes
Na raiz do repositório:
```bash
dotnet test
```

### Docker
Na raiz do repositório:
```bash
docker build -f MariaEmMeuLar/Dockerfile -t maria-em-meu-lar .
docker run -p 8080:8080 --env-file MariaEmMeuLar/.env maria-em-meu-lar
```

---

## Próximos passos
- [ ] Migrations do EF Core versionadas no repositório
- [ ] `docker-compose` com a aplicação + MySQL
- [ ] Testes de integração dos controllers
- [ ] Upload de imagens da galeria pelo painel admin

---

## Autor

**Pedro Vinícius Lima Guimarães** – Desenvolvedor Back-end .NET

[![LinkedIn](https://img.shields.io/badge/LinkedIn-0A66C2?logo=linkedin&logoColor=white)](https://www.linkedin.com/in/pedrovinicius--dev/)
[![GitHub](https://img.shields.io/badge/GitHub-181717?logo=github)](https://github.com/PedroVini09)
