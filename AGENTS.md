# Repository Guidelines

## Estrutura do projeto e organização de módulos

O repositório está em sua fase inicial: ainda não há código, testes ou manifesto de dependências. Preserve a raiz para configurações e documentação principal. Ao implementar a aplicação, use esta estrutura:

- `src/` para código de produção, organizado por domínio, como `src/agendamentos/`.
- `tests/` para testes automatizados que reflitam a estrutura de `src/`.
- `assets/` para recursos estáticos, como imagens e massas de teste.
- `specs/` para especificações de domínio antes da implementação.

Não versione saídas geradas, credenciais ou bancos de dados locais.

## Comandos de desenvolvimento

Ainda não há gerenciador de pacotes ou ferramentas configuradas. Ao introduzi-los, registre os comandos oficiais no README e mantenha-os compatíveis com a CI. O conjunto mínimo deve incluir execução local, testes, lint, formatação e build — por exemplo, `npm run dev`, `npm test`, `npm run lint`, `npm run format` e `npm run build`.

Não adicione um segundo gerenciador de pacotes ou executor de tarefas sem uma justificativa documentada.

## Estilo de código e nomenclatura

Escreva identificadores de código e nomes de domínio em inglês. Comentários, documentação, commits e pull requests devem permanecer em português. Use o formatador e o linter definidos pela tecnologia e faça commits apenas com código formatado. Para JavaScript e TypeScript, prefira indentação de 2 espaços, salvo regra contrária da ferramenta.

Use `kebab-case` em arquivos e diretórios (`create-booking.ts`), `PascalCase` em classes e tipos (`Booking`) e `camelCase` em funções, métodos e variáveis (`createBooking`). Preserve nomes técnicos obrigatórios de bibliotecas, protocolos e APIs externas. Mantenha módulos pequenos e próximos do domínio a que pertencem.

## Regras de negócio

Nomeie regras de negócio em inglês, com métodos declarativos que expressem claramente a invariante ou política correspondente ao arquivo `Requisitos/02-dominio-regras-negocio.md`. Nesta etapa, apenas declare e organize os nomes dessas regras; mantenha seus corpos vazios e não implemente seu comportamento.

## Testes

Não crie nem implemente testes automatizados nesta etapa; essa implementação será feita pelo mantenedor do projeto. Preserve os testes existentes sem adicionar novos casos. Quando a implementação de testes for autorizada, espelhe os caminhos de produção e use nomes descritivos em inglês.

## Commits e pull requests

Como não há histórico de commits utilizável, use Conventional Commits com descrições em português: `feat: validar conflito de horários` ou `fix: impedir agendamento duplicado`. Faça commits pequenos e com uma única finalidade. Pull requests devem explicar a mudança, os testes realizados, as issues relacionadas e incluir capturas de tela para alterações visuais. Nunca versione segredos; use `.env.example` para documentar configurações seguras.
