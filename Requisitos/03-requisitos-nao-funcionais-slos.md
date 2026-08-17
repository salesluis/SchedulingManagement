# 03 — Requisitos Não Funcionais e SLOs

## 1. Princípio

“Milhões de usuários” é uma meta de evolução, não um número suficiente para dimensionamento.

Capacidade real deve ser modelada por:

- usuários ativos diários;
- usuários ativos simultâneos;
- requests por segundo;
- eventos criados por segundo;
- quantidade total de eventos;
- slots calculados por segundo;
- mensagens enviadas por segundo;
- tamanho médio dos calendários;
- regiões geográficas.

## 2. Disponibilidade

### SLO inicial sugerido para produção madura

- RNF001 — API principal: 99,95% de disponibilidade mensal.
- RNF002 — leitura de calendário: 99,95%.
- RNF003 — criação/alteração de eventos: 99,95%.
- RNF004 — booking público: 99,95%.
- RNF005 — processamento de notificações: SLO separado por atraso, não apenas uptime.

99,95% permite aproximadamente 21,6 minutos de indisponibilidade em um mês de 30 dias.

Não definir 99,999% sem necessidade comercial: cada “9” aumenta significativamente custo e complexidade.

## 3. Latência

Alvos sugeridos medidos no servidor:

- RNF010 — p50 de leituras comuns < 150 ms.
- RNF011 — p95 de leituras comuns < 500 ms.
- RNF012 — p99 de leituras comuns < 1 s.
- RNF013 — criação de evento p95 < 750 ms, sem aguardar notificações externas.
- RNF014 — consulta de disponibilidade p95 < 1 s para janela normal.
- RNF015 — operações assíncronas devem expor métricas de queue delay.

## 4. Escalabilidade

- RNF020 — API deve ser stateless.
- RNF021 — Instâncias devem poder escalar horizontalmente.
- RNF022 — Nenhuma sessão deve depender de memória local de uma instância.
- RNF023 — Workers devem ser horizontalmente escaláveis.
- RNF024 — Particionamento de dados deve ser possível sem mudar IDs públicos.
- RNF025 — Jobs não podem depender de cron executando em uma única máquina.
- RNF026 — Limites e quotas devem existir para impedir abuso.

## 5. Consistência

- RNF030 — Escritas do núcleo de calendário usam banco transacional.
- RNF031 — Dupla reserva deve possuir garantia de consistência forte no ponto de confirmação.
- RNF032 — Busca, analytics e notificações podem ser eventualmente consistentes.
- RNF033 — Cache nunca pode ser fonte de verdade para exclusividade.
- RNF034 — Eventos publicados para processamento assíncrono não podem ser perdidos após commit bem-sucedido.

## 6. Durabilidade

- RNF040 — Eventos confirmados devem sobreviver à perda de uma instância da aplicação.
- RNF041 — Banco de produção deve possuir replicação adequada.
- RNF042 — Backups automáticos devem existir.
- RNF043 — Backups devem ser testados por restauração.
- RNF044 — Deve existir PITR quando suportado pela infraestrutura.
- RNF045 — Retenção deve ser definida por categoria de dado.

## 7. Disaster Recovery

Metas iniciais sugeridas:

- RPO <= 5 minutos para dados críticos.
- RTO <= 60 minutos inicialmente.

Em maturidade e conforme necessidade comercial:

- RPO próximo de zero;
- RTO de poucos minutos;
- failover automatizado.

## 8. Segurança

- RNF050 — TLS obrigatório.
- RNF051 — Senhas devem ser armazenadas com algoritmo adaptativo adequado.
- RNF052 — Refresh tokens devem ser rotacionáveis/revogáveis.
- RNF053 — Segredos não podem estar no código-fonte.
- RNF054 — Princípio do menor privilégio.
- RNF055 — Rate limiting por identidade/IP/recurso.
- RNF056 — Proteção contra brute force.
- RNF057 — Auditoria para ações administrativas/sensíveis.
- RNF058 — Proteção contra IDOR/BOLA em todos os recursos identificáveis.
- RNF059 — Headers e políticas web seguras.
- RNF060 — Dependências devem ser verificadas continuamente.
- RNF061 — Webhooks externos devem possuir validação de autenticidade.
- RNF062 — Dados sensíveis devem ser criptografados em trânsito e, quando necessário, em repouso.

## 9. Privacidade

- RNF070 — Dados pessoais devem possuir finalidade definida.
- RNF071 — O sistema deve permitir política de retenção.
- RNF072 — Exclusão/anomização deve considerar obrigações legais.
- RNF073 — Logs não devem registrar tokens, senhas ou conteúdo pessoal desnecessário.
- RNF074 — Acesso administrativo a dados deve ser auditado.

## 10. Observabilidade

Toda requisição deve poder ser correlacionada.

Obrigatório:

- logs estruturados;
- métricas;
- traces distribuídos;
- correlation/trace id;
- dashboards;
- alertas;
- métricas de filas;
- métricas de dependências externas.

Métricas essenciais:

- requests/sec;
- error rate;
- p50/p95/p99;
- saturation;
- conexões do banco;
- cache hit ratio;
- queue depth;
- queue age;
- failed jobs;
- webhook retries;
- notification delivery latency;
- booking conflicts;
- lock contention.

## 11. Error budget

Cada SLO deve produzir um error budget.

Quando o consumo do budget estiver acima da política:

- reduzir frequência de releases arriscados;
- priorizar confiabilidade;
- investigar regressões;
- aumentar testes/canary/rollback.

## 12. Deploy

- RNF080 — Deploy sem indisponibilidade perceptível sempre que possível.
- RNF081 — Migrations devem ser compatíveis com rolling deployment.
- RNF082 — Mudanças de schema críticas devem usar estratégia expand/contract.
- RNF083 — Rollback deve ser conhecido antes do deploy.
- RNF084 — Health checks de liveness e readiness devem ser separados.

## 13. Resiliência

- RNF090 — Timeouts em todas as chamadas externas.
- RNF091 — Retry apenas para falhas transitórias e operações seguras/idempotentes.
- RNF092 — Exponential backoff com jitter.
- RNF093 — Circuit breaker quando adequado.
- RNF094 — Bulkheads/limites de concorrência para dependências frágeis.
- RNF095 — Falha do WhatsApp não deve indisponibilizar calendário ou booking.
