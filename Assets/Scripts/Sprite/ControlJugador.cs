using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Control principal del jugador: movimiento, vida, inventario, puntos y estados
public class ControlJugador : MonoBehaviour
{
    // MOVIMIENTO
    [Header("Movimiento")]
    public int fuerzaSalto;
    public int velocidad;

    // VIDA
    [Header("Vida")]
    public int vidas;
    public int maxVidas = 3;

    // UI
    [Header("UI")]
    public Canvas canvas;

    // SONIDO
    [Header("Sonido")]
    public AudioClip saltoSonido;
    public AudioClip heridoSonido;
    public AudioClip muerteSonido;
    public AudioClip victoriaSonido;

    // TIEMPO Y PUNTOS
    private float tiempoDisponible;
    private float tiempoEmpleado;
    private float tiempoInicio;

    private int puntos;

    // ESTADO DEL JUGADOR
    private float entradaX;          // Input horizontal
    private bool vulnerable;         // Evita daño continuo
    private bool estaMuerto;         // Estado de muerte

    // COMPONENTES
    private Rigidbody2D fisicas;
    private SpriteRenderer sprite;
    private Animator animator;
    private ControlHUB hud;

    private string tagJugador;

    // VICTORIA GLOBAL
    private static bool player1Meta;
    private static bool player2Meta;
    private static bool cambiandoEscena;

    // HABILIDADES
    private bool escudoActivo;
    private bool dobleSaltoActivo;
    private bool yaUsadoDobleSalto;

    void Start()
    {
        // Obtener componentes principales del jugador
        fisicas = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponent<Animator>();

        sprite.flipX = false;

        // Configuración inicial del tiempo de partida
        tiempoDisponible = 180;
        tiempoInicio = Time.time;

        // Reset de estado global de victoria
        player1Meta = false;
        player2Meta = false;
        cambiandoEscena = false;

        // Estados iniciales
        estaMuerto = false;
        vulnerable = true;

        escudoActivo = false;
        dobleSaltoActivo = false;
        yaUsadoDobleSalto = false;

        tagJugador = gameObject.tag;

        puntos = 0;

        hud = canvas.GetComponent<ControlHUB>();

        // Inicializa UI de vidas según jugador
        if (tagJugador == "Player1")
            hud.setTextVidasPlayer1(vidas);
        else
            hud.setTextVidasPlayer2(vidas);

        hud.setTextPuntos(puntos);
        hud.setTextTiempo((int)tiempoDisponible);
    }

