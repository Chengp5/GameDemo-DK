using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : SingleTon<QuestUI>
{
    [Header("Elements")]
    public GameObject questPanel;
    public ItemTooltip tooltip;
    bool isOpened;

    [Header("QuestName")]
    public RectTransform questlistTransform;
    public QuestNameButton questNameButton;

    [Header("Text Content")]
    public Text questContentText;

    [Header("Requirement")]
    public RectTransform requireTransform;
    public QuestRequirement requirement;

    [Header("Reward Panel")]
    public RectTransform rewardTransform;
    public ItemUI rewardUI;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            isOpened = !isOpened;
            questPanel.SetActive(isOpened);
            questContentText.text = string.Empty;
            SetupQuestList();
            if (!isOpened)
                tooltip.gameObject.SetActive(false);
        }
    }
    public void SetupQuestList()
    {
        foreach (Transform item in questlistTransform)
        {
            Destroy(item.gameObject);
        }
        foreach (Transform item in rewardTransform)
        {
            Destroy(item.gameObject);
        }
        foreach (Transform item in requireTransform)
        {
            Destroy(item.gameObject);
        }

        foreach (var task in QuestManager.Instance.tasks)
        {
            var newTask = Instantiate(questNameButton, questlistTransform);
            newTask.SetupNameButton(task.questData);
            newTask.questContentText = questContentText;
        }
    }

        public void SetupRequireList(QuestData_SO questData)
        {
            foreach(Transform item in requireTransform)
            {
                Destroy(item.gameObject);
            }
            foreach(var require in questData.questRequires)
            {
                var q = Instantiate(requirement, requireTransform);
            if (questData.isFnished)
                q.setupRequirement(require.name, true);
            else
                q.SetupRequirement(require.name, require.requireAmount, require.currentAmount);            
            }
        }
    public void SetupRewardItem(List<InventoryItem> items)
    {
        foreach(Transform t in rewardTransform)
        {
            Destroy(t.gameObject);
        }
        foreach (var i in items)
        {
            var item = Instantiate(rewardUI, rewardTransform);
            item.SetupItemUI(i.itemData, i.amount);
        }
    }
}
