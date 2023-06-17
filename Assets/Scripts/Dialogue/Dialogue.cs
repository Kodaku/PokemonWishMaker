using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pokemon.Dialogue
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "PokemonSO/Dialogue", order = 0)]
    public class Dialogue : ScriptableObject
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
                nodeLookup.Add(dialogueNode.uniqueID, dialogueNode);
            }
        }

        public void CreateNode(DialogueNode parent) {
            DialogueNode newNode = new DialogueNode();
            newNode.uniqueID = System.Guid.NewGuid().ToString();
            parent.children.Add(newNode.uniqueID);
            nodes.Add(newNode);
            OnValidate();
        }

        public void DeleteNode(DialogueNode nodeToDelete)
        {
            nodes.Remove(nodeToDelete);
            OnValidate();
            CleanDanglingChildren(nodeToDelete);
        }

        private void CleanDanglingChildren(DialogueNode nodeToDelete)
        {
            foreach (DialogueNode node in GetAllNodes())
            {
                node.children.Remove(nodeToDelete.uniqueID);
            }
        }

#if UNITY_EDITOR
        private void Awake() {
            OnValidate();
            if (nodes.Count == 0) {
                DialogueNode rootNode = new DialogueNode();
                rootNode.uniqueID = System.Guid.NewGuid().ToString();
                nodes.Add(rootNode);
            }
        }
#endif
    }
}
