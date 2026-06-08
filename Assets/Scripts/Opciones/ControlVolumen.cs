using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Clase encargada de controlar el volumen del juego
public class ControlVolumen : MonoBehaviour
{
    [Header("UI")]
    public Slider slider;        // Slider de volumen
    public Image imagenMute;     // Icono que aparece cuando está en mute

    [Header("Datos")]
    public float sliderValue;    // Valor actual del volumen

    void Start()
    {
        // Carga el volumen guardado o usa 0.5 por defecto
        slider.value = PlayerPrefs.GetFloat("volumenAudio", 0.5f);

        // Aplica el volumen global del juego
        AudioListener.volume = slider.value;

        // Actualiza el icono de mute
        RevisarSiEstoyMute();
    }

    // Método llamado cuando cambia el slider
    public void ChangeSlider(float valor)
    {
        sliderValue = valor;

        // Guarda el valor en PlayerPrefs
        PlayerPrefs.SetFloat("volumenAudio", sliderValue);

        // Aplica el volumen al audio global
        AudioListener.volume = slider.value;

        // Actualiza el icono de mute
        RevisarSiEstoyMute();
    }

    // Comprueba si el volumen está en 0 para mostrar el icono de mute
    public void RevisarSiEstoyMute()
    {
        if (sliderValue == 0)
        {
            imagenMute.enabled = true;
        }
        else
        {
            imagenMute.enabled = false;
        }
    }
}
