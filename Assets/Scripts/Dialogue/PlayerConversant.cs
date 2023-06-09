using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace Pokemon.Dialogue {
    public class PlayerConversant : MonoBehaviour
    {
        [SerializeField] string playerName;
        Dialogue currentDialogue;
        DialogueNode currentNode = null;
        AIConversant currentConversant = null;
        bool isChoosing = false;
        public event Action onConversationUpdated;

        // IEnumerator Start() {
        //     yield return new WaitForSeconds(2.0f);
        //     StartDialogue(testDialogue);
        // }

        public void StartDialogue(AIConversant newConversant, Dialogue newDialogue) {
            currentConversant = newConversant;
            currentDialogue = newDialogue;
            currentNode = currentDialogue.GetRootNode();
            TriggerEnterAction();
            onConversationUpdated();
        }

        public string GetCurrentConversantName() {
            if(isChoosing) {
                return playerName;
            }
            else {
                return currentConversant.NPCName;
            }
        }

        public void Quit() {
            currentDialogue = null;
            TriggerExitAction();
            currentNode = null;
            isChoosing = false;
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
            TriggerEnterAction();
            isChoosing = false;
            Next();
        }

        public void Next() {
            int numPlayerResponses = currentDialogue.GetPlayerChildren(currentNode).Count();
            if(numPlayerResponses > 0) {
                isChoosing = true;
                TriggerExitAction();
                onConversationUpdated();
                return;
            }

            DialogueNode[] children = currentDialogue.GetAIChildren(currentNode).ToArray();
            int randomIndex = UnityEngine.Random.Range(0, children.Count());
            TriggerExitAction();
            currentNode = children[randomIndex];
            TriggerEnterAction();
            onConversationUpdated();
        }

        public bool HasNext() {
            return currentDialogue.GetAllChildren(currentNode).Count() > 0;
        }

        private void TriggerEnterAction() {
            if (currentNode != null) {
                TriggerAction(currentNode.OnEnterAction);
            }
        }

        private void TriggerExitAction() {
            if (currentNode != null) {
                TriggerAction(currentNode.OnExitAction);
            }
        }

        private void TriggerAction(string action) {
            if (action == "") {
                return;
            }

            foreach(DialogueTrigger trigger in currentConversant.GetComponents<DialogueTrigger>()) {
                trigger.Trigger(action);
            }
        }
    }
}
