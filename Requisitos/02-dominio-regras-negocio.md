# 02 — Domínio e Regras de Negócio

## 1. Contextos de domínio

### Identity

Responsável por:

- User;
- Credential;
- Session;
- Device;
- WorkspaceMembership.

### Calendar

Responsável por:

- Calendar;
- CalendarMember;
- Event;
- EventAttendee;
- RecurrenceRule;
- EventException.

### Availability

Responsável por:

- AvailabilitySchedule;
- AvailabilityWindow;
- AvailabilityException;
- BusyInterval;
- Slot.

### Booking

Responsável por:

- BookingType;
- BookingPage;
- Booking;
- BookingHold;
- BookingFormAnswer.

### Notification

Responsável por:

- Notification;
- Reminder;
- DeliveryAttempt;
- NotificationPreference.

### Integration

Responsável por:

- IntegrationConnection;
- WebhookInbox;
- ProviderMessage;
- provider-specific metadata.

## 2. Agregados principais

### Calendar

Invariantes:

- possui exatamente um proprietário lógico;
- pode possuir membros;
- permissões são avaliadas antes de mutações;
- timezone deve ser um identificador válido;
- calendário arquivado não aceita eventos novos.

### Event

Campos conceituais:

- EventId;
- CalendarId;
- OrganizerId;
- Title;
- Description;
- StartInstant;
- EndInstant;
- TimeZone;
- AllDay;
- Transparency;
- Visibility;
- Status;
- Version;
- Recurrence;
- CreatedAt;
- UpdatedAt.

Invariantes:

- `EndInstant > StartInstant`;
- evento de dia inteiro segue semântica própria de datas;
- alterações usam optimistic concurrency;
- evento cancelado não volta silenciosamente para confirmado;
- uma ocorrência materializada deve manter referência à série.

### Booking

Representa uma reserva criada a partir de uma página pública.

Não deve ser confundido com `Event`.

Um Booking referencia o Event criado.

Isso permite que eventos normais existam sem regras comerciais de booking.

## 3. Regras gerais de data e horário

- RN001 — Instantes persistidos devem ser normalizados em UTC.
- RN002 — O timezone original deve ser preservado quando necessário para semântica humana/recorrência.
- RN003 — O sistema deve utilizar timezones IANA.
- RN004 — Nunca calcular recorrência mensal apenas somando uma quantidade fixa de segundos.
- RN005 — Mudanças de horário de verão devem ser resolvidas pelo timezone, não por offset fixo.
- RN006 — Evento all-day deve ser modelado por datas e não como um evento arbitrário 00:00–23:59:59.

## 4. Permissões

Níveis sugeridos:

- `FreeBusyReader`;
- `Reader`;
- `Writer`;
- `Owner`.

Regras:

- RN010 — `FreeBusyReader` vê apenas ocupado/livre.
- RN011 — `Reader` pode visualizar detalhes permitidos.
- RN012 — `Writer` pode criar/editar conforme política.
- RN013 — `Owner` administra compartilhamento e configurações.
- RN014 — Um evento privado pode ocultar detalhes mesmo de alguns leitores.
- RN015 — Autorização deve ser validada no servidor a cada operação protegida.

## 5. Eventos

- RN020 — Cada evento pertence a um calendário.
- RN021 — Evento pode ou não consumir disponibilidade.
- RN022 — Cancelamento deve manter informação suficiente para sincronização e auditoria.
- RN023 — Alterações concorrentes utilizam versão/ETag.
- RN024 — Atualização com versão antiga deve retornar conflito.
- RN025 — Operações destrutivas devem possuir semântica definida de soft-delete/tombstone quando necessárias para sync.

## 6. Recorrência

- RN030 — A série guarda a regra; ocorrências não precisam ser persistidas infinitamente.
- RN031 — Instâncias podem ser expandidas sob demanda dentro de uma janela.
- RN032 — Alteração de uma ocorrência cria exceção.
- RN033 — Cancelamento de uma ocorrência cria exceção/tombstone.
- RN034 — Alteração “esta e futuras” pode dividir a série em duas.
- RN035 — Uma expansão deve possuir limites para impedir regras maliciosas que gerem volume ilimitado.

## 7. Disponibilidade

Um slot só é disponível quando satisfaz simultaneamente:

1. está dentro da regra semanal;
2. não está em exceção indisponível;
3. respeita antecedência mínima;
4. respeita horizonte máximo;
5. possui duração suficiente;
6. respeita buffers;
7. não conflita com intervalos ocupados;
8. não possui hold/reserva concorrente ativa.

- RN040 — Disponibilidade exibida ao cliente é apenas uma projeção.
- RN041 — A disponibilidade deve ser revalidada na confirmação.
- RN042 — Um slot listado anteriormente pode deixar de existir.
- RN043 — O frontend nunca possui autoridade final para reservar.

## 8. Hold

- RN050 — Um hold é temporário.
- RN051 — Hold deve possuir TTL.
- RN052 — Hold expirado não bloqueia slot.
- RN053 — Hold deve ser idempotente.
- RN054 — Apenas um hold/reserva exclusiva pode ganhar a disputa pelo mesmo recurso/intervalo quando exclusividade for exigida.
- RN055 — A confirmação deve ocorrer atomicamente em relação à regra de exclusividade.

## 9. Booking

Estados sugeridos:

- `Pending`;
- `Confirmed`;
- `Cancelled`;
- `Rejected`;
- `Completed`;
- `NoShow`.

Regras:

- RN060 — Booking cria ou referencia um evento.
- RN061 — Booking cancelado deve refletir a política configurada no evento.
- RN062 — Remarcação não deve editar silenciosamente uma reserva passada sem registro.
- RN063 — O sistema deve armazenar snapshot das informações relevantes do tipo de agendamento quando necessário.
- RN064 — Alterações futuras no BookingType não podem modificar historicamente reservas existentes.
- RN065 — BookingType pode exigir aprovação.
- RN066 — BookingType pode confirmar automaticamente.
- RN067 — O proprietário define quais calendários bloqueiam seus slots.

## 10. Participantes

- RN070 — Um participante possui estado de resposta.
- RN071 — Um participante externo pode existir sem UserId.
- RN072 — E-mail/telefone do participante não deve conceder automaticamente acesso à conta.
- RN073 — Atualizações relevantes devem manter sequência/versionamento para evitar sobrescrever respostas recentes.

## 11. Idempotência

Operações críticas devem aceitar `Idempotency-Key`, especialmente:

- criar booking;
- confirmar hold;
- criar evento via integração;
- processar webhook;
- enviar comando externo.

- RN080 — A mesma chave com o mesmo payload retorna o mesmo resultado lógico.
- RN081 — A mesma chave com payload incompatível deve gerar erro.
- RN082 — O registro de idempotência possui período de retenção definido.
