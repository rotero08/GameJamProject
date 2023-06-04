using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateIA : MonoBehaviour
{
    public GameObject player;
    public GameObject bullet;

    private RaycastHit2D groundDetector;
    private RaycastHit2D playerDetector;

    private Rigidbody2D pirateRigB;

    private BoxCollider2D pirateBoxC;

    public float speed = 5;
    public float shuttingRange = 5;
    public float jumpForce = 1;
    private float rayDesface;

    private int orientation;

    public bool grounded;
    void Start()
    {
        pirateRigB = GetComponent<Rigidbody2D>();
        pirateBoxC = GetComponent<BoxCollider2D>();
        rayDesface = (pirateBoxC.size.x / 2 + 0.1f );
    }
    void Update()
    {
        orientation = ((player.transform.position.x < transform.position.x) ? -1 : 1);

        groundDetector = Physics2D.Raycast(transform.position + new Vector3(orientation * rayDesface, 0), new Vector3(0, -1, 0), 1f);
        playerDetector = Physics2D.Raycast(transform.position + new Vector3(orientation * rayDesface, 0), new Vector3(orientation, 0, 0), shuttingRange);

        Debug.DrawRay(transform.position + new Vector3(orientation * rayDesface, 0), new Vector3(orientation*shuttingRange,0,0),Color.white);

        transform.Translate(new Vector2(1,0) * orientation * speed * Time.deltaTime);

        if (groundDetector.collider == null && grounded == true)
        {
            grounded = false;
            pirateRigB.AddForce(new Vector2(0, 1) * jumpForce, ForceMode2D.Impulse);
            speed = 3f;
        }

        if (playerDetector.collider!=null)
        {
            if (speed > 0) speed -= 0.1f;
            Debug.Log("Fire!!"+transform.position.GetType());
            Instantiate(bullet, transform.position,bullet.transform.rotation);
        }
        


    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.collider.CompareTag("Ground"))
        {
            grounded = true;
            if (playerDetector.collider == null) speed = 5;
        }
    }
}