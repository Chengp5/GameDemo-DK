using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp: MonoBehaviour
{
    // Start is called before the first frame update

    public ItemData_SO itemData;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            //TODO:添加到背包
            InventoryManager.Instance.inventoryData.AddItem(itemData, itemData.itemAmout);
            InventoryManager.Instance.inventoryUI.RefreshUI();
            //GameManager.Instance.playerStats.EquipWeapon(itemData);


            QuestManager.Instance.updateProgress(itemData.itemName, itemData.itemAmout);
            Destroy(gameObject);
            

        }
    }
}
