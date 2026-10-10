# Distrito Zero v0.1 Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Entregar um projeto Unity 3D importável e executável com o vertical slice do Capítulo 1: movimentação em terceira pessoa, interação com terminal, desafio de lógica, porta desbloqueável, drone inimigo, combate, XP, missão sequencial e conclusão.

**Architecture:** O projeto será organizado por features (`Player`, `Interaction`, `Challenges`, `Missions`, `Enemies`, `Progression`, `Core`) e comunicação por eventos C# para reduzir acoplamento. A cena `DistritoZero` usará primitivas e materiais simples na v0.1, permitindo validar o gameplay antes de investir em assets 3D externos.

**Tech Stack:** Unity 6-compatible project, C#, Unity Test Framework, Input System opcional/fallback para Input legado, VS Code, Git.

**Spec:** `docs/superpowers/specs/2026-10-09-distrito-zero-design.md`

## Global Constraints

- Jogo 3D em terceira pessoa.
- Linguagem C# e edição prevista no VS Code.
- Vertical slice offline; sem backend, multiplayer, banco externo, crafting ou inventário complexo.
- Capítulo inicial: `Distrito Zero`.
- Foco educacional da v0.1: lógica de programação.
- Resposta errada em desafio permite nova tentativa sem punição severa.
- Comunicação entre sistemas globais deve preferir eventos a referências rígidas.
- O projeto deve permanecer importável no Unity sem depender de assets comerciais ou arquivos binários proprietários.

## Review Focus

- Referências de componentes ausentes devem falhar de forma segura e produzir erro claro, não `NullReferenceException` em loop.
- Interação repetida com terminal/porta não pode avançar a missão duas vezes.
- Reinício após morte deve restaurar vida e estado de combate sem corromper o fluxo da missão.
- Resposta errada não deve desbloquear porta nem conceder XP.
- Drone destruído deve conceder XP uma única vez e emitir uma única conclusão de objetivo.

---

### Task 1: Estrutura do projeto e núcleo de eventos

**Files:**
- Create: `Assets/Scripts/Core/GameEvents.cs`
- Create: `Assets/Scripts/Core/GameManager.cs`
- Create: `Assets/Tests/EditMode/Core/GameEventsTests.cs`
- Create: `Assets/Scenes/DistritoZero.unity`
- Create: `Packages/manifest.json`
- Create: `ProjectSettings/ProjectVersion.txt`
- Create: `.gitignore`
- Create: `README.md`

**Interfaces:**
- Produces: `GameEvents.ChallengeCompleted`, `GameEvents.EnemyDefeated`, `GameEvents.PlayerDied`, `GameEvents.ChapterCompleted`.
- Produces: `GameManager.RestartCheckpoint()` and `GameManager.CompleteChapter()`.

- [ ] **Step 1: Write failing EditMode tests for one-shot event publication and reset behavior.**
- [ ] **Step 2: Run Unity EditMode tests and verify failure because `GameEvents` does not exist.**
- [ ] **Step 3: Implement `GameEvents` and the minimal `GameManager` API.**
- [ ] **Step 4: Create the Unity project metadata, folders, base scene and README.**
- [ ] **Step 5: Run EditMode tests and verify PASS.**
- [ ] **Step 6: Commit `chore: bootstrap Distrito Zero Unity project`.**

### Task 2: Player em terceira pessoa

**Files:**
- Create: `Assets/Scripts/Player/PlayerController.cs`
- Create: `Assets/Scripts/Player/PlayerHealth.cs`
- Create: `Assets/Scripts/Player/PlayerCombat.cs`
- Create: `Assets/Tests/EditMode/Player/PlayerHealthTests.cs`
- Create: `Assets/Tests/EditMode/Player/PlayerCombatTests.cs`

**Interfaces:**
- Produces: `PlayerHealth.TakeDamage(float amount)`, `PlayerHealth.ResetHealth()`, `PlayerHealth.IsDead`.
- Produces: `PlayerCombat.TryAttack(IDamageable target)`.
- Consumes: `GameEvents.PlayerDied` publication when health reaches zero.

