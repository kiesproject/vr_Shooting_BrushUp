using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackRe : MonoBehaviour, IShootingDown
{
    public GameObject[] bossMuzzles;
    public GameObject[] beamMuzzles;

    public GameObject bullet;
    public GameObject beem_bullet;
    public GameObject explosion_Boss;
    public GameObject explosion_Boss_mini;

    [SerializeField]
    private float max_hp = 20;
    [SerializeField]
    private float hp = 20;
    private bool dead = false;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Boss_effect>().enabled = false;
        GameManager.instance.Enemy_Count();
        max_hp = 500;
        hp = max_hp;

        Danmaku1();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //弾幕①
    private void Danmaku1()
    {
        if(bullet != null)
        {
            for (int i=0; i < 3; i++)
            {
                for (int j=0; j < 20; j++)
                {
                    Instantiate(bullet, bossMuzzles[0].transform.position, Quaternion.Euler(bossMuzzles[0].transform.localRotation.eulerAngles + new Vector3(10 * j - 10, i * 5 - 50, 0)));
                    Instantiate(bullet, bossMuzzles[1].transform.position, Quaternion.Euler(bossMuzzles[1].transform.localRotation.eulerAngles + new Vector3(10 * j - 10, i * 5 + 50, 0)));
                    Instantiate(bullet, bossMuzzles[2].transform.position, Quaternion.Euler(bossMuzzles[2].transform.localRotation.eulerAngles + new Vector3(10 * j - 10, i * 5 + 150, 0)));
                    Instantiate(bullet, bossMuzzles[3].transform.position, Quaternion.Euler(bossMuzzles[3].transform.localRotation.eulerAngles + new Vector3(10 * j - 10, i * 5 + 230, 0)));

                }
            }
        }
    }

    private void Danmaku2()
    {
        if (bullet != null)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    Instantiate(bullet, bossMuzzles[0].transform.position, Quaternion.Euler(bossMuzzles[0].transform.localRotation.eulerAngles + new Vector3(10 * j - 10, i * 5 - 50, 0)));
                    Instantiate(bullet, bossMuzzles[1].transform.position, Quaternion.Euler(bossMuzzles[1].transform.localRotation.eulerAngles + new Vector3(10 * j - 10, i * 5 + 50, 0)));
                    Instantiate(bullet, bossMuzzles[2].transform.position, Quaternion.Euler(bossMuzzles[2].transform.localRotation.eulerAngles + new Vector3(10 * j - 10, i * 5 + 150, 0)));
                    Instantiate(bullet, bossMuzzles[3].transform.position, Quaternion.Euler(bossMuzzles[3].transform.localRotation.eulerAngles + new Vector3(10 * j - 10, i * 5 + 230, 0)));

                }
            }
        }
    }


    //   +=================================+
    //  ||          IShootingDown          ||
    //   +=================================+

    //ダメージを受ける
    public void Damage(float damage)
    {
        //HPからダメージ分減らす
        hp -= damage;
        //撃墜判定
        Down_Chack();
    }

    //追撃判定
    public void Down_Chack()
    {
        if (this.hp <= 0 && !dead)
        {
            hp = 0;
            dead = true;
            Shooting_down();
        }
    }

    //追撃処理(一度のみ)
    public void Shooting_down()
    {
        GameManager.instance.Enemy_Down_Count();

    }

    public float Get_Max_Hp()
    {
        return max_hp;
    }

    public float Get_Hp()
    {
        return hp;
    }

}
