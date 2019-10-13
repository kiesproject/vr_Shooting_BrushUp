using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class PlayerControl : MonoBehaviour
{
    private GameManager GM;
    private float horizontal;
    private float vertical;

    private float horizontal_r;
    private float vertical_r;

    private PlayerBase playerBase;
    private SteamVR_Action_Pose VR_Action_Pose = SteamVR_Actions._default.Pose;

    [SerializeField]
    private float speed = 1.0f;
    [SerializeField]
    private GameObject PlayerModel;
    

    private Vector3 headVector;

    // Start is called before the first frame update
    void Start()
    {
        GM = GameManager.instance;
        playerBase = transform.parent.gameObject.GetComponent<PlayerBase>(); //PlayerBaseを取得 
    }

    // Update is called once per frame
    void Update()
    {
        //キー入力
        horizontal = GM.Horizontal;
        vertical = GM.Vertical;

        //Debug.Log("hor: "+horizontal+" ver"+vertical);
        
    }

    void FixedUpdate()
    {
        //移動のメソッド
        Move_Player_Meta();
        //RotateHead();
    }

    //操作移動するメソッド
    void Move_Player_Meta()
    {
        //入力情報をリセット
        Vector3 moveVector = Vector3.zero;
        float dx, dy;
        LimitMove(out dx, out dy);

        //入力情報を設定
        moveVector = (Vector3.right - (dx * Vector3.right)) * speed * horizontal - (Vector3.up - (dy * Vector3.up)) * speed * vertical;

        //移動する
        transform.localPosition += moveVector * Time.deltaTime * 5;
        PlayerModel.transform.localPosition = transform.localPosition  -0.04f* Vector3.forward - 0.05f * moveVector;
        //PlayerModel.transform.LookAt(transform.position);

        RotateHead();
        PlayerModel.transform.localRotation = Quaternion.Euler(240 * headVector);
        //PlayerModel.transform.localRotation = Quaternion.Euler(new Vector3(-50 * vertical, 50 * horizontal, -50 * horizontal));
    }

    void VirInputUpdate()
    {
        float d  = 0.04f;
        float d2 = 0.1f;
        float pls = 0.05f;
        float reg = 1.5f;

        if (d2 < horizontal )
        {
            horizontal_r += pls * Mathf.Abs( horizontal);
            if (horizontal_r > 1) horizontal_r = 1;
        }

        /*
        if (d < horizontal && horizontal < d2)
        {
            horizontal_r += pls * 0.5f;
        }*/

        if(horizontal < -d)
        {
            horizontal_r += -pls * Mathf.Abs( horizontal);
            if (horizontal_r < -1) horizontal_r = -1;
        }

        /*
        if (-d2 < horizontal && horizontal < -d)
        {
            horizontal_r += -pls * 0.5f;
        }*/

        //--- --- --- --- ---

        if (d2 < vertical )
        {
            vertical_r += pls * Mathf.Abs( vertical);
            if (vertical_r > 1) vertical_r = 1;
        }

        /*
        if (d < vertical && vertical < d2)
        {
            vertical_r += pls * 0.5f;
        }*/

        if (vertical < -d)
        {
            vertical_r += -pls * Mathf.Abs( vertical);
            if (vertical_r < -1) vertical_r = -1;
        }

        /*
        if (-d2 < vertical && vertical < -d)
        {
            vertical_r += -pls * 0.5f;
        }*/

        //入力外

        if (-d < horizontal && horizontal < d)
        {
            horizontal_r += -horizontal_r / 100;
        }

        if (-d < vertical && vertical < d)
        {
            vertical_r += -vertical_r / 100;
        }


        Debug.Log("ver: " + vertical + " hor: " + horizontal);

        Debug.Log("verr: " + vertical_r + " horr: " + horizontal_r);
    }

    //本体を回す
    void RotateHead()
    {
        float m = 350.0f;
        float d = 5.0f;

        VirInputUpdate();
        var x = Vector3.up * (horizontal_r - headVector.x) / d;
        var y = Vector3.right * (vertical_r - headVector.y) / d;
        var z = Vector3.forward * (horizontal_r - headVector.z) / d;
        //Debug.Log("x: (" + x.x + ", "+ x.y + ", " + x.z +")");
        //Debug.Log("x: " + x+ "y: " + y + "z: " + z);

        //if (Mathf.Abs(x.y) < 0.01f) x = Vector3.zero;
        //if (Mathf.Abs(y.x) < 0.01f) y = Vector3.zero;
        //if (Mathf.Abs(z.z) < 0.01f) z = Vector3.zero;

        //方向ベクトルを設定(ゆっくりもどる)
        headVector = (x + y - z);
        headVector = new Vector3(y.x, x.y, 0);

        //Debug.Log("headVector:" + headVector);

        //プレイヤーモデルを動かす
    }

    //動ける範囲を制限する
    void LimitMove(out float dx, out float dy)
    {
        float limit = 4.0f; //矩形半径
        float a = 0.5f; 

        if ((limit - a <= transform.localPosition.x) && (horizontal > 0))
        {
            float distance = Mathf.Abs((limit - a) - transform.localPosition.x) * (1.0f / a);
            //Debug.Log("distance.x:" + distance);
            if (distance > 1)
            {
                dx = 1;
            }
            else
            {
                dx = distance;
            }
        }
        else if ((transform.localPosition.x <= -limit + a) && (horizontal < 0))
        {

            float distance = Mathf.Abs((-limit + a) - transform.localPosition.x)* (1.0f / a);
            //Debug.Log("distance.x:" + distance);
            if (distance > 1)
            {
                dx = 1;
            }
            else
            {
                dx = distance;
            }
        }
        else
        {
            dx = 0;
        }

        if ((limit - a <= transform.localPosition.y) && (vertical < 0))
        {
            float distance = Mathf.Abs((limit - a) - transform.localPosition.y) * (1.0f / a);
            //Debug.Log("distance.y:" + distance);
            if (distance > 1)
            {
                dy = 1;
            }
            else
            {
                dy = distance;
            }
        }
        else if ((transform.localPosition.y <= -limit + a) && (vertical > 0))
        {

            float distance = Mathf.Abs((-limit + a) - transform.localPosition.y) * (1.0f / a);
            //Debug.Log("distance.y:" + distance);
            if (distance > 1)
            {
                dy = 1;
            }
            else
            {
                dy = distance;
            }
        }
        else
        {
            dy = 0;
        }
    }
}
