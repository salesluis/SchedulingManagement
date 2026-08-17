# 07 — Roadmap para Começar Codando do Zero

## Regra principal

Não comece implementando “Google Calendar inteiro”.

Construa um vertical slice funcional e aumente a complexidade por camadas.

---

# Fase 0 — Fundação

Objetivo: projeto executa, testa e observa.

Implementar:

- solution/repositório;
- API;
- banco;
- migrations;
- health checks;
- logging estruturado;
- tracing;
- tratamento padronizado de erros;
- autenticação básica;
- testes unitários/integrados;
- CI.

Ainda não implementar:

- microserviços;
- Kafka por obrigação;
- Kubernetes por obrigação;
- sharding;
- multi-region.

---

# Fase 1 — Calendar Core

Entidades:

```text
User
Calendar
CalendarMember
Event
```

Casos de uso:

- criar calendário;
- listar calendários;
- criar evento;
- editar evento;
- remover/cancelar evento;
- buscar eventos por janela temporal.

Critérios:

- autorização;
- UTC + timezone;
- optimistic concurrency;
- paginação/janelas;
- índices;
- integration tests.

Ao final você já possui um calendário real.

---

# Fase 2 — Disponibilidade

Adicionar:

```text
AvailabilitySchedule
AvailabilityWindow
AvailabilityException
```

Implementar:

- configuração semanal;
- exceções;
- cálculo de busy intervals;
- geração de slots;
- buffers.

Criar testes pesados de borda:

- meia-noite;
- timezone;
- DST;
- intervalos encostados;
- evento all-day;
- múltiplos calendários.

---

# Fase 3 — Booking público

Adicionar:

```text
BookingType
Booking
BookingHold
```

Implementar:

- slug;
- página pública;
- listar slots;
- hold;
- confirmar;
- cancelar;
- remarcação;
- Idempotency-Key;
- garantia de não sobreposição.

Aqui devem existir testes concorrentes reais.

Exemplo:

```text
100 requests tentando o mesmo slot
=> exatamente 1 reserva confirmada
```

quando a capacidade daquele slot for 1.

---

# Fase 4 — Async infrastructure

Adicionar:

- transactional outbox;
- worker;
- broker/queue;
- retry;
- DLQ;
- inbox.

Mover para assíncrono:

- notificações;
- audit projection;
- efeitos externos.

---

# Fase 5 — WhatsApp

Adicionar adapter do provider.

Implementar primeiro:

- confirmação;
- cancelamento;
- lembrete;
- webhook de status;
- idempotência;
- retries.

Depois:

- interações inbound;
- fluxos conversacionais.

---

# Fase 6 — Colaboração

Adicionar:

```text
EventAttendee
Invitations
RSVP
Calendar sharing
```

Implementar:

- participante interno;
- participante externo;
- permissões;
- atualização de convites.

---

# Fase 7 — Recorrência

Recorrência é propositalmente posterior porque aumenta muito a complexidade temporal.

Implementar:

- RRULE/subconjunto definido;
- expansão por janela;
- exceções;
- editar uma ocorrência;
- editar série;
- esta e futuras.

Criar grande suíte de testes.

---

# Fase 8 — Realtime + sync incremental

Adicionar:

- SSE/WebSocket;
- change cursor;
- endpoint incremental;
- reconexão.

Não usar realtime como fonte de verdade.

---

# Fase 9 — Hardening

- rate limiting;
- quotas;
- load tests;
- chaos/failure tests básicos;
- backup restore test;
- dashboard;
- alerts;
- SLO;
- error budget;
- dependency scanning;
- pentest.

---

# Fase 10 — Escala

Só executar conforme métricas.

Possíveis passos:

1. read replicas;
2. cache;
3. separar workers;
4. separar Notification;
5. separar Realtime;
6. particionar tabelas;
7. search dedicado;
8. sharding;
9. cross-region DR;
10. serviços independentes para hotspots.

## Sinais para extrair um módulo

Extrair quando houver pelo menos um motivo mensurável:

- necessidade de escalar independentemente;
- deploy independente necessário;
- isolamento de falha;
- tecnologia muito diferente;
- propriedade clara por equipe;
- banco/perfil de carga incompatível.

“Pode ter milhões no futuro” sozinho não é motivo suficiente.

---

# Primeiros commits sugeridos

```text
01 chore: create solution and projects
02 chore: configure database and migrations
03 feat(identity): create user authentication
04 feat(calendar): create calendar
05 feat(calendar): create event
06 feat(calendar): query events by range
07 feat(calendar): add optimistic concurrency
08 feat(availability): configure weekly schedule
09 feat(availability): calculate busy intervals
10 feat(availability): generate available slots
11 feat(booking): create booking type
12 feat(booking): expose public slots
13 feat(booking): add idempotent booking command
14 feat(booking): prevent overlapping reservations
15 feat(outbox): publish integration events
16 feat(notification): process reminder jobs
17 feat(whatsapp): send booking confirmation
```

---

# Estrutura lógica sugerida

Para .NET, uma estrutura possível:

```text
src/
  Api/

  Modules/
    Identity/
      Domain/
      Application/
      Infrastructure/
      Contracts/

    Calendar/
      Domain/
      Application/
      Infrastructure/
      Contracts/

    Availability/
      Domain/
      Application/
      Infrastructure/
      Contracts/

    Booking/
      Domain/
      Application/
      Infrastructure/
      Contracts/

    Notifications/
      Domain/
      Application/
      Infrastructure/
      Contracts/

    Integrations/
      WhatsApp/

  BuildingBlocks/
    Observability/
    Messaging/
    Persistence/
    Security/

tests/
  Unit/
  Integration/
  Architecture/
  Concurrency/
```

Evite um `Shared` gigantesco com todas as regras.

`BuildingBlocks` deve conter apenas infraestrutura transversal realmente genérica.
