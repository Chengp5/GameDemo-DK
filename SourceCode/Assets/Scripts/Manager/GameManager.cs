using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : SingleTon<GameManager>
{
    public CharaterStats playerStats;

    List<IEndGameObserver> endGameObservers = new List<IEndGameObserver>();
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
    private CinemachineFreeLook followeCam;
    public void  registerPlayer(CharaterStats player)
    {
        playerStats = player;
        followeCam = FindObjectOfType<CinemachineFreeLook>(); 
        if(followeCam!=null)
        {
            followeCam.Follow = playerStats.transform.GetChild(2);
            followeCam.LookAt = playerStats.transform.GetChild(2);
        }
    }
    public void addObserver(IEndGameObserver observer)
    {
       if(!endGameObservers.Contains(observer))
        endGameObservers.Add(observer);
    }
    public void removerObserver(IEndGameObserver observer)
    {
        if (endGameObservers.Contains(observer))
            endGameObservers.Remove(observer);
    }
    public void NotifyAllObservers()
    {
         foreach(var observer in endGameObservers)
        {
            observer.EndNotify();
        }
    }
    public Transform getEntrance()
    {
        foreach(var item in FindObjectsOfType<TransitionDestination>())
        {
            if (item.destinationTag == TransitionDestination.DestinationTag.ENTER)
                return item.transform;
        }
        return null;
    }
}
