using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerState : MonoBehaviour
{
    private Text levelText;
    private Image healthBar;
    private Image expBar;

    private void Awake()
    {
        levelText = transform.GetChild(2).GetComponent<Text>();
        healthBar = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        expBar = transform.GetChild(1).GetChild(0).GetComponent<Image>();
    }

    private void Update()
    {
        levelText.text = "Level  " + GameManager.Instance.playerState.characterData.currentLevel;
        UpdateHealth();
        UpdateExp();
    }

    void UpdateHealth()
    {
        float sliderPercent = (float)GameManager.Instance.playerState.CurrentHealth / GameManager.Instance.playerState.MaxHealth;
        healthBar.fillAmount = sliderPercent;
    }

    void UpdateExp()
    {
        float sliderPercent = (float)GameManager.Instance.playerState.characterData.currentExp / GameManager.Instance.playerState.characterData.levelUpExp;
        expBar.fillAmount = sliderPercent;
    }
}