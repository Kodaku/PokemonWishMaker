using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCPathFollowing : MonoBehaviour
{
    [SerializeField] NPCPathType pathType;
    [SerializeField] NPCPath npcPath;
    [SerializeField] float moveSpeed = 3.0f;
    private int nodeIndex;
    private GameObject currentNode;
    private Animator myAnimator;
    private Rigidbody2D myRb;
    private bool isWalking = true;

    private void Awake() {
        myAnimator = GetComponent<Animator>();
        myRb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        nodeIndex = 0;
        currentNode = npcPath.Nodes[nodeIndex];
        myAnimator.SetBool("IsWalking", true);
        isWalking = true;
    }

    private float ApproximateValue(float value) {
        if (value >= 0.5f) {
            return 1.0f;
        } else if((value < 0.5f && value > 0.0f) || (value > -0.5f && value < 0.0f)) {
            return 0.0f;
        } else if(value < -0.5f) {
            return -1.0f;
        }
        return value;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveDirection = Walk();
        
        moveDirection.x = ApproximateValue(moveDirection.x);
        moveDirection.y = ApproximateValue(moveDirection.y);

        Debug.Log(moveDirection);

        if (moveDirection.x != 0) moveDirection.y = 0;

        if (moveDirection.y != 0) moveDirection.x = 0;

        if (isWalking) {
            myRb.velocity = moveSpeed * moveDirection;
        }
        else {
            myRb.velocity = Vector2.zero;
        }
        myAnimator.SetFloat("moveX", moveDirection.x);
        myAnimator.SetFloat("moveY", moveDirection.y);
    }

    private Vector2 Walk() {
        float step = moveSpeed * Time.deltaTime;
        // transform.position = Vector3.MoveTowards(transform.position, currentNode.transform.position, step);
        if (Vector3.Distance(transform.position, currentNode.transform.position) < 0.1f) {
            nodeIndex++;
            if (pathType == NPCPathType.CIRCULAR_LOOPING) {
                nodeIndex = nodeIndex % npcPath.Nodes.Length;
            }
            currentNode = npcPath.Nodes[nodeIndex];
        }
        Vector2 moveDirection = (currentNode.transform.position - transform.position).normalized;
        return moveDirection;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Player") {
            myAnimator.SetBool("IsWalking", false);
            isWalking = false;
        }
    }

    private void OnCollisionExit2D(Collision2D other) {
        if (other.gameObject.tag == "Player") {
            myAnimator.SetBool("IsWalking", true);
            isWalking = true;
        }
    }
}
