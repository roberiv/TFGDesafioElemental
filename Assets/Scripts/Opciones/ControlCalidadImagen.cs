using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Clase encargada de controlar la calidad gráfica del juego
public class ControlCalidadImagen : MonoBehaviour
{
    [Header("UI")]
    public TMP_Dropdown dropdown; // Dropdown para seleccionar la calidad

    [Header("Datos")]
    public int calidad; // Nivel de calidad actual seleccionado

    void Start()
    {
        // Carga la calidad guardada en PlayerPrefs (por defecto 3 si no existe)
        calidad = PlayerPrefs.GetInt("numeroDeCalidad", 3);

        // Actualiza el dropdown con el valor guardado
        dropdown.value = calidad;

        // Aplica la calidad al iniciar
        AjustarCalidad();
    }

    void Update()
    {
        // Actualmente no se utiliza
    }

    // Cambia la calidad gráfica del juego
    public void AjustarCalidad()
    {
        // Aplica la calidad seleccionada en el dropdown
        QualitySettings.SetQualityLevel(dropdown.value);

        // Guarda la configuración del usuario
        PlayerPrefs.SetInt("numeroDeCalidad", dropdown.value);

        // Actualiza la variable interna
        calidad = dropdown.value;
    }
}
