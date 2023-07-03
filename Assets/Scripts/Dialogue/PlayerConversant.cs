using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace Pokemon.Dialogue {
    public class PlayerConversant : MonoBehaviour
    {
        [SerializeField] Dialogue testDialogue;
        Dialogue currentDialogue;
        DialogueNode currentNode = null;
        bool isChoosing = false;
        public event Action onConversationUpdated;

        IEnumerator Start() {
            yield return new WaitForSeconds(2.0f);
            StartDialogue(testDialogue);
        }

        public void StartDialogue(Dialogue newDialogue) {
            currentDialogue = newDialogue;
            currentNode = currentDialogue.GetRootNode();
            onConversationUpdated();
        }

        public bool IsActive() {
            return currentDialogue != null;
        }

        public bool IsChoosing() {
            return isChoosing;
        }

        public string GetText() {
            if (currentNode == null) {
                return "";
            }
            return  currentNode.Text;
        }

        public IEnumerable<DialogueNode> GetChoices() {
            return currentDialogue.GetPlayerChildren(currentNode);
        }

        public void SelectChoice(DialogueNode chosenNode) {
            currentNode = chosenNode;
            isChoosing = false;
            Next();
        }

        public void Next() {
            int numPlayerResponses = currentDialogue.GetPlayerChildren(currentNode).Count();
            if(numPlayerResponses > 0) {
                isChoosing = true;
                onConversationUpdated();
                return;
            }

            DialogueNode[] children = currentDialogue.GetAIChildren(currentNode).ToArray();
            int randomIndex = UnityEngine.Random.Range(0, children.Count());
            currentNode = children[randomIndex];
            onConversationUpdated();
        }

        public bool HasNext() {
            return currentDialogue.GetAllChildren(currentNode).Count() > 0;
        }
    }
}
