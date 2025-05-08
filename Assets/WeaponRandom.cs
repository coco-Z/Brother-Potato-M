using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponRandom : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image backImage; //背景
    public Button _button; //按钮
    public List<WeaponUI> weaponList = new List<WeaponUI>(); //所有武器脚本

    private void Awake()
    {
        backImage = GetComponent<Image>();
        _button = GetComponent<Button>();
        

    }

    // Start is called before the first frame update
    void Start()
    {
        _button.onClick.AddListener(() =>
        {
           
            foreach (WeaponUI weapon in WeaponSelectPanel.Instance._weaponList.GetComponentsInChildren<WeaponUI>())
            {
                weaponList.Add(weapon);
            }

            WeaponUI w = GameManager.Instance.RandomOne(weaponList) as WeaponUI;

            w.RenewUI(w.weaponData);
            w.ButtonClick(w.weaponData);

        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        backImage.color = new Color(207f / 255f, 207f / 255f, 207f / 255f); //背景高亮
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        backImage.color = new Color(34 / 255f, 34 / 255f, 34 / 255f);
    }
}
