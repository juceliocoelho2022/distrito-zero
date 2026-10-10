# Distrito Zero — Design do Vertical Slice

Data: 2026-10-09

## 1. Visão do jogo

**Distrito Zero** é o primeiro capítulo de um RPG 3D cyberpunk educacional desenvolvido em **Unity + C#**, com edição no **VS Code**. O jogo combina exploração em terceira pessoa, combate leve, missões e desafios tecnológicos.

O objetivo do projeto é criar uma experiência jogável que ensine conceitos de lógica de programação e tecnologia sem transformar o gameplay em um simples quiz.

## 2. Público e proposta

O jogo é pensado para estudantes e iniciantes em tecnologia, mas deve funcionar como jogo mesmo para quem ainda não domina programação.

A aprendizagem acontece integrada às missões: o jogador resolve problemas para abrir portas, recuperar sistemas, ativar terminais e avançar na narrativa.

## 3. Tema e ambientação

O cenário é uma cidade cyberpunk controlada por sistemas automatizados, corporações e inteligências artificiais. O primeiro capítulo ocorre no **Distrito Zero**, uma zona isolada após uma falha crítica de infraestrutura.

Elementos visuais principais:
- ruas urbanas escuras;
- iluminação neon;
- hologramas;
- drones de segurança;
- terminais tecnológicos;
- portas e sistemas eletrônicos;
- prédios altos e atmosfera futurista.

## 4. Estrutura do Capítulo 1

### Objetivo narrativo

O jogador desperta em uma área bloqueada por uma IA hostil. Para escapar do Distrito Zero, precisa restaurar parte da rede local, desbloquear uma passagem e neutralizar um drone de segurança.

### Fluxo da missão

1. Jogador começa em uma pequena área segura.
2. Tutorial curto ensina movimentação.
3. Jogador encontra o primeiro terminal.
4. Terminal apresenta um desafio simples de lógica.
5. Ao resolver o desafio, a energia de uma porta é restaurada.
6. A nova área contém um drone hostil.
7. Jogador aprende o combate básico.
8. Após derrotar o drone, recebe XP.
9. Uma segunda interação libera a saída.
10. Tela de conclusão encerra o capítulo.

## 5. Mecânicas principais

### Movimento
- terceira pessoa;
- andar;
- correr;
- pular;
- câmera acompanhando o jogador.

### Combate
- ataque básico;
- dano;
- vida;
- morte e reinício simples;
- inimigo com comportamento de perseguição e ataque.

### Interação
- detectar objetos interativos;
- exibir mensagem de interação;
- ativar terminais;
- abrir portas;
- futuramente conversar com NPCs.

### Missões
- objetivo atual exibido na tela;
- progressão sequencial;
- atualização automática após ações relevantes.

### Progressão
- XP simples;
- recompensa ao concluir objetivo importante;
- base preparada para níveis e habilidades futuras.

## 6. Sistema educacional

No primeiro capítulo, o foco será **lógica de programação**.

Exemplo de desafio:

> Um sistema precisa escolher qual porta abrir. Se a energia for maior que 50%, abra a porta principal; caso contrário, use a rota de emergência.

O jogador deverá identificar a lógica correta entre alternativas apresentadas.

Os desafios futuros podem incluir:
- variáveis;
- condições;
- loops;
- funções;
- orientação a objetos;
- SQL;
- APIs;
- redes;
- segurança;
- IA.

## 7. Arquitetura de sistemas

### Player
Responsabilidades:
- movimento;
- corrida;
- pulo;
- vida;
- ataque;
- recebimento de dano.

Componentes sugeridos:
- `PlayerController`
- `PlayerHealth`
- `PlayerCombat`

### Camera
Responsabilidades:
- seguir o jogador;
- controlar rotação;
- manter enquadramento em terceira pessoa.

Componente sugerido:
- Cinemachine, se disponível no projeto.

### Enemy
Responsabilidades:
- detectar jogador;
- perseguir;
- atacar;
- receber dano;
- morrer.

