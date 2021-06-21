using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;


public class Menu : MonoBehaviour
{
    Button newGameBtn;
    Button continueBtn;
    Button quitBtn;

    PlayableDirector director;
    private void Awake()
    {
        newGameBtn = transform.GetChild(1).GetComponent<Button>();
        continueBtn = transform.GetChild(2).GetComponent<Button>();
        quitBtn = transform.GetChild(3).GetComponent<Button>();


        quitBtn.onClick.AddListener(quitGame);
        newGameBtn.onClick.AddListener(playTimeLine);
        continueBtn.onClick.AddListener(continueGame);

        director = FindObjectOfType<PlayableDirector>();
        director.stopped += NewGame ;
    }

    void playTimeLine()
    {
        director.Play();
    }
    void NewGame(PlayableDirector obj)
    {
        PlayerPrefs.DeleteAll();
        SceneController.Instance.transitionToFirstLevel();
    }
    void continueGame()
    {
        SceneController.Instance.transitionToLoadGame();
    }
    void quitGame()
    {
        Application.Quit();
        Debug.Log("quit");
    }
}
