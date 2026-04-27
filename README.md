# Alta.Back.Template

Repositório template para novos microserviços backend da Alta Sistemas, adaptado para a arquitetura Alta.

## Visão Geral
Este template fornece uma estrutura baseada em API .NET 10, com infraestrutura mínima configurada (Log, DI, Tratamento de Erro, Swagger) e suporte oficial aos ambientes:
- **Desenvolvimento**
- **Teste**
- **Homologacao**
- **Producao**

## Arquitetura de Pastas

A estrutura do projeto segue uma organização lógica para facilitar a manutenção e escalabilidade:

- **`Controllers/`**: Pontos de entrada da API (Endpoints).
- **`Data/`**: Configurações do Entity Framework Core e Contexto do Banco de Dados.
- **`Models/`**:
  - **`Dto/Input`**: Objetos de Transferência de Dados para entrada (Requests).
  - **`Dto/Output`**: Objetos de Transferência de Dados para saída (Responses).
  - **`Entity`**: Entidades mapeadas para o banco de dados.
  - **`Enum`**: Enumeradores específicos do domínio.
- **`Repositories/`**: Camada de acesso a dados (Repository Pattern).
- **`Services/`**: Camada de regras de negócio.
- **`Utils/`**: Utilitários e ferramentas auxiliares específicas do projeto.

A biblioteca core `Alta.Back.Lib` é integrada via **Git Submodule** na raiz.

## Como criar um novo serviço
1. Clique em **"Use this template"** no GitHub para criar um novo repositório.
2. Clone o seu novo repositóriio.
3. Renomeie a Solution (`.sln`), o Projeto (`.csproj`) e os Namespaces da aplicação para o nome do seu serviço.
4. Ajuste o `Assemblyname` e `RootNamespace` no `.csproj`.

## Git Submodules (Alta.Back.Lib)
Este repositório depende da `Alta.Back.Lib` localizada na raiz.

### Clonando
```bash
git clone --recurse-submodules <url-do-repo>
```
Se esqueceu de clonar recursivo:
```bash
git submodule update --init --recursive
```

### Atualizando a Lib
A lib é travada em um commit específico. Para atualizar:
```bash
cd Alta.Back.Lib
git fetch
git checkout <hash-ou-tag-desejada>
cd ..
git add Alta.Back.Lib
git commit -m "chore: atualizando lib para versão X"
```
**Nota:** O pipeline ignora mudanças internas na lib para evitar builds desnecessários, a menos que o ponteiro do submodule no projeto principal seja alterado.

## Ambientes
A aplicação converte a string de ambiente para o enum `AmbienteAplicacaoEnum`.
Certifique-se de configurar a variável de ambiente `ASPNETCORE_ENVIRONMENT` corretamente no servidor.

| Branch | Ambiente | Arquivo |
|---|---|---|
| (local) | Desenvolvimento | appsettings.Desenvolvimento.json |
| teste | Teste | appsettings.Teste.json |
| homolog | Homologacao | appsettings.Homologacao.json |
| main | Producao | appsettings.Producao.json |

## Executando Localmente
```bash
dotnet run --project src/Alta.Back.Template.Api --launch-profile Desenvolvimento
```
Acesse `/health` para verificar a saúde.
Acesse `/swagger` para documentação (somente Desenvolvimento).

## Pipelines
Workflows do GitHub Actions estão configurados em `.github/workflows/`.
Eles são acionados em push/pr para `teste`, `homolog` e `main`.
