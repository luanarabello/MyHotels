# GestaoLoja — Programação Web (ISEC/IPC)

Aplicação de gestão de uma loja (produtos, categorias e modos de entrega), feita em **Blazor Web App (.NET 8)** com
**Entity Framework Core** e **ASP.NET Core Identity**. É a **solução de referência** (versão do professor): inclui as páginas CRUD completas, com upload de
imagens. A versão para os alunos, sem essas páginas, está em `GestaoLoja_Alunos`.

Este ficheiro explica como correr o projecto, como escolher a base de dados, quais são os utilizadores de teste e o
que fazer quando alguma coisa corre mal.

---

## 1. Requisitos

| O quê | Notas |
|---|---|
| Windows + **Visual Studio 2022** (17.x) |  **"ASP.NET e desenvolvimento web"** |
| **.NET 8 SDK** | O ficheiro `global.json` (nesta pasta) fixa o SDK 8. Confirme com `dotnet --version` (deve começar por `8.`) |
| *(só para a opção LocalDB)* SQL Server Express **LocalDB** | Vem com o Visual Studio (componente "Armazenamento e processamento de dados"). Confirme com `sqllocaldb info` |

## 2. Como correr

1. Abra `GestaoLoja.sln` no Visual Studio. O projecto de arranque é **GestaoLoja**.
2. **Crie a base de dados** (só na primeira vez, e sempre que mudar de base de dados): em
   *Ferramentas → Gerenciador de Pacotes do NuGet → Console do Gerenciador de Pacotes*, com o projecto
   **GestaoLoja** seleccionado, execute:
   ```
   Update-Database
   ```
3. Carregue em **F5** (ou `dotnet run --project GestaoLoja` num terminal).
4. Na primeira execução a aplicação cria sozinha as roles, os utilizadores de teste e os dados iniciais
   (3 categorias e 1 modo de entrega). Pode correr várias vezes: não duplica nada.

Se arrancar a aplicação **antes** do `Update-Database`, ela abre na mesma, mas o log mostra
*"Falha ao inicializar os dados base"* (as tabelas ainda não existem). Corra o `Update-Database` e reinicie.

### Utilizadores de teste (só em Development)

| Utilizador | Palavra-passe | Role |
|---|---|---|
| `admin@gestaoloja.pt` | `Admin#123` | `Admin` |
| `cliente@gestaoloja.pt` | `Cliente#123` | `Cliente` |

Estão no `appsettings.json`, secção `"Seed"`. **Só são criados quando o ambiente é Development**: nunca devem
existir palavras-passe destas fora do desenvolvimento.

---

## 3. Escolher a base de dados (SQLite ou LocalDB)

O projecto suporta duas bases de dados. **Escolhe-se uma** (por omissão está activa a SQLite):

| | SQLite | SQL Server LocalDB |
|---|---|---|
| O que é | Um ficheiro (`Data/app.db`) | Base de dados `FrutariaPWeb` no servidor local do Visual Studio |
| Precisa de instalar algo? | Não | Sim, o LocalDB (vem com o Visual Studio) |
| Enunciado das fichas | — | É a opção do enunciado |

### Passos para mudar de uma para a outra

1. **`Program.cs` (topo do ficheiro):** deixe activa **uma só** destas duas linhas e comente a outra com `//`:
   ```csharp
   #define USAR_SQLITE
   //#define USAR_LOCALDB
   ```
   Para o LocalDB fica `//#define USAR_SQLITE` e `#define USAR_LOCALDB`. Se deixar as duas activas, ou nenhuma,
   o projecto não compila e a mensagem de erro diz o que fazer.
2. **`appsettings.json`:** a secção `ConnectionStrings` tem as duas ligações. A SQLite usa `DefaultConnection`
   e o LocalDB usa `LocalDbConnection`. Só tem de a alterar se a sua instância do LocalDB não se chamar
   `MSSQLLocalDB` (veja o nome com `sqllocaldb info`) ou se quiser outro nome para a base de dados.
3. **Apague a pasta `Data/Migrations`** (e, se vier da SQLite, o ficheiro `Data/app.db`). As migrations são
   específicas de cada base de dados: as da SQLite **não funcionam** no SQL Server, e vice-versa.
