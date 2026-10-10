# Distrito Zero v0.1.0

RPG 3D cyberpunk educacional feito em **Unity + C#**, preparado para edição no **VS Code**.

## Conceito Visual — Tela Inicial

![Conceito visual da tela inicial do Distrito Zero](docs/images/distrito-zero-main-menu-concept.jpg)

Esta arte define a direção visual inicial do projeto: uma cidade cyberpunk em ruínas, atmosfera de sobrevivência, protagonista em terceira pessoa, iluminação dramática e interface com foco em **Novo Jogo**, **Configurações**, **Créditos** e **Sair**.

> A imagem acima é uma referência conceitual de direção de arte. A implementação final no Unity poderá evoluir com animações, partículas, iluminação dinâmica, áudio ambiente e transições de interface.

## Como abrir

1. Instale o **Unity Hub**.
2. Instale **Unity 6** (o projeto foi preparado para a linha Unity 6; se sua revisão for diferente, aceite o upgrade do projeto).
3. No Unity Hub, clique em **Add / Add project from disk** e selecione esta pasta.
4. Abra `Assets/Scenes/DistritoZero.unity`.
5. Pressione **Play**. O cenário é montado automaticamente em runtime.

## Controles

- `WASD`: movimentação
- `Shift`: correr
- `Espaço`: pular
- `E`: interagir
- `F`: atacar
- `R`: reiniciar depois de morte/conclusão

## Fluxo da v0.1

1. Aproxime-se do terminal azul e pressione `E`.
2. Resolva o desafio de lógica.
3. A porta recebe energia; aproxime-se e pressione `E` para abri-la.
4. Derrote o drone usando `F` quando estiver próximo.
5. Receba 100 XP.
6. Caminhe até a plataforma verde no final da rua.
7. O capítulo é concluído.

## VS Code

Use as extensões **C#** e **C# Dev Kit**. No Unity, configure VS Code como External Script Editor, se desejar.

## Testes

O projeto contém testes em `Assets/Tests/EditMode` e `Assets/Tests/PlayMode`. Execute em **Window > General > Test Runner** dentro do Unity.

> Observação: este ZIP foi gerado fora do Unity Editor. A estrutura e o código foram validados estaticamente, mas os testes Unity devem ser executados localmente após a primeira importação, pois o ambiente de geração não possui o Unity Editor.

## Escopo da v0.1

Sem assets comerciais, backend, multiplayer, banco externo ou mundo aberto. A prioridade desta versão é validar o loop jogável e manter uma arquitetura simples para evolução incremental.