- [ ] **Step 1: Write failing tests for damage, death exactly once, reset, attack damage and dead-target protection.**
- [ ] **Step 2: Run tests and verify expected failures.**
- [ ] **Step 3: Implement health and combat with explicit one-shot death handling.**
- [ ] **Step 4: Implement CharacterController-based walk/run/jump and third-person facing.**
- [ ] **Step 5: Run EditMode tests and verify PASS.**
- [ ] **Step 6: Commit `feat: add third person player systems`.**

### Task 3: Interação, terminal e porta

**Files:**
- Create: `Assets/Scripts/Interaction/IInteractable.cs`
- Create: `Assets/Scripts/Interaction/InteractionDetector.cs`
- Create: `Assets/Scripts/Interaction/TerminalInteractable.cs`
- Create: `Assets/Scripts/Interaction/DoorInteractable.cs`
- Create: `Assets/Tests/EditMode/Interaction/InteractionTests.cs`

**Interfaces:**
- Produces: `IInteractable.CanInteract`, `IInteractable.Interact()`.
- Produces: `DoorInteractable.Unlock()` and idempotent open state.
- Consumes: challenge start callback from `TerminalInteractable`.

- [ ] **Step 1: Write failing tests proving terminal activation and door unlock/open are idempotent.**
- [ ] **Step 2: Run tests and verify expected failures.**
- [ ] **Step 3: Implement the interaction contract, detector, terminal and door state machine.**
- [ ] **Step 4: Run EditMode tests and verify PASS.**
- [ ] **Step 5: Commit `feat: add interaction terminal and door`.**

### Task 4: Challenge educacional de lógica

**Files:**
- Create: `Assets/Scripts/Challenges/ChallengeData.cs`
- Create: `Assets/Scripts/Challenges/ChallengeManager.cs`
- Create: `Assets/Scripts/Challenges/ChallengeUI.cs`
- Create: `Assets/Tests/EditMode/Challenges/ChallengeManagerTests.cs`

**Interfaces:**
- Produces: `ChallengeManager.StartChallenge(ChallengeData data)`.
- Produces: `ChallengeManager.SubmitAnswer(int answerIndex) -> bool`.
- Publishes: `GameEvents.ChallengeCompleted` exactly once per challenge.
- Consumes: terminal request to begin challenge.

- [ ] **Step 1: Write failing tests for correct answer, wrong answer retry, invalid index, and duplicate completion prevention.**
- [ ] **Step 2: Run tests and verify expected failures.**
- [ ] **Step 3: Implement ScriptableObject challenge data and manager logic.**
- [ ] **Step 4: Implement minimal Canvas UI with prompt, alternatives and feedback.**
- [ ] **Step 5: Run EditMode tests and verify PASS.**
- [ ] **Step 6: Commit `feat: add logic challenge system`.**

### Task 5: Drone inimigo e combate

**Files:**
- Create: `Assets/Scripts/Combat/IDamageable.cs`
- Create: `Assets/Scripts/Enemies/EnemyHealth.cs`
- Create: `Assets/Scripts/Enemies/EnemyAI.cs`
- Create: `Assets/Scripts/Enemies/EnemyCombat.cs`
- Create: `Assets/Tests/EditMode/Enemies/EnemyHealthTests.cs`

**Interfaces:**
- Produces: `IDamageable.TakeDamage(float amount)`.
- Produces: `EnemyHealth.IsDead`.
- Publishes: `GameEvents.EnemyDefeated` once on death.
- Consumes: player transform and `PlayerHealth.TakeDamage(float)` for chase/attack.

- [ ] **Step 1: Write failing tests for enemy damage, single death event, no repeated XP trigger and post-death damage safety.**
- [ ] **Step 2: Run tests and verify expected failures.**
- [ ] **Step 3: Implement enemy health/combat and simple chase/attack AI using distance thresholds.**
- [ ] **Step 4: Run EditMode tests and verify PASS.**
- [ ] **Step 5: Commit `feat: add security drone enemy`.**

