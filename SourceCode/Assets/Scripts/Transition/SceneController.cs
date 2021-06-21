using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class SceneController : SingleTon<SceneController>, IEndGameObserver
{
    public GameObject playerPrefab;

    GameObject player;
    bool fadeFinished;
    NavMeshAgent agent;

    public SceneFader sceneFaderPrefab;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
    private void Start()
    {
        GameManager.Instance.addObserver(this);
        fadeFinished = true;
    }
    public void TransitionToDestination(TransitionPoint transitionPoint)
    {
        switch (transitionPoint.transitionType)
        {
            case TransitionPoint.TransitionType.SameScene:
                {
                    StartCoroutine(Transition(SceneManager.GetActiveScene().name, transitionPoint.destinationTag));
                }
                break;
            case TransitionPoint.TransitionType.DifferentScene:
                {
                    StartCoroutine(Transition(transitionPoint.sceneName, transitionPoint.destinationTag));
                }
                break;
        }
    }
    IEnumerator Transition(string sceneName, TransitionDestination.DestinationTag destinationTag)
    {
        SceneFader fade = Instantiate(sceneFaderPrefab);
        yield return StartCoroutine(fade.FadeOut(1f));
        SaveManager.Instance.savePlayerData();
        if (SceneManager.GetActiveScene().name != sceneName)
        {
            yield return SceneManager.LoadSceneAsync(sceneName);
            yield return Instantiate(playerPrefab, GetDestination(destinationTag).transform.position, GetDestination(destinationTag).transform.rotation);
            SaveManager.Instance.loarPlayerData();
            yield return StartCoroutine(fade.FadeIn(1f));
            yield break;
        }
        else
        {
            player = GameManager.Instance.playerStats.gameObject;
            agent = player.GetComponent<NavMeshAgent>();
            agent.enabled = false;
            player.transform.SetPositionAndRotation(GetDestination(destinationTag).transform.position, GetDestination(destinationTag).transform.rotation);
            agent.enabled = true;
            yield return null;
            yield return StartCoroutine(fade.FadeIn(1f));
        }
    }

    public void transitionToFirstLevel()
    {
        StartCoroutine(loadLevel("SampleScene"));
    }
    IEnumerator loadLevel(string sceneName)
    {
        SceneFader fade = Instantiate(sceneFaderPrefab);
        if (sceneName != "")
        {
            yield return StartCoroutine(fade.FadeOut(1f));
            yield return SceneManager.LoadSceneAsync(sceneName);
            yield return player = Instantiate(playerPrefab,GameManager.Instance.getEntrance().position
                ,GameManager.Instance.getEntrance().rotation);

            SaveManager.Instance.savePlayerData();
            yield return StartCoroutine(fade.FadeIn(1f));
            yield break;
        }
    }
    private TransitionDestination GetDestination(TransitionDestination.DestinationTag destinationTag)
    {
        var entrances = FindObjectsOfType<TransitionDestination>();
        for(int i=0;i<entrances.Length;++i)
        {
            if(entrances[i].destinationTag==destinationTag)
            {
                return entrances[i];

                    }
        }
        return null;
    }
    public void transistionToMain()
    {
        StartCoroutine(LoadMain());
    }
    public void transitionToLoadGame()
    {
        StartCoroutine(loadLevel(SaveManager.Instance.SceneName));
    }

    IEnumerator LoadMain()
    {
        SceneFader fade = Instantiate(sceneFaderPrefab);
        yield return StartCoroutine(fade.FadeOut(1f));
        yield return SceneManager.LoadSceneAsync("Main");
        yield return StartCoroutine(fade.FadeIn(1f));
        yield break;
    }

    public void EndNotify()
    {
        if (fadeFinished)
        {
            fadeFinished = false;
            StartCoroutine(LoadMain());
        }
    }
}
