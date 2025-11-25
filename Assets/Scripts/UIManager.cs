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
    public TextMeshProUGUI message;
    public GameObject messageBox;
    public Image modeIcon;
    public GameObject GameEndUI;
    public TextMeshProUGUI scorefinalText;
    public TextMeshProUGUI flipfinalText;

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

    public void UpdateScore(float s) 
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

    public void SendUIMessage(string text)
    {
        message.text = text;
        messageBox.SetActive(true);
    }
    public void GameEndMessage(float score, int flips)
    {
        GameEndUI.SetActive(true);
        scorefinalText.text = score.ToString("00.00");
        flipText.text = flips.ToString();
    }

}
