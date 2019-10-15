using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEffect : MonoBehaviour
{
    [SerializeField]
    GameObject EffectObject;

    [SerializeField]
    GameObject Effect_Exprosion;
    Transform transformC;


    // Start is called before the first frame update
    void Start()
    {
        transformC = GameManager.instance.Player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        var rnd = (int)Random.Range(0, 10);
        if (rnd == 1) Put_Effect();
    }

    void Put_Effect()
    {
        float d = 400;

        //Vector3 rv = new Vector3(Random.Range(-d, d), Random.Range(-d, d), Random.Range(-d, d));
        float r = Random.Range(0, 270) + 180;
        float k = Random.Range(d, 30);
        float z = Random.Range(-10, 10);
        Vector3 rv = new Vector3(k * Mathf.Sin(r * Mathf.Deg2Rad), z * 20, k * Mathf.Cos(r * Mathf.Deg2Rad));


        Vector3 vector = transformC.position + rv;
        var o = Instantiate(EffectObject, vector, Quaternion.identity);
        StartCoroutine(shoot(o));

    }

    private IEnumerator shoot(GameObject o)
    {
        float d = 40;

        Vector3 rv = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));
        Vector3 first = o.transform.position;
        var rig = o.GetComponent<Rigidbody>();
        rig.velocity = d * rv;

        yield return new WaitForSeconds(1f);
        Instantiate(Effect_Exprosion, o.transform.position, Quaternion.identity);
        Destroy(o);
        

    }

}
