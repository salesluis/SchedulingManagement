# 04 — Arquitetura e System Design

## 1. Arquitetura inicial recomendada

```text
                 ┌──────────────────────┐
                 │   Web / Mobile Web   │
                 └──────────┬───────────┘
                            │ HTTPS
                    ┌───────▼────────┐
                    │ Load Balancer  │
                    └───────┬────────┘
                            │
                ┌───────────▼───────────┐
                │ API / Modular Monolith│
                │                       │
                │ Identity              │
                │ Calendar              │
                │ Availability          │
                │ Booking               │
                │ Notifications         │
                │ Integrations          │
                └──────┬───────┬────────┘
                       │       │
              ┌────────▼─┐   ┌─▼──────────┐
              │PostgreSQL│   │   Redis    │
              └─────┬────┘   └────────────┘
                    │
          transactional outbox
                    │
             ┌──────▼───────┐
             │ Queue / Bus  │
             └──────┬───────┘
                    │
        ┌───────────┼────────────┐
        │           │            │
   ┌────▼────┐ ┌────▼─────┐ ┌────▼─────┐
   │Reminder │ │WhatsApp   │ │Search/   │
   │Worker   │ │Worker     │ │Projection│
   └─────────┘ └────┬─────┘ └──────────┘
                    │
             ┌──────▼─────────┐
             │ WhatsApp Cloud │
             │ API / Provider │
             └────────────────┘
```

## 2. Por que começar assim

O problema inicial exige transações fortes entre:

- calendário;
- evento;
- disponibilidade;
- booking.

Separar isso cedo demais em microserviços cria:

- transações distribuídas;
- sagas;
- falhas parciais;
- duplicidade de modelos;
- mais observabilidade;
- mais deploys;
- mais infraestrutura.

Um monólito modular mantém essas operações locais, sem impedir escala horizontal da API.

## 3. Regras do monólito modular

Cada módulo possui:

- domínio;
- application/use cases;
- interface pública;
- infraestrutura;
- tabelas/schema lógico próprio quando conveniente.

Um módulo não deve acessar diretamente internals de outro.

Comunicação:

- chamada síncrona por contrato interno quando consistência imediata é necessária;
- domain/integration events para efeitos assíncronos.

## 4. Banco primário

PostgreSQL é adequado como primeira fonte transacional.

Responsabilidades:

- usuários;
- calendários;
- eventos;
- permissões;
- bookings;
- holds duráveis quando necessário;
- idempotency records;
- outbox.

O banco deve ser fonte da verdade de reservas.

## 5. Redis

Usos adequados:

- cache de leituras;
- rate limiting;
- dados efêmeros;
- presença/realtime;
- caches de disponibilidade;
- coordination específica.

Não usar Redis como única garantia de reserva permanente.

## 6. Queue/Event Bus

Usado para:

- WhatsApp;
- e-mail;
- reminders;
- webhooks de saída;
- indexação;
- analytics;
- limpeza;
- projeções.

Entrega deve ser considerada pelo menos uma vez.

Por isso consumers precisam ser idempotentes.

## 7. Outbox

Fluxo:

```text
BEGIN TRANSACTION
  INSERT event
  INSERT outbox_message
COMMIT

OutboxPublisher:
  lê outbox
  publica no broker
  marca como publicado
```

Isso evita:

```text
salvou evento no banco
        +
processo caiu
        +
mensagem nunca foi publicada
```

## 8. Realtime

Pode usar WebSocket/SSE.

Fluxo:

```text
evento alterado
   ↓
commit
   ↓
outbox
   ↓
event bus
   ↓
realtime gateway
   ↓
clientes inscritos
```

O cliente deve conseguir recuperar estado por REST/sync caso perca mensagens.

## 9. Leitura de calendário

Não retornar todo histórico.

Endpoint trabalha com janela:

```text
GET /calendars/{id}/events?from=...&to=...
```

Índice principal deve favorecer:

- CalendarId;
- intervalo temporal;
- status.

## 10. Recorrência

Evitar persistir milhões de ocorrências futuras.

Modelo:

```text
RecurringEvent
  recurrence_rule
  start
  timezone

EventException
  occurrence_original_start
  override/cancelled
```

Consulta de uma janela:

1. busca séries que podem intersectar a janela;
2. expande ocorrências somente na janela;
3. aplica exceções;
4. mistura eventos não recorrentes;
5. ordena.

Em escala alta, ocorrências próximas podem ser materializadas/cacheadas.

## 11. Disponibilidade

Pipeline:

```text
AvailabilitySchedule
      ↓
gera janelas permitidas
      ↓
remove exceções
      ↓
carrega busy intervals
      ↓
aplica duração + buffers
      ↓
gera slots
      ↓
remove holds/reservas
      ↓
retorna projeção
```

Resultado pode ser cacheado por:

```text
(owner, bookingType, dateRange, timezone, version)
```

Qualquer alteração relevante incrementa/invalida versão.

## 12. Particionamento futuro

Não começar necessariamente particionado.

Quando necessário, opções:

- particionamento temporal para grandes tabelas de eventos/auditoria;
- sharding por `UserId`, `WorkspaceId` ou `CalendarId`;
- separação de tenants grandes;
- read replicas para leituras tolerantes a lag.

IDs devem ser independentes do nó físico.

## 13. Multi-region

### Fase inicial

Single region + múltiplas AZs.

### Fase posterior

- edge/CDN para conteúdo;
- read replicas regionais;
- roteamento geográfico;
- disaster recovery cross-region.

Active-active de escrita é uma decisão posterior e cara, principalmente por conflitos de calendário.

## 14. Busca

Começar com busca do PostgreSQL se suficiente.

Extrair mecanismo dedicado quando:

- volume;
- relevância;
- autocomplete;
- filtros;
- latência

justificarem.

A busca é projeção assíncrona, nunca fonte de verdade.

## 15. Caminho de extração de serviços

Ordem provável quando houver necessidade real:

1. Notification Service;
2. WhatsApp Integration Service;
3. Realtime Gateway;
4. Search;
5. Reminder/Scheduler;
6. Availability Service;
7. Calendar write service somente se escala justificar.

O último item deve ser extraído com mais cautela porque contém as invariantes mais críticas.
