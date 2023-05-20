using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    Vector2 moveInput;
    Rigidbody2D myRb;
    // Start is called before the first frame update
    void Start()
    {
        myRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Run();
    }

    void Run() {
        myRb.velocity = moveInput * walkSpeed;
    }

    void OnMove(InputValue inputValue) {
        moveInput = inputValue.Get<Vector2>();
    }
}
