using System.Collections;
using UnityEngine;

public class Enemy5 : EnemyBase
{
    public Transform pos;

    public new void Awake()
    {
        base.Awake();

        pos = GameObject.Find("RedCirclePos").transform;
    }

    public override void LaunchSkill(Vector2 dir)
    {
        int childCount = pos.childCount;

        for (int i = 0; i < childCount; i++)
        {
            Transform childTransform = pos.GetChild(i);
            if (childTransform == null)
            {
                continue;
            }
            GameObject go = Instantiate(GameManager.Instance.redCircle_prefab, childTransform);

            StartCoroutine(PosAutoDestroy(go));
        }
    }

    IEnumerator PosAutoDestroy(GameObject go)
    {
        yield return new WaitForSeconds(1);
        GameObject bullet = Instantiate(GameManager.Instance.enemyBullet_prefab, go.transform.position, Quaternion.identity);
        
        Destroy(go);
        bullet.GetComponent<EnemyBullet>().deadTime = 2; //1秒后死亡
        
   
        
    }
}