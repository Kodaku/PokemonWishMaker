using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pokemon.SceneManagement;
using UnityEngine.SceneManagement;

namespace Pokemon.SceneManagement
{
    public class ScenesManager : MonoBehaviour
    {
        [SerializeField]
        private SceneGraph sceneGraph;
        private SceneGraphNode currentGraphNode;
        // Start is called before the first frame update
        void Start()
        {
            currentGraphNode = sceneGraph.GetCurrentSceneGraphNode(GetThisSceneName());
        }

        public SceneNames GetThisSceneName()
        {
            return ScenesMapping.FromSceneNameStringToSceneNameEnum(SceneManager.GetActiveScene().name);
        }

        public SceneNames GetCorrectNextSceneName(SceneNames nextSceneName)
        {
            SceneGraphNode nextScene = sceneGraph.GetCurrentSceneGraphNode(nextSceneName);
            if (nextScene != null)
            {
                return nextScene.SceneName;
            }
            return SceneNames.DEFAULT;
        }
    }
}
