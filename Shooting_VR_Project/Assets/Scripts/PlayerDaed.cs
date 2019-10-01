using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDaed : MonoBehaviour
{
    [SerializeField]
    private Player player;

    [SerializeField]
    private GameObject explosion;

    private AirFighter_Controller ac;
    private Rigidbody rig;

    private Vector3 velocity;


    // Start is called before the first frame update
    void Start()
    {
        ac = GetComponent<AirFighter_Controller>();
        //rig = GetComponent<Rigidbody>();
        velocity = Vector3.forward * 9;

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        
    }

    public void Dead()
    {
        if (GameManager.instance.GameState != 1) return;
        if (GetComponent<Rigidbody>() != null)
        {
            Debug.Log("[PlayerDead] キャンセル");
            return;
        }

        rig = gameObject.AddComponent<Rigidbody>();
        rig = GetComponent<Rigidbody>();
        ac.Dead();

        float d = 1.5f;
        Vector3 vector = new Vector3(Random.Range(-d, d), Random.Range(-d, d), Random.Range(-d, d));
        rig.velocity = velocity + vector;
        
        rig.useGravity = false;
        rig.constraints = RigidbodyConstraints.FreezeRotationZ;
        rig.AddForceAtPosition(transform.forward * 10, transform.position + Vector3.one);

        StartCoroutine(Deading());
    }

    private IEnumerator Deading()
    {
        float d = 1.0f;
        GameManager.instance.GameState = 3;

        for (int i = 0; i < 10; i++)
        {
            Vector3 vector = new Vector3(Random.Range(-d, d), Random.Range(-d, d), Random.Range(-d, d));
            Instantiate(explosion, player.transform.position + player.transform.forward * 4 + vector, Quaternion.identity);
            yield return new WaitForSeconds(0.3f);
        }

        
    }

}