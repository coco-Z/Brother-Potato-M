using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCardUI : MonoBehaviour
{
    public ItemData itemData; //数据
    public Button _button; //按钮
    public CanvasGroup _canvasGroup; //设置透明度

    private void Awake()
    {
        _button = transform.GetChild(4).GetComponent<Button>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _button.onClick.AddListener(() =>
        {
            //购物  
            bool result = ShopPanel.Instance.Shopping(itemData);
            if (result)
            {
                _canvasGroup.alpha = 0;
                _canvasGroup.interactable = false;
            }
           
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
