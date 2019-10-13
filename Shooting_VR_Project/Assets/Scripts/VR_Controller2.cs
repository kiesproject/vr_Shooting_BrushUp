﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VR_Controller2 : MonoBehaviour
{
    [SerializeField]
    Transform arrow_object;

    Transform player_transform;
    GameManager GM;

    [SerializeField]
    Transform test_parent;

    [SerializeField]
    bool vr = false;

    float hor = 0;
    float ver = 0;

    float delay_hor = 0;
    float delay_ver = 0;

    //
    const float s = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.LogError("どこやねん！！！");
        try
        {
            GM = GameManager.instance;
            player_transform = GM.Player.transform;
            
        }
        catch
        {
            player_transform = test_parent;
        }

    }

    

    // Update is called once per frame
    void Update()
    {
        GetMove_Control(out hor, out ver);

        /*
        if (Mathf.Abs(hor - delay_hor) < 0.001)
            hor = delay_hor;

        if (Mathf.Abs(ver - delay_ver) < 0.001)
            ver = delay_ver;
        */

        if (GM.VR_Swicth)GM.Move_key(hor, ver);
        //Debug.Log("hor: " + hor + " | ver:" + ver);
        
        //delay_hor = hor;
        //delay_ver = ver;
    }

    void GetMove_Control(out float x, out float y)
    {
        x = 0; y = 0;

        //Vector3 dv = arrow_object.localPosition - transform.localPosition;
        Vector3 dv = arrow_object.position - transform.position;
        Debug.DrawLine(transform.position, arrow_object.position, Color.blue);
        Vector3 v = player_transform.InverseTransformDirection(dv);

        var dis = Vector3.Distance(arrow_object.position, transform.position);

        x = v.x / dis;
        y = v.z / dis;

        x = Mathf.Floor(x * 100) / 100;
        y = Mathf.Floor(y * 100) / 100;
 

    }

}