4. **Crie a migration e a base de dados**, na Console do Gerenciador de Pacotes:
   ```
   Add-Migration BDInicial -OutputDir Data/Migrations
   Update-Database
   ```
   (Num terminal: `dotnet ef migrations add BDInicial -o Data/Migrations` e `dotnet ef database update`,
   a partir da pasta `GestaoLoja`.)
5. Arranque a aplicação. O seed corre sozinho.

Para **ver** a base de dados: no LocalDB, *Exibir → Pesquisador de Objetos do SQL Server* e a ligação
`(localdb)\MSSQLLocalDB`; na SQLite, uma extensão do Visual Studio ou o programa *DB Browser for SQLite*.

> **Nota:** as páginas de demonstração (`/demos`) usam sempre uma base de dados SQLite própria
> (`Data/demos.db`), qualquer que seja a opção escolhida acima. Não mexem nos dados da loja.

---

## 4. Roles e permissões

As roles estão em `AppConfig.cs` e são criadas no arranque por `Data/Inicializacao.cs`.

| Quem | O que pode fazer |
|---|---|
| Anónimo | Ver a lista e os detalhes de produtos e categorias, e as demos |
| Utilizador sem role (acabou de se registar) | O mesmo que o anónimo, mas autenticado |
| `Cliente` | O mesmo que o anónimo (ainda não tem funcionalidades próprias) |
| `Admin` | Tudo o anterior **e** criar, editar e apagar produtos, categorias e modos de entrega |

- As páginas `Create`, `Edit` e `Delete` têm `@attribute [Authorize(Roles = AppConfig.RoleAdmin)]`.
- Um utilizador sem permissão é mandado para `/Account/AccessDenied` (se abrir o URL directamente) ou vê
  "Acesso negado" (se chegar por um link dentro da aplicação). Um anónimo vai para o Login.
- As Razor Pages geradas em `Pages/CategoriaPages` ficam protegidas por uma policy (`SoAdmin`) no `Program.cs`.
- **Atenção:** a página `/categorias` (`Components/Pages/Categorias.razor`) é um exemplo didáctico rudimentar,
  usado para mostrar conceitos básicos, e **não** está protegida por roles: qualquer pessoa pode criar, editar e
  apagar categorias por ela. Proteja-a, se quiser, com `@attribute [Authorize(Roles = AppConfig.RoleAdmin)]`.
- **Quem se regista na página Register não fica com nenhuma role.** Fica como exercício atribuir a role
  `Cliente` no registo.

Para acrescentar uma role: junte o nome a `AppConfig.Roles` e use-o em `[Authorize(Roles = "...")]`. Ela é criada
no próximo arranque.

Para tornar **Admin** uma conta que já existe (por exemplo, a sua): coloque o email dessa conta em
`Seed:AdminEmail`, no `appsettings.json`. No arranque seguinte, a conta passa a ter a role `Admin` (a
palavra-passe do `appsettings.json` só é usada quando a conta ainda não existe). Em alternativa, entre como
`admin@gestaoloja.pt`.

---

## 5. Cultura (pt-PT)

A aplicação usa sempre a cultura **pt-PT**, qualquer que seja a configuração do Windows ou do browser do aluno:
números com vírgula decimal (`2,50`), datas `dd/MM/aaaa` e moeda em euros (`2,50 €`). Está configurada no
`Program.cs` (`CultureInfo` e `UseRequestLocalization`).

Consequência prática: em campos de texto que representam números com decimais, a vírgula é o separador
(`2,5`). Nos campos `<input type="number">` o browser decide como o mostra, mas o valor chega sempre bem ao servidor.

---

## 6. Estrutura do projecto

```
GestaoLoja/                      <- esta pasta (a solução)
├── global.json                  <- fixa o SDK 8
├── README.md
└── GestaoLoja/                  <- o projecto
    ├── Program.cs               <- arranque: base de dados (#define), Identity, roles, cultura, seed
    ├── AppConfig.cs             <- constantes (nomes das roles)
    ├── appsettings.json         <- connection strings e utilizadores de teste
    ├── Data/                    <- ApplicationDbContext, ApplicationUser, Inicializacao (seed), Migrations/
    ├── Entities/                <- classes de domínio: Categoria, Produto, ModoEntrega
    ├── Services/                <- ImagemService (upload de imagens) e serviços das demos
    ├── Components/
    │   ├── Layout/              <- MainLayout e menu lateral
    │   ├── Account/             <- páginas do Identity (login, registo…)
    │   └── Pages/               <- páginas da aplicação: ProdutoPages, CategoriaPages, ModoEntregaPages, Demos/…
    ├── Pages/CategoriaPages/    <- Razor Pages geradas pelo scaffold do Visual Studio
    └── wwwroot/                 <- ficheiros estáticos; imagens dos produtos em wwwroot/img
```

