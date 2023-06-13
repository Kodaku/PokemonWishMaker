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
                foreach(DialogueNode dialogueNode in selectedDialogue.GetAllNodes()) {
                    EditorGUILayout.LabelField(dialogueNode.text);
                }
            }
        }
    }
}
