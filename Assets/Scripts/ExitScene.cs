using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Pokemon.SceneManagement;

public class ExitScene : MonoBehaviour
{
    [SerializeField] SceneNames nextScene;
    [SerializeField] GameObject thisSceneEntrance;
    [SerializeField] GameObject nextSceneEntrance;
    // This is just to verify that the next scene actually exists
    private ScenesManager scenesManager;

    private void Start()
    {
        scenesManager = GameObject.FindGameObjectWithTag("ScenesManager").GetComponent<ScenesManager>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            string sceneName = ScenesMapping.FromSceneNameEnumToSceneName(scenesManager.GetThisSceneName());
            
            if (SceneInfoManager.ContainsSceneInfo(sceneName)) {
                string nextSceneName = ScenesMapping.FromSceneNameEnumToSceneName(nextScene);
                // TextAsset textAsset = Resources.Load<TextAsset>(sceneName);
                // SceneInfo sceneInfo = JsonUtility.FromJson<SceneInfo>(textAsset.text);
                GameInfo sceneInfo = SceneInfoManager.GetSceneInfo(sceneName);
                sceneInfo.playerPositionX = thisSceneEntrance.transform.position.x;
                sceneInfo.playerPositionY = thisSceneEntrance.transform.position.y;

                SceneInfoManager.UpdateSceneInfo(sceneName, sceneInfo);
                // AssetDatabase.ImportAsset("Assets/Resources/" + sceneName + ".txt");
                if (SceneInfoManager.ContainsSceneInfo(nextSceneName)) {
                    GameInfo nextSceneInfo = SceneInfoManager.GetSceneInfo(nextSceneName);
                    nextSceneInfo.playerPositionX = nextSceneEntrance.transform.position.x;
                    nextSceneInfo.playerPositionY = nextSceneEntrance.transform.position.y;

                    SceneInfoManager.UpdateSceneInfo(nextSceneName, nextSceneInfo);
                }
                else {
                    GameInfo nextSceneInfo = new GameInfo();
                    nextSceneInfo.playerPositionX = nextSceneEntrance.transform.position.x;
                    nextSceneInfo.playerPositionY = nextSceneEntrance.transform.position.y;

                    SceneInfoManager.AddSceneInfo(nextSceneName, nextSceneInfo);
                }
                SceneNames correctNextSceneName = scenesManager.GetCorrectNextSceneName(nextScene);
                if (correctNextSceneName != SceneNames.DEFAULT)
                {
                    SceneManager.LoadScene(nextSceneName);
                }
            }
        }
    }
}
