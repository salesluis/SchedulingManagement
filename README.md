# Especificações — Plataforma de Calendário e Agendamentos

Este conjunto substitui a especificação anterior centrada em empresas/profissionais/serviços por uma plataforma de calendário multiusuário, inspirada funcionalmente em produtos como Google Calendar, com um módulo próprio de agendamento público e integração com WhatsApp.

## Princípio de arquitetura

O sistema deve ser **capaz de evoluir para milhões de usuários**, mas não deve começar com a complexidade operacional de uma infraestrutura de milhões de usuários.

A primeira versão deve preferir:

- monólito modular;
- banco relacional transacional;
- cache;
- fila/event bus;
- workers assíncronos;
- API stateless;
- observabilidade desde o início;
- contratos e limites claros entre módulos.

A arquitetura lógica deve permitir extração futura de módulos para serviços independentes sem reescrever as regras centrais.

## Desenvolvimento

O projeto usa o SDK do .NET e um único fluxo de ferramentas:

```bash
dotnet restore
dotnet build
dotnet test
dotnet format --verify-no-changes
dotnet format
```

O núcleo ainda é uma biblioteca de classes; o comando de execução local será adicionado quando existir um projeto de aplicação/API.

As entidades de domínio usam primary constructors e o padrão Domain Notification com Flunt. Entradas inválidas não lançam exceções de validação: consulte `IsValid` e `Notifications` na entidade criada.

## Documentos

1. `00-visao-produto.md`
   - objetivo;
   - escopo;
   - atores;
   - capacidades do produto;
   - o que não faz parte da primeira versão.

2. `01-requisitos-funcionais.md`
   - calendários;
   - eventos;
   - recorrência;
   - participantes;
   - compartilhamento;
   - disponibilidade;
   - agendamento público;
   - notificações;
   - pesquisa;
   - sincronização.

3. `02-dominio-regras-negocio.md`
   - agregados e entidades;
   - invariantes;
   - regras de conflito;
   - recorrência;
   - permissões;
   - booking;
   - estados.

4. `03-requisitos-nao-funcionais-slos.md`
   - disponibilidade;
   - latência;
   - escalabilidade;
   - segurança;
   - observabilidade;
   - backup;
   - disaster recovery;
   - privacidade;
   - SLOs.

5. `04-arquitetura-system-design.md`
   - arquitetura macro;
   - módulos;
   - componentes;
   - bancos;
   - cache;
   - filas;
   - realtime;
   - particionamento;
   - estratégia de evolução.

6. `05-consistencia-concorrencia.md`
   - criação de eventos;
   - disputa pelo mesmo horário;
   - idempotência;
   - locks;
   - transações;
   - outbox;
   - consistência eventual.

7. `06-whatsapp-notificacoes.md`
   - integração com WhatsApp Business Platform;
   - templates;
   - webhooks;
   - retries;
   - deduplicação;
   - lembretes.

8. `07-roadmap-implementacao.md`
   - ordem para começar codando;
   - milestones;
   - o que adiar;
   - critérios para extrair microserviços.

9. `08-api-inicial.md`
   - recursos HTTP iniciais;
   - comandos;
   - idempotência;
   - paginação;
   - erros.

## Decisões estruturais principais

- O núcleo deixa de ser `Empresa -> Profissional -> Serviço -> Agendamento`.
- O núcleo passa a ser `User -> Calendar -> Event -> Attendee`.
- Agendamento público é uma capacidade adicional construída sobre disponibilidade e calendários.
- WhatsApp é um canal de notificação/entrada, não o dono da regra de negócio.
- Datas persistidas devem utilizar UTC; timezone IANA é armazenado separadamente.
- Conflitos de agenda não devem depender apenas de validação no frontend.
- Operações críticas devem ser idempotentes.
- Efeitos colaterais externos devem ser assíncronos.
