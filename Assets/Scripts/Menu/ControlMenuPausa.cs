using UnityEngine.SceneManagement;
using UnityEngine;

// Clase encargada de controlar el menú de pausa del juego
public class ControlMenuPausa : MonoBehaviour
{
    // Estado global que indica si el juego está pausado
    public static bool juegoPausado = false;

    [Header("UI de Pausa")]
    public GameObject pauseMenu;     // Panel del menú de pausa
    public GameObject pauseButton;   // Botón de pausa en pantalla

    private void Update()
    {
        // Si el inventario está abierto, no permitir pausar el juego
        if (InventoryUI.inventarioAbierto)
            return;

        // Detecta la tecla Escape para pausar/reanudar
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (juegoPausado)
                ResumeGame();
            else
                PauseGame();
        }
    }

    // Activa el estado de pausa
    public void PauseGame()
    {
        juegoPausado = true;

        Time.timeScale = 0f;          // Detiene el tiempo del juego
        pauseButton.SetActive(false); // Oculta botón de pausa
        pauseMenu.SetActive(true);    // Muestra menú de pausa
    }

    // Reanuda el juego
    public void ResumeGame()
    {
        juegoPausado = false;

        Time.timeScale = 1f;           // Reanuda el tiempo
        pauseButton.SetActive(true);   // Muestra botón de pausa
        pauseMenu.SetActive(false);    // Oculta menú de pausa
    }

    // Reinicia la escena actual
    public void RestartGame()
    {
        juegoPausado = false;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        Time.timeScale = 1f;
    }

    // Vuelve al menú principal
    public void MenuPrincipal()
    {
        juegoPausado = false;

        SceneManager.LoadScene("Menu");
    }
}