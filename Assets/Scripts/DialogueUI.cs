using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] GameObject dialogueBox;
    [SerializeField] TMP_Text textLabel;
    [SerializeField] DialogueObject testDialogue;
    private ResponseHandler responseHandler;
    private TypeWriterEffect typeWriterEffect;

    private void Start() {
        typeWriterEffect = GetComponent<TypeWriterEffect>();
        responseHandler = GetComponent<ResponseHandler>();
        CloseDialogueBox();
        ShowDialogue(testDialogue);
    }

    public void ShowDialogue(DialogueObject dialogueObject) {
        dialogueBox.SetActive(true);
        StartCoroutine(StepThroughDialogue(dialogueObject));
    }

    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject) {
        for(int i = 0; i < dialogueObject.Dialogue.Length; i++) {
            string dialogue = dialogueObject.Dialogue[i];
            yield return typeWriterEffect.Run(dialogue, textLabel);

            if (i == dialogueObject.Dialogue.Length - 1 && dialogueObject.HasResponses) {
                break;
            }

            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.A));
        }

        if(dialogueObject.HasResponses) {
            responseHandler.ShowResponses(dialogueObject.Responses);
        } else {
            CloseDialogueBox();
        }
    }

    private void CloseDialogueBox() {
        dialogueBox.SetActive(false);
        textLabel.text = string.Empty;
    }
}
