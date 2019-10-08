using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public static GameManager instance;
    public GameObject Player;
    public List<GameObject> TargetEnemyList;

    [SerializeField]
    public bool VR_Swicth = false;

    public float timeScale = 1;

    #region 変数群

    //Playerのキー入力

    //===x軸の入力状況===
    private float _Horizontal;

    //===y軸の入力状況===
    private float _Vertical;

    //===発射トリガー===
    [HideInInspector]
    public bool Shoot_Trigger = false;

    //===ミサイル発射トリガー===
    [HideInInspector]
    public bool Missile_Trigger = false;
    

    //===切り替えボタン===
    private bool _Weapon_Switch;

    //===カメラ切り替えボタン===
    private bool _Camera_Switch;

    private float Push_Up_Time = 0;

    //===武装状態===
    //0 : 通常装備
    //1 : ミサイル
    //2 : 特殊装備(切り替え不可) 
    private int _Weapon = 0;

    //x軸読み取り専用
    public float Horizontal
    {
        get { return _Horizontal; }
    }

    //y読み取り専用
    public float Vertical
    {
        get { return _Vertical; }
    }

    //発射トリガー読み取り専用
    //public bool Shoot_Trigger
    

    //武器switch
    public bool Weapon_Switch
    {
        get { return _Weapon_Switch; }
    }

    //カメラ切り替え読み取り専用
    public bool Camera_Switch
    {
        get { return _Camera_Switch; }
    }

    //武器装備状況
    public int Weapon
    {
        get { return _Weapon; }
    }

    #endregion

    public int downCount = 0; //撃墜した数
    [HideInInspector] public int enemyCounter = 0; //敵の数

    //ゲームの遷移状態
    public int GameState = 0;
    private int GameStatedump = 0;

    public void Enemy_Count()
    {
        enemyCounter++;
    }

    public void Enemy_Down_Count()
    {

        downCount++;
        Debug.Log("[GameManager] <"+ gameObject.name +"> カウント後 " + downCount + "体撃破");

    }


    //一番最初に実行
    private void Awake()
    {
        //ゲームマネージャーにアクセス出来るようにする。
        if (instance == null)
        { instance = this; }
        else
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(gameObject);

        Player = GameObject.Find("Player");

    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = timeScale;
        
        {
            //自動で戻す奴
            Up_Weapon_Switch();
            if (!VR_Swicth)
            {
                var x = Input.GetAxis("Horizontal");
                var y = Input.GetAxis("Vertical");

                Move_key(x, y);
            }

            if (Input.GetKey(KeyCode.V))
            { 
                Shoot_Trigger = true; //ショットを発射
                //Debug.Log("発射よ！！");
            }
            else
            {
                Shoot_Trigger = false;
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                Push_Weapon_Switch();
            }

        }


        if (GameState != GameStatedump)
        {
            //リセットが実行された
            if (GameState == 0)
            {
                Player_Update();
            }
        }
        GameStatedump = GameState;
    }

    //--------------------入力-----------------------

    //武器切り替えボタンを押した
    public void Push_Weapon_Switch()
    {
        this._Weapon_Switch = !_Weapon_Switch;
        if (!Weapon_Switch)
        {
            _Weapon = 0;
        }
        else
        {
            _Weapon = 1;
        }

    }

    //ボタンを押した後自動的に戻す
    private void Up_Weapon_Switch()
    {
        //_Weapon_Switch = false;
        //_Shoot_Trigger = false;
        //Missile_Trigger = false;

    }

    //カメラ切り替えボタンを押しているかどうか
    public void Push_Camera_Switch(bool input)
    {
        if (input == true)
        {
            this._Camera_Switch = true;
        }
        else
        {
            input = false;
        }
    }

    //ショットを撃つ
    public void Push_Trigger()
    {
        this.Shoot_Trigger = true;
    }

    //ミサイルを打つ
    public void Push_Missile()
    {
        this.Missile_Trigger = true;
    }


    //移動の入力状況を設定する。
    public void Move_key(float x, float y)
    {
        this._Horizontal = x;
        this._Vertical = y;
    }

    //武器状況
    public void SetWeapon(int num)
    {
        _Weapon = num;
    }
    
    //敵からの死亡報告
    public void TargetEnemyDead(GameObject o)
    {
        //リストから除く
        if (TargetEnemyList.Contains(o))
            TargetEnemyList.Remove(o);
    }

    public void Start_Sort_TargetList()
    {
        /*
        Debug.Log("---Before List---");
        foreach (GameObject o in TargetEnemyList)
        {
            Debug.Log("| " + Distance_PtoE(o));
        }
        */

        int right = TargetEnemyList.Count - 1;
        int left = 0;
        Sort_TargetList(TargetEnemyList, left, right);

        /*
        Debug.Log("---After List---");
        foreach (GameObject o in TargetEnemyList)
        {
            Debug.Log("| " + Distance_PtoE(o));
        }
        */
        
        //UnityEditor.EditorApplication.isPaused = true;
    }

    /// <summary>
    /// EnemyTargetListをソートする
    /// </summary>
    private void Sort_TargetList(List<GameObject> list, int left, int right)
    {
        int pivot;

        if (left < right)
        {
            pivot = partition(list, left, right);
            Sort_TargetList(list, left, pivot - 1);
            Sort_TargetList(list, pivot + 1, right);
        }

    }

    private int partition(List<GameObject> list, int left, int right)
    {
        int i, j, pivot;
        i = left;
        j = right + 1;
        pivot = left;

        //Debug.Log("count: " + list.Count);

        do
        {
            do { i++; /*Debug.Log("297: " + i)*/; if (i > list.Count-1) break; } while (Distance_PtoE(list[i]) < Distance_PtoE(list[pivot]));
            do { j--; /*Debug.Log("298: " + i)*/; } while (Distance_PtoE(list[pivot]) < Distance_PtoE(list[j]));

            if (i < j)
            {
                var dump = list[i];
                list[i] = list[j];
                list[j] = dump;
            }
        } while (i < j);

        var dump2 = list[pivot];
        list[pivot] = list[j];
        list[j] = dump2;

        return j;
    }

    /// <summary>
    /// playerの座標から指定されたEnemeyの座標までの距離を算出する(2乗で扱う)
    /// </summary>
    /// <param name="enemyO"></param>
    /// <returns></returns>
    public float Distance_PtoE(GameObject enemyO)
    {
        Vector3 possP = Player.transform.position;
        Vector3 possE = enemyO.transform.position;
        return (possP - possE).sqrMagnitude;

    }

    /// <summary>
    /// プレイヤーを取得する。()
    /// </summary>
    public void Player_Update()
    {

        Player = GameObject.Find("Player");
    }


}
