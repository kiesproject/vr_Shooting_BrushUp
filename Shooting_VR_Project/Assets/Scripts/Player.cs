using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Player : MonoBehaviour, IShootingDown
{
    GameManager GM;

    [SerializeField]
    GameObject bullet_N; //標準装備の弾丸

    [SerializeField]
    GameObject muzzle1;
    [SerializeField]
    GameObject muzzle2;

    [SerializeField]
    private PlayerDaed PlayerDaed;

    float time = 0;
    bool shootNegativeFlag = false;

    //float debuffTime = 0; //バフに使用するタイマー
    bool isDebuff = false;
    float debuff_desmove_Per = 1;
    float debuff_desshoot_Per = 1;

    private SteamVR_Action_Boolean Action_Boolean = SteamVR_Actions._default.GrabPinch;

    //---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ----
    [SerializeField]
    private float max_hp = 20;
    [SerializeField]
    private float hp = 20;
    private bool dead = false;

    private float localTime = 0;
    private bool isInvincible;


    private RaycastHit hit;
    private int layerMask;
    private bool rayHit;
    private AimMaker am;

    //private float player_speed;

    private void Awake()
    {
        //GM = GameManager.instance;
        //GameManager.instance.Player = this.gameObject;
    }

    // Start is called before the first frame update
    private void Start()
    {
        GM = GameManager.instance;

        if (GM.Player == this.gameObject)
            GM.Player = this.gameObject;

        max_hp = 40;
        hp = max_hp;

        layerMask = bullet_N.GetComponent<Bullet>().layer;
        am = GetComponent<AimMaker>();
    }

    private void FixedUpdate()
    {
        if (isInvincible)
        {

        }
    }

    IEnumerator Invincible()
    {
        isInvincible = true;
        yield return new WaitForSeconds(3);
        isInvincible = false;
    }

    // Update is called once per frame
    private void Update()
    {
        Debug.Log(isInvincible);
        Raying_Aim();

        if (GM.GameState != 2)
        {
            Input_Shoot();
        }
        Count_LocalTime();

        if (Input.GetKeyDown(KeyCode.G))
        {
            PlayerDaed.Dead();
        }

    }

    private void Raying_Aim()
    {
        am.ChangeAiming(false);

        //Rayの処理
        Ray ray = new Ray(transform.position, transform.forward);
        //Debug.DrawRay(transform.position, transform.forward, Color.red);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            Debug.Log(hit.collider.gameObject.name);

            rayHit = true;
            am.ChangeAiming(true) ;
        }
        else
        {
            rayHit = false;
        }
    }

    //時間をカウントする
    void Count_LocalTime()
    {
        localTime += Time.deltaTime;
    }

    /// <summary>
    /// 時間を返す
    /// </summary>
    public float Get_LocalTime()
    {
        return localTime;
    }

    //ショットの受付
    void Input_Shoot()
    {
        if (GM.Shoot_Trigger  && !shootNegativeFlag)
        {
            switch (GM.Weapon)
            {
                case 0:
                    Normal_Shoot();
                    shootNegativeFlag = true;
                    break;
                case 1:
                    GM.Push_Missile();
                    shootNegativeFlag = true;

                    break;
                default:
                    break;
            }
        }
        else
        {
            GameManager.instance.Missile_Trigger = false;
        }
        
        if (shootNegativeFlag)
        {
            time += Time.deltaTime;
            if (time > 0.3f * debuff_desshoot_Per)
            {
                time = 0;
                shootNegativeFlag = false;
            }
        }

    }

    //通常のショット発射する
    private void Normal_Shoot()
    {
        if (bullet_N == null) return;
        if (muzzle1 == null) return;

        GameObject bullet1 = Instantiate(bullet_N, muzzle1.transform.position, muzzle1.transform.rotation) as GameObject;
        GameObject bullet2 = Instantiate(bullet_N, muzzle2.transform.position, muzzle1.transform.rotation) as GameObject;

    }

    private IEnumerator Chack_Debuff() //デバフの処理を行う
    {
        isDebuff = true;
        debuff_desmove_Per = 0.5f;
        debuff_desshoot_Per = 0.5f;

        yield return new WaitForSeconds(2);

        isDebuff = false;
        debuff_desmove_Per = 1.0f;
        debuff_desshoot_Per = 1.0f;
    }

    public void Set_Debuff()
    {
        StartCoroutine(Chack_Debuff());
    }

    public void Get_Debuff_value(out float m, out float s)
    {
        m = debuff_desmove_Per;
        s = debuff_desshoot_Per;
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


    //撃墜判定
    public void Shooting_down()
    {
        //ゲームオーバー呼び出し
        PlayerDaed.Dead();
    }




    //ダメージを与える
    public void Damage(float damage)
    {
        if (isInvincible) return;

        //HPからダメージ分減らす
        hp -= damage;

        //撃墜判定
        Down_Chack();

        StartCoroutine(Invincible());
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
