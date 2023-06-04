using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private float horizontal;
    private float vertical;
    private Vector2 movement;
    private float speed = 8f;
    private float jumpingPower = 6f;
    private bool isFacingRight = true;

    private bool isCloseToLadder = false;
    private bool hasStartedClimb = false;
    private Transform ladder;

    void Update()
    {
        if (!isFacingRight && horizontal > 0f)
        {
            Flip();
        }
        else if (isFacingRight && horizontal < 0f)
        {
            Flip();
        }
        
        if (isCloseToLadder && vertical != 0f)
        {
            hasStartedClimb = true;
        }


    }

    private void FixedUpdate()
    {
        horizontal = movement.x;
        vertical = movement.y;

        //Debug.Log(hasStartedClimb);

        if (!hasStartedClimb)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }

        if (hasStartedClimb)
        {
            ladder.GetChild(2).GetComponent<EdgeCollider2D>().enabled = false;
            float height = GetComponent<SpriteRenderer>().size.y;
            float topHandlerY = Half(ladder.transform.GetChild(0).transform.position.y + height);
            float bottomHandlerY = Half(ladder.transform.GetChild(1).transform.position.y + height);
            float transformY = Half(transform.position.y);
            float transformVY = transformY + vertical;

            if (transformVY > topHandlerY || transformVY < bottomHandlerY)
            {
                ResetClimbing();
            }
            else if (transformY <= topHandlerY && transformY >= bottomHandlerY)
            {
                rb.gravityScale = 0f;
                rb.velocity = new Vector2(horizontal * speed, vertical * speed);
            }
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }

        if (context.canceled && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    public void Move(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }

    private void ResetClimbing()
    {
        ladder.GetChild(2).GetComponent<EdgeCollider2D>().enabled = true;
        if (hasStartedClimb)
        {
            hasStartedClimb = false;
            rb.gravityScale = 1f;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("Trigger Detected");
        if (collision.gameObject.tag.Equals("Ladder"))
        {
            isCloseToLadder = true;
            ladder = collision.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Ladder"))
        {
            rb.gravityScale = 1f;
            isCloseToLadder = false;
            hasStartedClimb = false;
        }
    }

    public static float Half(float value)
    {
        return Mathf.Floor(value) + 0.5f;
    }
}
