using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Clase encargada de controlar la pantalla completa y la resolución del juego
public class ControlPantallaCompleta : MonoBehaviour
{
    [Header("UI")]
    public Toggle toggle;                 // Toggle de pantalla completa
    public TMP_Dropdown resolucionesDropDown; // Dropdown de resoluciones

    private Resolution[] resoluciones;    // Array de resoluciones disponibles

    void Start()
    {
        // Inicializa el estado del toggle según si el juego está en pantalla completa
        if (Screen.fullScreen)
        {
            toggle.isOn = true;
        }
        else
        {
            toggle.isOn = false;
        }

        // Carga y configura las resoluciones disponibles
        RevisarResolucion();
    }

    void Update()
    {
        // (Actualmente no se usa)
    }

    // Activa o desactiva pantalla completa
    public void ActiveFULLS(bool pantallaCompleta)
    {
        Screen.fullScreen = pantallaCompleta;
    }

    // Obtiene y filtra las resoluciones disponibles del sistema
    public void RevisarResolucion()
    {
        resoluciones = Screen.resolutions;

        resolucionesDropDown.ClearOptions();

        List<string> opciones = new List<string>();          // Texto de opciones para el dropdown
        List<Resolution> resolucionesFiltradas = new List<Resolution>(); // Resoluciones sin duplicados

        HashSet<string> resolucionesUnicas = new HashSet<string>(); // Evita duplicados

        int resolucionActual = 0;

        // Recorre todas las resoluciones del sistema
        foreach (Resolution r in resoluciones)
        {
            string resolucionString = r.width + " x " + r.height;

            // Evita agregar resoluciones repetidas
            if (!resolucionesUnicas.Contains(resolucionString))
            {
                resolucionesUnicas.Add(resolucionString);

                opciones.Add(resolucionString);
                resolucionesFiltradas.Add(r);

                // Guarda la resolución actual del sistema
                if (r.width == Screen.currentResolution.width &&
                    r.height == Screen.currentResolution.height)
                {
                    resolucionActual = resolucionesFiltradas.Count - 1;
                }
            }
        }

        // Guarda el array filtrado
        resoluciones = resolucionesFiltradas.ToArray();

        // Actualiza el dropdown
        resolucionesDropDown.AddOptions(opciones);
        resolucionesDropDown.value = resolucionActual;
        resolucionesDropDown.RefreshShownValue();
    }

    // Cambia la resolución del juego
    public void CambiarResolucion(int indiceResolucion)
    {
        // Guarda la selección del usuario
        PlayerPrefs.SetInt("numeroResolucion", resolucionesDropDown.value);

        // Aplica la nueva resolución
        Resolution resolution = resoluciones[indiceResolucion];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
