using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public static UIManager Instance { get; private set; }

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI flipText;
    public TextMeshProUGUI comboText;
    public TextMeshProUGUI mode;
    public Image modeIcon;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
    }

    public void UpdateScore(int s) 
    { 
        scoreText.text = s.ToString("00.00");
    }
    public void UpdateFlips(int s)
    {
        flipText.text = s.ToString();
    }
    public void UpdateCombo(int s)
    {
        comboText.text = "x" + s.ToString();
    }
    public void UpdateMode(string gameMode, Sprite icon)
    {
        modeIcon.sprite = icon;
        mode.text = gameMode;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
