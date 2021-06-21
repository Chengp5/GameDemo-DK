using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public GameObject healthBarUIPrefab;

    public bool alwaysVisible;

    public float visibleTime;

    private float timeleft;

    public Transform barPoint;

    Image healthSlider;

    Transform UIbar;

    Transform cam;

    CharaterStats currentStats;

    private void Awake()
    {
        currentStats = GetComponent<CharaterStats>();
        currentStats.updateHealthBarOnAttack += UpdateHealthBar;
    }
    private void OnEnable()
    {
        cam = Camera.main.transform;
        foreach(Canvas canvas in FindObjectsOfType<Canvas>())
        {
            if(canvas.renderMode==RenderMode.WorldSpace)
            {
                UIbar=Instantiate(healthBarUIPrefab, canvas.transform).transform;
                healthSlider = UIbar.GetChild(0).GetComponent<Image>();
                UIbar.gameObject.SetActive(alwaysVisible);
            }
        }
    }
    private void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        if(currentHealth<=0)
        {
            if(UIbar.gameObject!=null)
            Destroy(UIbar.gameObject);
        }
        UIbar.gameObject.SetActive(true); 
        timeleft = visibleTime;
        float sliderPercent =(float)currentHealth / maxHealth;
        healthSlider.fillAmount = sliderPercent;

    }
    private void LateUpdate()
    {
         if(UIbar!=null)
        {
            UIbar.position = barPoint.position;
            UIbar.forward = -cam.forward;
            if(timeleft<=0&&!alwaysVisible)
            {
                UIbar.gameObject.SetActive(false);
            }
            else
            {
                timeleft -= Time.deltaTime;
            }
        }
    }
}
