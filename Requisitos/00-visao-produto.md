# 00 — Visão do Produto

## 1. Objetivo

Construir uma plataforma de calendário multiusuário capaz de centralizar:

- calendários pessoais;
- calendários profissionais;
- eventos;
- compromissos;
- reuniões;
- lembretes;
- compartilhamento;
- convites;
- disponibilidade;
- agendamento público;
- notificações;
- integração com WhatsApp.

O sistema deve permitir que um usuário administre sua agenda como em um calendário convencional e, opcionalmente, disponibilize regras públicas para que terceiros encontrem horários livres e criem solicitações ou reservas.

## 2. Problema

Hoje, profissionais e organizações frequentemente mantêm dois sistemas separados:

1. o calendário real;
2. a conversa com o cliente pelo WhatsApp.

Isso produz:

- conflitos de horário;
- confirmações manuais;
- lembretes manuais;
- duplicidade de informação;
- dificuldade de compartilhar disponibilidade;
- dificuldade de delegar acesso ao calendário;
- ausência de uma única fonte de verdade sobre compromissos.

## 3. Proposta

A agenda é a fonte de verdade.

O WhatsApp funciona como canal de comunicação e automação.

A página pública de booking usa a disponibilidade calculada a partir dos calendários do usuário.

## 4. Tipos de usuário

### 4.1 Usuário autenticado

Pode:

- possuir calendários;
- criar eventos;
- receber convites;
- compartilhar calendários;
- configurar disponibilidade;
- publicar páginas de agendamento;
- configurar notificações.

### 4.2 Participante externo

Pode receber convite sem necessariamente possuir conta.

### 4.3 Cliente de página pública

Pode consultar disponibilidade e criar uma reserva conforme as regras definidas pelo proprietário.

### 4.4 Administrador de workspace

Opcional para evolução B2B.

Pode administrar:

- membros;
- políticas;
- calendários organizacionais;
- segurança;
- integrações.

## 5. Capacidades do produto

### Calendário

- múltiplos calendários por usuário;
- visualização diária, semanal, mensal e agenda;
- eventos com horário;
- eventos de dia inteiro;
- eventos recorrentes;
- timezone;
- participantes;
- localização;
- links;
- descrição;
- anexos em evolução futura;
- cores e categorias.

### Colaboração

- compartilhamento de calendário;
- níveis de permissão;
- convites;
- RSVP;
- atualização de participantes;
- cancelamento;
- delegação.

### Disponibilidade

- horário semanal;
- exceções;
- férias;
- buffers;
- duração;
- antecedência mínima;
- horizonte máximo;
- calendários que bloqueiam disponibilidade.

### Booking

- link público;
- tipos de agendamento;
- formulário customizável;
- disponibilidade;
- hold temporário;
- confirmação;
- cancelamento;
- remarcação;
- prevenção de dupla reserva.

### WhatsApp

- confirmação;
- lembrete;
- alteração;
- cancelamento;
- ações iniciadas pelo usuário;
- webhooks de status;
- templates aprovados;
- rastreamento de envio.

## 6. Fora do primeiro núcleo

Não é requisito para começar:

- videoconferência própria;
- sistema de pagamentos;
- marketplace;
- faturamento;
- CRM completo;
- inteligência artificial;
- arquitetura de dezenas de microserviços;
- multi-region active-active;
- mecanismo de busca distribuído dedicado;
- data warehouse;
- aplicativo mobile nativo.

Esses itens podem existir posteriormente.

## 7. Objetivo de escala

A aplicação deve ser projetada para não possuir dependência estrutural que impeça evolução para milhões de contas.

Isso significa:

- API stateless;
- IDs globalmente únicos;
- processamento assíncrono;
- isolamento lógico dos domínios;
- idempotência;
- particionamento possível;
- cache invalidável;
- observabilidade;
- ausência de sessão local em uma instância;
- nenhum worker único como ponto crítico.

Não significa provisionar infraestrutura de milhões de usuários na primeira versão.
