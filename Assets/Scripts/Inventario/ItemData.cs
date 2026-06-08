using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Permite crear nuevos items desde el menú del Unity Editor
[CreateAssetMenu(fileName = "ItemData", menuName = "Inventory/New Item")]

// ScriptableObject que almacena los datos base de un item del inventario
public class ItemData : ScriptableObject
{
    public enum ItemPower
    {
        escudo,
        vida, 
        salto
    }

    public string itemName;
    public Sprite itemIcon;
    public string itemDescripcion;
    public ItemPower itemPower;

}
