using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public GameObject healthBarPrefab; //血条
    public Transform barPoint; //敌人头顶位置
    public bool alwaysVisible; //是否一直显示
    public float visibleTime; //显示时间
    private float timeLeft; //剩余显示时间

    Image healthSlider; //血量
    Transform UIbar; //血条显示位置
    Transform cam; //相机位置

    CharacterStates currentStates;

    private void Awake()
    {
        currentStates = GetComponent<CharacterStates>();
        currentStates.UpdateHealthBarOnAttack += UpdateHealthBar;
    }

    private void OnEnable()
    {
        cam = Camera.main.transform;

        foreach (Canvas canvas in FindObjectsOfType<Canvas>())
        {
            if (canvas.renderMode == RenderMode.WorldSpace)
            {
                UIbar = Instantiate(healthBarPrefab, canvas.transform).transform;
                healthSlider = UIbar.GetChild(0).GetComponent<Image>();
                UIbar.gameObject.SetActive(alwaysVisible);
            }
        }
    }

    private void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        if (currentHealth <= 0)
            Destroy(UIbar.gameObject);
        UIbar.gameObject.SetActive(true);
        timeLeft = visibleTime;
        float sliderPercent = (float)currentHealth / maxHealth;
        healthSlider.fillAmount = sliderPercent;
    }

    private void LateUpdate()
    {
        if (UIbar != null)
        {
            UIbar.position = barPoint.position;
            UIbar.forward = -cam.forward;

            if (timeLeft <= 0 && !alwaysVisible)
                UIbar.gameObject.SetActive(false);
            else
                timeLeft -= Time.deltaTime;
        }
    }
}
