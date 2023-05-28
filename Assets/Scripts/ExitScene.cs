using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitScene : MonoBehaviour
{
    [SerializeField] string nextSceneName;
    [SerializeField] GameObject thisSceneEntrance;
    [SerializeField] GameObject nextSceneEntrance;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            string sceneName = SceneManager.GetActiveScene().name;
            
            if (SceneInfoManager.ContainsSceneInfo(sceneName)) {
                // TextAsset textAsset = Resources.Load<TextAsset>(sceneName);
                // SceneInfo sceneInfo = JsonUtility.FromJson<SceneInfo>(textAsset.text);
                SceneInfo sceneInfo = SceneInfoManager.GetSceneInfo(sceneName);
                sceneInfo.playerPositionX = thisSceneEntrance.transform.position.x;
                sceneInfo.playerPositionY = thisSceneEntrance.transform.position.y;

                SceneInfoManager.UpdateSceneInfo(sceneName, sceneInfo);
                // AssetDatabase.ImportAsset("Assets/Resources/" + sceneName + ".txt");
                if (SceneInfoManager.ContainsSceneInfo(nextSceneName)) {
                    SceneInfo nextSceneInfo = SceneInfoManager.GetSceneInfo(nextSceneName);
                    nextSceneInfo.playerPositionX = nextSceneEntrance.transform.position.x;
                    nextSceneInfo.playerPositionY = nextSceneEntrance.transform.position.y;

                    SceneInfoManager.UpdateSceneInfo(nextSceneName, nextSceneInfo);
                }
                else {
                    SceneInfo nextSceneInfo = new SceneInfo();
                    nextSceneInfo.playerPositionX = nextSceneEntrance.transform.position.x;
                    nextSceneInfo.playerPositionY = nextSceneEntrance.transform.position.y;

                    SceneInfoManager.AddSceneInfo(nextSceneName, nextSceneInfo);
                }
                SceneManager.LoadScene(nextSceneName);
            }
        }
    }
}
