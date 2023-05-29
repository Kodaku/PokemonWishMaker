using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class ResponseHandler : MonoBehaviour
{
    [SerializeField] RectTransform responseBox;
    [SerializeField] RectTransform responseButtonTemplate;
    [SerializeField] RectTransform responseContainer;
    private DialogueUI dialogueUI;
    private List<GameObject> temporaryResponseButtons = new List<GameObject>();

    private void Start() {
        dialogueUI = GetComponent<DialogueUI>();
        responseBox.gameObject.SetActive(false);
        responseButtonTemplate.gameObject.SetActive(false);
    }

    public void ShowResponses(Response[] responses) {
        float responseBoxHeight = 0.0f;

        foreach(Response response in responses) {
            GameObject responseButton = Instantiate(responseButtonTemplate.gameObject, responseContainer);
            responseButton.gameObject.SetActive(true);
            responseButton.GetComponent<TMP_Text>().text = response.ResponseText;
            responseButton.GetComponent<Button>().onClick.AddListener(() => OnPickedResponse(response));

            temporaryResponseButtons.Add(responseButton);

            responseBoxHeight += responseButtonTemplate.sizeDelta.y;
        }

        responseBox.sizeDelta = new Vector2(responseBox.sizeDelta.x, responseBoxHeight);
        responseBox.gameObject.SetActive(true);
    }
    
    private void OnPickedResponse(Response response) {
        responseBox.gameObject.SetActive(false);
        foreach(GameObject button in temporaryResponseButtons) {
            Destroy(button);
        }
        temporaryResponseButtons.Clear();
        dialogueUI.ShowDialogue(response.DialogueObject);
    }
}
