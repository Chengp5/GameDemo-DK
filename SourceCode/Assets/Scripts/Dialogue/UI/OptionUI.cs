using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
public class OptionUI : MonoBehaviour
{
    public Text optionText;

    private Button thisButton;
    private DialoguePiece currentPiece;

    private bool takeQuest;
    private string nextPieceID;
    private void Awake()
    {
        thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(OnOptionClicked);
    }
    public void UpdateOption(DialoguePiece piece, DialogueOption opton)
    {
        currentPiece = piece;
        optionText.text = opton.text;
        nextPieceID = opton.targetID;
        takeQuest = opton.takeQuest;
    }

    public void OnOptionClicked()
    {
        if(currentPiece.quest!=null)
        {
            var newTask = new QuestManager.QuestTask
            {
                questData = Instantiate(currentPiece.quest)
            };

            if(takeQuest)
            {
             //TODO 添加到列表
                if(QuestManager.Instance.haveQuest(newTask.questData))
                {
                    if(QuestManager.Instance.getTask(newTask.questData).IsCompleted)
                    {
                        newTask.questData.GiveRewards();
                        QuestManager.Instance.getTask(newTask.questData).IsFInished = true;
                    }
                }
                else
                {
                    QuestManager.Instance.tasks.Add(newTask);
                    QuestManager.Instance.getTask(newTask.questData).IsStarted=true;
               
                    foreach(var requireItem in newTask.questData.requireTargetName())
                    {
                        InventoryManager.Instance.checkQuestItemInBag(requireItem);
                    }
                
                }
            }
         }

        if(nextPieceID=="")
        {
            DialogueUI.Instance.dialoguePanel.SetActive(false);
            return;
        }
        else
        {
            DialogueUI.Instance.UpdateMainDialogue(DialogueUI.Instance.currentData.dialogueIndex[nextPieceID]);
        }
    }
}
