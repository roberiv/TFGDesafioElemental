using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Clase encargada del movimiento de plataformas activables
public class ControlPlataforma : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 2f;     // Velocidad de la plataforma
    public Vector3 posicionFin;      // Punto final del recorrido

    [Header("Estado interno")]
    private Vector3 posicionInicio;  // Punto inicial
    private bool activa;             // Indica si la plataforma está activa
    private bool yendoAFin;          // Dirección del movimiento

    [Header("Componentes")]
    private Animator animator;       // Animación de la plataforma

    void Start()
    {
        // Guarda la posición inicial
        posicionInicio = transform.position;

        // Empieza yendo hacia el punto final
        yendoAFin = true;

        // Obtiene el animator si existe
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Activa animación si existe
        if (animator != null)
        {
            animator.SetBool("Moverse", activa);
        }

        // Si está desactivada, no se mueve
        if (!activa) return;

        // Determina el objetivo actual (ida o vuelta)
        Vector3 objetivo = yendoAFin ? posicionFin : posicionInicio;

        // Mueve la plataforma hacia el objetivo
        transform.position = Vector3.MoveTowards(
            transform.position,
            objetivo,
            velocidad * Time.deltaTime
        );

        // Cambia dirección cuando llega al punto
        if (Vector3.Distance(transform.position, objetivo) < 0.01f)
        {
            yendoAFin = !yendoAFin;
        }
    }

    // Activa o desactiva la plataforma
    public void Activar(bool estado)
    {
        activa = estado;
    }
}