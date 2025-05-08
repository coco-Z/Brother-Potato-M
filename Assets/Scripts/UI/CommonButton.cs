using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CommonButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image image;
    private TextMeshProUGUI text;

    private void Awake()
    {
        image = GetComponent<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    
    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = new Color(255, 255, 255);
        text.color = new Color(0, 0, 0);
        
        //音效
        Instantiate(GameManager.Instance.menuMusic);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = new Color(0, 0, 0);
        text.color = new Color(255, 255, 255);
    }
}
