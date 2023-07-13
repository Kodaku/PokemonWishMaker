using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pokemon.SceneManagement
{
    [System.Serializable]
    public class NextSceneInfo
    {
        [SerializeField]
        private string toSceneName;
        [SerializeField]
        private GameObject toSceneEntrance;

        public string ToSceneName => toSceneName;

        public GameObject ToSceneEntrance => toSceneEntrance;

        public void SetToSceneName(string newToSceneName)
        {
            toSceneName = newToSceneName;
        }
        public void SetToSceneEntrance(GameObject newToSceneEntrance)
        {
            toSceneEntrance = newToSceneEntrance;
        }
    }
}
