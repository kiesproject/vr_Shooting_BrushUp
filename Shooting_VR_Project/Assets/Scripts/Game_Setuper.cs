using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Setuper : MonoBehaviour
{

    private void Awake()
    {
        GameManager.instance.Player_Update();
        Debug.Log("[Game_Setuper] プレイヤーをセットアップ");
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
