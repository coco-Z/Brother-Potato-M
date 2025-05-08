using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class WeaponSelectPanel : MonoBehaviour
{

    public static WeaponSelectPanel Instance;
    public CanvasGroup _canvasGroup;
    public CanvasGroup _weaponDetailsCanvasGroup;
    public Transform _weaponContent; 
    
    
  

    public GameObject weapon_prefab; //预制体
    public Transform _weaponList;
    
    public Image _weaponAvatar;
    public TextMeshProUGUI _weaponName;
    public TextMeshProUGUI _weaponType;
    public TextMeshProUGUI _weaponDescribe;
    public GameObject _weaponDetails;
    
    

    private void Awake()
    {
        Instance = this;

        _canvasGroup = GetComponent<CanvasGroup>();
        _weaponContent = GameObject.Find("WeaponContent").transform;
        
    

        weapon_prefab = Resources.Load<GameObject>("Prefabs/Weapon");
        _weaponList = GameObject.Find("WeaponList").transform;

        _weaponAvatar = GameObject.Find("Avatar_Weapon").GetComponent<Image>();
        _weaponName = GameObject.Find("WeaponName").GetComponent<TextMeshProUGUI>();
        _weaponType = GameObject.Find("WeaponType").GetComponent<TextMeshProUGUI>();
        _weaponDescribe = GameObject.Find("WeaponDescribe").GetComponent<TextMeshProUGUI>();
        
        _weaponDetails = GameObject.Find("WeaponDetails");
        _weaponDetailsCanvasGroup = GameObject.Find("WeaponDetails").GetComponent<CanvasGroup>();
    }
    

    // Start is called before the first frame update
    void Start()
    {
        foreach (WeaponData weaponData in GameManager.Instance.weaponDatas)
        {
            WeaponUI w = Instantiate(weapon_prefab, _weaponList).GetComponent<WeaponUI>();
            w.SetData(weaponData);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

