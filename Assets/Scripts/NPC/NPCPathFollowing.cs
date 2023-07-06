using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pokemon.Dialogue;

public class NPCPathFollowing : MonoBehaviour
{
    [SerializeField] NPCPath npcPath;
    [SerializeField] float moveSpeed = 3.0f;
    [SerializeField] bool isStatic = false;
    private Vector2 currentDirection;
    private Animator myAnimator;
    private Rigidbody2D myRb;
    private bool isWalking = true;
    private bool isWaiting = false;

    private void Awake() {
        myAnimator = GetComponent<Animator>();
        myRb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        npcPath.Initialize();
        isWalking = !isStatic;
        myAnimator.SetBool("IsWalking", isWalking);
    }

    
    // Update is called once per frame
    void Update()
    {
        if (AIConversant.isShowingDialogue) {
            return;
        }
        Walk();

        if (isWalking && !isStatic) {
            myRb.velocity = moveSpeed * currentDirection;
        }
        else {
            myRb.velocity = Vector2.zero;
        }
        myAnimator.SetFloat("moveX", currentDirection.x);
        myAnimator.SetFloat("moveY", currentDirection.y);
    }

    private void Walk() {
        GameObject currentNode = npcPath.CurrentNode;
        if (Vector3.Distance(transform.position, currentNode.transform.position) < 0.1f && !isStatic) {
            npcPath.NextNodeIndex();
            currentDirection = npcPath.GetCurrentDirection();
        }
        if (isStatic && !isWaiting) {
            StartCoroutine(WaitAndMove());
        }
    }

    private IEnumerator WaitAndMove() {
        isWaiting = true;
        yield return new WaitForSeconds(Random.Range(1, 2));
        npcPath.NextNodeIndex();
        currentDirection = npcPath.GetCurrentDirection();
        isWaiting = false;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Player") {
            myAnimator.SetBool("IsWalking", false);
            isWalking = false;
        }
    }

    private void OnCollisionExit2D(Collision2D other) {
        if (other.gameObject.tag == "Player") {
            isWalking = !isStatic;
            myAnimator.SetBool("IsWalking", isWalking);
        }
    }
}
