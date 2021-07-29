using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : SingleTon<SaveManager>
{
    string sceneName="level";
    public string SceneName
    {
        get { return PlayerPrefs.GetString(sceneName); }
    }
    protected override void  Awake() 
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneController.Instance.transistionToMain();
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            savePlayerData();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            loarPlayerData();
        }
    }

    public void savePlayerData()
    {
        Save(GameManager.Instance.playerStats.characterData, GameManager.Instance.playerStats.characterData.name);
     
    }
    public void loarPlayerData()
    {
        Load(GameManager.Instance.playerStats.characterData, GameManager.Instance.playerStats.characterData.name);
    }
    public void Save(Object data, string key)
    {
        var jsonData = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(key, jsonData);
        PlayerPrefs.SetString(sceneName, SceneManager.GetActiveScene().name);
        PlayerPrefs.Save();
    }
    public void Load(Object data, string key)
    {
        if(PlayerPrefs.HasKey(key))
        {
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key), data);
        }
    }

}
