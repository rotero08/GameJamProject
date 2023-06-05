using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floatObject : MonoBehaviour
{
    public float fuerzaFlotacion = 1f;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        rb.AddForce(Vector2.up * fuerzaFlotacion);
    }
}
