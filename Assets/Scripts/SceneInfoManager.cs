using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneInfoManager
{
    public static Dictionary<string, SceneInfo> sceneInfoDict = new Dictionary<string, SceneInfo>();

    public static void AddSceneInfo(string sceneName, SceneInfo sceneInfo) {
        sceneInfoDict.Add(sceneName, sceneInfo);
    }

    public static SceneInfo GetSceneInfo(string sceneName) {
        return sceneInfoDict[sceneName];
    }

    public static void UpdateSceneInfo(string sceneName, SceneInfo newSceneInfo) {
        sceneInfoDict[sceneName] = newSceneInfo;
    }

    public static bool ContainsSceneInfo(string sceneName) {
        return sceneInfoDict.ContainsKey(sceneName);
    }
}
