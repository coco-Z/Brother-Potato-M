using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RoleUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image _backImage; //背景图片
    public Image _avatar;  //角色头像
    public Button _button;  //按钮
    
    public RoleData roleData;

    private void Awake()
    {
        _backImage = GetComponent<Image>();
        _avatar = transform.GetChild(0).GetComponent<Image>();
        _button = GetComponent<Button>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetData(RoleData roleData)
    {
        this.roleData = roleData;


        if (roleData.unlock == 0 && PlayerPrefs.GetInt(roleData.name, 1) == 0 )
        {
            _avatar.sprite = Resources.Load<Sprite>("Image/UI/锁");
        }
        else
        {
            _avatar.sprite = Resources.Load<Sprite>(roleData.avatar);
        }
       
        
        _button.onClick.AddListener(() =>
        {
            ButtonClick(roleData);
        });
        
    }


    public void ButtonClick(RoleData r)
    {
        //记录下选角色的信息 
        GameManager.Instance.currentRole = r;
        
        //关闭角色选择面板
        RoleSelectPanel.Instance._canvasGroup.alpha = 0;
        RoleSelectPanel.Instance._canvasGroup.interactable = false;
        RoleSelectPanel.Instance._canvasGroup.blocksRaycasts = false;


        //克隆角色的UI
        GameObject go = Instantiate(RoleSelectPanel.Instance._roleDetails, WeaponSelectPanel.Instance._weaponContent);
        go.transform.SetSiblingIndex(0);

        //打开武器选择面板
        WeaponSelectPanel.Instance._canvasGroup.alpha = 1;
        WeaponSelectPanel.Instance._canvasGroup.interactable = true;
        WeaponSelectPanel.Instance._canvasGroup.blocksRaycasts = true;


    }
    
    //鼠标移入
    public void OnPointerEnter(PointerEventData eventData)
    {
        _backImage.color = new Color(207/255f, 207/255f , 207/255f);

        if (RoleSelectPanel.Instance._contentCanvasGroup.alpha != 1)
        {
            RoleSelectPanel.Instance._contentCanvasGroup.alpha = 1;  //显示大屏UI
        }
        
        
        RenewUI(roleData);

    }

    public void RenewUI(RoleData r)
    {
        //如果未解锁
        if (r.unlock == 0)
        {
            RoleSelectPanel.Instance._roleName.text = "???";
            RoleSelectPanel.Instance._avatar.sprite = Resources.Load<Sprite>("Image/UI/锁");
            RoleSelectPanel.Instance._roleDescribe.text = r.unlockConditions;
            RoleSelectPanel.Instance._text3.text = "尚无记录"; 


        }
        else //如果解锁 
        {
            RoleSelectPanel.Instance._roleName.text = r.name;
            RoleSelectPanel.Instance._avatar.sprite = Resources.Load<Sprite>(r.avatar);
            RoleSelectPanel.Instance._roleDescribe.text = r.describe;
            RoleSelectPanel.Instance._text3.text = GetRecord(r.record);
        }
        
        
    }
    
    //获取通关记录
    private string GetRecord(int rRecord)
    {
        string result = "";
        
        switch (rRecord)
        {
            case -1:
                result = "尚无记录";
                break;
            case 0:
                result = "通关危险0";
                break;
            case 1:
                result = "通关危险1";
                break;
            case 2:
                result = "通关危险2";
                break;
            case 3:
                result = "通关危险3";
                break;
            case 4:
                result = "通关危险4";
                break;
            case 5:
                result = "通关危险5";
                break;
        }


        return result;
    }

    //鼠标移出
    public void OnPointerExit(PointerEventData eventData)
    {
        _backImage.color = new Color(34/255f, 34/255f , 34/255f);
    }
}
