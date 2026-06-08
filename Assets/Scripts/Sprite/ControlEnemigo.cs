using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Clase encargada del comportamiento del enemigo
public class ControlEnemigo : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad;        // Velocidad de movimiento del enemigo
    public Vector3 posicionFin;    // Punto final del recorrido
    public int damage;             // Daño que inflige al jugador

    [Header("Estado interno")]
    private Vector3 posicionInicio;   // Punto inicial del recorrido
    private bool moviendoAFin;        // Indica dirección del movimiento
    private SpriteRenderer sprite;    // Referencia al sprite del enemigo

    void Start()
    {
        // Guarda la posición inicial del enemigo
        posicionInicio = transform.position;

        // Empieza moviéndose hacia el punto final
        moviendoAFin = true;

        // Obtiene el SpriteRenderer del enemigo (o hijo)
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        // Llama al método de movimiento en cada frame
        MoverEnemigo();
    }

    // Controla el movimiento de ida y vuelta entre dos puntos
    private void MoverEnemigo()
    {
        // Si se está moviendo hacia el punto final
        if (moviendoAFin)
        {
            // Mueve el enemigo hacia posicionFin
            transform.position = Vector3.MoveTowards(
                transform.position,
                posicionFin,
                velocidad * Time.deltaTime
            );

            // Si llega al destino, cambia dirección
            if (transform.position == posicionFin)
            {
                moviendoAFin = false;
            }
        }
        else
        {
            // Mueve el enemigo hacia la posición inicial
            transform.position = Vector3.MoveTowards(
                transform.position,
                posicionInicio,
                velocidad * Time.deltaTime
            );

            // Si llega al origen, cambia dirección
            if (transform.position == posicionInicio)
            {
                moviendoAFin = true;
            }
        }
    }

    // Detecta colisión con jugadores
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si colisiona con Player1 o Player2
        if (collision.gameObject.CompareTag("Player1") || collision.gameObject.CompareTag("Player2"))
        {
            // Aplica daño al jugador
            collision.gameObject.GetComponent<ControlJugador>().QuitarVida(damage);

            // Mensaje de debug
            Debug.Log("Daño a: " + collision.gameObject.tag);
        }
    }
}
