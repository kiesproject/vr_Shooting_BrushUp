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

    private int routeState = 0;
    private Vector3 targetPoss;
    private Vector3 firstPoss;
    private bool isRouting = false;

    private int dumpState = 0;
    private bool isShooting = false;

    private bool beamMode = false;
    private bool beamModeFlag = false;

    [SerializeField]
    private GameObject beamBase;

    Vector3[] target = {  new Vector3(0,0,0),
                          new Vector3(5, 0, 0),
                          new Vector3(0, 4, -2),
                          new Vector3(0, 0, 5),
                          new Vector3(4, 3, 0),
                          new Vector3(5, -3, 1),
                          new Vector3(0, 0, 5),
    };

    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<Boss_effect>().enabled = false;
        GameManager.instance.Enemy_Count();
        max_hp = 500;
        hp = max_hp;

        firstPoss = transform.position;
        targetPoss = firstPoss + target[0];
        //Danmaku1();
    }

    // Update is called once per frame
    void Update()
    {
        if (beamMode)
        {
            
            if (!beamModeFlag)
            {
                beamModeFlag = true;
                beamBase.transform.LookAt(GameManager.instance.Player.transform);
                StartCoroutine(BeamShoot());
            }
        }
        else
        {
            beamModeFlag = false;
            Move();
            Shoot();
            transform.Rotate(Vector3.up * 3);
        }

        
    }

    private IEnumerator DownAnimPlay()
    {
        for (int i=0; i<6; i++)
        {
            Explosion_mini();
            yield return new WaitForSeconds(0.1f);
        }

        GameManager.instance.GameState = 2;
        Explosion();

    }


    private void Explosion()
    {
        Instantiate(explosion_Boss, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    private void Explosion_mini()
    {
        for (int i = 0; i < 4; i++)
            Instantiate(explosion_Boss_mini, transform.position + new Vector3(Random.Range(-7, 7), Random.Range(-7, 7), Random.Range(-7, 7)), Quaternion.identity);
    }


    private IEnumerator BeamShoot()
    {
        //Instantiate(beem_bullet, beamMuzzles[0].transform.position, beamMuzzles[0].transform.rotation);
        //Instantiate(beem_bullet, beamMuzzles[1].transform.position, beamMuzzles[1].transform.rotation);
        //Instantiate(beem_bullet, beamMuzzles[2].transform.position, beamMuzzles[2].transform.rotation);
        //Instantiate(beem_bullet, beamMuzzles[3].transform.position, beamMuzzles[3].transform.rotation);

        for(int i=0; i<4; i++)
        {
            beamMuzzles[i].SetActive(true);
            beamMuzzles[i].GetComponent<ParticleSystem>().Play();
        }

        for (int i=0; i < 60*3; i++)
        {
            beamBase.transform.Rotate(Vector3.up * 0.5f + Vector3.right * 0.1f);
            yield return null;
        }

        for (int i = 0; i < 4; i++)
        {
            beamMuzzles[i].GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
            beamMuzzles[i].SetActive(false);
        }
        beamMode = false;
    }


    private void Shoot()
    {
        //if (dumpState != routeState) isShooting = true;
        //dumpState = routeState;

        if (isShooting)
        {
            switch (routeState)
            {
                case 1:
                    Danmaku1();
                    isShooting = false;
                    break;


                case 3:
                    Danmaku2();
                    isShooting = false;
                    break;

                case 4:
                    isShooting = false;
                    Danmaku3();
                    break;

                case 5:
                    Danmaku3();
                    isShooting = false;
                    break;



                case 6:
                    if (hp <= max_hp / 2)
                    {
                        beamMode = true;
                        isShooting = false;
                        routeState = 0;
                    }
                    else
                    {
                        Danmaku2();
                        isShooting = false;
                    }
                    break;

                default:
                    Danmaku1();
                    isShooting = false;
                    break;
            }
        }


    }

    private IEnumerator ShootDanmaku1()
    {
        for (int i = 0; i < 100; i++)
        {
            var v = transform.position - GameManager.instance.Player.transform.position;
            Instantiate(bullet, bossMuzzles[0].transform.position, Quaternion.Euler(v));
            Instantiate(bullet, bossMuzzles[1].transform.position, Quaternion.Euler(v));
            Instantiate(bullet, bossMuzzles[2].transform.position, Quaternion.Euler(v));
            Instantiate(bullet, bossMuzzles[3].transform.position, Quaternion.Euler(v));


            yield return new WaitForSeconds(0.5f);
            
        }
        //isShooting = false;
    }

    private IEnumerator ShootDanmaku2()
    {
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(0.4f);
            Danmaku2();
        }
        isShooting = false;
    }

    private IEnumerator ShootDanmaku3()
    {
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(0.4f);
            Danmaku3();
        }
        isShooting = false;
    }

    //移動
    private void Move()
    {
        if (!isRouting && !isShooting)
        {
            StartCoroutine(MoveLine(target[routeState] + firstPoss, 140));
            routeState++;
        }

        if (routeState >= target.Length) routeState = 0;
    }

    private IEnumerator MoveLine(Vector3 targetPoss, float d)
    {
        isRouting = true;
        //Vector3 f = transform.position;

        for (int i = 0; i < d; i++)
        {
            transform.position = Vector3.Lerp(transform.position, targetPoss, i / d);
            yield return null;
        }

        yield return new WaitForSeconds(1);

        isRouting = false;
        isShooting = true;
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
                    var v0 = bossMuzzles[0].transform.rotation.eulerAngles;
                    var v1 = bossMuzzles[1].transform.rotation.eulerAngles;
                    var v2 = bossMuzzles[2].transform.rotation.eulerAngles;
                    var v3 = bossMuzzles[3].transform.rotation.eulerAngles;

                    Instantiate(bullet, bossMuzzles[0].transform.position, Quaternion.Euler(v0 + bossMuzzles[0].transform.localRotation.eulerAngles + new Vector3(10 * j - 90, i * 10 - 50, 0)));
                    Instantiate(bullet, bossMuzzles[1].transform.position, Quaternion.Euler(v1 + bossMuzzles[1].transform.localRotation.eulerAngles + new Vector3(10 * j - 90, i * 10 + 50, 0)));
                    Instantiate(bullet, bossMuzzles[2].transform.position, Quaternion.Euler(v2 + bossMuzzles[2].transform.localRotation.eulerAngles + new Vector3(10 * j - 90, i * 10 + 150, 0)));
                    Instantiate(bullet, bossMuzzles[3].transform.position, Quaternion.Euler(v3 + bossMuzzles[3].transform.localRotation.eulerAngles + new Vector3(10 * j - 90, i * 10 + 230, 0)));

                }
            }
        }
    }

    //弾幕②
    private void Danmaku2()
    {
        if (bullet != null)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    var v0 = bossMuzzles[0].transform.rotation.eulerAngles;
                    var v1 = bossMuzzles[1].transform.rotation.eulerAngles;
                    var v2 = bossMuzzles[2].transform.rotation.eulerAngles;
                    var v3 = bossMuzzles[3].transform.rotation.eulerAngles;

                    Instantiate(bullet, bossMuzzles[0].transform.position, Quaternion.Euler(v0 + bossMuzzles[0].transform.localRotation.eulerAngles + new Vector3(10 * j - 10, i * 5 - 50, 0)));
                    Instantiate(bullet, bossMuzzles[1].transform.position, Quaternion.Euler(v1 + bossMuzzles[1].transform.localRotation.eulerAngles + new Vector3(10 * j - 10, i * 5 + 50, 0)));
                    Instantiate(bullet, bossMuzzles[2].transform.position, Quaternion.Euler(v2 + bossMuzzles[2].transform.localRotation.eulerAngles + new Vector3(10 * j - 10, i * 5 + 150, 0)));
                    Instantiate(bullet, bossMuzzles[3].transform.position, Quaternion.Euler(v3 + bossMuzzles[3].transform.localRotation.eulerAngles + new Vector3(10 * j - 10, i * 5 + 230, 0)));

                }
            }
        }
    }

    //弾幕③
    private void Danmaku3()
    {
        if (bullet != null)
        {
            for (int i = 0; i < 20; i++)
            {
                var v0 = bossMuzzles[0].transform.rotation.eulerAngles;
                var v1 = bossMuzzles[1].transform.rotation.eulerAngles;
                var v2 = bossMuzzles[2].transform.rotation.eulerAngles;
                var v3 = bossMuzzles[3].transform.rotation.eulerAngles;

                Instantiate(bullet, bossMuzzles[0].transform.position, Quaternion.Euler(v0 + bossMuzzles[0].transform.localRotation.eulerAngles + new Vector3(10 * i, i * 10 - 50, 0)));
                Instantiate(bullet, bossMuzzles[1].transform.position, Quaternion.Euler(v1 + bossMuzzles[1].transform.localRotation.eulerAngles + new Vector3(10 * i, i * 10 + 50, 0)));
                Instantiate(bullet, bossMuzzles[2].transform.position, Quaternion.Euler(v2 + bossMuzzles[2].transform.localRotation.eulerAngles + new Vector3(10 * i, i * 10 + 150, 0)));
                Instantiate(bullet, bossMuzzles[3].transform.position, Quaternion.Euler(v3 + bossMuzzles[3].transform.localRotation.eulerAngles + new Vector3(10 * i, i * 10 + 230, 0)));
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
        StartCoroutine(DownAnimPlay());

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
