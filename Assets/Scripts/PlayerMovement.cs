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
    // Start is called before the first frame update
    void Start()
    {
        myRb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // myAnimator.Update(Time.deltaTime);
        Walk();
        Run();
    }

    void Walk() {
        myAnimator.SetBool("IsWalkingDown", false);
        myAnimator.SetBool("IsWalkingUp", false);
        myAnimator.SetBool("IsWalkingLeft", false);
        myAnimator.SetBool("IsWalkingRight", false);
        if (moveInput.x > 0) {
            myAnimator.SetBool("IsWalkingRight", true);
        }
        else if(moveInput.x < 0) {
            myAnimator.SetBool("IsWalkingLeft", true);
        }
        else if(moveInput.y > 0) {
            myAnimator.SetBool("IsWalkingUp", true);
        }
        else if (moveInput.y < 0) {
            myAnimator.SetBool("IsWalkingDown", true);
        }
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
    }

    void OnRun(InputValue inputValue) {
        isRunning = inputValue.isPressed;
    }
}
