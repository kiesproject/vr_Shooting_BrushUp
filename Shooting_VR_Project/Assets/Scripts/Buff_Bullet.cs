using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Buff_Bullet : Bullet
{
    protected new void OnCollisionEnter(Collision c)
    {
        //当たった物が戦闘機
        var player = c.gameObject.GetComponent<Player>();
        if (player != null)
        {
            var fighter = c.gameObject.GetComponent<IShootingDown>();

            if (CompareLayer(layer, c.gameObject.layer))
            {
                Debug.Log("ダメージ");
                //ダメージを与える
                fighter.Damage(damege);
                player.Set_Debuff();
            }


        }
        Explosion();
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Buff_Bullet))]
    public class BBullet_Editor : Bullet_Editor { }
#endif
}
