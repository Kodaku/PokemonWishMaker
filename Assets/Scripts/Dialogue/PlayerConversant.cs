using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Pokemon.Dialogue {
    public class PlayerConversant : MonoBehaviour
    {
        [SerializeField] Dialogue currentDialogue;
        DialogueNode currentNode = null;

        private void Awake() {
            currentNode = currentDialogue.GetRootNode();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public string GetText() {
            if (currentNode == null) {
                return "";
            }
            return  currentNode.Text;
        }

        public IEnumerable<string> GetChoices() {
            yield return "I've lived here for all my life";
            yield return "I came here from Nowhere";
        }

        public void Next() {
            DialogueNode[] children = currentDialogue.GetAllChildren(currentNode).ToArray();
            int randomIndex = Random.Range(0, children.Count());
            currentNode = children[randomIndex];
        }

        public bool HasNext() {
            return currentDialogue.GetAllChildren(currentNode).Count() > 0;
        }
    }
}
