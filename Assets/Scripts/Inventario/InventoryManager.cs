using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryManager : MonoBehaviour
{
    // Instancia única (Singleton)
    public static InventoryManager instance;

    [Header("Inventario")]
    public List<Item> inventory = new List<Item>(); // Lista de items del jugador

    [Header("Debug")]
    public ItemData testItemData; // Item de prueba (solo para debugging)

    private void Awake()
    {
        // Implementación del Singleton
        if (instance == null)
        {
            instance = this;

            // No destruir al cambiar de escena
            DontDestroyOnLoad(gameObject);

            // Suscribirse al evento de carga de escena
            SceneManager.sceneLoaded += AlCargarEscena;
        }
        else
        {
            // Si ya existe una instancia, destruye duplicados
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        // Buena práctica: desuscribirse del evento para evitar errores
        if (instance == this)
        {
            SceneManager.sceneLoaded -= AlCargarEscena;
        }
    }

    // Se ejecuta automáticamente cada vez que se carga una escena
    private void AlCargarEscena(Scene escena, LoadSceneMode modo)
    {
        // Limpia el inventario al cambiar de escena
        if (inventory != null)
        {
            inventory.Clear();
        }
    }

    // Añadir item al inventario
    public void AddItem(ItemData a, int b)
    {
        // Buscar si el item ya existe en el inventario
        foreach (Item item in inventory)
        {
            if (item.itemData == a)
            {
                // Si existe, aumenta la cantidad
                item.itemQuantity += b;

                // Actualiza la UI del inventario en la escena actual
                InventoryUI uiActual = FindObjectOfType<InventoryUI>();
                if (uiActual != null) uiActual.RefreshInventoryUI();

                return;
            }
        }

        // Si no existe, lo añade como nuevo item
        inventory.Add(new Item { itemData = a, itemQuantity = b });

        // Actualiza la UI del inventario
        InventoryUI uiDeLaEscena = FindObjectOfType<InventoryUI>();
        if (uiDeLaEscena != null) uiDeLaEscena.RefreshInventoryUI();
    }
}
