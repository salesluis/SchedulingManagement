# 08 — Contrato Inicial de API

## 1. Convenções

Base:

```text
/api/v1
```

JSON.

Todos os timestamps de instante são ISO-8601.

Exemplo:

```json
{
  "start": "2026-08-20T13:00:00Z",
  "end": "2026-08-20T14:00:00Z",
  "timeZone": "America/Bahia"
}
```

## 2. Calendários

```http
POST /api/v1/calendars
GET  /api/v1/calendars
GET  /api/v1/calendars/{calendarId}
PATCH /api/v1/calendars/{calendarId}
DELETE /api/v1/calendars/{calendarId}
```

## 3. Eventos

```http
POST /api/v1/calendars/{calendarId}/events

GET /api/v1/calendars/{calendarId}/events
    ?from=2026-08-01T00:00:00Z
    &to=2026-09-01T00:00:00Z

GET    /api/v1/events/{eventId}
PATCH  /api/v1/events/{eventId}
DELETE /api/v1/events/{eventId}
```

## 4. Concorrência

Resposta:

```http
ETag: "event-version-18"
```

Atualização:

```http
If-Match: "event-version-18"
```

Se já mudou:

```http
409 Conflict
```

ou `412 Precondition Failed`, conforme a convenção escolhida.

O importante é manter uma regra consistente.

## 5. Idempotência

Criação crítica:

```http
POST /api/v1/public/booking-pages/{slug}/bookings
Idempotency-Key: 01K2...
```

## 6. Disponibilidade

```http
GET /api/v1/public/booking-pages/{slug}/availability
    ?from=2026-08-20
    &to=2026-08-27
    &timeZone=America/Bahia
```

Resposta:

```json
{
  "timeZone": "America/Bahia",
  "slots": [
    {
      "start": "2026-08-20T13:00:00Z",
      "end": "2026-08-20T13:30:00Z"
    }
  ],
  "version": "av_238918"
}
```

`version` é útil para cache/diagnóstico, mas não garante que o slot continuará livre.

## 7. Booking

```http
POST /api/v1/public/booking-pages/{slug}/holds
POST /api/v1/public/booking-pages/{slug}/bookings
GET  /api/v1/bookings/{bookingId}
POST /api/v1/bookings/{bookingId}/cancel
POST /api/v1/bookings/{bookingId}/reschedule
```

Evitar endpoints genéricos como:

```text
POST /booking/updateStatus
```

Prefira comandos que expressem intenção.

## 8. Erros

Formato inspirado em Problem Details:

```json
{
  "type": "https://errors.example.com/booking-slot-conflict",
  "title": "Slot is no longer available",
  "status": 409,
  "code": "BOOKING_SLOT_CONFLICT",
  "traceId": "..."
}
```

Códigos estáveis são importantes para clientes.

## 9. Paginação

Para feeds extensos, preferir cursor:

```http
GET /api/v1/notifications?cursor=...
```

Evitar depender de offset em tabelas enormes para paginação profunda.

## 10. Rate limiting

Respostas devem expor status apropriado:

```http
429 Too Many Requests
```

e retry metadata quando aplicável.

## 11. Webhooks externos

```http
POST /api/v1/webhooks/whatsapp/{connectionId}
```

Esse endpoint:

- valida;
- registra;
- responde;
- processa depois.

## 12. Sync incremental

Evolução:

```http
GET /api/v1/sync/calendar
    ?cursor=abc123
```

Resposta:

```json
{
  "changes": [],
  "nextCursor": "abc124"
}
```

Se cursor for inválido/expirado:

```text
SYNC_RESET_REQUIRED
```

e cliente executa full sync da janela/escopo permitido.
