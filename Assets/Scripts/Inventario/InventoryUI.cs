using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Clase encargada de controlar la interfaz del inventario
public class InventoryUI : MonoBehaviour
{
    // Estado global: indica si el inventario está abierto
    public static bool inventarioAbierto = false;

    [Header("Paneles de Inventario")]
    public GameObject inventoryPanel1; // Inventario principal (se abre con tecla I)
    public GameObject inventoryPanel2; // Segundo inventario (estado alternativo)

    [Header("Prefabs de Slots Diferentes")]
    public GameObject itemSlotPrefab1; // Prefab para el inventario 1
    public GameObject itemSlotPrefab2; // Prefab para el inventario 2

    [Header("Configuración de Contenedores")]
    public Transform containerInventory1; // Contenedor de slots del inventario 1
    public Transform containerInventory2; // Contenedor de slots del inventario 2


    private void Start()
    {
        // Asigna referencias dinámicamente en la escena
        AsignarReferencias();

        // Estado inicial de los paneles
        if (inventoryPanel1 != null) inventoryPanel1.SetActive(false);
        if (inventoryPanel2 != null) inventoryPanel2.SetActive(true);

        // Si el inventario existe, lo refresca en UI
        if (InventoryManager.instance != null)
        {
            RefreshInventoryUI();
        }
    }

    private void Update()
    {
        // Bloquea el inventario si el menú de pausa está activo
        if (ControlMenuPausa.juegoPausado)
            return;

        // Abrir/cerrar inventario con tecla I
        if (Input.GetKeyDown(KeyCode.I))
        {
            // Reasigna referencias por seguridad al cambiar de escena
            AsignarReferencias();
            ToggleInventarios();
        }
    }

    // Busca referencias en la escena actual por nombre
    private void AsignarReferencias()
    {
        if (inventoryPanel1 == null)
            inventoryPanel1 = GameObject.Find("PanelInventario");

        if (inventoryPanel2 == null)
            inventoryPanel2 = GameObject.Find("PanelInventario2");

        if (containerInventory1 == null)
        {
            GameObject container1Obj = GameObject.Find("PanelInventarioContenedor");
            if (container1Obj != null)
                containerInventory1 = container1Obj.transform;
        }

        if (containerInventory2 == null)
        {
            GameObject container2Obj = GameObject.Find("PanelInventarioContenedor2");
            if (container2Obj != null)
                containerInventory2 = container2Obj.transform;
        }
    }

    // Abre o cierra los inventarios
    public void ToggleInventarios()
    {
        if (inventoryPanel1 == null)
        {
            Debug.LogWarning("¡Falta asignar Inventory Panel 1!");
            return;
        }

        // Estado contrario del panel principal
        bool estaAbiertoInv1 = !inventoryPanel1.activeSelf;

        inventarioAbierto = estaAbiertoInv1;

        // Activa/desactiva panel principal
        inventoryPanel1.SetActive(estaAbiertoInv1);

        // Activa/desactiva panel secundario
        if (inventoryPanel2 != null)
            inventoryPanel2.SetActive(!estaAbiertoInv1);

        // Pausa el juego si el inventario está abierto
        if (estaAbiertoInv1)
        {
            Time.timeScale = 0;
            RefreshInventoryUI();
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    // Refresca toda la interfaz del inventario
    public void RefreshInventoryUI()
    {
        // Limpia slots anteriores
        LimpiarContenedor(containerInventory1);
        LimpiarContenedor(containerInventory2);

        // Recorre el inventario del manager
        foreach (Item item in InventoryManager.instance.inventory)
        {
            // Crea slots en inventario 1
            if (containerInventory1 != null && itemSlotPrefab1 != null)
            {
                CrearSlot(item, itemSlotPrefab1, containerInventory1);
            }

            // Crea slots en inventario 2
            if (containerInventory2 != null && itemSlotPrefab2 != null)
            {
                CrearSlot(item, itemSlotPrefab2, containerInventory2);
            }
        }
    }

    // Crea un slot visual del item en el contenedor indicado
    private void CrearSlot(Item item, GameObject prefabAInstanciar, Transform contenedor)
    {
        GameObject newItemSlot = Instantiate(prefabAInstanciar, contenedor);

        ItemSlopUI itemSlotUI = newItemSlot.GetComponent<ItemSlopUI>();

        if (itemSlotUI != null)
        {
            // Asigna datos visuales del item
            itemSlotUI.itemIconImage.sprite = item.itemData.itemIcon;
            itemSlotUI.itemNameText.text = item.itemData.itemName;
            itemSlotUI.itemQuantity.text = "x" + item.itemQuantity;
        }
    }

    // Limpia todos los objetos dentro de un contenedor
    private void LimpiarContenedor(Transform contenedor)
    {
        if (contenedor == null) return;

        foreach (Transform t in contenedor)
        {
            Destroy(t.gameObject);
        }
    }
}