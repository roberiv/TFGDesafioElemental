using System.Collections;
using UnityEngine;

public class ControlFrutasInventario : MonoBehaviour
{
    [Header("Datos del ítem")]
    public ItemData itemData;        // Información del ítem (ScriptableObject)
    public int quantity = 1;         // Cantidad que se añade al inventario

    [Header("Audio")]
    public AudioClip audioClip;      // Sonido al recoger la fruta

    // Componentes internos
    private Animator animator;
    private Collider2D col;

    // Control para evitar doble recogida
    private bool recogida;



    private void Start()
    {
        // Obtiene referencias de componentes del objeto
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
    }

    // Se ejecuta cuando otro collider entra en el trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Si ya fue recogida, no hacer nada
        if (recogida) return;

        // Verifica si el objeto que entra es un jugador
        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            recogida = true; // Marca como recogida para evitar duplicados

            // Reproduce sonido si existe
            if (audioClip != null)
                ControladorSonido.Sonido.EjecutarSonido(audioClip);

            // Añade el ítem al inventario
            InventoryManager.instance.AddItem(itemData, quantity);

            // Busca la UI del inventario y la actualiza
            InventoryUI ui = FindObjectOfType<InventoryUI>();
            if (ui != null)
                ui.RefreshInventoryUI();

            // Desactiva el collider para evitar más interacciones
            col.enabled = false;

            // Activa la animación de recogida
            animator.SetTrigger("Obtener");

            // Inicia la corrutina para destruir el objeto después de un tiempo
            StartCoroutine(RecogerFruta());
        }
    }

    // Corrutina que destruye el objeto después de un pequeño delay
    private IEnumerator RecogerFruta()
    {
        yield return new WaitForSeconds(0.4f);
        Destroy(gameObject);
    }
}