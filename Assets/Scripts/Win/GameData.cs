using System.Collections;

// Clase estática que almacena datos globales del juego
// Estos datos se mantienen entre escenas sin necesidad de un GameObject
public static class GameData
{
    // Vidas del jugador 1
    public static int vidasPlayer1;

    // Vidas del jugador 2
    public static int vidasPlayer2;

    // Tiempo restante de la partida
    public static int tiempoRestante;

    // Puntuación total acumulada
    public static int puntuacionTotal;
}