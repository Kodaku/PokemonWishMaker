using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEditor;

public class ExitScene : MonoBehaviour
{
    [SerializeField] string nextSceneName;
    [SerializeField] GameObject nextEntrance;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            TextAsset textAsset = Resources.Load<TextAsset>(SceneManager.GetActiveScene().name);
            if (textAsset != null) {
                SceneInfo sceneInfo = JsonUtility.FromJson<SceneInfo>(textAsset.text);
                sceneInfo.playerPositionX = nextEntrance.transform.position.x;
                sceneInfo.playerPositionY = nextEntrance.transform.position.y;

                string filePath = Application.dataPath + "/Resources/" + SceneManager.GetActiveScene().name + ".json";
                string text = JsonUtility.ToJson(sceneInfo);
                File.WriteAllText(filePath, text);
                AssetDatabase.Refresh();
                SceneManager.LoadScene(nextSceneName);
            }
        }
    }
}
