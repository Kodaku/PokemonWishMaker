using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pokemon.Dialogue;

public class PickableItem : MonoBehaviour
{
    [SerializeField] Dialogue dialogue;
    [SerializeField] private ItemSO item;
    private bool isCollidingWithPlayer = false;
    private Animator playerAnimator;
    public static bool isShowingDialogue = false;
    private PlayerConversant playerConversant;

    private void Start()
    {
        playerAnimator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" && !isCollidingWithPlayer)
        {
            isCollidingWithPlayer = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" && isCollidingWithPlayer)
        {
            isCollidingWithPlayer = false;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A) && isCollidingWithPlayer && !isShowingDialogue) {
                DialogueNode currentNode = null;
                foreach(DialogueNode dialogueNode in dialogue.GetAllNodes())
                {
                    string dialogueText = dialogueNode.Text;
                    dialogueText = dialogueText.Replace("{playerName}", playerConversant.PlayerName);
                    Debug.Log(dialogueText);
                    dialogueText = dialogueText.Replace("{itemName}", item.ItemName);
                    dialogueNode.SetText(dialogueText);
                    currentNode = dialogueNode;
                }
                dialogue.SetNodes(new List<DialogueNode>(){currentNode});
                playerConversant.StartDialogue(this, dialogue);
                isShowingDialogue = true;
                playerAnimator.SetBool("IsPickingUpItem", true);
                GetComponent<SpriteRenderer>().enabled = false;
            }
            else if (Input.GetKeyDown(KeyCode.A) && isCollidingWithPlayer && isShowingDialogue) {
                if (!playerConversant.HasNext()) {
                    DialogueNode currentNode = null;
                    foreach(DialogueNode dialogueNode in dialogue.GetAllNodes())
                    {
                        dialogueNode.SetText("{playerName} obtained a {itemName}");
                        currentNode = dialogueNode;
                    }
                    dialogue.SetNodes(new List<DialogueNode>(){currentNode});
                    playerConversant.Quit();
                    isShowingDialogue = false;
                    playerAnimator.SetBool("IsPickingUpItem", false);
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBag>().AddItemToBag(item);
                    Destroy(gameObject);
                    return;
                }
                playerConversant.Next();
            }
    }
}
