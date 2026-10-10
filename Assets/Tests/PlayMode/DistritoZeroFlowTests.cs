using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using DistritoZero.Core;
using DistritoZero.Missions;

namespace DistritoZero.Tests.PlayMode
{
    public class DistritoZeroFlowTests
    {
        [UnityTest]
        public IEnumerator Bootstrap_Creates_Player_And_Mission()
        {
            yield return null;
            Assert.IsNotNull(Object.FindFirstObjectByType<DistritoZero.Player.PlayerHealth>());
            Assert.IsNotNull(Object.FindFirstObjectByType<MissionManager>());
        }
    }
}
