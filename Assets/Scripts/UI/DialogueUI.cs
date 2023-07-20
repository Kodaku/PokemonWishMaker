using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pokemon.Dialogue;
using TMPro;
using UnityEngine.UI;

namespace Pokemon.UI
{
    public class DialogueUI : MonoBehaviour
    {
        PlayerConversant playerConversant;
        [SerializeField] TextMeshProUGUI AIText;
        [SerializeField] Button nextButton;
        [SerializeField] Transform choiceRoot;
        [SerializeField] GameObject AIResponse;
        [SerializeField] GameObject choicePrefab;
        [SerializeField] TextMeshProUGUI conversantName;
        // Start is called before the first frame update
        void Start()
        {
            playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
            playerConversant.onConversationUpdated += UpdateUI;
            // nextButton.onClick.AddListener(() => playerConversant.Next());

            UpdateUI();
        }

        private void UpdateUI() {
            gameObject.SetActive(playerConversant.IsActive());
            if (!playerConversant.IsActive()) {
                return;
            }
            string currentConversantName = playerConversant.GetCurrentConversantName();
            if (currentConversantName.Length == 0)
            {
                conversantName.text = "";
            }
            else
            {
                conversantName.text = playerConversant.GetCurrentConversantName() + ":";
            }
            AIResponse.SetActive(!playerConversant.IsChoosing());
            choiceRoot.gameObject.SetActive(playerConversant.IsChoosing());
            // Choosing
            if(playerConversant.IsChoosing())
            {
                BuildChoiceList();
            }
            // Responding
            else {
                AIText.text = playerConversant.GetText();
                // nextButton.gameObject.SetActive(playerConversant.HasNext());
            }
        }

        private void BuildChoiceList()
        {
            foreach (Transform item in choiceRoot)
            {
                Destroy(item.gameObject);
            }
            foreach (DialogueNode choice in playerConversant.GetChoices())
            {
                GameObject choiceInstance = Instantiate(choicePrefab, choiceRoot);
                var textComp = choiceInstance.GetComponentInChildren<TextMeshProUGUI>();
                textComp.text = choice.Text;
                Button button = choiceInstance.GetComponentInChildren<Button>();
                button.onClick.AddListener(() => {
                    playerConversant.SelectChoice(choice);
                });
            }
        }
    }
}
