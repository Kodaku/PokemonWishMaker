using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using System;

namespace Pokemon.Dialogue.Editor
{
    public class DialogueEditor : EditorWindow
    {
        Dialogue selectedDialogue = null;
        [NonSerialized]
        GUIStyle nodeStyle = null;
        [NonSerialized]
        GUIStyle playerNodeStyle = null;
        [NonSerialized]
        DialogueNode draggingNode = null;
        [NonSerialized]
        Vector2 draggingOffset;
        [NonSerialized]
        DialogueNode creatingNode = null;
        [NonSerialized]
        DialogueNode deletingNode = null;
        [NonSerialized]
        DialogueNode linkingParentNode = null;
        Vector2 scrollPosition;
        [NonSerialized]
        bool draggingCanvas = false;
        [NonSerialized]
        Vector2 draggingCanvasOffset;
        const float canvasSize = 4000;
        const float backgroundSize = 50;

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

            playerNodeStyle = new GUIStyle();
            playerNodeStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D;
            playerNodeStyle.normal.textColor = Color.white;
            playerNodeStyle.padding = new RectOffset(20, 20, 20, 20);
            playerNodeStyle.border = new RectOffset(12, 12, 12, 12);
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

                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
                // Debug.Log(scrollPosition);
                Rect canvas = GUILayoutUtility.GetRect(canvasSize, canvasSize);

                Texture2D backgroundTex = Resources.Load("background") as Texture2D;
                Rect texCoords = new Rect(0, 0, canvasSize / backgroundSize, canvasSize / backgroundSize);
                GUI.DrawTextureWithTexCoords(canvas, backgroundTex, texCoords);

                foreach(DialogueNode dialogueNode in selectedDialogue.GetAllNodes()) {
                    DrawConnections(dialogueNode);
                }
                foreach(DialogueNode dialogueNode in selectedDialogue.GetAllNodes())
                {
                    DrawNode(dialogueNode);
                }

                EditorGUILayout.EndScrollView();

                if(creatingNode != null) {
                    selectedDialogue.CreateNode(creatingNode);
                    creatingNode = null;
                }
                if(deletingNode != null) {
                    selectedDialogue.DeleteNode(deletingNode);
                    deletingNode = null;
                }
            }
        }

        private void DrawConnections(DialogueNode dialogueNode) {
            Vector3 startPosition  = new Vector2(dialogueNode.Rect.xMax, dialogueNode.Rect.center.y);
            foreach(DialogueNode childNode in selectedDialogue.GetAllChildren(dialogueNode)) {
                Vector3 endPosition = new Vector2(childNode.Rect.xMin, childNode.Rect.center.y);
                Vector3 controlPointOffset = endPosition - startPosition;
                controlPointOffset.y = 0;
                controlPointOffset.x *= 0.8f;
                Handles.DrawBezier(startPosition,
                                endPosition,
                                startPosition + controlPointOffset,
                                endPosition - controlPointOffset,
                                Color.white, null, 4.0f);
            }
        }

        private void ProcessEvent() {
            if (Event.current.type == EventType.MouseDown && draggingNode == null) {
                draggingNode = GetNodeAtPoint(Event.current.mousePosition + scrollPosition);
                if (draggingNode != null) {
                    draggingOffset =  draggingNode.Rect.position - Event.current.mousePosition;
                    Selection.activeObject = draggingNode;
                }
                else {
                    draggingCanvas = true;
                    draggingCanvasOffset = Event.current.mousePosition + scrollPosition;
                    Selection.activeObject = selectedDialogue;
                }
            }
            else if(Event.current.type == EventType.MouseDrag && draggingNode != null) {
                Rect currentRect = draggingNode.Rect;
                currentRect.position = Event.current.mousePosition + draggingOffset;

                draggingNode.SetRect(currentRect);
                
                GUI.changed = true;
            }
            else if(Event.current.type == EventType.MouseDrag && draggingCanvas) {
                // Update scroll position
                scrollPosition = draggingCanvasOffset - Event.current.mousePosition;
                GUI.changed = true;
            }
            else if(Event.current.type == EventType.MouseUp && draggingNode != null) {
                draggingNode = null;
            }
            else if(Event.current.type == EventType.MouseUp && draggingCanvas) {
                draggingCanvas = false;
            }
        }

        private DialogueNode GetNodeAtPoint(Vector2 point) {
            DialogueNode foundNode = null;
            foreach(DialogueNode dialogueNode in selectedDialogue.GetAllNodes()) {
                if (dialogueNode.Rect.Contains(point)) {
                    foundNode = dialogueNode;
                }
            }
            return foundNode;
        }

        private void DrawNode(DialogueNode dialogueNode)
        {
            GUIStyle style = nodeStyle;
            if (dialogueNode.IsPlayerSpeaking) {
                style = playerNodeStyle;
            }
            GUILayout.BeginArea(dialogueNode.Rect, style);
            
            string newText = EditorGUILayout.TextField(dialogueNode.Text);
            dialogueNode.SetText(newText);

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("x"))
            {
                deletingNode = dialogueNode;
            }

            DrawLinkButtons(dialogueNode);

            if (GUILayout.Button("+"))
            {
                creatingNode = dialogueNode;
            }

            GUILayout.EndHorizontal();

            GUILayout.EndArea();
        }

        private void DrawLinkButtons(DialogueNode dialogueNode)
        {
            if (linkingParentNode == null)
            {
                if (GUILayout.Button("link"))
                {
                    linkingParentNode = dialogueNode;
                }
            }
            else if(linkingParentNode == dialogueNode)
            {
                if(GUILayout.Button("cancel")) {
                    linkingParentNode = null;
                }
            }
            else if(linkingParentNode.Children.Contains(dialogueNode.name)) {
                if (GUILayout.Button("unlink")) {
                    
                    linkingParentNode.RemoveChild(dialogueNode.name);
                    linkingParentNode = null;
                }
            }
            else {
                if (GUILayout.Button("child"))
                {
                    linkingParentNode.AddChild(dialogueNode.name);
                    linkingParentNode = null;
                }
            }
        }
    }
}
