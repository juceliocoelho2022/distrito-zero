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
            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.buildIndex);
        }

        public void CompleteChapter() => ChapterFinished = true;
    }
}
