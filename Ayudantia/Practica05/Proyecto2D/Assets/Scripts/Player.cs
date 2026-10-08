using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Atributos del Player")]
    public float velocidad = 2f;
    public int vida = 3; 

    protected Rigidbody2D rb;
    protected SpriteRenderer sr;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    public virtual void RecibirDaño(int cantidad, Collider2D colAtacante = null)
    {
        vida -= cantidad;
        Debug.Log(gameObject.name + " recibió " + cantidad + " de daño. Vida restante: " + vida);

        if (vida <= 0)
        {
            Morir();
        }
    }

    protected virtual void Morir()
    {
        Debug.Log(gameObject.name + " ha muerto.");
        Destroy(gameObject);
    }
}