Componentes sugeridos:
- `EnemyAI`
- `EnemyHealth`
- `EnemyCombat`

### InteractionSystem
Responsabilidades:
- identificar objetos interativos;
- mostrar prompt;
- disparar ações.

Interface sugerida:
- `IInteractable`

Implementações iniciais:
- `TerminalInteractable`
- `DoorInteractable`

### ChallengeSystem
Responsabilidades:
- carregar desafio;
- apresentar alternativas;
- validar resposta;
- emitir resultado.

Componentes sugeridos:
- `ChallengeManager`
- `ChallengeData`
- `ChallengeUI`

### MissionSystem
Responsabilidades:
- controlar estado da missão;
- armazenar objetivo atual;
- reagir aos eventos do jogo;
- atualizar interface.

Componentes sugeridos:
- `MissionManager`
- `MissionStep`
- `MissionUI`

### GameManager
Responsabilidades:
- estado geral do jogo;
- início e fim do capítulo;
- reinício após morte;
- integração entre sistemas globais.

## 8. Comunicação entre sistemas

Preferência por eventos em vez de dependências rígidas.

Exemplo:

`ChallengeCompleted`
→ `MissionManager` avança a missão
→ `DoorInteractable` recebe sinal para desbloquear
→ UI atualiza o objetivo

Isso reduz acoplamento e facilita adicionar novas missões posteriormente.

## 9. Interface inicial

HUD mínimo:
- barra de vida;
- objetivo atual;
- indicador de interação;
- XP recebido;
- painel de desafio.

Evitar menus complexos no primeiro vertical slice.

## 10. Tratamento de falhas

### Jogador morre
- tela curta de derrota;
- reinício do último ponto seguro ou início da arena.

### Resposta errada
- feedback visual;
- jogador pode tentar novamente;
- sem punição severa.

### Inimigo perde o jogador
- retorna ao estado de patrulha ou espera.

## 11. Escopo que fica fora do primeiro capítulo

Não implementar agora:
- mundo aberto;
- multiplayer;
- crafting;
- inventário complexo;
- árvore completa de habilidades;
- dezenas de inimigos;
- geração procedural;
- sistema de diálogo avançado;
- backend online;
- ranking online;
- banco de dados externo.

Esses itens podem ser adicionados após validar o vertical slice.

## 12. Testes

### Testes de gameplay
- jogador consegue andar, correr e pular;
- câmera acompanha corretamente;
- ataque causa dano;
- inimigo persegue e ataca;
- inimigo pode morrer;
- terminal pode ser ativado;
- desafio aceita resposta correta;
- porta desbloqueia após sucesso;
- missão avança na ordem esperada;
- capítulo pode ser concluído.

### Testes técnicos
- ausência de erros no Console;
- referências de componentes validadas;
- estados de missão não quebram ao reiniciar cena;
- objetos interativos não disparam múltiplas vezes indevidamente.

## 13. Critérios de sucesso do vertical slice

O protótipo será considerado validado quando o jogador conseguir:

1. iniciar o capítulo;
2. movimentar-se livremente;
3. localizar o terminal;
4. resolver um desafio de lógica;
5. desbloquear uma nova área;
6. enfrentar e derrotar um drone;
7. receber XP;
8. concluir a missão;
9. chegar à tela final sem erros bloqueadores.

## 14. Stack inicial

- Unity
- C#
- Visual Studio Code
- Git
- GitHub

Pacotes opcionais:
- Cinemachine
- Input System

## 15. Direção de evolução

Após validar o Distrito Zero, o jogo poderá crescer por capítulos:

- Capítulo 2: variáveis e tipos de dados;
- Capítulo 3: condicionais;
- Capítulo 4: loops;
- Capítulo 5: funções;
- Capítulo 6: orientação a objetos;
- capítulos avançados: banco de dados, APIs, segurança, redes e IA.

A progressão pedagógica acompanha a progressão do personagem e do mundo.
