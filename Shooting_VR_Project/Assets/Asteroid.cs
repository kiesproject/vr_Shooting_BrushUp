using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Transform myTransform = this.transform;

        Vector3 pos = myTransform.position;
        pos.z += 0.1f;

        myTransform.position = pos;

        //this.transform.Rotate(new Vector3(1, 1/2, 1/2));

        this.transform.Rotate(new Vector3(1, 1, 1));
    }
}
