using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PlayerBase : MonoBehaviour
{
    private GameManager GM;
    private float moveForceMultiplier = 20.0f;
    public Rigidbody Rigidbody { private set; get; }

    AirFighter_Controller ac;

    // Start is called before the first frame update
    private void Start()
    {
        GM = GameManager.instance;

        //Airghter_Controllerを保証する
        ac = GetComponent<AirFighter_Controller>();
        if (ac == null)
        {
            ac = gameObject.AddComponent<AirFighter_Controller>();
        }

        //ac.Launch_AriFighter();

        Rigidbody = GetComponent<Rigidbody>();
        if (Rigidbody == null)
            Rigidbody = gameObject.AddComponent<Rigidbody>();
        Rigidbody.isKinematic = true;
        Rigidbody.useGravity = false;


        var player = GM.Player.GetComponent<Player>();
        player.Write_PlayerSpeed(ac.airFighter_speed);

    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        //移動のメソッド
        //Move_Player();

    }

    //デバッグのみ使用可能
    public void SetSpeed_forDebug(float a)
    {
        ac.airFighter_speed = a;
    }

    //操作移動するメソッド
    void Move_Player()
    {
        transform.position += transform.forward * 0.1f;
    }

#if UNITY_EDITOR

#endif

}



