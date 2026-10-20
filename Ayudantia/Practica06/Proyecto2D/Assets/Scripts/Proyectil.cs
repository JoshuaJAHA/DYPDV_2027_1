using UnityEngine;

public class Proyectil : MonoBehaviour
{
    [Header("Configuración del Proyectil")]
    public float velocidadBase = 10f;
    public float tiempoVida = 2f;
    public int daño = 1;

    private Vector2 direccion;
    private float velocidadExtra = 0f;

    public void Inicializar(Vector2 dir, float velocidadJugador)
    {
        direccion = dir.normalized;
        // Solo sumamos la velocidad en la misma dirección que dispara
        velocidadExtra = Mathf.Abs(velocidadJugador);
        Destroy(gameObject, tiempoVida);
    }

    void Update()
    {
        float velocidadFinal = velocidadBase + velocidadExtra;
        transform.position += (Vector3)(direccion * velocidadFinal * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemigo"))
        {
            EnemigoIA enemigo = col.GetComponent<EnemigoIA>();
            if (enemigo != null)
            {
                enemigo.RecibirDaño(daño);
            }
            Destroy(gameObject);
        }
        else if (col.CompareTag("Pared") || col.CompareTag("Obstaculo"))
        {
            Destroy(gameObject);
        }
    }
}
