using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RoleRandom : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image backImage;
    public Button _button;
    public List<RoleUI> unlockedRoles = new List<RoleUI>(); 
    
    
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
            foreach (RoleUI role in RoleSelectPanel.Instance._roleList.GetComponentsInChildren<RoleUI>())
            {
                if (role.roleData.unlock == 1)
                {
                    unlockedRoles.Add(role);
                }
            }

            RoleUI r = GameManager.Instance.RandomOne(unlockedRoles) as RoleUI;
            
            r.RenewUI(r.roleData);
            r.ButtonClick(r.roleData);
        });
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        backImage.color = new Color(207 / 255f, 207 / 255f, 207 / 255f);
        RoleSelectPanel.Instance._contentCanvasGroup.alpha = 0;
        
        
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        backImage.color = new Color(34 / 255f, 34 / 255f, 34 / 255f);
    }
    
    
}
