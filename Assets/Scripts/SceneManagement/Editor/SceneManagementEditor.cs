using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System;

namespace Pokemon.SceneManagement.Editor
{
    public class SceneManagementEditor : EditorWindow
    {
        SceneGraph selectedSceneGraph = null;
        [NonSerialized]
        GUIStyle nodeStyle = null;
        [NonSerialized]
        GUIStyle playerNodeStyle = null;
        [NonSerialized]
        SceneGraphNode draggingNode = null;
        [NonSerialized]
        Vector2 draggingOffset;
        [NonSerialized]
        SceneGraphNode creatingNode = null;
        [NonSerialized]
        SceneGraphNode deletingNode = null;
        [NonSerialized]
        SceneGraphNode linkingParentNode = null;
        Vector2 scrollPosition;
        [NonSerialized]
        bool draggingCanvas = false;
        [NonSerialized]
        Vector2 draggingCanvasOffset;
        const float canvasSize = 4000;
        const float backgroundSize = 50;

        [MenuItem("Window/SceneManagementEditor")]
        public static void ShowEditorWindow() {
            GetWindow(typeof(SceneManagementEditor), false, "Scene Management Editor");
        }

        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line) {
            SceneGraph sceneGraph = EditorUtility.InstanceIDToObject(instanceID) as SceneGraph;
            if (sceneGraph != null) {
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
            SceneGraph newSceneGraph = Selection.activeObject as SceneGraph;
            if (newSceneGraph != null) {
                selectedSceneGraph = newSceneGraph;
                Repaint();
            }
        }

        private void OnGUI() {
            if (selectedSceneGraph == null) {
                EditorGUILayout.LabelField("No scene graph selected");
            }
            else {
                ProcessEvent();

                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
                // Debug.Log(scrollPosition);
                Rect canvas = GUILayoutUtility.GetRect(canvasSize, canvasSize);

                Texture2D backgroundTex = Resources.Load("background") as Texture2D;
                Rect texCoords = new Rect(0, 0, canvasSize / backgroundSize, canvasSize / backgroundSize);
                GUI.DrawTextureWithTexCoords(canvas, backgroundTex, texCoords);

                foreach(SceneGraphNode sceneGraphNode in selectedSceneGraph.GetAllNodes()) {
                    DrawConnections(sceneGraphNode);
                }
                foreach(SceneGraphNode sceneGraphNode in selectedSceneGraph.GetAllNodes())
                {
                    DrawNode(sceneGraphNode);
                }

                EditorGUILayout.EndScrollView();

                if(creatingNode != null) {
                    selectedSceneGraph.CreateNode(creatingNode);
                    creatingNode = null;
                }
                if(deletingNode != null) {
                    selectedSceneGraph.DeleteNode(deletingNode);
                    deletingNode = null;
                }
            }
        }

        private void DrawConnections(SceneGraphNode sceneGraphNode) {
            Vector3 startPosition  = new Vector2(sceneGraphNode.Rect.xMax, sceneGraphNode.Rect.center.y);
            foreach(SceneGraphNode childNode in selectedSceneGraph.GetAllChildren(sceneGraphNode)) {
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
                    Selection.activeObject = selectedSceneGraph;
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

        private SceneGraphNode GetNodeAtPoint(Vector2 point) {
            SceneGraphNode foundNode = null;
            foreach(SceneGraphNode sceneGraphNode in selectedSceneGraph.GetAllNodes()) {
                if (sceneGraphNode.Rect.Contains(point)) {
                    foundNode = sceneGraphNode;
                }
            }
            return foundNode;
        }

        private void DrawNode(SceneGraphNode sceneGraphNode)
        {
            GUIStyle style = nodeStyle;
            
            GUILayout.BeginArea(sceneGraphNode.Rect, style);
            
            string newSceneName = EditorGUILayout.TextField(sceneGraphNode.SceneName);
            sceneGraphNode.SetSceneName(newSceneName);

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("x"))
            {
                deletingNode = sceneGraphNode;
            }

            DrawLinkButtons(sceneGraphNode);

            if (GUILayout.Button("+"))
            {
                creatingNode = sceneGraphNode;
            }

            GUILayout.EndHorizontal();

            GUILayout.EndArea();
        }

        private void DrawLinkButtons(SceneGraphNode sceneGraphNode)
        {
            if (linkingParentNode == null)
            {
                if (GUILayout.Button("link"))
                {
                    linkingParentNode = sceneGraphNode;
                }
            }
            else if(linkingParentNode == sceneGraphNode)
            {
                if(GUILayout.Button("cancel")) {
                    linkingParentNode = null;
                }
            }
            else if(linkingParentNode.Children.Contains(sceneGraphNode.name)) {
                if (GUILayout.Button("unlink")) {
                    
                    linkingParentNode.RemoveChild(sceneGraphNode.name);
                    linkingParentNode = null;
                }
            }
            else {
                if (GUILayout.Button("child"))
                {
                    linkingParentNode.AddChild(sceneGraphNode.name);
                    linkingParentNode = null;
                }
            }
        }
    }
}
