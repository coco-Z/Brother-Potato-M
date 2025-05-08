using System;
using UnityEngine;

public class EnemyBase: MonoBehaviour
{

    
    
    public float attackTimer = 0; //攻击定时器
    public bool isContact = false; // 是否接触到玩家
    public bool isCooling = false; //攻击冷却
    
    [SerializeField]
    public EnemyData enemyData;
    
    
    // public float hp; //血量
    // public float damage; // 攻击力
    // public float speed; // 移动速度
    // public float attackTime; //攻击间隔
    // public int provideExp = 1; //经验值

    
    
   
    public float skillTimer = 0;  //技能计时器 
    public bool skilling = false; //技能是否正在释放
    
    
    
    public void Awake()
    {
       
    }

    public void Start()
    {
        
    }

    public void Update()
    {
        if (Player.Instance.isDead)
        {
            return;
        }
        
        
        Move(); // 移动
        
        //攻击
        if (isContact && !isCooling)
        {
            Attack();
        }

        //更新攻击计时器
        if (isCooling)
        {
            attackTimer -= Time.deltaTime;
            
            if (attackTimer <= 0)
            {
                attackTimer = 0;
                isCooling = false;
            }
        }
        
        
        //技能
        UpdateSkill();

    }

    public void SetElite()
    {
        enemyData.hp *= 2; 
        enemyData.damage *= 2;

        GetComponent<SpriteRenderer>().color = new Color(255 / 255f, 113 / 255f, 113 / 255f);

    }

    private void UpdateSkill()
    {
        if (enemyData.skillTime < 0)
        {
            return; 
        }

        //判定是否冷却
        if (skillTimer <= 0 )
        {
            //判断距离
            float dis = Vector2.Distance(transform.position, Player.Instance.transform.position);
            
            if (dis <= enemyData.range)
            {
                //发动技能
                Vector2 dir = (Player.Instance.transform.position - transform.position).normalized;
                LaunchSkill(dir);

                skillTimer = enemyData.skillTime; 
            }
        }
        else
        {
            skillTimer -= Time.deltaTime;

            if (skillTimer < 0)
            {
                skillTimer = 0;
            }
        }
        
        
    }

    public virtual void LaunchSkill(Vector2 dir)
    {
      
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isContact = true;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isContact = false;
        }
    }


    //自动移动
    public void Move()
    {
        if (skilling)
        {
            return; 
        }
        
        Vector2 direction = (Player.Instance.transform.position - transform.position).normalized;
        transform.Translate(direction * enemyData.speed * Time.deltaTime);

        TurnAround();
    }
    
    //自动转向
    public void TurnAround()
    {
        //玩家在怪物右边
        if (Player.Instance.transform.position.x - transform.position.x >= 0.1)
        {
            //怪物向右看
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x ), transform.localScale.y, transform.localScale.z);
        }else if (Player.Instance.transform.position.x - transform.position.x < 0.1)
        {
            //怪物向左看
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x ), transform.localScale.y, transform.localScale.z);
            
        }
    }
    
    
    //攻击
    public void Attack()
    {
        //如果攻击冷却, 则返回
        if (isCooling)
        {
            return;
        }
        
        Player.Instance.Injured(enemyData.damage);
        
        //攻击进入冷却
        isCooling = true;
        attackTimer = enemyData.attackTime;  

    }
    
    
    //受伤
    public void Injured(float attack)
    {
        // if (isDead)
        // {
        //     return;
        // }
        
        //判断本次攻击是否会死亡
        if (enemyData.hp - attack <= 0 )
        {
            enemyData.hp = 0;
            Dead();
        }
        else
        {
            enemyData.hp -= attack;
        }
    }
    
    //死亡
    public void Dead()
    {
        //增加玩家经验值
        GameManager.Instance.exp += enemyData.provideExp * GameManager.Instance.propData.expMuti;
        GamePanel.Instance.RenewExp();
        
        // 掉落金币
        Instantiate(GameManager.Instance.moeny_prefab, transform.position, Quaternion.identity);
        
        //销毁自己
        Destroy(gameObject);
    }
    
}