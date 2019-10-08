using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    Vector3 rotationAngle;

    //public GameObject player;
    //Player PL;

    //[SerializeField]
    //protected float damage = 5;

    // Start is called before the first frame update
    void Start()
    {
        rotationAngle = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        //PL = GameManager.instance.Player.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        //Transform myTransform = this.transform;
        //myTransform.position += new Vector3(0, 0, speed * Time.deltaTime);
        //Vector3 pos = myTransform.position;
        //pos.z += 0.1f;
        //myTransform.position = pos;

        this.transform.Rotate(rotationAngle);
    }

    private void OnTrigerEnter(Collider other)
    {
        if(other.gameObject.name == "PlayerBase")
        {
            Debug.Log("hit");
            //PL.Damage(damage);
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
        //Debug.Log("hit");
    //}
}
