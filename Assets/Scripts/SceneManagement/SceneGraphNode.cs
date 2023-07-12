using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Pokemon.SceneManagement
{
    public class SceneGraphNode : ScriptableObject
    {
        [SerializeField]
        private string sceneName;
        [SerializeField]
        private List<string> children = new List<string>();
        [SerializeField]
        private Rect rect = new Rect(0, 0, 200, 100);
        [SerializeField]
        private GameObject thisSceneEntrance;
        [SerializeField]
        private Dictionary<string, NextSceneInfo> nextScenesEntrances = new Dictionary<string, NextSceneInfo>();
        private string previousSceneName;

        public string SceneName => sceneName;

        public List<string> Children => children;

        public Rect Rect => rect;

        public string PreviousSceneName => previousSceneName;

        public Dictionary<string, NextSceneInfo> NextSceneEntrances => nextScenesEntrances;

        public void AddSceneEntrance(string sceneName, NextSceneInfo nextSceneInfo)
        {
            if (!nextScenesEntrances.ContainsKey(sceneName))
            {
                nextScenesEntrances.Add(sceneName, nextSceneInfo);
            }
        }

# if UNITY_EDITOR
        public void SetRect(Rect newRect) {
            Undo.RecordObject(this, "Move SceneGraphNode node");
            rect = newRect;
            EditorUtility.SetDirty(this);
        }

        public void SetSceneName(string newSceneName) {
            if (newSceneName != sceneName) {
                Undo.RecordObject(this, "Update SceneGraphNode Scene Name");
                sceneName = newSceneName;
                EditorUtility.SetDirty(this);
            }
        }

        public void AddChild(string childID) {
            Undo.RecordObject(this, "Add SceneGraphNode link");
            children.Add(childID);
            EditorUtility.SetDirty(this);
        }

        public void RemoveChild(string childID) {
            Undo.RecordObject(this, "Remove SceneGraphNode link");
            children.Remove(childID);
            EditorUtility.SetDirty(this);
        }
    }
#endif
}
