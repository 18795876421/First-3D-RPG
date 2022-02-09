using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class Menul : MonoBehaviour
{
    Button newGameBtn;
    Button continueBtn;
    Button quitBtn;
    PlayableDirector playableDirector;
    private void Awake()
    {
        newGameBtn = transform.GetChild(1).GetComponent<Button>();
        continueBtn = transform.GetChild(2).GetComponent<Button>();
        quitBtn = transform.GetChild(3).GetComponent<Button>();

        newGameBtn.onClick.AddListener(PlayTimeline);
        continueBtn.onClick.AddListener(ContinueGame);
        quitBtn.onClick.AddListener(QuitGame);

        playableDirector = FindObjectOfType<PlayableDirector>();
        playableDirector.stopped += NewGame;
    }

    void PlayTimeline()
    {
        playableDirector.Play();
    }

    void NewGame(PlayableDirector director)
    {
        PlayerPrefs.DeleteAll();
        SceneController.Instance.LoadFirstScene();
    }

    void ContinueGame()
    {
        SceneController.Instance.TransitionToLoadGame();
    }

    void QuitGame()
    {
        Application.Quit();
    }
}