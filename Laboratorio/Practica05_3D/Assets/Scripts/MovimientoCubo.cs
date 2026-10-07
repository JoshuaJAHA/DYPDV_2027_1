using UnityEngine;

public class MovimientoCubo : MonoBehaviour
{
    [Header("Puntos de destino")]
    public Transform puntoA; // Posición de la primera torre/rampa
    public Transform puntoB; // Posición de la segunda torre/rampa

    [Header("Configuración")]
    public float velocidad = 3.0f;

    private Vector3 objetivo;

    private void Start()
    {
        // Al iniciar, la plataforma se dirige hacia el punto B
        if (puntoB != null)
        {
            objetivo = puntoB.position;
        }
    }

    private void Update()
    {
        if (puntoA == null || puntoB == null) return;

        // Mover gradualmente la plataforma hacia el objetivo
        transform.position = Vector3.MoveTowards(transform.position, objetivo, velocidad * Time.deltaTime);

        // Cuando la plataforma llega casi al punto objetivo, cambia la meta
        if (Vector3.Distance(transform.position, objetivo) < 0.1f)
        {
            objetivo = (objetivo == puntoA.position) ? puntoB.position : puntoA.position;
        }
    }
}