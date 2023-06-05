using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vacaFlotante : MonoBehaviour
{
    public float fuerzaFlotacion = 1f;
    public float duracionFlotacion = 5f;
    public float velocidadRotacion = 90f;

    private Rigidbody2D rb;
    private float tiempoInicioFlotacion;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        tiempoInicioFlotacion = Time.time;
    }
    private void Update()
    {
        if(Time.time - tiempoInicioFlotacion >= duracionFlotacion)
        {
            rb.velocity = Vector2.zero;
            enabled = false;
        }
        else
        {
            rb.AddForce(Vector2.up * fuerzaFlotacion);

            float anguloRotacion = velocidadRotacion * Time.deltaTime;
            //transform.Rotate(0f, 0f, anguloRotacion);
        }
    }
}
