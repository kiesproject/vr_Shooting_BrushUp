using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Normal_Enemy : MonoBehaviour ,IShootingDown
{
    float shootTime = 0;

    [SerializeField]
    private GameObject muzzle;
    [SerializeField]
    private GameObject explosion;
    [SerializeField]
    private GameObject bullet;

    private GameObject player;

    private Vector3 target_1_v3;
    private Vector3 target_2_v3;

    //---- ---- ----- ----- ----- ----- ----- ----- ---- ----- ----- -----
    [SerializeField]
    private float max_hp = 20;
    [SerializeField]
    private float hp = 20;
    private bool dead = false;

    AirFighter_Controller ac;


    // Start is called before the first frame update
    private void Start()
    {
        ac = GetComponent<AirFighter_Controller>();
        if (ac == null)
        {
            ac = gameObject.AddComponent<AirFighter_Controller>();
        }

        max_hp = 4;
        hp = max_hp;

        GameManager.instance.Enemy_Count();
        player = GameManager.instance.Player;
        //ac.Launch_AriFighter();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!ac.isFring) return;
        if(Distance_Player() < 20)
        {
            if (shootTime == 0)
            {
                target_1_v3 = player.transform.position + new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f));
                target_2_v3 = muzzle.transform.forward * Distance_Player();
            }

            
            if (shootTime > 1.8)
            {
                shootTime = 0;
                muzzle.transform.LookAt(player.transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f)));

                Instantiate(bullet, muzzle.transform.position, muzzle.transform.rotation);
                return;
            }
            shootTime += Time.deltaTime;
        }
    }

    public void Shooting_down()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        GameManager.instance.Enemy_Down_Count();
        Destroy(this.gameObject);

    }

    //ダメージを与える
    public void Damage(float damage)
    {
        //HPからダメージ分減らす
        hp -= damage;
        //撃墜判定
        Down_Chack();
    }

    //死亡したかどうか
    protected void Down_Chack()
    {
        if (this.hp <= 0 && !dead)
        {
            hp = 0;
            dead = true;
            Shooting_down();
        }
    }

    public float Get_Max_Hp()
    {
        return max_hp;
    }
    public float Get_Hp()
    {
        return hp;
    }

    void Shoot()
    {
        if (Distance_Player() < 10)
        {
            shootTime += Time.deltaTime;
            if (shootTime > 1)
            {
                shootTime = 0;
                Instantiate( bullet, muzzle.transform.position, muzzle.transform.rotation);
            }
            
        }
   
        var v = player.transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
        muzzle.transform.LookAt(v);
    }

    float Distance_Player() //プレイヤーとの距離
    { return Vector3.Distance(transform.position, player.transform.position); }


#if UNITY_EDITOR


#endif
}
