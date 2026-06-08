using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Clase encargada de controlar la interfaz (HUD) del juego
public class ControlHUB : MonoBehaviour
{
    // Referencias a los textos del HUD en pantalla
    [Header("HUD")]
    public TextMeshProUGUI textVidasPlayer1; // Texto de vidas del jugador 1
    public TextMeshProUGUI textVidasPlayer2; // Texto de vidas del jugador 2
    public TextMeshProUGUI textTiempo;       // Texto del tiempo de partida
    public TextMeshProUGUI textObjetos;      // Texto de puntuación u objetos recogidos


    // Actualiza el texto de vidas del Player 1
    public void setTextVidasPlayer1(int vidas)
    {
        textVidasPlayer1.text = " " + vidas;
    }

    // Actualiza el texto de vidas del Player 2
    public void setTextVidasPlayer2(int vidas)
    {
        textVidasPlayer2.text = " " + vidas;
    }

    // Actualiza el tiempo mostrado en pantalla
    public void setTextTiempo(int tiempo)
    {
        textTiempo.text = " " + tiempo;
    }

    // Actualiza la puntuación u objetos recogidos
    public void setTextPuntos(int puntuacion)
    {
        textObjetos.text = " " + puntuacion;
    }
}
