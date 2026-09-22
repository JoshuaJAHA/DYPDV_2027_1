using UnityEngine;

public class Jugador : MonoBehaviour
{
    public bool enSuelo = false; 

    private void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("Ha comenzado la colisión con el objeto: " + col.gameObject.name);

        if (col.gameObject.CompareTag("Suelo")) 
        {
            enSuelo = true; 
        }
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        Debug.Log("Manteniendo colisión continua con: " + col.gameObject.name);
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Suelo")) 
        {
            enSuelo = false; 
        }
    }
}
//finn