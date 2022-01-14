using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class SceneController : Singleton<SceneController>
{
    GameObject player;
    NavMeshAgent playerAgent;
    public GameObject playerPrefab;

    private void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    public void TransitionToDestination(TransitionPoint transitionPoint)
    {
        //判断传送方式
        if (TransitionPoint.TransitionType.SameScene.Equals(transitionPoint.transitionType))
        {
            StartCoroutine(Transition(SceneManager.GetActiveScene().name, transitionPoint.destinationTag));
        }
        else
        {
            StartCoroutine(Transition(transitionPoint.sceneName, transitionPoint.destinationTag));
        }
    }

    //传送
    IEnumerator Transition(string sceneName, TransitionDestination.DestinationTag destinationTag)
    {
        //保存数据
        SaveManager.Instance.SavePlayerData();
        //判断是否同场景传送
        if (SceneManager.GetActiveScene().name == sceneName)
        {
            player = GameManager.Instance.playerState.gameObject;
            playerAgent = player.GetComponent<NavMeshAgent>();
            playerAgent.enabled = false;
            player.transform.SetPositionAndRotation(GetDestination(destinationTag).transform.position, GetDestination(destinationTag).transform.rotation);
            playerAgent.enabled = true;
            yield return null;
        }
        else
        {
            yield return SceneManager.LoadSceneAsync(sceneName);
            yield return Instantiate(playerPrefab, GetDestination(destinationTag).transform.position, GetDestination(destinationTag).transform.rotation);
            //读取数据
            SaveManager.Instance.LoadPlayerData();
            yield break;
        }
    }

    //获取传送目的地
    private TransitionDestination GetDestination(TransitionDestination.DestinationTag destinationTag)
    {
        var entrances = FindObjectsOfType<TransitionDestination>();
        foreach (TransitionDestination destination in entrances)
        {
            if (destinationTag.Equals(destination.destinationTag))
            {
                return destination;
            }
        }
        return null;
    }

    //加载场景
    IEnumerator LoadScene(string sceneName)
    {
        yield return SceneManager.LoadSceneAsync(sceneName);
        yield return Instantiate(playerPrefab, GameManager.Instance.GetEntrance().position, GameManager.Instance.GetEntrance().rotation);
        yield break;
    }

    public void LoadFirstScene()
    {
        StartCoroutine(LoadScene("Scene1"));
    }

    public void TransitionToLoadGame()
    {
        StartCoroutine(LoadScene(SaveManager.Instance.SceneName));
    }

    IEnumerator LoadMenul()
    {
        yield return SceneManager.LoadSceneAsync("Menul");
        yield break;
    }

    public void TransitionToMenul()
    {
        StartCoroutine(LoadMenul());
    }

}
