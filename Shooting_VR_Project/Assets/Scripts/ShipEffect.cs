using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipEffect : MonoBehaviour, IShootingDown
{
    [SerializeField]
    private float max_hp = 2;
    [SerializeField]
    private float hp = 2;
    private bool dead = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Down_Chack();
    }

    public void Damage(float damage)
    {
        //HPからダメージ分減らす
        hp -= damage;
        //撃墜判定
        Down_Chack();
    }

    //死亡したかどうか
    protected void Down_Chack()
    {
        if (this.hp <= 0 && !dead)
        {
            hp = 0;
            dead = true;
            
        }
    }

    public float Get_Max_Hp()
    {
        return max_hp;
    }
    public float Get_Hp()
    {
        return hp;
    }

    public void Shooting_down()
    {
        Debug.Log("シップ死んだ");

    }
}
