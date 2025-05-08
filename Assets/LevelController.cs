using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;


public class LevelController : MonoBehaviour
{
    public static LevelController Instance;
    public float waveTimer;
    public GameObject _failPanel; //失败面板
    public GameObject _successPanel; //成功面板
    public List<EnemyBase> enemy_list; //所有敌人列表
    public Transform _map;
    
    public GameObject enemy1_prefab;  // 敌人1预制体
    public GameObject enemy2_prefab;  // 敌人2预制体
    public GameObject enemy3_prefab;  // 敌人3预制体
    public GameObject enemy4_prefab;  // 敌人4预制体
    public GameObject enemy5_prefab;  // 敌人5预制体
    public Transform enemyFather; 
    
    public GameObject redfork_prefab;  //红叉预制体
    public TextAsset levelTextAsset; //json 
    public List<LevelData> levelDatas = new List<LevelData>(); //所有关卡信息
    public LevelData currentLevelData; //当前关卡信息

    private int _levelInfo = 0; // 0正常，-1失败，1成功

    public Dictionary<string, GameObject> enemyPrefabDic = new Dictionary<string, GameObject>();

    private void Awake()
    {
        Instance = this;

        _failPanel = GameObject.Find("FailPanel");
        _successPanel = GameObject.Find("SuccessPanel");
        enemy1_prefab = Resources.Load<GameObject>("Prefabs/Enemy1");
        enemy2_prefab = Resources.Load<GameObject>("Prefabs/Enemy2");
        enemy3_prefab = Resources.Load<GameObject>("Prefabs/Enemy3");
        enemy4_prefab = Resources.Load<GameObject>("Prefabs/Enemy4");
        enemy5_prefab = Resources.Load<GameObject>("Prefabs/Enemy5");
        
        
        redfork_prefab = Resources.Load<GameObject>("Prefabs/RedFork");
        _map = GameObject.Find("Map").transform;
        
        levelTextAsset = Resources.Load<TextAsset>("Data/" + GameManager.Instance.currentDifficulty.levelName);
        Debug.Log(levelTextAsset);
        levelDatas = JsonConvert.DeserializeObject<List<LevelData>>(levelTextAsset.text);

        enemyFather = GameObject.Find("Enemys").transform;
        
        enemyPrefabDic.Add("enemy1", enemy1_prefab);
        enemyPrefabDic.Add("enemy2", enemy2_prefab);
        enemyPrefabDic.Add("enemy3", enemy3_prefab);
        enemyPrefabDic.Add("enemy4", enemy4_prefab);
        enemyPrefabDic.Add("enemy5", enemy5_prefab);
    }

    // Start is called before the first frame update
    void Start()
    {
        _levelInfo = 0; //正常

        currentLevelData = levelDatas[GameManager.Instance.currentWave - 1]; //保存当前关卡信息
        waveTimer = currentLevelData.waveTimer; //当前关卡的时间

        
        //生成敌人
        GenerateEnemy();
        
        //生成武器
        GenerateWeapon();
    }

    private void GenerateWeapon()
    {
        Debug.Log("生成武器开始");

        int i = 0;
        foreach (WeaponData weaponData in GameManager.Instance.currentWeapons)
        {
            GameObject go = Resources.Load<GameObject>("Prefabs/" + weaponData.name);
            WeaponBase wb = Instantiate(go, Player.Instance.weaponsPos.GetChild(i)).GetComponent<WeaponBase>();
            wb.data = weaponData;
            
            i++;
        }
        
        
        
        Debug.Log("生成武器结束");
    }


    private void GenerateEnemy()
    {
        //遍历所有波次信息
        foreach (WaveData waveData in currentLevelData.enemys)
        {
            //遍历敌人数量
            for (int i = 0; i < waveData.count; i++)
            {
                StartCoroutine(SwawnEnemies(waveData));
            }
         
        }   
        
       
    }

