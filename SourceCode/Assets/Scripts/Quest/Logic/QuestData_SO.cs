using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[CreateAssetMenu(fileName ="new Quest",menuName ="Quest/Quest Data")]
public class QuestData_SO : ScriptableObject
{
    
    [System.Serializable]
    public class QuestRequire
    {
        public string name;
        public int requireAmount;
        public int currentAmount;
    }
    public string questName;
    [TextArea]
    public string description;

    public bool isStarted;
    public bool isCompleted;
    public bool isFnished;

    public List<QuestRequire> questRequires = new List<QuestRequire>();
    public List<InventoryItem> rewards = new List<InventoryItem>();
    public void checkQuestProgress()
    {
        var finishRequires = questRequires.Where(r => r.requireAmount <= r.currentAmount);
        isCompleted = finishRequires.Count() == questRequires.Count;
    }

    public List<string> requireTargetName()
    {
        List<string> targetNameList = new List<string>();
        foreach(var requie in questRequires)
        {
            targetNameList.Add(requie.name);
        }
        return targetNameList;
    }    
    public void GiveRewards()
    {
        foreach(var reward in rewards)
        {
            if(reward.amount<0)
            {
                int requireCount = Mathf.Abs(reward.amount); 
                if(InventoryManager.Instance.QuestItemInBag(reward.itemData)!=null)
                {
                    if(InventoryManager.Instance.QuestItemInBag(reward.itemData).amount<=requireCount)
                    {
                        requireCount -= InventoryManager.Instance.QuestItemInBag(reward.itemData).amount;
                        InventoryManager.Instance.QuestItemInBag(reward.itemData).amount = 0;
                        if(InventoryManager.Instance.QuestItemInActionBar(reward.itemData)!=null)
                        {
                            InventoryManager.Instance.QuestItemInActionBar(reward.itemData).amount -= requireCount;  
                        }
                    }
                    else
                        InventoryManager.Instance.QuestItemInBag(reward.itemData).amount-=requireCount;


                }
                else
                {
                    InventoryManager.Instance.QuestItemInActionBar(reward.itemData).amount -= requireCount;
                }
            }
            else
            {
                InventoryManager.Instance.inventoryData.AddItem(reward.itemData, reward.amount);
            }
            InventoryManager.Instance.inventoryUI.RefreshUI();
            InventoryManager.Instance.actionUI.RefreshUI();
        }
    }
}
