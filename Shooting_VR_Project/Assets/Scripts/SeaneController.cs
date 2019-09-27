using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SeaneController : MonoBehaviour
{
    public static SeaneController sceanController;

    //現在のシーンの配列のindex用
    static int nowSceanIndex = 0;

    //テスト用にウェーブごとで一時停止する
    [SerializeField]
    bool testFlag = false;
    [SerializeField]
    string testText;
    GameObject PB; //

    //切り替えるためのシーンの名前（文字列）
    [SerializeField] string[] _sceneSequence;

    //デストロイできないオブジェクトに指定
    private void Awake()
    {
        if (sceanController == null) sceanController = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        nowSceanIndex = 0;

        Scene_LoadOne();

        //デバッグ用
        PB = GameObject.FindGameObjectWithTag("PlayerBase");

    }

    private void Update()
    {
        //Debug.Log("nowSceanIndex " + nowSceanIndex);
        //テスト用にNキーでシーン切り替え
        //if (Input.GetKeyDown(KeyCode.N)) SwitchScean();
    }

    public void Scene_LoadOne()
    {
        if (_sceneSequence.Length == 0) return;
        try
        {
            SceneManager.LoadSceneAsync(_sceneSequence[nowSceanIndex], LoadSceneMode.Additive);
        }
        catch { Debug.Log("Scene Load Failed"); }
    }


    //シーン切り替えメソッド(外部参照可)
    public void SwitchScean()
    {
        if (_sceneSequence.Length == 0) return;


        if (nowSceanIndex != _sceneSequence.Length - 1)
        {
#if UNITY_EDITOR
            if (testFlag)
            {
                if (_sceneSequence[nowSceanIndex + 1] == testText && testText!=null)
                {
                    Scene_Load();
                    EditorApplication.isPaused = true;
                    if (PB == null)
                        Debug.Log("sore");
                    PB.GetComponent<PlayerBase>().SetSpeed_forDebug(0.25f);
                }
                else
                {
                    Scene_Unload();
                }
                nowSceanIndex++;
                return;
            }
#endif
            Scene_Load();
            Scene_Unload();
            nowSceanIndex++;

            
        }
    }

    /// <summary>
    /// 次のロード
    /// </summary>
    void Scene_Load()
    {
        if (_sceneSequence.Length == 0) return;

        try
        {
            SceneManager.LoadSceneAsync(_sceneSequence[nowSceanIndex + 1], LoadSceneMode.Additive);
        }
        catch { Debug.Log("Scene Load Failed"); }
    }

    /// <summary>
    /// 現在のシーンをアンロード
    /// </summary>
    void Scene_Unload()
    {
        if (_sceneSequence.Length == 0) return;

        try
        {
            SceneManager.UnloadSceneAsync(_sceneSequence[nowSceanIndex]);
        }
        catch { Debug.Log("Scene UnLoad Failed"); }
    }

    //ゲームをリセットする
    public void GameReset()
    {
        //var o = GameObject.Find("MissileManager");
        //SceneManager.MoveGameObjectToScene(o, SceneManager.GetActiveScene());
        GameManager.instance.GameState = 0;
        GameManager.instance.Player = null;
        var o2 = GameObject.Find("SceneManager");
        SceneManager.MoveGameObjectToScene(o2, SceneManager.GetActiveScene());

        //Destroy(o);
        Destroy(o2);
        SceneManager.LoadScene("Title");
    }

}
