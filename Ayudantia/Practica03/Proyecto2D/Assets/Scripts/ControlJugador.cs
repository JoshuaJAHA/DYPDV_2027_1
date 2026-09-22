using UnityEngine;

public class ControlJugador : MonoBehaviour
{
    [Header("Movimiento Horizontal")]
    public float velocidadActual = 0f;
    public float velocidadMax = 5f;
    public float aceleracion = 10f;
    public float desaceleracion = 8f; // Desaceleración al soltar 

    [Header("Salto y Gravedad Mejorada")]
    public float velocidadVertical = 0f;
    public float fuerzaSalto = 10f;
    public float gravedad = -20f;
    public float gravedadCaida = -30f; // Gravedad más fuerte 

    [Header("Coyote Time y Jump Buffering")]
    public float tiempoCoyote = 0.1f;
    public float tiempoBufferSalto = 0.1f;
    private float coyoteTimer = 0f;
    private float bufferTimer = 0f;

    [Header("Variables para Animaciones")]
    public bool estaCaminando;
    public bool estaSaltando;
    public bool estaCayendo;

    private float tiempoAnterior;
    private Jugador jugador;

    private void Awake()
    {
        jugador = GetComponent<Jugador>();
    }

    private void Start()
    {
        tiempoAnterior = Time.time;
    }

    private void Update()
    {
        // Cálculo de delta time
        float delta = Time.time - tiempoAnterior;
        tiempoAnterior = Time.time;
        if (delta > 0.1f) delta = 0.1f;

        // Movimiento Lateral y Desaceleración Automática
        float h = Input.GetAxisRaw("Horizontal");

        if (h != 0)
        {
            // Aceleración con input
            velocidadActual += h * aceleracion * delta;
            velocidadActual = Mathf.Clamp(velocidadActual, -velocidadMax, velocidadMax);
        }
        else
        {
            // Desaceleración progresiva 
            if (velocidadActual > 0)
            {
                velocidadActual -= desaceleracion * delta;
                if (velocidadActual < 0.1f) velocidadActual = 0;
            }
            else if (velocidadActual < 0)
            {
                velocidadActual += desaceleracion * delta;
                if (velocidadActual > -0.1f) velocidadActual = 0;
            }
        }

        // Coyote Time 
        if (jugador.enSuelo)
        {
            coyoteTimer = tiempoCoyote;
        }
        else
        {
            coyoteTimer -= delta;
        }

        // Jump Buffering 
        if (Input.GetButtonDown("Jump") || Input.GetAxis("Jump") > 0)
        {
            bufferTimer = tiempoBufferSalto;
        }
        else
        {
            bufferTimer -= delta;
        }

        // Salto con Coyote Time 
        if (bufferTimer > 0 && coyoteTimer > 0)
        {
            velocidadVertical = fuerzaSalto;
            jugador.enSuelo = false;
            bufferTimer = 0f;
            coyoteTimer = 0f;
        }

        // Gravedad Mejorada (Caída más rápida)
        if (jugador.enSuelo)
        {
            velocidadVertical = 0f;
        }
        else
        {
            if (velocidadVertical < 0)
            {
                // Cayendo: gravedad más fuerte
                velocidadVertical += gravedadCaida * delta;
            }
            else
            {
                // Subiendo: gravedad normal
                velocidadVertical += gravedad * delta;
            }
        }

        // Aplicación de movimiento
        transform.position += new Vector3(
            velocidadActual * delta,
            velocidadVertical * delta,
            0
        );

        // Actualización de Estados para Animaciones
        estaCaminando = Mathf.Abs(velocidadActual) > 0.1f;
        estaSaltando = velocidadVertical > 0.1f;
        estaCayendo = velocidadVertical < -0.1f;
    }
}