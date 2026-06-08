using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Clase encargada de mostrar la pantalla de victoria
public class ControlWin : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI TextoWin; // Texto donde se muestra la puntuación final

    void Start()
    {
        // Muestra la puntuación total al iniciar la escena de victoria
        setTextPuntuacion(GameData.puntuacionTotal);
    }

    // Actualiza el texto de puntuación en pantalla
    public void setTextPuntuacion(int puntuacion)
    {
        TextoWin.text = puntuacion.ToString();
    }
}
