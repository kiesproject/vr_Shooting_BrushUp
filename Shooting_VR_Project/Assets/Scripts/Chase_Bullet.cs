using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase_Bullet : Bullet
{

    public GameObject player_Poss;
    public GameObject enemy_Poss;

    float t = 0;
    bool flag = false;

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
        Debug.Log("情報は整った");

        if (!flag)
        {
            StartCoroutine(ChaseBullet());
            flag = true;
        }

        try
        {
            var d = enemy_Poss;
            var d2 = player_Poss;
        }
        catch
        {
            enemy_Poss = null;
            player_Poss = null;
            return;
        }
    }

    private IEnumerator ChaseBullet()
    {
        for (int i = 0; i < 30; i++)
        {
            transform.position = Vector3.Lerp(player_Poss.transform.position, enemy_Poss.transform.position, i/30);
            yield return null;
        }
        Damage();
    }

    void Damage()
    {
        var air = enemy_Poss.GetComponent<IShootingDown>();
        //ダメージを与える
        air.Damage(damege);
        Explosion();
    }
}