using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateIA : MonoBehaviour
{
    public GameObject player;

    private RaycastHit2D groundDetector;

    private Rigidbody2D pirateRigB;

    private float speed = 5;
    private float jumpForce = 1;

    public bool grounded;
    // Start is called before the first frame update
    void Start()
    {
        pirateRigB = GetComponent<Rigidbody2D>();
        Debug.Log(transform.position + new Vector3(1, 0));
    }

    // Update is called once per frame
    void Update()
    {

        transform.Translate(new Vector2(1,0) * ((player.transform.position.x < transform.position.x)?-1:1) * speed * Time.deltaTime);

        groundDetector = Physics2D.Raycast(transform.position + new Vector3(1, -1), new Vector3(0, -1, 0), 1f);

        Debug.DrawRay(transform.position + new Vector3(1, 0), new Vector3(0,-1,0),Color.white);

        if (groundDetector.collider == null && grounded == true)
        {
            grounded = false;
            pirateRigB.AddForce(new Vector2(1, 1) * jumpForce, ForceMode2D.Impulse);
        }
        else
        {

        }

    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.collider.CompareTag("Ground"))
        {
            grounded = true;
        }
    }
}