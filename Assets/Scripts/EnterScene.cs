using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEditor;

public class EnterScene : MonoBehaviour
{
    [SerializeField] GameObject player;

    private void Awake() {
        string sceneName = SceneManager.GetActiveScene().name;
        if (!SceneInfoManager.ContainsSceneInfo(sceneName)) {
            GameInfo sceneInfo = new GameInfo();
            sceneInfo.playerPositionX = player.transform.position.x;
            sceneInfo.playerPositionY = player.transform.position.y;
            
            SceneInfoManager.AddSceneInfo(SceneManager.GetActiveScene().name, sceneInfo);
            // AssetDatabase.ImportAsset("Assets/Resources/" + SceneManager.GetActiveScene().name + ".txt");
        }
        else {
            GameInfo sceneInfo = SceneInfoManager.GetSceneInfo(sceneName);
            player.transform.position = new Vector2(sceneInfo.playerPositionX, sceneInfo.playerPositionY);
        }
    }
}
