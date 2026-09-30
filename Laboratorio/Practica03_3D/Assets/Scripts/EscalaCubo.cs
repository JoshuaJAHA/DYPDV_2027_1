using UnityEngine;

public class EscalaCubo : MonoBehaviour
{
  public float velocidadCrecimiento = 2.0f; // Velocidad de cambio de escala del cubo

  public float escalaMinima = 1.0f; // Escala mínima del cubo

  void Update()
  {
    if(Input.GetKey(KeyCode.Space))
    {
      // Cambiar la escala del cubo en el eje Y
      transform.localScale += Vector3.one * velocidadCrecimiento * Time.deltaTime;
    }
    else
        {
            transform.localScale = Vector3.Max(transform.localScale - Vector3.one * velocidadCrecimiento * Time.deltaTime, Vector3.one * escalaMinima);
        }  

  }
}   
