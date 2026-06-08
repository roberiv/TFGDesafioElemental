using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Clase encargada del comportamiento de las frutas en el juego
public class ControlFrutas : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip audioClip; // Sonido al recoger la fruta

    [Header("Valor de la fruta")]
    private int valor; // Puntos que da la fruta

    [Header("Componentes")]
    private SpriteRenderer sprite;         // Referencia al sprite
    private CapsuleCollider2D collider2D;   // Collider de la fruta
    private Animator animator;             // Animación de la fruta

    [Header("Estado")]
    private bool recogida; // Evita que se recoja más de una vez

    void Start()
    {
        // Obtiene el collider y lo configura como trigger
        collider2D = GetComponent<CapsuleCollider2D>();
        collider2D.isTrigger = true;

        // Obtiene componentes necesarios
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        // Asigna el valor según el tipo de fruta
        CrearFruta();
    }

    // Detecta colisión con jugadores
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (recogida)
            return;

        // Verifica si es un jugador
        if (collision.CompareTag("Player1") || collision.CompareTag("Player2"))
        {
            recogida = true;

            // Reproduce sonido si existe
            if (audioClip != null)
                ControladorSonido.Sonido.EjecutarSonido(audioClip);

            ControlJugador jugador = collision.GetComponent<ControlJugador>();

            if (jugador != null)
            {
                // Suma puntos al jugador
                jugador.IncrementarPuntos(valor);

                // Desactiva el collider para evitar más colisiones
                collider2D.enabled = false;

                // Activa animación de recogida
                animator.SetTrigger("Obtener");

                // Destruye el objeto tras la animación
                StartCoroutine(RecogerFruta());
            }
        }
    }

    // Corrutina para destruir la fruta después de la animación
    private IEnumerator RecogerFruta()
    {
        yield return new WaitForSeconds(0.4f);
        Destroy(gameObject);
    }

    // Asigna el valor de la fruta según su tag
    private void CrearFruta()
    {
        if (gameObject.CompareTag("Kiwi"))
        {
            valor = 1;
        }
        else if (gameObject.CompareTag("Melon"))
        {
            valor = 5;
        }
        else if (gameObject.CompareTag("Oranje"))
        {
            valor = 10;
        }
    }
}
