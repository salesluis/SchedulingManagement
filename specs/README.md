# Especificações de domínio

Este diretório reúne as especificações que orientam a implementação. Crie uma pasta para cada domínio em `specs/<nome-do-dominio>/`, usando nomes em português e `kebab-case`, como `specs/agendamentos/` ou `specs/disponibilidade-profissional/`.

## Como iniciar um domínio

Copie os arquivos de `specs/_modelo/` para a nova pasta e preencha-os na ordem numérica. Cada documento deve descrever o comportamento desejado antes que o código seja criado ou alterado.

```sh
mkdir -p specs/agendamentos
cp specs/_modelo/*.md specs/agendamentos/
```

Mantenha cada especificação próxima do problema de negócio: registre apenas decisões, regras e cenários que orientem desenvolvimento, testes e revisão. Atualize a especificação no mesmo pull request que modificar o comportamento do domínio.

Não use `_modelo/` como especificação real nem altere seus arquivos para atender a um único domínio.