    void Update()
    {
        // Si el juego está pausado no se controla el jugador
        if (ControlMenuPausa.juegoPausado)
            return;

        // Control de salto e inventario según jugador
        if (tagJugador == "Player1")
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
                HandleJump();

            HandleItemInputPlayer1();
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.W))
                HandleJump();

            HandleItemInputPlayer2();
        }
    }

    void FixedUpdate()
    {
        if (ControlMenuPausa.juegoPausado)
            return;

        if (estaMuerto) return;

        // Movimiento horizontal según jugador
        if (tagJugador == "Player1")
        {
            entradaX = (Input.GetKey(KeyCode.RightArrow) ? 1 : 0) +
                       (Input.GetKey(KeyCode.LeftArrow) ? -1 : 0);
        }
        else
        {
            entradaX = (Input.GetKey(KeyCode.D) ? 1 : 0) +
                       (Input.GetKey(KeyCode.A) ? -1 : 0);
        }

        // Aplicar velocidad
        fisicas.velocity = new Vector2(entradaX * velocidad, fisicas.velocity.y);

        // Girar sprite según dirección
        if (fisicas.velocity.x < 0) sprite.flipX = true;
        else if (fisicas.velocity.x > 0) sprite.flipX = false;

        AnimarJugador();

        // Gestión del tiempo de partida
        tiempoEmpleado = Time.time - tiempoInicio;
        int tiempoRestante = (int)(tiempoDisponible - tiempoEmpleado);

        hud.setTextTiempo(tiempoRestante);

        // Guardar datos globales
        if (tagJugador == "Player1")
            GameData.vidasPlayer1 = vidas;
        else
            GameData.vidasPlayer2 = vidas;

        GameData.tiempoRestante = tiempoRestante;

        // Game Over por tiempo
        if (tiempoRestante <= 0)
            SceneManager.LoadScene("GameOver");
    }

    // SALTO
    void HandleJump()
    {
        // Salto normal desde el suelo
        if (tocandoSuelo())
        {
            fisicas.velocity = new Vector2(fisicas.velocity.x, 0);
            fisicas.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);

            if (saltoSonido != null)
                ControladorSonido.Sonido.EjecutarSonido(saltoSonido);

            yaUsadoDobleSalto = false;
        }
        // Doble salto
        else if (dobleSaltoActivo)
        {
            fisicas.velocity = new Vector2(fisicas.velocity.x, 0);
            fisicas.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);

            dobleSaltoActivo = false;
            yaUsadoDobleSalto = true;

            animator.SetTrigger("DobleSalto");
        }
    }

    // INVENTARIO INPUT
    void HandleItemInputPlayer1()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) UsarItemInventario(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) UsarItemInventario(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) UsarItemInventario(2);
    }

    void HandleItemInputPlayer2()
    {
        if (Input.GetKeyDown(KeyCode.Alpha8)) UsarItemInventario(0);
        if (Input.GetKeyDown(KeyCode.Alpha9)) UsarItemInventario(1);
        if (Input.GetKeyDown(KeyCode.Alpha0)) UsarItemInventario(2);
    }

    // USO DE ITEMS
    private void UsarItemInventario(int slot)
    {
        if (InventoryManager.instance == null) return;
        if (slot >= InventoryManager.instance.inventory.Count) return;

        Item item = InventoryManager.instance.inventory[slot];

        switch (item.itemData.itemPower)
        {
            case ItemData.ItemPower.vida:
                if (vidas >= maxVidas) return;

                vidas = Mathf.Min(vidas + 1, maxVidas);
                break;

            case ItemData.ItemPower.escudo:
                if (escudoActivo) return;
                StartCoroutine(EscudoCoroutine());
                break;

            case ItemData.ItemPower.salto:
                if (dobleSaltoActivo) return;
                dobleSaltoActivo = true;
                yaUsadoDobleSalto = false;
                break;
        }

        // Reduce item usado
        item.itemQuantity--;

        if (item.itemQuantity <= 0)
            InventoryManager.instance.inventory.RemoveAt(slot);

        InventoryUI ui = FindObjectOfType<InventoryUI>();
        if (ui != null) ui.RefreshInventoryUI();
    }

    // ESCUDO
    IEnumerator EscudoCoroutine()
    {
        escudoActivo = true;
        yield return new WaitForSeconds(3f);
        escudoActivo = false;
    }

    // DAÑO
    public void QuitarVida(int damage)
    {
        if (escudoActivo) return;
        if (!vulnerable) return;

        vidas -= damage;
        if (vidas < 0) vidas = 0;

        vulnerable = false;

        if (vidas > 0)
        {
            animator.SetTrigger("Herido");

            if (heridoSonido != null)
                ControladorSonido.Sonido.EjecutarSonido(heridoSonido);

            Invoke("HacerVulnerable", 1f);
        }
        else
        {
            estaMuerto = true;
            animator.SetTrigger("Morir");

            if (muerteSonido != null)
                ControladorSonido.Sonido.EjecutarSonido(muerteSonido);

            fisicas.velocity = Vector2.zero;
            fisicas.constraints = RigidbodyConstraints2D.FreezeAll;

            StartCoroutine(MorirGameOver());
        }

        // Actualiza HUD
        if (tagJugador == "Player1")
            hud.setTextVidasPlayer1(vidas);
        else
            hud.setTextVidasPlayer2(vidas);
    }

    public void HacerVulnerable()
    {
        vulnerable = true;
    }

    IEnumerator MorirGameOver()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("GameOver");
    }

    // VICTORIA
    IEnumerator Victoria()
    {
        cambiandoEscena = true;

        GameData.puntuacionTotal =
            (GameData.vidasPlayer1 * 10) +
            (GameData.vidasPlayer2 * 10) +
            (GameData.tiempoRestante * 2);

        if (ControladorSonido.Sonido != null)
            ControladorSonido.Sonido.EjecutarSonido(victoriaSonido);

        yield return new WaitForSeconds(saltoSonido.length);

        SceneManager.LoadScene("Win");
    }

    // TRIGGERS
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("End"))
        {
            if (tagJugador == "Player1") player1Meta = true;
            if (tagJugador == "Player2") player2Meta = true;

            if (player1Meta && player2Meta && !cambiandoEscena)
                StartCoroutine(Victoria());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("End")) return;

        if (tagJugador == "Player1") player1Meta = false;
        if (tagJugador == "Player2") player2Meta = false;
    }

    // ANIMACIONES
    void AnimarJugador()
    {
        bool enSuelo = tocandoSuelo();

        animator.SetBool("Correr", entradaX != 0 && enSuelo);
        animator.SetBool("Saltar", fisicas.velocity.y > 0 && !enSuelo);
        animator.SetBool("Caer", fisicas.velocity.y < 0 && !enSuelo);
    }

    bool tocandoSuelo()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position + Vector3.down * 1.2f,
            Vector2.down,
            0.2f
        );

        return hit.collider != null;
    }

    // PUNTOS
    public void IncrementarPuntos(int valor)
    {
        puntos += valor;
        hud.setTextPuntos(puntos);
    }
}