using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Vector2 movementInput;
    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer renderer;

    public float moveSpeed = 1f;
    public ContactFilter2D movementFilter;
    public float collisionOffset = 0.05f;

    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();


    //this is where you get the components that you need
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if (movementInput != Vector2.zero)
        {
            bool success = TryMove(movementInput);

            if(!success) {
                success = TryMove(new Vector2(movementInput.x, 0));

                if(!success){
                    success = TryMove(new Vector2(movementInput.y, 0));
                }
            }
            animator.SetBool("isMoving", success);
            animator.SetFloat("direction_x", movementInput.x);
            animator.SetFloat("direction_y", movementInput.y);
        }
        else {
            //named this isMoving in the animator map window
            animator.SetBool("isMoving", false);
        }

        // //changing direction of sprite when walking bkwrds
        // if(movementInput.x < 0) {
        //     renderer.flipX = true;
        // } else if(movementInput.x > 0) {
        //     renderer.flipX = false;
        // }

    }

    //moving here
    private bool TryMove(Vector2 direction) {

            int count = rb.Cast(movementInput.normalized, movementFilter, castCollisions, moveSpeed * Time.fixedDeltaTime + collisionOffset);
            if (count == 0)
            {
                rb.MovePosition(rb.position + movementInput * moveSpeed * Time.fixedDeltaTime);
                return true;
            }
            else {
                return false;
            }
    }

    void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }
}
