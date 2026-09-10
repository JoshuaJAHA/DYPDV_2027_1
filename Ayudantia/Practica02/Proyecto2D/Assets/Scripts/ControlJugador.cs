using UnityEngine;

public class ControlJugador : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidadActual = 0f; 
    public float velocidadMax = 5f; 
    public float aceleracion = 10f; 

    [Header("Salto y Gravedad Manual")]
    public float velocidadVertical = 0f; 
    public float gravedad = -20f; 
    public float tiempoMaxSalto = 0.2f; 
    private float tiempoSaltoActual = 0f; 

    private float tiempoAnterior; 
    private Jugador jugador; 

    private void Awake()
    {
        jugador = GetComponent<Jugador>(); 
    }

    private void Start()
    {
        tiempoAnterior = Time.time; //inicialización del tiempo ]
    }

    private void Update()
    {
        // calculo de deltaTime
        float delta = Time.time - tiempoAnterior; 
        tiempoAnterior = Time.time; 

        if (delta > 0.1f) delta = 0.1f;

        // movimiento Lateral con aceleración
        float h = Input.GetAxis("Horizontal"); 
        velocidadActual += h * aceleracion * delta; 
        velocidadActual = Mathf.Clamp(velocidadActual, -velocidadMax, velocidadMax); 
        
        transform.position += new Vector3(velocidadActual * delta, 0, 0); 

        // inicio del salto
        if (Input.GetAxis("Jump") > 0 && jugador.enSuelo) 
        {
            velocidadVertical = 10f; 
            jugador.enSuelo = false; 
            tiempoSaltoActual = 0f; 
        }

        // Mantener salto mas grnde
        if (!jugador.enSuelo && Input.GetAxis("Jump") > 0) 
        {
            if (tiempoSaltoActual < tiempoMaxSalto) 
            {
                velocidadVertical += 20f * delta; 
                tiempoSaltoActual += delta; 
            }
        }

        // se cancela salto grande si se suelta el botón
        if (Input.GetAxis("Jump") == 0) 
        {
            tiempoSaltoActual = tiempoMaxSalto; 
        }

        // aplicar Gravedad y movimiento vertical
        if (jugador.enSuelo) 
        {
            velocidadVertical = 0f; 
        }
        else
        {
            velocidadVertical += gravedad * delta; 
        }

        transform.position += new Vector3(0, velocidadVertical * delta, 0); 
    }
} //finn