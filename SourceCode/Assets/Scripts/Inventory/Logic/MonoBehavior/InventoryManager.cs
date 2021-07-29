using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : SingleTon<InventoryManager>
{
    public class DragData
    {
        public SlotHolder originalHolder;

        public RectTransform originalParent;
    }

    //TODO:save data
    [Header("Inventory Data")]

    public InventoryData_SO inventoryTemplate;
    public InventoryData_SO actionTemplate;
    public InventoryData_SO equipmentTemplate;

    public InventoryData_SO  inventoryData;

    public InventoryData_SO actionData;

    public InventoryData_SO equipmentData;

    [Header("ContainerS")]

    public ContainerUI inventoryUI;

    public ContainerUI actionUI;

    public ContainerUI equipmentUI;

    [Header("Drag Canvas")]
    public Canvas dragCanvas;

    public DragData currentDrag;

    [Header("UI Panel")]

    public GameObject bagPanel;

    public GameObject statsPanel;

    bool isOpen = false;

    [Header("Stats Text")]
    public Text healthText;

    public Text attackText;


    [Header("Tooltip")]

    public ItemTooltip itemTooltip;

    protected override void Awake()
    {
        base.Awake();
        if (inventoryTemplate != null)
            inventoryData = Instantiate(inventoryTemplate);
        if (actionTemplate != null)
            actionData = Instantiate(actionTemplate);
        if (equipmentTemplate != null)
            equipmentData = Instantiate(equipmentTemplate);
    }

    private void Start()
    {
        loadData();
        inventoryUI.RefreshUI();
        actionUI.RefreshUI();
        equipmentUI.RefreshUI();

    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            isOpen = !isOpen;
            bagPanel.SetActive(isOpen);
            statsPanel.SetActive(isOpen);

        }
        UpdateStatsText(GameManager.Instance.playerStats.MaxHealth, GameManager.Instance.playerStats.attackData.minDamage
            , GameManager.Instance.playerStats.attackData.maxDamage);
    }

    public void SaveData()
    {
        SaveManager.Instance.Save(inventoryData, inventoryData.name);
        SaveManager.Instance.Save(actionData, actionData.name);
        SaveManager.Instance.Save(equipmentData, equipmentData.name);
    }
    public void loadData()
    {
        SaveManager.Instance.Load(inventoryData, inventoryData.name);
        SaveManager.Instance.Load(actionData, actionData.name);
        SaveManager.Instance.Load(equipmentData, equipmentData.name);
    }
    public void UpdateStatsText(int health, int min, int max)
    {
        healthText.text = health.ToString();
        attackText.text = min + " - " + max;
    }

    #region
    public bool CheckInInventoryUI(Vector3 position)
    {
        for(int i=0;i<inventoryUI.slotHolders.Length;++i)
        {
            RectTransform t = inventoryUI.slotHolders[i].transform as RectTransform;
            if(RectTransformUtility.RectangleContainsScreenPoint(t,position))
            {
                return true;
            }

        }
        return false;
    }
    public bool CheckInactionUI(Vector3 position)
    {
        for (int i = 0; i < actionUI.slotHolders.Length; ++i)
        {
            RectTransform t = actionUI.slotHolders[i].transform as RectTransform;
            if (RectTransformUtility.RectangleContainsScreenPoint(t, position))
            {
                return true;
            }

        }
        return false;
    }
    public bool CheckInequipmentUI(Vector3 position)
    {
        for (int i = 0; i < equipmentUI.slotHolders.Length; ++i)
        {
            RectTransform t = equipmentUI.slotHolders[i].transform as RectTransform;
            if (RectTransformUtility.RectangleContainsScreenPoint(t, position))
            {
                return true;
            }

        }
        return false;
    }
    #endregion

    #region 检测物品
    public void checkQuestItemInBag(string questItemName)
    {
        foreach (var item in inventoryData.items)
            {
                if(item.itemData!=null)
            {
                if(item.itemData.itemName==questItemName)
                {
                    QuestManager.Instance.updateProgress(item.itemData.itemName, item.amount);
                }
            }
            }
        foreach (var item in actionData.items)
        {
            if (item.itemData != null)
            {
                if (item.itemData.itemName == questItemName)
                {
                    QuestManager.Instance.updateProgress(item.itemData.itemName, item.amount);
                }
            }
        }
    }
    #endregion

    public InventoryItem QuestItemInBag(ItemData_SO questItem)
    {
        return inventoryData.items.Find(i => i.itemData == questItem);
    }
    public InventoryItem QuestItemInActionBar(ItemData_SO questItem)
    {
        return actionData.items.Find(i => i.itemData == questItem);
    }
}
