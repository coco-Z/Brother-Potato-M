using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public WeaponData weaponData;
    
    public Image _backImage; //背景图片
    public Image _avatar;  //角色头像
    public Button _button;  //按钮

    private void Awake()
    {
        _backImage = GetComponent<Image>();
        _button = GetComponent<Button>();
        _avatar = transform.GetChild(0).GetComponent<Image>();

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
    //注入数据
    public void SetData(WeaponData w)
    {
        weaponData = w;

        _avatar.sprite = Resources.Load<Sprite>(weaponData.avatar);
        
        //点击事件
        _button.onClick.AddListener(() =>
        {
            ButtonClick(weaponData);
        });

    }
    
    //按钮点击
    public void ButtonClick(WeaponData w)
    {
       //记录当前武器
       GameManager.Instance.currentWeapons.Add(w);
       
       //克隆UI
       GameObject weapon_clone = Instantiate(WeaponSelectPanel.Instance._weaponDetails, DifficultySelectPanel.Instance._difficultyContent);
       weapon_clone.transform.SetSiblingIndex(0);
       
       GameObject role_clone = Instantiate(RoleSelectPanel.Instance._roleDetails, DifficultySelectPanel.Instance._difficultyContent);
       role_clone.transform.SetSiblingIndex(0);
       
       //关闭当前面板
       WeaponSelectPanel.Instance._canvasGroup.alpha = 0;
       WeaponSelectPanel.Instance._canvasGroup.interactable = false;
       WeaponSelectPanel.Instance._canvasGroup.blocksRaycasts = false;
       
       //打开下一个面板
       DifficultySelectPanel.Instance._canvasGroup.alpha = 1;
       DifficultySelectPanel.Instance._canvasGroup.interactable = true;
       DifficultySelectPanel.Instance._canvasGroup.blocksRaycasts = true;

    }
    
    //鼠标移入
    public void OnPointerEnter(PointerEventData eventData)
    {
        _backImage.color = new Color(207 / 255f, 207 / 255f, 207 / 255f);

        if ( WeaponSelectPanel.Instance._weaponDetailsCanvasGroup.alpha != 1 )
        {
            WeaponSelectPanel.Instance._weaponDetailsCanvasGroup.alpha = 1;
        }
       
        
        RenewUI(weaponData);
    }

    public void RenewUI(WeaponData w)
    {
        WeaponSelectPanel.Instance._weaponAvatar.sprite = Resources.Load<Sprite>(w.avatar); //修改头像
        WeaponSelectPanel.Instance._weaponName.text = w.name;  //修改名称
        WeaponSelectPanel.Instance._weaponType.text = w.isLong == 0 ? "近战" : "远程"; //修改武器类型
        WeaponSelectPanel.Instance._weaponDescribe.text = w.describe; //修改武器描述
    }

    //鼠标移出
    public void OnPointerExit(PointerEventData eventData)
    {
        _backImage.color = new Color(34 / 255f, 34 / 255f, 34 / 255f);

    }
}
