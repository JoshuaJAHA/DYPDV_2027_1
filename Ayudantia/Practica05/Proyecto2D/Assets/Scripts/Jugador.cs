using System.Collections;
using UnityEngine;

public class Jugador : Player
{
    public bool enSuelo = false;

    [Header("Inmunidad y Parpadeo")]
    public float tiempoInmunidad = 1.5f;
    private bool esInmune = false;

    protected override void Awake()
    {
        base.Awake();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        // Cancela cualquier velocidad física residual para evitar que la física de Unity resbale al personaje
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
        }
    }

    public override void RecibirDaño(int cantidad, Collider2D colAtacante = null)
    {
        if (esInmune) return;

        base.RecibirDaño(cantidad, colAtacante);

        if (vida > 0)
        {
            StartCoroutine(EfectoParpadeo(colAtacante));
        }
    }

    private IEnumerator EfectoParpadeo(Collider2D colAtacante)
    {
        esInmune = true;
        Collider2D miCol = GetComponent<Collider2D>();

        // Ignorar colisiones físicas entre Jugador y el atacante durante la inmunidad
        if (miCol != null && colAtacante != null)
        {
            Physics2D.IgnoreCollision(miCol, colAtacante, true);
        }

        float tiempoPasado = 0f;
        float intervaloParpadeo = 0.1f;

        while (tiempoPasado < tiempoInmunidad)
        {
            if (sr != null)
            {
                sr.enabled = !sr.enabled;
            }
            yield return new WaitForSeconds(intervaloParpadeo);
            tiempoPasado += intervaloParpadeo;
        }

        if (sr != null)
        {
            sr.enabled = true;
        }

        // Reactivar colisiones físicas al terminar la inmunidad
        if (miCol != null && colAtacante != null)
        {
            Physics2D.IgnoreCollision(miCol, colAtacante, false);
        }

        esInmune = false;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Suelo"))
        {
            enSuelo = true;
        }
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Suelo"))
        {
            enSuelo = true;
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Suelo"))
        {
            enSuelo = false;
        }
    }
}