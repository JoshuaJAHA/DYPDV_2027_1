using UnityEngine;

public class PlataformaMovil : MonoBehaviour
{
    [Header("Puntos de Recorrido")]
    public Transform puntoA;
    public Transform puntoB;
    public float velocidad = 2f;

    private Transform objetivo;

    void Start()
    {
        objetivo = puntoA;
    }

    void Update()
    {
        if (puntoA == null || puntoB == null) return;

        transform.position = Vector2.MoveTowards(
            transform.position,
            objetivo.position,
            velocidad * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, objetivo.position) < 0.1f)
        {
            objetivo = (objetivo == puntoA) ? puntoB : puntoA;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            // Solo emparentar si el jugador está pisando la plataforma desde arriba
            bool pisandoDesdeArriba = false;
            foreach (ContactPoint2D punto in col.contacts)
            {
                // Si la normal apunta hacia abajo respecto al obstáculo / arriba para el jugador
                if (punto.normal.y < -0.5f)
                {
                    pisandoDesdeArriba = true;
                    break;
                }
            }

            if (pisandoDesdeArriba)
            {
                // Guardamos la escala global original antes de emparentar
                Vector3 escalaMundo = col.transform.lossyScale;
                col.transform.SetParent(transform);
                
                // Compensamos la escala local dividiendo entre la escala de la plataforma
                col.transform.localScale = new Vector3(
                    escalaMundo.x / transform.lossyScale.x,
                    escalaMundo.y / transform.lossyScale.y,
                    escalaMundo.z / transform.lossyScale.z
                );
            }
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Vector3 escalaMundo = col.transform.lossyScale;
            col.transform.SetParent(null);
            col.transform.localScale = escalaMundo;
        }
    }
}
