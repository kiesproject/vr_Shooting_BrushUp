using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase_Bullet : Bullet
{

    public GameObject player_Poss;
    public GameObject enemy_Poss;

    float t = 0;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        if (enemy_Poss == null || player_Poss==null)
        {
            Straight();
            return;
        }

        Debug.Log("aaaaa");
        if(t >= 1)
        {
            Damage();
        }

        transform.position = Vector3.Lerp(player_Poss.transform.position, enemy_Poss.transform.position, t);

        t += Time.deltaTime;
    }

    void Damage()
    {
        var air = enemy_Poss.GetComponent<AirFighter>();
        //ダメージを与える
        air.Damage(damege);
        Explosion();
    }
}