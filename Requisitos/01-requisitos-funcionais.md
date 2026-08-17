# 01 — Requisitos Funcionais

## Identidade e conta

- RF001 — O sistema deve permitir cadastro de usuário.
- RF002 — O sistema deve permitir login e encerramento de sessão.
- RF003 — O sistema deve permitir recuperação de conta.
- RF004 — O usuário deve poder possuir múltiplas sessões/dispositivos.
- RF005 — O usuário deve poder revogar sessões individualmente.
- RF006 — O sistema deve permitir verificação de e-mail e/ou telefone.
- RF007 — O usuário deve configurar locale e timezone padrão.

## Calendários

- RF010 — Um usuário deve poder possuir múltiplos calendários.
- RF011 — Deve existir ao menos um calendário principal por usuário.
- RF012 — O usuário deve poder criar, editar, arquivar e excluir calendários conforme regras de retenção.
- RF013 — Cada calendário deve possuir nome, timezone, cor, proprietário e visibilidade.
- RF014 — Um calendário deve poder ser compartilhado.
- RF015 — O sistema deve permitir definir permissões por usuário/grupo.
- RF016 — O usuário deve poder escolher quais calendários aparecem na visualização.

## Eventos

- RF020 — O usuário deve poder criar evento com início e fim.
- RF021 — O evento pode ser de dia inteiro.
- RF022 — Um evento deve possuir timezone de origem.
- RF023 — O evento pode possuir título, descrição, localização e URL.
- RF024 — O usuário deve poder editar um evento.
- RF025 — O usuário deve poder cancelar/excluir um evento.
- RF026 — O sistema deve manter histórico relevante de alterações.
- RF027 — Eventos devem possuir versão para controle de concorrência.
- RF028 — O sistema deve suportar criação idempotente de eventos.
- RF029 — Um evento pode bloquear ou não a disponibilidade.
- RF030 — Um evento pode ser marcado como público, privado ou padrão do calendário.

## Recorrência

- RF040 — O sistema deve suportar eventos recorrentes.
- RF041 — Devem ser suportadas recorrências diárias, semanais, mensais e anuais.
- RF042 — Recorrências devem suportar término por data ou quantidade.
- RF043 — Deve ser possível editar apenas uma ocorrência.
- RF044 — Deve ser possível editar uma ocorrência e as futuras.
- RF045 — Deve ser possível editar toda a série.
- RF046 — Exceções devem ser armazenadas sem duplicar indefinidamente toda a série.

## Participantes e convites

- RF050 — Um evento deve poder possuir participantes.
- RF051 — Participantes podem ser usuários internos ou endereços externos.
- RF052 — Participantes devem poder responder `accepted`, `declined`, `tentative`.
- RF053 — O organizador deve visualizar respostas.
- RF054 — Alterações relevantes devem poder gerar novas notificações.
- RF055 — O organizador pode permitir ou impedir que convidados modifiquem o evento.
- RF056 — O organizador pode permitir ou impedir que convidados convidem terceiros.

## Visualização

- RF060 — O sistema deve fornecer visualização diária.
- RF061 — O sistema deve fornecer visualização semanal.
- RF062 — O sistema deve fornecer visualização mensal.
- RF063 — O sistema deve fornecer visualização em lista/agenda.
- RF064 — A navegação deve carregar somente a janela temporal necessária.
- RF065 — Eventos de calendários distintos devem poder ser sobrepostos na mesma visualização.

## Busca

- RF070 — O usuário deve poder buscar eventos por texto.
- RF071 — A busca deve respeitar autorização.
- RF072 — A busca deve permitir filtro por período.
- RF073 — A busca deve permitir filtro por calendário.
- RF074 — Indexação avançada pode ser assíncrona.

## Disponibilidade

