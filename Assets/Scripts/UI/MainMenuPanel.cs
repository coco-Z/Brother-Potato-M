using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Newtonsoft.Json;  

public class MainMenuPanel : MonoBehaviour
{
    private Button startButton; 
    private Button settingsButton; 
    private Button progressButton; 
    private Button exitButton;

    private void Awake()
    {
        startButton = GameObject.Find("StartButton").GetComponent<Button>();
        settingsButton = GameObject.Find("SettingsButton").GetComponent<Button>();
        progressButton = GameObject.Find("ProgressButton").GetComponent<Button>();
        exitButton = GameObject.Find("ExitButton").GetComponent<Button>();
    }


    // Start is called before the first frame update
    void Start()
    {
        startButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("02-LevelSelect");
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
