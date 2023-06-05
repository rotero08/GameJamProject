using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PirateIA : MonoBehaviour
{
    public GameObject player;
    public GameObject bullet;

    private RaycastHit2D groundDetector;
    private RaycastHit2D ceilDetector;
    private int stairsDetector;
    private RaycastHit2D playerDetector;

    private Rigidbody2D pirateRigB;

    private BoxCollider2D pirateBoxC;

    public float baseSpeed = 4;
    [SerializeField] private float realSpeed;
    public float shuttingRange = 10;
    public float jumpForce = 2;
    private float rayDesfaceX;
    private float rayDesfaceY;
    private float lastShoot=0;
    private float cooldown=1;

    private Collider2D[] stairsResults = new Collider2D[2];

    private bool upStairs;

    private int orientation;

    public bool grounded;

    public int health = 100;

    public GameObject deathEffect;

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        //Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    void Start()
    {
        realSpeed = baseSpeed;
        pirateRigB = GetComponent<Rigidbody2D>();
        pirateBoxC = GetComponent<BoxCollider2D>();
        rayDesfaceX = (pirateBoxC.size.x / 2 + 0.25f );
        rayDesfaceY = (pirateBoxC.size.y / 2 + 0.25f);
    }

    void Update()
    {

        orientation = ((player.transform.position.x < transform.position.x) ? -1 : 1);

        ContactFilter2D filter = new ContactFilter2D();

        filter.useTriggers = true;
        
        int stairsDetector = Physics2D.OverlapCircle(transform.position, 0.1f, filter, stairsResults);

        //Debug.Log(stairsResults[1].gameObject.tag);

        //filter.SetLayerMask(LayerMask.GetMask("Stairs"));

        groundDetector = Physics2D.Raycast(transform.position + new Vector3(orientation * rayDesfaceX, 0), new Vector3(0, -1, 0), 1f);
        ceilDetector = Physics2D.Raycast(transform.position+ new Vector3(0,rayDesfaceY), new Vector2(0, 1),1f);
        playerDetector = Physics2D.Raycast(transform.position + new Vector3(orientation * rayDesfaceX, 0), new Vector3(orientation, 0, 0),shuttingRange, LayerMask.GetMask("Default"));


        Debug.DrawRay(transform.position + new Vector3(orientation * rayDesfaceX, 0), new Vector3(orientation * shuttingRange, 0, 0), Color.white);
        Debug.DrawRay(transform.position + new Vector3(orientation * rayDesfaceX, 0), new Vector3(0, -1, 0), Color.red);
        Debug.DrawRay(transform.position + new Vector3(0, rayDesfaceY), new Vector3(0, 1, 0), Color.blue);
        
        if(((ceilDetector.collider != null) && ceilDetector.collider.CompareTag("Ground")))
        {
            grounded = false;
        }


        if (transform.position.x != player.transform.position.x)
        {
            if(grounded && !upStairs)
            {
                pirateRigB.velocity = new Vector2(1, 0) * orientation * realSpeed;
            }

            if ((groundDetector.collider == null) && grounded && !upStairs)
            {
                Debug.Log(groundDetector.collider);
                grounded = false;
                pirateRigB.AddForce(new Vector2(0, 1) * jumpForce, ForceMode2D.Impulse);
                realSpeed = baseSpeed * 0.5f;
            }

            if (((playerDetector.collider!=null) && playerDetector.collider.CompareTag("Player")) && Time.time-lastShoot > cooldown && grounded)
            {
                realSpeed = 0;
                Shutting();
                lastShoot = Time.time;
            }

        }
       // Debug.Log(stairsDetector.collider);

    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.collider.CompareTag("Ground"))
        {
            grounded = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Stairs") && transform.position.y < player.transform.position.y - 0.5 && ((stairsResults[1] != null) && stairsResults[1].gameObject.CompareTag("Stairs")))
        {
            pirateRigB.gravityScale = 0;
            upStairs = true;
            grounded= false;
            pirateRigB.velocity =new Vector2(0,1)*baseSpeed;
            transform.Translate(Vector3.up * baseSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Stairs"))
        {
            upStairs = false;
            grounded=true;
            pirateRigB.gravityScale = 1;
            realSpeed = baseSpeed;
            pirateRigB.velocity = Vector2.zero;
        }
    }

    private void Shutting(){
        Instantiate(bullet, transform.position,Quaternion.AngleAxis(orientation*90,Vector3.back));
    }
    
}