using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class QuestManager : SingleTon<QuestManager>
{
    [System.Serializable]
    public class QuestTask
    {
        public QuestData_SO questData;
        public bool IsStarted
        {
            get
            {
                return questData.isStarted;
            }
            set
            {
                questData.isStarted = value;
            }
        }
        public bool IsCompleted
        {
            get
            {
                return questData.isCompleted;
            }
            set
            {
                questData.isCompleted = value;
            }
        }
        public bool IsFInished
        {
            get
            {
                return questData.isFnished;
            }
            set
            {
                questData.isFnished = value;
            }
        }


    }

    public List<QuestTask> tasks = new List<QuestTask>();

    private void Start()
    {
        loadQuestManager();
    }
    public void loadQuestManager()
    {
        var questCount = PlayerPrefs.GetInt("QuestCount");
        for(int i=0;i<questCount;++i)
        {
            var newQeust = ScriptableObject.CreateInstance<QuestData_SO>();
            SaveManager.Instance.Load(newQeust, "task" + i);
            tasks.Add(new QuestTask { questData = newQeust });
        }
    }
    public void SaveQuestManager()
    {
        PlayerPrefs.SetInt("QuestCount", tasks.Count);
        for(int i=0;i<tasks.Count;++i)
        {
            SaveManager.Instance.Save(tasks[i].questData, "task" + i);
        }
    }

    public bool haveQuest(QuestData_SO data)
    {
        if (data != null)
        {
            return tasks.Any(q => q.questData.questName == data.questName);
        }
        else
            return false;
    }
    public void updateProgress(string name, int amount)
    {
        foreach(var task in tasks)
        {
            if (task.IsCompleted) continue;
            var matchTask = task.questData.questRequires.Find(r => r.name == name);
            if (matchTask != null)
                matchTask.currentAmount += amount;
            task.questData.checkQuestProgress();
        }
    }

    public QuestTask getTask(QuestData_SO data)
    {
        return tasks.Find(q => q.questData.questName == data.questName);
    }
}