### Task 6: Missão, XP e fluxo completo do capítulo

**Files:**
- Create: `Assets/Scripts/Missions/MissionStep.cs`
- Create: `Assets/Scripts/Missions/MissionManager.cs`
- Create: `Assets/Scripts/Missions/MissionUI.cs`
- Create: `Assets/Scripts/Progression/PlayerExperience.cs`
- Create: `Assets/Tests/EditMode/Missions/MissionManagerTests.cs`
- Create: `Assets/Tests/EditMode/Progression/PlayerExperienceTests.cs`

**Interfaces:**
- Produces: `MissionManager.CurrentStep`, `MissionManager.Advance(MissionEvent eventType)`.
- Produces: `PlayerExperience.AddXp(int amount)` and `PlayerExperience.CurrentXp`.
- Consumes: challenge/enemy/interaction events.
- Publishes: `GameEvents.ChapterCompleted` at final step.

- [ ] **Step 1: Write failing tests for ordered progression, ignored out-of-order events, duplicate-event safety, XP once per reward and chapter completion.**
- [ ] **Step 2: Run tests and verify expected failures.**
- [ ] **Step 3: Implement mission state machine and XP component.**
- [ ] **Step 4: Implement HUD objective and temporary XP feedback.**
- [ ] **Step 5: Run EditMode tests and verify PASS.**
- [ ] **Step 6: Commit `feat: add mission and progression flow`.**

### Task 7: Montagem do vertical slice e validação

**Files:**
- Modify: `Assets/Scenes/DistritoZero.unity`
- Create: `Assets/Scripts/Core/RuntimeSceneBootstrap.cs`
- Create: `Assets/Tests/PlayMode/DistritoZeroFlowTests.cs`
- Modify: `README.md`

**Interfaces:**
- Consumes all systems from Tasks 1–6.
- Produces a complete playable flow from spawn to chapter-complete screen.

- [ ] **Step 1: Write PlayMode smoke tests for spawn, terminal challenge, door unlock, drone defeat, XP award and final completion.**
- [ ] **Step 2: Assemble a primitive-based cyberpunk test level with spawn, terminal, door, arena, drone and exit.**
- [ ] **Step 3: Wire player camera, HUD, challenge UI, health bar, mission UI and end screen.**
- [ ] **Step 4: Add defensive runtime validation for missing required references.**
- [ ] **Step 5: Run EditMode + PlayMode test suites and verify PASS with no blocking Console errors.**
- [ ] **Step 6: Update README with Unity import/run instructions and controls.**
- [ ] **Step 7: Commit `feat: deliver Distrito Zero vertical slice v0.1`.**

### Task 8: Package v0.1

**Files:**
- Create: `CHANGELOG.md`
- Create: `VERSION`
- Create: `docs/ROADMAP.md`

**Interfaces:**
- Produces: project version `0.1.0` and ZIP artifact excluding `Library/`, `Temp/`, `Logs/`, `obj/` and local IDE state.

- [ ] **Step 1: Record v0.1 features, controls, known limitations and roadmap.**
- [ ] **Step 2: Verify project tree contains no generated Unity cache directories or secrets.**
- [ ] **Step 3: Package the repository as `Distrito-Zero-v0.1.0.zip`.**
- [ ] **Step 4: Re-open the ZIP structure and verify required Unity project folders are present.**
- [ ] **Step 5: Commit `docs: prepare v0.1 release package`.**

## Self-review

- Spec coverage: player, camera, combat, enemy, interaction, challenge, mission, XP, death/restart and chapter completion are mapped to Tasks 1–7.
- Out-of-scope systems remain excluded.
- One-shot/idempotency failure modes are explicitly tested in the owning tasks.
- Feature boundaries keep files focused and support later chapter expansion.
- Packaging is separated from gameplay implementation and does not add runtime dependencies.
