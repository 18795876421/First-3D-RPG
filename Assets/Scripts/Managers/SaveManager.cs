using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SaveManager : Singleton<SaveManager>
{
    string sceneName = "sceneName";
    public string SceneName { get { return PlayerPrefs.GetString(sceneName); } }
    private void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneController.Instance.TransitionToMenul();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            SavePlayerData();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadPlayerData();
        }
    }

    public void SavePlayerData()
    {
        Save(GameManager.Instance.playerState.characterData, GameManager.Instance.playerState.characterData.name);
    }

    public void LoadPlayerData()
    {
        Load(GameManager.Instance.playerState.characterData, GameManager.Instance.playerState.characterData.name);
    }

    public void Save(Object data, string key)
    {
        var jsonData = JsonUtility.ToJson(data, true);
        PlayerPrefs.SetString(key, jsonData);
        PlayerPrefs.SetString(sceneName, SceneManager.GetActiveScene().name);
        PlayerPrefs.Save();
    }

    public void Load(Object data, string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key), data);
        }
    }
}
