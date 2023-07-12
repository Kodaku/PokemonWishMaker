using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Pokemon.SceneManagement
{
    [CreateAssetMenu(fileName = "New Scene Graph", menuName = "PokemonSO/SceneGraph", order = 1)]
    public class SceneGraph : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] 
        List<SceneGraphNode> nodes = new List<SceneGraphNode>();
        [SerializeField] Vector2 newNodeOffset = new Vector2(250, 0);

        Dictionary<string, SceneGraphNode> nodeLookup = new Dictionary<string, SceneGraphNode>();

        // TODO: DA RIVEDERE
        public IEnumerable<SceneGraphNode> GetAllNodes()
        {
            List<SceneGraphNode> correctNodes = new List<SceneGraphNode>();
            for(int i = 0; i < nodes.Count; i++)
            {
                SceneGraphNode currentNode = nodes[i];
                for(int j = i + 1; j < nodes.Count; j++)
                {
                    if (currentNode.SceneName == nodes[j].SceneName)
                    {
                        if (!IsCurrentNodesInList(currentNode, correctNodes))
                        {
                            foreach(string previousSceneName in nodes[j].NextSceneEntrances.Keys)
                            {
                                currentNode.AddSceneEntrance(previousSceneName, nodes[j].NextSceneEntrances[previousSceneName]);
                            }
                            correctNodes[i] = currentNode;
                        }
                        else
                        {
                            correctNodes.Add(currentNode);
                        }
                    }
                }
            }
            return correctNodes;
            // return nodes;
        }

        public bool IsCurrentNodesInList(SceneGraphNode currentNode, List<SceneGraphNode> correctNodes)
        {
            bool found = false;
            foreach(SceneGraphNode sceneGraphNode in correctNodes)
            {
                if(sceneGraphNode.SceneName == currentNode.SceneName)
                {
                    found = true;
                }
            }
            return found;
        }

        public IEnumerable<SceneGraphNode> GetAllChildren(SceneGraphNode parentNode) {
            foreach(string childID in parentNode.Children) {
                if (nodeLookup.ContainsKey(childID)) {
                    yield return nodeLookup[childID];
                }
            }
        }

        public SceneGraphNode GetCurrentSceneGraphNode(string sceneName)
        {
            foreach(SceneGraphNode sceneGraphNode in GetAllNodes())
            {
                if (sceneGraphNode.SceneName == sceneName)
                {
                    return sceneGraphNode;
                }
            }
            return null;
        }

        private void OnValidate() {
            nodeLookup.Clear();
            foreach(SceneGraphNode sceneGraphNode in nodes) {
                nodeLookup.Add(sceneGraphNode.name, sceneGraphNode);
            }
        }
#if UNITY_EDITOR
        public void CreateNode(SceneGraphNode parent)
        {
            SceneGraphNode newNode = MakeNode(parent);
            Undo.RegisterCreatedObjectUndo(newNode, "Created new node: " + newNode.name);
            Undo.RecordObject(this, "Added Scene Graph Node");
            AddNode(newNode);
        }

        public void DeleteNode(SceneGraphNode nodeToDelete)
        {
            Undo.RecordObject(this, "Deleted Scene Graph Node");
            nodes.Remove(nodeToDelete);
            Debug.Log("DELETING NODE");
            OnValidate();
            CleanDanglingChildren(nodeToDelete);
            Undo.DestroyObjectImmediate(nodeToDelete);
        }

        private void CleanDanglingChildren(SceneGraphNode nodeToDelete)
        {
            foreach (SceneGraphNode node in GetAllNodes())
            {
                node.RemoveChild(nodeToDelete.name);
            }
        }

        private void AddNode(SceneGraphNode newNode)
        {
            nodes.Add(newNode);
            Debug.Log("ADDING NEW NODE");
            OnValidate();
        }

        private SceneGraphNode MakeNode(SceneGraphNode parent)
        {
            SceneGraphNode newNode = CreateInstance<SceneGraphNode>();
            newNode.name = System.Guid.NewGuid().ToString();
            if (parent != null)
            {
                parent.AddChild(newNode.name);
                Rect newRect = parent.Rect;
                newRect.position += newNodeOffset;
                newNode.SetRect(newRect);
            }

            return newNode;
        }
#endif
        public void OnAfterDeserialize()
        {

        }

        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            if (nodes.Count == 0) {
                SceneGraphNode newNode = MakeNode(null);
                AddNode(newNode);
            }
            if (AssetDatabase.GetAssetPath(this) != ""){
                foreach(SceneGraphNode node in GetAllNodes()) {
                    if (AssetDatabase.GetAssetPath(node) == "") {
                        AssetDatabase.AddObjectToAsset(node, this);
                    }
                }
            }
#endif
        }
    }
}
