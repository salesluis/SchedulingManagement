# 06 — WhatsApp e Notificações

## 1. Fronteira arquitetural

O domínio não deve chamar diretamente a API do WhatsApp.

Errado:

```text
BookingService
   -> HttpClient Meta
```

Preferível:

```text
Booking confirmado
   ↓
BookingConfirmed
   ↓
Outbox
   ↓
Broker
   ↓
Notification Worker
   ↓
WhatsApp Adapter
   ↓
Provider
```

## 2. Modelo

```text
Notification
- Id
- UserId/Recipient
- Type
- Channel
- Template
- ScheduledAt
- Status

DeliveryAttempt
- NotificationId
- Attempt
- Provider
- ProviderMessageId
- Status
- ErrorCode
- StartedAt
- FinishedAt
```

## 3. Casos

- booking criado;
- booking aprovado;
- booking rejeitado;
- booking cancelado;
- booking remarcado;
- lembrete;
- evento alterado;
- convite;
- RSVP quando aplicável.

## 4. Templates

Templates do provider devem ser tratados como recursos externos versionados.

Internamente:

```text
NotificationTemplate
  logical_name = booking_reminder
  locale = pt-BR
  provider_template_id
  version
```

O domínio referencia `booking_reminder`, não o nome físico específico do provider.

## 5. Webhooks

Endpoint:

```text
POST /webhooks/whatsapp/{connectionId}
```

Fluxo:

1. validar autenticidade conforme provider;
2. extrair ID do evento;
3. registrar Inbox;
4. responder rapidamente;
5. processar assíncronamente;
6. atualizar status;
7. publicar evento interno se necessário.

## 6. Regra de resposta rápida

O webhook não deve executar uma cadeia longa antes de responder ao provider.

Objetivo:

```text
Receive
Validate
Persist
ACK
```

Depois:

```text
Worker -> process
```

## 7. Deduplicação

Webhook pode chegar repetido.

A chave externa precisa possuir unicidade lógica.

O mesmo evento externo não pode:

- confirmar duas vezes;
- cancelar duas vezes;
- criar duas reservas;
- disparar duas automações equivalentes.

## 8. Retries

Para envio:

- timeout;
- retry com exponential backoff;
- jitter;
- limite;
- DLQ.

Erros permanentes devem ser classificados e não repetidos indefinidamente.

## 9. Rate limits

O provider possui limites próprios e esses limites podem mudar.

O adapter deve:

- observar respostas do provider;
- limitar concorrência;
- respeitar retry hints;
- expor métricas;
- proteger o restante da aplicação.

## 10. Falha do WhatsApp

Calendário continua operacional.

Booking pode ser confirmado mesmo se WhatsApp estiver indisponível.

Estado exemplo:

```text
Booking = Confirmed
Notification = Pending/Failed
```

Nunca:

```text
WhatsApp caiu => Booking rollback
```

## 11. Privacidade

Evitar inserir em mensagens mais informação que o necessário.

Não registrar payload completo em logs comuns quando contiver:

- telefone;
- nome;
- conteúdo de mensagem;
- tokens;
- identificadores sigilosos.

## 12. Entrada pelo WhatsApp

Evolução possível:

```text
Cliente: quero marcar amanhã
        ↓
Webhook
        ↓
Conversation/Application Layer
        ↓
consulta Availability
        ↓
propõe slots
        ↓
cliente escolhe
        ↓
Booking command
```

Mesmo nesse cenário, WhatsApp apenas traduz interação em comandos.

A regra de disponibilidade continua no módulo Availability/Booking.
