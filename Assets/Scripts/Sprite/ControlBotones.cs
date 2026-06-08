using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Clase encargada de controlar los botones del menú
public class ControlBotones : MonoBehaviour
{
    // Método para iniciar el juego
    public void Jugar()
    {
        // Restablece la velocidad del juego por si estaba en pausa
        Time.timeScale = 1f;

        // Carga la escena del nivel principal del juego
        SceneManager.LoadScene("Nivel");
    }

    // Método para ir a la pantalla de créditos
    public void Creditos()
    {
        // Carga la escena de créditos
        SceneManager.LoadScene("Creditos");
    }

    // Método para salir del juego
    public void Salir()
    {
        // Muestra un mensaje en la consola (solo funciona en el editor)
        Debug.Log("Salir...");

        // Cierra la aplicación (solo funciona en build final)
        Application.Quit();
    }

    // Método para volver al menú principal
    public void Volver()
    {
        // Carga la escena del menú principal
        SceneManager.LoadScene("Menu");
    }
}