## 7. Demos e glossário (material de estudo)

No menu lateral, **Demos** tem páginas pequenas que mostram, a funcionar, os conceitos importantes (binding,
validação, lifecycle, Dependency Injection, Entity Framework, Identity, segurança, componentes…). Cada uma tem:

- a **demonstração**, a **explicação** e o **código-fonte** dos ficheiros que a fazem funcionar;
- links para os **termos** que usa, no **Glossário** (`/demos/glossario`), que explica em português os termos
  técnicos (mantidos em inglês).

---

## 8. Problemas frequentes

| Sintoma | Causa e solução |
|---|---|
| `Unable to copy file ... GestaoLoja.exe ... being used by another process` | A aplicação ainda está a correr. Pare-a (*Parar depuração*) antes de compilar ou de usar o `dotnet ef`. |
| `SQLite Error: table "AspNetRoles" already exists` (ou outra tabela) ao fazer `Update-Database` | A base de dados já tem tabelas de outra migration. Apague `Data/app.db` e repita o `Update-Database`. |
| `no such table` / `no such column` (SQLite) ou `Invalid object name` / `Invalid column name` (SQL Server) | Falta aplicar uma migration: `Update-Database`. Se mudou uma classe de `Entities/` ou o `ApplicationUser`, crie primeiro a migration: `Add-Migration NomeDescritivo`. |
| Erros de tipos ou sintaxe SQL ao fazer `Update-Database` depois de mudar de SQLite para LocalDB (ou ao contrário) | Esqueceu-se do passo 3 da secção 3: apague a pasta `Data/Migrations` e crie a migration outra vez. |
| `A network-related or instance-specific error ... LocalDB` | O LocalDB não está instalado ou a instância tem outro nome. Corra `sqllocaldb info`, ajuste `LocalDbConnection` no `appsettings.json`, ou use a SQLite. |
| `The Entity Framework tools version ... is older than that of the runtime` | Aviso inofensivo. Pode actualizar o pacote `Microsoft.EntityFrameworkCore.Tools` para a versão do runtime. |
| `dotnet scaffold ... Invalid --model` | É um problema da ferramenta `dotnet scaffold` 20.x. Use a ferramenta clássica, compatível com .NET 8 (ver secção 9). |
| O Visual Studio pede um SDK que não tem | Instale o **.NET 8 SDK** (o `global.json` exige a versão 8). |
| Não consigo fazer login com um utilizador novo | O projecto exige a conta confirmada. Na página de confirmação aparece uma ligação "Click here to confirm your account" (é o que acontece em Development, sem email real). |

---

## 9. Scaffolding na linha de comandos (alternativa ao Visual Studio)

Se o scaffolding do Visual Studio falhar, instale a ferramenta clássica (compatível com .NET 8) uma só vez:

```
dotnet tool install -g dotnet-aspnet-codegenerator --version 8.0.23
```

e, na pasta do projecto (`GestaoLoja/GestaoLoja`), gere as páginas CRUD de uma entidade:

```
dotnet aspnet-codegenerator blazor CRUD -m Categoria -dc ApplicationDbContext -dbProvider sqlite
```

Os comandos são **exemplos**: troque `-m Categoria` pela entidade pretendida (`Produto`, `ModoEntrega`) e, se usar
o LocalDB, `-dbProvider sqlite` por `-dbProvider sqlserver`.

---

## 10. Nota sobre segurança

Os utilizadores e palavras-passe de teste servem só para estudo. Numa aplicação real: não guardar palavras-passe
no `appsettings.json` (usar *User Secrets* ou variáveis de ambiente), não criar utilizadores de teste fora de
Development, e nunca mostrar connection strings numa página.
