using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Pokemon.Dialogue
{
    public class DialogueNode : ScriptableObject
    {
        [SerializeField]
        bool isPlayerSpeaking = false;
        [SerializeField]
        private string text;
        [SerializeField]
        private List<string> children = new List<string>();
        [SerializeField]
        private Rect rect = new Rect(0, 0, 200, 100);
        [SerializeField] string onEnterAction;
        [SerializeField] string onExitAction;

        public string Text => text;

        public List<string> Children => children;

        public Rect Rect => rect;

        public bool IsPlayerSpeaking => isPlayerSpeaking;

        public string OnEnterAction => onEnterAction;

        public string OnExitAction => onExitAction;

# if UNITY_EDITOR
        public void SetRect(Rect newRect) {
            Undo.RecordObject(this, "Move dialogue node");
            rect = newRect;
            EditorUtility.SetDirty(this);
        }

        public void SetText(string newText) {
            if (newText != text) {
                Undo.RecordObject(this, "Update dialogue text");
                text = newText;
                EditorUtility.SetDirty(this);
            }
        }

        public void AddChild(string childID) {
            Undo.RecordObject(this, "Add dialogue link");
            children.Add(childID);
            EditorUtility.SetDirty(this);
        }

        public void RemoveChild(string childID) {
            Undo.RecordObject(this, "Remove dialogue link");
            children.Remove(childID);
            EditorUtility.SetDirty(this);
        }

        public void SetPlayerSpeaking(bool newIsPlayerSpeaking) {
            Undo.RecordObject(this, "Change dialogue speaker");
            isPlayerSpeaking = newIsPlayerSpeaking;
            EditorUtility.SetDirty(this);
        }
    }
#endif
}
