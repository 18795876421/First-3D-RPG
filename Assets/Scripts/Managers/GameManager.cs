using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : Singleton<GameManager>
{
    public CharacterStates playerState;
    List<IEndGameObserver> endGameObservers = new List<IEndGameObserver>();
    private CinemachineFreeLook followCamera;

    private void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    public void RigisterPlayer(CharacterStates palyer)
    {
        playerState = palyer;
        followCamera = FindObjectOfType<CinemachineFreeLook>();
        if (followCamera != null)
        {
            followCamera.Follow = playerState.transform;
            followCamera.LookAt = playerState.transform;
        }
    }

    public void AddObserver(IEndGameObserver observer)
    {
        endGameObservers.Add(observer);
    }

    public void RemoveObserver(IEndGameObserver observer)
    {
        endGameObservers.Remove(observer);
    }

    public void NotifyObservers()
    {
        foreach (var observer in endGameObservers)
        {
            observer.EndNotify();
        }
    }
}
