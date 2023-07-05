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
        bool isShowingDialogue = false;

        public string NPCName => npcName;

        private void Start() {
            aiCollider = GetComponent<Collider2D>();
            isCollidingWithPlayer = false;
        }

        private void OnCollisionEnter2D(Collision2D other) {
            if (other.gameObject.tag == "Player" && !isCollidingWithPlayer) {
                Debug.Log("Collision with player");
                isCollidingWithPlayer = true;
            }
        }

        private void Update() {
            if(Input.GetKeyDown(KeyCode.A) && isCollidingWithPlayer && !isShowingDialogue) {
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>().StartDialogue(this, dialogue);
                isShowingDialogue = true;
            }
        }
    }
}
