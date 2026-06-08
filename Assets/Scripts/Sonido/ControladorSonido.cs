using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Clase encargada de gestionar los sonidos del juego
public class ControladorSonido : MonoBehaviour
{
    // Instancia única (Singleton) para acceder desde cualquier script
    public static ControladorSonido Sonido;

    // Componente AudioSource encargado de reproducir sonidos
    private AudioSource audioSource;

    private void Awake()
    {
        // Implementación del Singleton
        if (Sonido == null)
        {
            Sonido = this;

            // No destruir al cambiar de escena
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Evita duplicados
            Destroy(gameObject);
        }

        // Obtiene el AudioSource del objeto
        audioSource = GetComponent<AudioSource>();
    }

    // Reproduce un sonido puntual (efecto)
    public void EjecutarSonido(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
