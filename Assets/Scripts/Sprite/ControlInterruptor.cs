using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Clase encargada del comportamiento de un interruptor en el juego
public class ControlInterruptor : MonoBehaviour
{
    [Header("Referencia")]
    public ControlPlataforma plataforma; // Plataforma que se activa/desactiva con el interruptor

    [Header("Audio")]
    public AudioClip audioClip; // Sonido al activar el interruptor

    [Header("Estado")]
    private bool activado; // Estado actual del interruptor

    [Header("Componentes")]
    private BoxCollider2D col;     // Collider del interruptor
    private SpriteRenderer sprite; // Sprite visual del interruptor
    private Animator anim;         // Animación del interruptor

    void Start()
    {
        // Obtiene el collider y lo configura como trigger
        col = GetComponent<BoxCollider2D>();
        col.isTrigger = true;

        // Obtiene componentes visuales y de animación
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        // Estado inicial del interruptor
        activado = false;
        anim.SetBool("Activado", activado);
    }

    // Detecta cuando un jugador entra en el área del interruptor
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Solo jugadores pueden activarlo
        if (collision.CompareTag("Player1") || collision.CompareTag("Player2"))
        {
            // Cambia el estado del interruptor
            activado = !activado;

            // Reproduce sonido si existe
            if (audioClip != null)
                ControladorSonido.Sonido.EjecutarSonido(audioClip);

            // Actualiza animación según estado
            anim.SetBool("Activado", activado);

            // Activa o desactiva la plataforma asociada
            if (plataforma != null)
            {
                plataforma.Activar(activado);
            }

            // Debug para pruebas
            Debug.Log("Interruptor activado: " + activado);
        }
    }
}
