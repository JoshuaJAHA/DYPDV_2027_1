using UnityEngine;

public class EnemigoIA : Player
{
    [Header("Patrullaje")]
    public float direccion = -1f; // -1: Izquierda, 1: Derecha
    private bool estaMuerto = false;
    private Animator anim;

    protected override void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (estaMuerto) return;

        if (rb != null)
        {
            rb.velocity = new Vector2(direccion * velocidad, rb.velocity.y);
        }

        if (anim != null)
        {
            anim.SetBool("Caminando", Mathf.Abs(rb.velocity.x) > 0.1f);
        }
    }

    protected override void Morir()
    {
        estaMuerto = true;

        if (anim != null)
        {
            anim.SetTrigger("Morir");
        }

        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        Collider2D col2d = GetComponent<Collider2D>();
        if (col2d != null) col2d.enabled = false;

        Debug.Log(gameObject.name + " reproduciendo animación de muerte.");

        Destroy(gameObject, 0.8f);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (estaMuerto) return;

        // 1. Rebotar al chocar con Pared u Obstaculo
        if (col.gameObject.CompareTag("Pared") || col.gameObject.CompareTag("Obstaculo"))
        {
            direccion *= -1f;
            if (sr != null)
            {
                sr.flipX = !sr.flipX;
            }
        }

        // 2. Colisión con el Jugador
        if (col.gameObject.CompareTag("Player"))
        {
            // Pisotón desde arriba
            if (col.contacts.Length > 0 && col.contacts[0].normal.y < -0.5f)
            {
                Debug.Log("¡El Jugador pisó al Enemigo!");

                ControlJugador ctrl = col.gameObject.GetComponent<ControlJugador>();
                if (ctrl != null)
                {
                    ctrl.velocidadVertical = 7f;
                }

                RecibirDaño(1);
            }
            else
            {
                // Ataque lateral
                Jugador jugadorScript = col.gameObject.GetComponent<Jugador>();
                if (jugadorScript != null)
                {
                    Debug.Log("¡El Enemigo atacó al Jugador!");

                    // Ignorar colisión física inmediatamente para que el motor de física no empuje al jugador
                    Physics2D.IgnoreCollision(col.collider, col.otherCollider, true);

                    ControlJugador ctrl = col.gameObject.GetComponent<ControlJugador>();
                    if (ctrl != null)
                    {
                        ctrl.velocidadActual = 0f;
                        ctrl.velocidadVertical = 3f;
                    }

                    // Aplicar daño e iniciar parpadeo
                    jugadorScript.RecibirDaño(1, col.otherCollider);

                    // El enemigo cambia de dirección
                    direccion *= -1f;
                    if (sr != null) sr.flipX = !sr.flipX;
                }
            }
        }
    }
}