using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDaed : MonoBehaviour
{
    private AirFighter_Controller ac;
    private Rigidbody rig;


    // Start is called before the first frame update
    void Start()
    {
        ac = GetComponent<AirFighter_Controller>();


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Dead()
    {
        rig = gameObject.AddComponent<Rigidbody>();
        rig.useGravity = false; 

    }

}
