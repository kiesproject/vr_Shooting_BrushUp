using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Setuper : MonoBehaviour
{

    private void Awake()
    {
      
        Debug.Log("[Game_Setuper] プレイヤーをセットアップ");
    }


    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.Player_Update();
        GameManager.instance.GameState = 1;
        SoundManager.instance.SmoothPlayBGM("battle");
        MissionUI.instance.Put_StartUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
