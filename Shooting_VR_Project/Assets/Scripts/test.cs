using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public LayerMask mask;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray myRay = new Ray(transform.position, transform.forward);
        Debug.DrawRay(myRay.origin, myRay.direction * 20, Color.red, 3, false);
        if (Physics.Raycast(myRay, out hit, Mathf.Infinity, mask))
        {
            //hit.collider.gameObject
            Debug.Log("検出！！！！");
        }
    }
}
