using UnityEngine;

public class Jugador : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("Ha comenzado la colisión con el objeto: " + col.gameObject.name);
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        Debug.Log("Manteniendo colisión continua con: " + col.gameObject.name);
    }
}