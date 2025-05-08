using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 1; //攻击力
    public float deadTime = 5; //超时后销毁
    public float speed = 8; //速度
    public float timer; //定时器
    public Vector2 dir = Vector2.zero; //方向
    public string tagName; //碰撞检测的对象
    public bool isCritical = false; //暴击
    
    public void Awake()
    {
        
    }

    // Start is called before the first frame update
    public void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      
        
        //超时销毁
        timer += Time.deltaTime;
        if (timer >= deadTime)
        {
            Destroy(gameObject);
        }
        
        //移动
        transform.position += (Vector3)dir * speed * Time.deltaTime; 

    }
    
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag(tagName))
        {
            //敌人伤害玩家
            if (tagName == "Player")
            {
                Player.Instance.Injured(damage);
               
                
            }
            //玩家伤害敌人
            else if (tagName == "Enemy")
            {
                //暴击情况
                if (isCritical)
                {
                    //文字
                    Number number = Instantiate(GameManager.Instance.number_prefab).GetComponent<Number>();
                    number.text.text = damage.ToString();
                    number.text.color = new Color(255 / 255f, 178/255f, 0);
                    number.transform.position = transform.position; 
                }
                else
                {
                    //文字
                    Number number = Instantiate(GameManager.Instance.number_prefab).GetComponent<Number>();
                    number.text.text = damage.ToString();
                    number.text.color = new Color(255 / 255f, 255/255f, 255/255f);
                    number.transform.position = transform.position; 
                }
                
                col.gameObject.GetComponent<EnemyBase>().Injured(damage);
                
                
            }
            
            Destroy(gameObject);
           
        }
    }
}
