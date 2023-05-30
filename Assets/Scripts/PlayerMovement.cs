using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float walkSpeed = 5.0f;
    [SerializeField] float runSpeed = 8.0f;
    Vector2 moveInput;
    Rigidbody2D myRb;
    Animator myAnimator;
    bool isRunning  = false;
    bool isMoving;
    // Start is called before the first frame update
    void Awake()
    {
        myRb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!MainMenuUI.IsGamePaused) {
            Walk();
            Run();
            
            if (moveInput.x != 0) moveInput.y = 0;

            if (moveInput != Vector2.zero) {
                myAnimator.SetFloat("moveX", moveInput.x);
                myAnimator.SetFloat("moveY", moveInput.y);
            }
            myAnimator.SetBool("isWalking", isMoving);
            myAnimator.SetBool("IsRunning", isRunning);
        }
    }

    void Walk() {
        if(!isRunning) {
            myRb.velocity = moveInput * walkSpeed;
        }
    }

    void Run() {
        if (isRunning) {
            myRb.velocity = moveInput * runSpeed;
        }
        myAnimator.SetBool("IsRunning", isRunning);
    }

    void OnMove(InputValue inputValue) {
        moveInput = inputValue.Get<Vector2>();
        isMoving = moveInput.magnitude > 0;
    }

    void OnRun(InputValue inputValue) {
        isRunning = inputValue.isPressed;
    }
}
