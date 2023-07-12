using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pokemon.SceneManagement
{
    [System.Serializable]
    public class NextSceneInfo
    {
        [SerializeField]
        private string nextSceneName;
        [SerializeField]
        private GameObject nextSceneEntrance;

        public string NextSceneName => nextSceneName;

        public GameObject NextSceneEntrance => nextSceneEntrance;

        public void SetNextSceneName(string newNextSceneName)
        {
            nextSceneName = newNextSceneName;
        }
        public void SetNextSceneEntrance(GameObject newNextSceneEntrance)
        {
            nextSceneEntrance = newNextSceneEntrance;
        }
    }
}
