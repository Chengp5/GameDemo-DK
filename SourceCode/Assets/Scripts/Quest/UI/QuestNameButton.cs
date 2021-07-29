using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class QuestNameButton : MonoBehaviour
{

    public Text questNameText;

    public QuestData_SO currentData;
    public Text questContentText;
    
    public void SetupNameButton(QuestData_SO questData)
    {
        currentData = questData;
        if (questData.isCompleted)
        {
            questNameText.text = questData.questName + "(Íê³É)";
        }
        else
            questNameText.text = questData.questName;
    }
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(UpdateQeustContent);
    }

    private void UpdateQeustContent()
    {
        questContentText.text = currentData.description;
        QuestUI.Instance.SetupRequireList(currentData);
      
        QuestUI.Instance.SetupRewardItem(currentData.rewards) ;
    }
   
}
