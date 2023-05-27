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
        Debug.Log(SceneManager.GetActiveScene().name);
        TextAsset textAsset = Resources.Load<TextAsset>(SceneManager.GetActiveScene().name);
        if (textAsset != null) {
            SceneInfo sceneInfo = JsonUtility.FromJson<SceneInfo>(textAsset.text);
            player.transform.position = new Vector2(sceneInfo.playerPositionX, sceneInfo.playerPositionY);
        }
        else {
            SceneInfo sceneInfo = new SceneInfo();
            sceneInfo.playerPositionX = player.transform.position.x;
            sceneInfo.playerPositionY = player.transform.position.y;
            string filePath = Application.dataPath + "/Resources/" + SceneManager.GetActiveScene().name + ".json";
            string text = JsonUtility.ToJson(sceneInfo);
            File.WriteAllText(filePath, text);
            AssetDatabase.ImportAsset(filePath);
            AssetDatabase.Refresh();
        }

    }
}
