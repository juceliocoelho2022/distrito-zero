using UnityEngine;
using UnityEngine.SceneManagement;

namespace DistritoZero.Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public bool ChapterFinished { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void OnEnable() => GameEvents.ChapterCompleted += CompleteChapter;
        private void OnDisable() => GameEvents.ChapterCompleted -= CompleteChapter;

        public void RestartCheckpoint()
        {
            Scene scene = SceneManager.GetActiveScene();
            if (!scene.IsValid() || scene.buildIndex < 0)
            {
                Debug.LogError($"Cannot restart scene '{scene.name}': it is not registered in Build Settings.");
                return;
            }

            SceneManager.LoadScene(scene.buildIndex, LoadSceneMode.Single);
        }

        public void CompleteChapter() => ChapterFinished = true;
    }
}