- RF080 — O usuário deve definir horários semanais de disponibilidade.
- RF081 — Deve permitir múltiplos intervalos no mesmo dia.
- RF082 — Deve permitir exceções por data.
- RF083 — Eventos marcados como ocupados devem reduzir disponibilidade.
- RF084 — O usuário deve selecionar quais calendários participam do cálculo de conflito.
- RF085 — Deve ser possível definir buffer antes e depois.
- RF086 — Deve ser possível definir antecedência mínima.
- RF087 — Deve ser possível definir horizonte máximo de agendamento.
- RF088 — Deve ser possível definir timezone da página de booking.
- RF089 — O cliente deve visualizar slots no próprio timezone quando possível.

## Tipos de agendamento

- RF100 — O usuário deve criar tipos de agendamento.
- RF101 — Cada tipo deve possuir nome, duração e regras de disponibilidade.
- RF102 — Um tipo pode exigir aprovação ou confirmar automaticamente.
- RF103 — Um tipo pode possuir campos customizáveis.
- RF104 — Um tipo pode possuir localização física ou URL.
- RF105 — Um tipo deve permitir buffer próprio.
- RF106 — Um tipo deve possuir slug público único dentro do namespace definido.

## Página pública

- RF110 — Uma página pública deve listar horários disponíveis.
- RF111 — O cliente deve selecionar um slot.
- RF112 — O servidor deve revalidar a disponibilidade no momento da reserva.
- RF113 — O servidor deve poder criar um hold temporário antes da confirmação.
- RF114 — O cliente deve informar os campos obrigatórios.
- RF115 — A reserva deve gerar evento no calendário configurado.
- RF116 — A reserva deve poder adicionar o cliente como participante.
- RF117 — O cliente deve receber confirmação.
- RF118 — Deve existir fluxo seguro de cancelamento.
- RF119 — Deve existir fluxo seguro de remarcação.
- RF120 — Cancelamento/remarcação por link devem usar token de alta entropia e expiração/política definida.

## Conflito e concorrência

- RF130 — O sistema deve impedir dupla reserva quando a regra de disponibilidade exigir exclusividade.
- RF131 — Duas requisições concorrentes para o mesmo slot não podem resultar em duas reservas válidas.
- RF132 — Repetição da mesma requisição por timeout não deve duplicar agendamento.
- RF133 — A validação definitiva deve ocorrer no backend/banco, nunca apenas na UI.

## Notificações

- RF140 — O sistema deve possuir preferências por canal.
- RF141 — Deve suportar notificação in-app.
- RF142 — Deve suportar WhatsApp.
- RF143 — E-mail pode ser adicionado como canal.
- RF144 — Eventos devem permitir múltiplos lembretes.
- RF145 — Lembretes devem ser processados de forma assíncrona.
- RF146 — Falha de um canal não deve reverter a criação do evento.
- RF147 — O sistema deve registrar status de cada tentativa.

## WhatsApp

- RF150 — O usuário deve poder vincular/configurar integração suportada.
- RF151 — Mensagens proativas devem utilizar os mecanismos permitidos pela plataforma.
- RF152 — O sistema deve receber webhooks da plataforma.
- RF153 — Webhooks devem ser autenticados/validados conforme o provedor.
- RF154 — Webhooks repetidos não podem produzir efeitos duplicados.
- RF155 — O sistema deve tratar estados de envio, entrega, leitura e falha quando disponibilizados.
- RF156 — Templates devem possuir versionamento/configuração.
- RF157 — Alteração do provider de WhatsApp não deve modificar regras do domínio de calendário.

## Tempo real e sincronização

- RF170 — Alterações de agenda devem ser refletidas para sessões conectadas.
- RF171 — O realtime é uma otimização; a consistência não pode depender exclusivamente dele.
- RF172 — Clientes devem possuir mecanismo de ressincronização após perda de conexão.
- RF173 — O sistema deve oferecer cursor/versionamento para sincronização incremental.
- RF174 — Alterações devem produzir um identificador monotônico por stream ou cursor equivalente.

## Auditoria

- RF180 — Alterações sensíveis devem registrar ator, instante, recurso e ação.
- RF181 — Auditoria não deve ser editável pelo usuário comum.
- RF182 — Eventos de auditoria devem possuir correlação com a requisição.
