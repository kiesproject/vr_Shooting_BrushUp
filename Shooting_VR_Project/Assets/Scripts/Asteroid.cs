using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid: MonoBehaviour
{
    Vector3 rotationAngle;

    public AudioClip clip;
    AudioSource AS;

    [SerializeField, Tooltip("ダメージ量")]
    float damage = 2;

    // Start is called before the first frame update
    void Start()
    {
        rotationAngle = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        AS = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //隕石の挙動
        this.transform.Rotate(rotationAngle);
    }

    //隕石とプレイヤーの衝突判定
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<IShootingDown>() != null)
        {
            var fighter = other.gameObject.GetComponent<IShootingDown>();

            if (other.gameObject.layer == 9)
            {
                AS.PlayOneShot(clip);
                Debug.Log("ダメージ");
                fighter.Damage(damage);
            }
        }
    }
}
