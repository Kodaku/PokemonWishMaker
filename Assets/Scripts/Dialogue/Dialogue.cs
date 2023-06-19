using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Pokemon.Dialogue
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "PokemonSO/Dialogue", order = 0)]
    public class Dialogue : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] List<DialogueNode> nodes = new List<DialogueNode>();

        Dictionary<string, DialogueNode> nodeLookup = new Dictionary<string, DialogueNode>();

        public IEnumerable<DialogueNode> GetAllNodes() {
            return nodes;
        }

        public IEnumerable<DialogueNode> GetAllChildren(DialogueNode parentNode) {
            foreach(string childID in parentNode.children) {
                if (nodeLookup.ContainsKey(childID)) {
                    yield return nodeLookup[childID];
                }
            }
        }

        private void OnValidate() {
            nodeLookup.Clear();
            foreach(DialogueNode dialogueNode in nodes) {
                nodeLookup.Add(dialogueNode.name, dialogueNode);
            }
        }

        public void CreateNode(DialogueNode parent) {
            DialogueNode newNode = CreateInstance<DialogueNode>();
            newNode.name = System.Guid.NewGuid().ToString();
            UnityEditor.Undo.RegisterCreatedObjectUndo(newNode, "Created new node: ");
            if (parent != null) {
                parent.children.Add(newNode.name);
            }
            nodes.Add(newNode);
            OnValidate();
        }

        public void DeleteNode(DialogueNode nodeToDelete)
        {
            nodes.Remove(nodeToDelete);
            OnValidate();
            CleanDanglingChildren(nodeToDelete);
            UnityEditor.Undo.DestroyObjectImmediate(nodeToDelete);
        }

        private void CleanDanglingChildren(DialogueNode nodeToDelete)
        {
            foreach (DialogueNode node in GetAllNodes())
            {
                node.children.Remove(nodeToDelete.name);
            }
        }

        public void OnBeforeSerialize()
        {
            if (nodes.Count == 0) {
                CreateNode(null);
            }
            if (AssetDatabase.GetAssetPath(this) != ""){
                foreach(DialogueNode node in GetAllNodes()) {
                    if (AssetDatabase.GetAssetPath(node) == "") {
                        AssetDatabase.AddObjectToAsset(node, this);
                    }
                }
            }
        }

        public void OnAfterDeserialize()
        {
            
        }

#if UNITY_EDITOR
        private void Awake() {
            OnValidate();
        }
#endif
    }
}
