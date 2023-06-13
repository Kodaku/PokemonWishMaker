using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pokemon.Dialogue
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "PokemonSO/Dialogue", order = 0)]
    public class Dialogue : ScriptableObject
    {
        [SerializeField] List<DialogueNode> nodes = new List<DialogueNode>();

        public IEnumerable<DialogueNode> GetAllNodes() {
            return nodes;
        }

#if UNITY_EDITOR
        private void Awake() {
            if (nodes.Count == 0) {
                nodes.Add(new DialogueNode());
            }
        }
#endif
    }
}
