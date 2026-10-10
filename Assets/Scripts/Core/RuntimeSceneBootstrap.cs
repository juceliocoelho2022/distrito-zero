using UnityEngine;
using DistritoZero.Player;
using DistritoZero.Interaction;
using DistritoZero.Challenges;
using DistritoZero.Enemies;
using DistritoZero.Missions;
using DistritoZero.Progression;

namespace DistritoZero.Core
{
    public class RuntimeSceneBootstrap : MonoBehaviour
    {
        private static bool built;
        private Transform player;
        private InteractionDetector interaction;
        private PlayerHealth playerHealth;
        private PlayerExperience xp;
        private PlayerShooter shooter;
        private MissionManager mission;
        private ChallengeManager challenge;
        private GameManager game;
        private string flash = "";
        private float flashUntil;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void AutoCreate()
        {
            if (FindFirstObjectByType<RuntimeSceneBootstrap>() == null)
                new GameObject("RuntimeSceneBootstrap").AddComponent<RuntimeSceneBootstrap>();
        }

        private void Start()
        {
            if (built) return;
            built = true;
            BuildWorld();
            GameEvents.XpGained += OnXp;
            GameEvents.PlayerDied += OnDeath;
        }

        private void OnDestroy()
        {
            built = false;
            GameEvents.XpGained -= OnXp;
            GameEvents.PlayerDied -= OnDeath;
        }

        private void BuildWorld()
        {
            game = new GameObject("GameManager").AddComponent<GameManager>();
            challenge = new GameObject("ChallengeManager").AddComponent<ChallengeManager>();
            mission = new GameObject("MissionManager").AddComponent<MissionManager>();

            CreateCube("Street", new Vector3(0,-0.5f,8), new Vector3(18,1,32), new Color(0.08f,0.09f,0.12f));
            CreateCube("NeonLeft", new Vector3(-8,1,8), new Vector3(1,3,32), new Color(0.05f,0.3f,0.55f));
            CreateCube("NeonRight", new Vector3(8,1,8), new Vector3(1,3,32), new Color(0.45f,0.05f,0.5f));

            var p = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            p.name = "Player"; p.transform.position = new Vector3(0,1, -4);
            Object.Destroy(p.GetComponent<CapsuleCollider>());
            var cc = p.AddComponent<CharacterController>(); cc.height = 2f; cc.radius = .45f;
            p.AddComponent<PlayerController>();
            playerHealth = p.AddComponent<PlayerHealth>();
            p.AddComponent<PlayerCombat>();
            shooter = p.AddComponent<PlayerShooter>();
            interaction = p.AddComponent<InteractionDetector>();
            xp = p.AddComponent<PlayerExperience>();
            player = p.transform;

            var camObj = new GameObject("Main Camera"); camObj.tag = "MainCamera";
            var cam = camObj.AddComponent<Camera>(); cam.fieldOfView = 65f;
            var follow = camObj.AddComponent<ThirdPersonCamera>(); follow.target = player;

            var term = CreateCube("Terminal", new Vector3(-3,1,1), new Vector3(1,2,1), new Color(0f,0.9f,0.8f));
            term.AddComponent<TerminalInteractable>();

            CreateCube("BarrierLeft", new Vector3(-5,1.5f,5), new Vector3(6,3,0.7f), new Color(0.15f,0.08f,0.18f));
            CreateCube("BarrierRight", new Vector3(5,1.5f,5), new Vector3(6,3,0.7f), new Color(0.15f,0.08f,0.18f));
            var door = CreateCube("SecurityDoor", new Vector3(0,1.5f,5), new Vector3(4,3,0.7f), new Color(0.8f,0.1f,0.5f));
            door.AddComponent<DoorInteractable>();

            var drone = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            drone.name = "SecurityDrone"; drone.transform.position = new Vector3(0,1.3f,12); drone.transform.localScale = Vector3.one * 1.4f;
            SetColor(drone, new Color(0.9f,0.15f,0.1f));
            drone.AddComponent<EnemyHealth>(); drone.AddComponent<EnemyCombat>(); drone.AddComponent<EnemyAI>();

            var exit = CreateCube("ExitZone", new Vector3(0,0.15f,21), new Vector3(4,0.3f,3), new Color(0.1f,1f,0.3f));
            var box = exit.GetComponent<BoxCollider>(); box.isTrigger = true;
            exit.AddComponent<ExitZone>();

            var lightObj = new GameObject("Directional Light");
            var light = lightObj.AddComponent<Light>(); light.type = LightType.Directional; light.intensity = 1.1f; lightObj.transform.rotation = Quaternion.Euler(45,-30,0);
            RenderSettings.ambientLight = new Color(.15f,.15f,.22f);
        }

