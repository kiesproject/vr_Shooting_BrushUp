using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimMaker : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer[] Sprite;

    bool aiming = false;

    Color color0;
    Color color1;

    // Start is called before the first frame update
    void Start()
    {
        color0 = new Color(0, 1, 0.8f);
        color1 = Color.yellow;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("aiming:" + aiming);
        if (aiming)
        {
            for (int i = 0; i < Sprite.Length; i++)
                Sprite[i].color = new Color(color1.r, color1.g, color1.b, Sprite[i].color.a);
        }
        else
        {
            for (int i = 0; i < Sprite.Length; i++)
                Sprite[i].color = new Color(color0.r, color0.g, color0.b, Sprite[i].color.a);

        }
    }

    private void LateUpdate()
    {
        //aiming = false;
    }

    public void ChangeAiming(bool b)
    {

        aiming = b;
    }
}
