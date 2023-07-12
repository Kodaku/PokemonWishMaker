using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneInfoManager
{
    public static Dictionary<string, GameInfo> sceneInfoDict = new Dictionary<string, GameInfo>();

    public static void AddSceneInfo(string sceneName, GameInfo sceneInfo) {
        sceneInfoDict.Add(sceneName, sceneInfo);
    }

    public static GameInfo GetSceneInfo(string sceneName) {
        return sceneInfoDict[sceneName];
    }

    public static void UpdateSceneInfo(string sceneName, GameInfo newSceneInfo) {
        sceneInfoDict[sceneName] = newSceneInfo;
    }

    public static bool ContainsSceneInfo(string sceneName) {
        return sceneInfoDict.ContainsKey(sceneName);
    }
}
