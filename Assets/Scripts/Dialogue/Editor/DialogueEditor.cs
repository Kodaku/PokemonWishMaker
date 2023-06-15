using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Pokemon.Dialogue.Editor
{
    public class DialogueEditor : EditorWindow
    {
        Dialogue selectedDialogue = null;
        GUIStyle nodeStyle = null;
        DialogueNode draggingNode = null;
        Vector2 draggingOffset;

        [MenuItem("Window/DialogueEditor")]
        public static void ShowEditorWindow() {
            GetWindow(typeof(DialogueEditor), false, "Dialogue Editor");
        }

        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line) {
            Dialogue dialogue = EditorUtility.InstanceIDToObject(instanceID) as Dialogue;
            if (dialogue != null) {
                ShowEditorWindow();
                return true;
            }
            return false;
        }

        private void OnEnable() {
            Selection.selectionChanged += OnSelectionChanged;
            nodeStyle = new GUIStyle();
            nodeStyle.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
            nodeStyle.normal.textColor = Color.white;
            nodeStyle.padding = new RectOffset(20, 20, 20, 20);
            nodeStyle.border = new RectOffset(12, 12, 12, 12);
        }

        private void OnSelectionChanged() {
            Dialogue newDialogue = Selection.activeObject as Dialogue;
            if (newDialogue != null) {
                selectedDialogue = newDialogue;
                Repaint();
            }
        }

        private void OnGUI() {
            if (selectedDialogue == null) {
                EditorGUILayout.LabelField("No dialogue selected");
            }
            else {
                ProcessEvent();
                foreach(DialogueNode dialogueNode in selectedDialogue.GetAllNodes())
                {
                    OnGUINode(dialogueNode);
                }
            }
        }

        private void ProcessEvent() {
            if (Event.current.type == EventType.MouseDown && draggingNode == null) {
                draggingNode = GetNodeAtPoint(Event.current.mousePosition);
                if (draggingNode != null) {
                    draggingOffset =  draggingNode.rect.position - Event.current.mousePosition;
                }
            }
            else if(Event.current.type == EventType.MouseUp && draggingNode != null) {
                draggingNode = null;
            }
            else if(Event.current.type == EventType.MouseDrag && draggingNode != null) {
                Undo.RecordObject(selectedDialogue, "Moved dialogue");
                draggingNode.rect.position = Event.current.mousePosition + draggingOffset;
                GUI.changed = true;
            }
        }

        private DialogueNode GetNodeAtPoint(Vector2 point) {
            DialogueNode foundNode = null;
            foreach(DialogueNode dialogueNode in selectedDialogue.GetAllNodes()) {
                if (dialogueNode.rect.Contains(point)) {
                    foundNode = dialogueNode;
                }
            }
            return foundNode;
        }

        private void OnGUINode(DialogueNode dialogueNode)
        {
            GUILayout.BeginArea(dialogueNode.rect, nodeStyle);
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.LabelField("Node: ", EditorStyles.whiteLabel);
            string newText = EditorGUILayout.TextField(dialogueNode.text);
            string newUniqueID = EditorGUILayout.TextField(dialogueNode.uniqueID);
            // If any fiield has changed
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(selectedDialogue, "Update dialogue text");
                dialogueNode.text = newText;
                dialogueNode.uniqueID = newUniqueID;
            }

            foreach(DialogueNode childNode in selectedDialogue.GetAllChildren(dialogueNode)) {
                EditorGUILayout.LabelField(childNode.text);
            }

            GUILayout.EndArea();
        }
    }
}
