# 05 — Consistência, Concorrência e Dupla Reserva

## 1. Cenário crítico

Dois clientes recebem o slot:

```text
10:00 - 10:30
```

Ambos clicam “Confirmar” quase simultaneamente.

Isto é normal em uma aplicação distribuída.

A solução não pode ser:

```csharp
if (SlotEstaLivre())
{
    await CriarAgendamento();
}
```

porque há uma janela entre leitura e escrita.

## 2. Fluxo correto

```text
Cliente A                   Cliente B
   │                           │
   ├── POST /bookings ────────►│
   │                           ├── POST /bookings
   │                           │
   ▼                           ▼
             API
              │
              ▼
        Transação no DB
              │
     ┌────────┴────────┐
     │                 │
 A obtém exclusividade │ B conflita
     │                 │
 COMMIT                ROLLBACK/409
```

## 3. Garantia no banco

Para recursos que não aceitam sobreposição, a camada de persistência deve possuir uma garantia real.

Com PostgreSQL, intervalos/range types + exclusion constraints podem ser usados para modelar intervalos não sobrepostos em determinados cenários.

Outra estratégia é modelar slots discretos e aplicar unique constraint.

A escolha depende de:

- granularidade;
- recorrência;
- múltiplos recursos;
- duração variável.

## 4. Optimistic concurrency

Eventos devem possuir `Version`.

Exemplo:

```text
GET event
Version = 8

Cliente A salva versão 8 -> versão 9
Cliente B tenta salvar versão 8 -> 409 Conflict
```

Isso impede lost update.

## 5. Idempotency key

Problema:

```text
cliente envia POST
servidor salva
resposta se perde
cliente tenta novamente
```

Sem idempotência:

```text
Booking A
Booking B
```

Com idempotência:

```http
Idempotency-Key: 01K...
```

Servidor persiste:

```text
key
request_hash
resource_id
status
response
expires_at
```

Retry retorna o mesmo resultado lógico.

## 6. Outbox

A criação de booking deve comprometer:

- Booking;
- Event;
- OutboxMessage

na mesma transação quando estiverem na mesma fonte transacional.

WhatsApp acontece depois.

Nunca:

```text
1. chamar WhatsApp
2. salvar booking
```

Se o banco falhar, uma confirmação poderá ser enviada para uma reserva inexistente.

## 7. Inbox de webhooks

Providers podem reenviar webhooks.

Persistir identificador externo:

```text
provider
external_event_id UNIQUE
received_at
processed_at
payload_hash
```

Processamento:

```text
INSERT inbox
if duplicate -> ACK sem repetir efeito
process
mark processed
```

## 8. Locks distribuídos

Não usar distributed lock como primeira garantia de integridade.

Locks distribuídos podem otimizar coordenação, mas a regra definitiva deve existir onde a escrita autoritativa acontece.

## 9. Consistência forte vs eventual

### Forte

- confirmar booking;
- permissões;
- criação/edição do evento;
- reserva exclusiva;
- idempotência.

### Eventual

- WhatsApp;
- e-mail;
- busca;
- analytics;
- realtime;
- cache;
- relatórios.

## 10. Retry

Retries só devem ocorrer quando:

- erro é transitório;
- operação é idempotente;
- existe backoff;
- existe limite;
- existe observabilidade.

Não fazer retry cego em erro de validação, conflito ou autorização.

## 11. Dead Letter Queue

Após limite de tentativas:

```text
job -> DLQ
```

A DLQ deve permitir:

- inspeção;
- motivo;
- quantidade de tentativas;
- replay controlado;
- correlação.

## 12. Scheduler de lembretes

Evitar um timer em memória por evento.

Modelo recomendado:

```text
Reminder
  due_at
  status
  shard_key
```

Workers consultam/consomem itens vencendo.

Em escala alta, pode-se usar:

- delayed queues;
- buckets temporais;
- scheduler distribuído.

A regra central é: perda/restart de uma instância não pode apagar lembretes.