    IEnumerator  SwawnEnemies(WaveData waveData)
    {
        yield return new WaitForSeconds(waveData.timeAxis);
        
        if (waveTimer > 0 && !Player.Instance.isDead)
        {
            var spawnPoint = GetRandomPosition(_map.GetComponent<SpriteRenderer>().bounds);
            
            GameObject go = Instantiate(redfork_prefab, spawnPoint, Quaternion.identity); 
            yield return new WaitForSeconds(1);
            Destroy(go);

            if (waveTimer > 0 && !Player.Instance.isDead)
            {
                EnemyBase enemy = Instantiate(enemyPrefabDic[waveData.enemyName], spawnPoint, Quaternion.identity).GetComponent<EnemyBase>();
                enemy.transform.parent = enemyFather;

                foreach (EnemyData e in GameManager.Instance.enemyDatas)
                {
                    if (e.name == waveData.enemyName)
                    {
                        enemy.enemyData = e;

                        //判断是否为精英
                        if (waveData.elite == 1)
                        {
                            enemy.SetElite();
                        }
                    }
                }
                
               
                
                enemy_list.Add(enemy); 
            }
        }
    }
    
    
    
    //获取随机位置
    private Vector3 GetRandomPosition(Bounds bounds)
    {
        float safeDistance = 3.5f; //安全距离
        
        float randomX = Random.Range(bounds.min.x + safeDistance, bounds.max.x - safeDistance);
        float randomY = Random.Range(bounds.min.y + safeDistance, bounds.max.y - safeDistance);
        float randomZ = 0f;
        return new Vector3(randomX, randomY, randomZ);

    }

    // Update is called once per frame
    void Update()
    {
        

        if (waveTimer > 0 )
        {
            if (_levelInfo == 0)
            {
                waveTimer -= Time.deltaTime;
            }
            
            if (waveTimer <= 0)
            {
                waveTimer = 0;

                if (GameManager.Instance.currentWave < 5)
                {
                    NextWave();
                }
                else
                {
                    GoodGame();
                }
                
            }
        }

        GamePanel.Instance.RenewCountDown(waveTimer);

    }
    
    //进入下一关
    private void NextWave()
    {
        // 修复失败进入下一关
        if (_levelInfo == 1 || _levelInfo == -1)
        {
            return;
        }

        GameManager.Instance.money += GameManager.Instance.propData.harvest; //收获添加到金币中
        SceneManager.LoadScene("04-Shop");  //跳转商店
        GameManager.Instance.currentWave += 1; //波次+1


    }

    //生成敌人
    
    //游戏胜利
    public void GoodGame()
    {
        _levelInfo = 1; //成功
        _successPanel.GetComponent<CanvasGroup>().alpha = 1;
        StartCoroutine(GoMenu());
        
        //todo 所有敌人 消失
        for (int i = 0; i < enemy_list.Count; i++)
        {
            if (enemy_list[i])
            {
                enemy_list[i].Dead();
            }
          
        }

        if (PlayerPrefs.GetInt("多面手") == 0)
        {
            Debug.Log("多面手解锁");
            PlayerPrefs.SetInt("多面手", 1);

            for (int i = 0; i < GameManager.Instance.roleDatas.Count; i++)
            {
                if (GameManager.Instance.roleDatas[i].name == "多面手")
                {
                    GameManager.Instance.roleDatas[i].unlock = 1;
                }
            }
        }
        
    }
    
    
    //todo 波次完成
    
    //游戏失败
    public void BadGame()
    {
        _levelInfo = -1; //失败
        _failPanel.GetComponent<CanvasGroup>().alpha = 1;
        StartCoroutine(GoMenu());
        
        
        //todo 所有敌人 消失
        for (int i = 0; i < enemy_list.Count; i++)
        {
            if (enemy_list[i])
            {
                enemy_list[i].Dead();
            }
          
        }
        
    }
    
     IEnumerator GoMenu()
     {
         yield return new WaitForSeconds(3);
         SceneManager.LoadScene(0);
     }
}
