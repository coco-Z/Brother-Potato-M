using System;
using System.Collections;
using UnityEngine;

public class WeaponShort: WeaponBase
{
    
    public new void Awake()
    {
        base.Awake();
        moveSpeed = 10;
    }
    
    //开火
    public override void Fire()
    {
        if (isCooling || enemy == null)
        {
            return;
        }
        
        //打开碰撞器
        gameObject.GetComponent<CapsuleCollider2D>().enabled = true;

        isAiming = false;
        StartCoroutine(GoPosition(enemy.position));  //武器向敌人位置移动

        isCooling = true;

    }
    
       
    IEnumerator GoPosition(Vector3 enemyPosition)
    {
        var enemyPos = enemyPosition + new Vector3(0, enemy.GetComponent<SpriteRenderer>().size.y / 2, 0);
         
        while (Vector2.Distance(transform.position, enemyPos) > 0.1f)
        {
            Vector3 direction = (enemyPos - transform.position).normalized;

            Vector3 moveAmount = direction * moveSpeed * Time.deltaTime;

            transform.position += moveAmount;
             
            yield return null;
        }


        //关闭碰撞器
        gameObject.GetComponent<CapsuleCollider2D>().enabled = false;

        StartCoroutine(ReturnPosition());
            

    }


    IEnumerator ReturnPosition()
    {
        while ((Vector3.zero - transform.localPosition).magnitude > 0.1f )
        {
            Vector3 direction = (Vector3.zero - transform.localPosition).normalized;
            transform.localPosition += direction * moveSpeed * Time.deltaTime;
            yield return null;
        }

        //回到原点后可以进行瞄准, 防止在攻击过程中方向转动
        isAiming = true;
    }


    
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            //判断是否暴击
            bool isCritical = CriticalHits();
            if (isCritical)
            {
                col.GetComponent<EnemyBase>().Injured(data.damage  * data.critical_strikes_multiple);
                
                //文字
                Number number = Instantiate(GameManager.Instance.number_prefab).GetComponent<Number>();
                number.text.text = (data.damage  * data.critical_strikes_multiple).ToString();
                number.text.color = new Color(255 / 255f, 188/255f, 0);
                number.transform.position = transform.position; 
            }
            else
            {
                col.GetComponent<EnemyBase>().Injured(data.damage);
                //文字
                Number number = Instantiate(GameManager.Instance.number_prefab).GetComponent<Number>();
                number.text.text = (data.damage ).ToString();
                number.text.color = new Color(255 / 255f, 255/255f, 255/255f);
                number.transform.position = transform.position; 
            }
            
            //音效
            Instantiate(GameManager.Instance.attackMusic);
           
            // gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
        }
    }
    
    
}