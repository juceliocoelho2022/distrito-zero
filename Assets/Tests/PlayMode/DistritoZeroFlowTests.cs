using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using DistritoZero.Core;
using DistritoZero.Missions;

namespace DistritoZero.Tests.PlayMode
{
    public class DistritoZeroFlowTests
    {
        [UnityTest]
        public IEnumerator Bootstrap_Creates_Player_Mission_And_Camera()
        {
            yield return null;
            Assert.IsNotNull(Object.FindFirstObjectByType<DistritoZero.Player.PlayerHealth>());
            Assert.IsNotNull(Object.FindFirstObjectByType<MissionManager>());
            Assert.IsNotNull(Camera.main);
        }

        [UnityTest]
        public IEnumerator ActiveScene_IsRegisteredForRestart()
        {
            yield return null;
            Scene scene = SceneManager.GetActiveScene();
            Assert.GreaterOrEqual(scene.buildIndex, 0, "DistritoZero scene must be registered in Build Settings for restart.");
        }
    }
}
