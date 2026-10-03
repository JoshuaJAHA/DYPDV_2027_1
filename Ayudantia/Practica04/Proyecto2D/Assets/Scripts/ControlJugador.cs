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

    [Header("Efectos de Sonido (Práctica 4)")]
    public AudioClip sonidoSalto;
    public AudioClip sonidoPaso;

    private float tiempoAnterior;
    private Jugador jugador;
    private Animator anim;
    
    // CANALES DE AUDIO 
    private AudioSource audioPasos;
    private AudioSource audioEfectos;

    private void Awake()
    {
        jugador = GetComponent<Jugador>();
        anim = GetComponent<Animator>();

        // Buscamos si el jugador ya tiene AudioSources
        AudioSource[] audios = GetComponents<AudioSource>();

        if (audios.Length > 0) {
            audioPasos = audios[0];
        } else {
            audioPasos = gameObject.AddComponent<AudioSource>();
        }

        // Si no existe un segundo AudioSource para el salto, el script lo crea automáticamente
        if (audios.Length > 1) {
            audioEfectos = audios[1];
        } else {
            audioEfectos = gameObject.AddComponent<AudioSource>();
        }

        // Configuramos el canal de pasos para que sea un bucle 
        audioPasos.playOnAwake = false;
        audioPasos.loop = true; 
        if (sonidoPaso != null) audioPasos.clip = sonidoPaso;

        // Configuramos el canal de efectos (saltos) 
        audioEfectos.playOnAwake = false;
        audioEfectos.loop = false;
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
            velocidadActual += h * aceleracion * delta;
            velocidadActual = Mathf.Clamp(velocidadActual, -velocidadMax, velocidadMax);

            // Voltear el sprite
            if (h > 0)
                transform.localScale = new Vector3(1, 1, 1);
            else if (h < 0)
                transform.localScale = new Vector3(-1, 1, 1);
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

            // Reproducir sonido de salto en su canal independiente
            if (audioEfectos != null && sonidoSalto != null)
            {
                audioEfectos.PlayOneShot(sonidoSalto);
            }
        }

        // Gravedad Mejorada
        if (jugador.enSuelo)
        {
            velocidadVertical = 0f;
        }
        else
        {
            if (velocidadVertical < 0)
            {
                velocidadVertical += gravedadCaida * delta;
            }
            else
            {
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
        estaSaltando = !jugador.enSuelo && velocidadVertical > 0.1f;
        estaCayendo = !jugador.enSuelo && velocidadVertical < -0.1f;

        if (anim != null)
        {
            anim.SetBool("Caminando", estaCaminando);
            anim.SetBool("Saltando", estaSaltando);
            anim.SetBool("Cayendo", estaCayendo);
        }

        // AUDIO
        if (estaCaminando && jugador.enSuelo)
        {
            // Si está caminando y tocando el suelo, reproducimos el bucle de pasos
            if (!audioPasos.isPlaying && sonidoPaso != null)
            {
                audioPasos.Play();
            }
        }
        else
        {
            // Si deja de caminar, frena, salta o cae, el bucle de pasos se corta al instante
            if (audioPasos.isPlaying)
            {
                audioPasos.Stop();
            }
        }
    }
}