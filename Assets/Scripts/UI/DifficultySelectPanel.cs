using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DifficultySelectPanel : MonoBehaviour
{
    public static DifficultySelectPanel Instance;

    public Transform _difficultyContent;
    public CanvasGroup _canvasGroup;
    
    
    public GameObject difficulty_prefab;
    public Transform _difficultyList;
    
    public Image _difficultyAvatar;
    public TextMeshProUGUI _difficultyName; 
    public TextMeshProUGUI _difficultyDescribe; 
    


    private void Awake()
    {
        Instance = this;

        _difficultyContent = GameObject.Find("DifficultyContent").transform;
        _canvasGroup = GetComponent<CanvasGroup>();

       

        _difficultyList = GameObject.Find("DifficultyList").transform;
        difficulty_prefab = Resources.Load<GameObject>("Prefabs/Difficulty");

        _difficultyAvatar = GameObject.Find("Avatar_Difficulty").GetComponent<Image>();
        _difficultyName = GameObject.Find("DifficultyName").GetComponent<TextMeshProUGUI>();
        _difficultyDescribe = GameObject.Find("DifficultyDescribe").GetComponent<TextMeshProUGUI>();
        
        

    }

    
    
    // Start is called before the first frame update
    void Start()
    {
        foreach (DifficultyData difficultyData in GameManager.Instance.difficultyDatas)
        {
            DifficultyUI d = Instantiate(difficulty_prefab, _difficultyList).GetComponent<DifficultyUI>();
            d.SetData(difficultyData);

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

 
}