        private GameObject CreateCube(string name, Vector3 pos, Vector3 scale, Color color)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube); go.name = name; go.transform.position = pos; go.transform.localScale = scale; SetColor(go,color); return go;
        }

        private void SetColor(GameObject go, Color color)
        {
            var renderer = go.GetComponent<Renderer>();
            if (renderer != null) { var mat = new Material(Shader.Find("Standard")); mat.color = color; renderer.material = mat; }
        }

        private void OnXp(int amount) { flash = $"+{amount} XP"; flashUntil = Time.time + 2f; }
        private void OnDeath() { flash = "Você foi derrotado. Pressione R para reiniciar."; flashUntil = float.PositiveInfinity; }

        private void Update()
        {
            if (playerHealth != null && playerHealth.IsDead && Input.GetKeyDown(KeyCode.R)) game.RestartCheckpoint();
            if (game != null && game.ChapterFinished && Input.GetKeyDown(KeyCode.R)) game.RestartCheckpoint();
        }

        private void OnGUI()
        {
            GUI.Box(new Rect(15,15,480,105), "DISTRITO ZERO — v0.2.4");
            if (mission != null) GUI.Label(new Rect(30,45,450,25), "OBJETIVO: " + mission.ObjectiveText);
            if (playerHealth != null) GUI.Label(new Rect(30,70,220,25), $"VIDA: {playerHealth.CurrentHealth:0}/{playerHealth.MaxHealth:0}");
            if (xp != null) GUI.Label(new Rect(260,70,180,25), $"XP: {xp.CurrentXp}");
            if (interaction != null && !string.IsNullOrWhiteSpace(interaction.CurrentPrompt)) GUI.Label(new Rect(30,95,440,25), interaction.CurrentPrompt);

            GUI.Box(new Rect(Screen.width-310,15,295,135), "CONTROLES");
            GUI.Label(new Rect(Screen.width-295,45,270,95), "WASD: mover | Shift: correr\nEspaço: pular | E: interagir\nMouse: mirar | Clique esquerdo: atirar\nF: ataque corpo a corpo | R: reiniciar");

            DrawCrosshair();

            if (challenge != null && challenge.IsVisible)
            {
                var r = new Rect(Screen.width/2f-290, Screen.height/2f-140, 580, 280); GUI.Box(r, "DESAFIO DE LÓGICA");
                GUI.Label(new Rect(r.x+25,r.y+45,530,55), "Energia = 75%. Se energia > 50%, qual rota deve abrir?");
                if (GUI.Button(new Rect(r.x+25,r.y+110,530,45), "1 — Porta principal")) challenge.SubmitAnswer(0);
                if (GUI.Button(new Rect(r.x+25,r.y+165,530,45), "2 — Rota de emergência")) challenge.SubmitAnswer(1);
                GUI.Label(new Rect(r.x+25,r.y+220,530,35), challenge.Feedback);
            }

            if (Time.time < flashUntil) GUI.Label(new Rect(Screen.width/2f-180,145,360,40), flash);
            if (game != null && game.ChapterFinished)
            {
                GUI.Box(new Rect(Screen.width/2f-230, Screen.height/2f-90,460,180), "CAPÍTULO CONCLUÍDO");
                GUI.Label(new Rect(Screen.width/2f-190, Screen.height/2f-35,380,50), "Você escapou do Distrito Zero!\nPressione R para jogar novamente.");
            }
        }

        private void DrawCrosshair()
        {
            Color previous = GUI.color;
            if (shooter != null && Time.time < shooter.FeedbackUntil)
            {
                if (shooter.LastFeedbackState == ShotFeedbackState.Hit)
                    GUI.color = new Color(1f, 0.25f, 0.2f);
                else if (shooter.LastFeedbackState == ShotFeedbackState.Fired)
                    GUI.color = new Color(0.25f, 0.85f, 1f);
            }

            var style = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 26,
                fontStyle = FontStyle.Bold
            };
            GUI.Label(new Rect(Screen.width/2f-20, Screen.height/2f-20, 40, 40), "+", style);
            GUI.color = previous;
        }
    }

    public class ExitZone : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<PlayerHealth>() != null) MissionManager.Instance?.ExitReached();
        }
    }
}
