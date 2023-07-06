using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pokemon.Dialogue {
    public class AIConversant : MonoBehaviour
    {
        [SerializeField] Dialogue dialogue;
        [SerializeField] string npcName;
        Collider2D aiCollider;
        bool isCollidingWithPlayer = false;
        public static bool isShowingDialogue = false;
        private PlayerConversant playerConversant;

        public string NPCName => npcName;

        private void Start() {
            aiCollider = GetComponent<Collider2D>();
            isCollidingWithPlayer = false;
            playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
        }

        private void OnCollisionEnter2D(Collision2D other) {
            if (other.gameObject.tag == "Player" && !isCollidingWithPlayer) {
                Debug.Log("Collision with player");
                isCollidingWithPlayer = true;
            }
        }

        private void Update() {
            if(Input.GetKeyDown(KeyCode.A) && isCollidingWithPlayer && !isShowingDialogue) {
                playerConversant.StartDialogue(this, dialogue);
                isShowingDialogue = true;
            }
            else if (Input.GetKeyDown(KeyCode.A) && isCollidingWithPlayer && isShowingDialogue) {
                if (!playerConversant.HasNext()) {
                    playerConversant.Quit();
                    isShowingDialogue = false;
                    return;
                }
                playerConversant.Next();
            }
        }
    }
}
