using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class Message : MonoBehaviour
{
    // メッセージUI
    private Text messageText;
    // 表示するメッセージ
    [SerializeField]
    [TextArea(1, 5)]
    private string mainMessage;

    private string[] Messages =
    {
        "宇宙戦闘機spicaの発射を確認/味方機の救難信号をもとに座標地点に向け進行中\n目標：敵艦隊の殲滅\n/当機のラーニングプログラムを実行/指示に従い、機体を操作してください/",
        "当機体の操縦方法を説明します/",
        "右のコントローラーを立て、左右に傾けることで旋回します/奥に傾けると下降し、手前に傾けると上昇します/",
        "前方より小惑星帯が接近中\nコントローラーを操作し、衝突を回避してください/",
        "次は搭載武器の使用方法について説明します/右のコントローラーで照準を定め、トリガーを引くことでレーザー光線を発射します/",
        "敵の無人戦闘機体を確認\nレーザー光線で迎撃してください/",
        "敵の戦闘機体を複数確認\nミサイルを使用することで、敵を一掃することができます/左コントローラーのトリガーで武器を切り替えます\n左モニターの表示がMISSILEと表記されたことを確認してください/",
        "右コントローラーのトリガーを引いて発射します/",
        "正面にある緑色のゲージは当機のエネルギーです/ゲージがなくなると当機は推進力を失い、制御不能となります/",
        "ラーニングプログラムの工程は以上となります/60秒後目標地点に到達\n敵艦隊の制圧に向け準備してください/",
        ""
    };

    private float t;
    private float[] TransitionTime =
    {
        5.0f, 8.0f, 12.0f, 16.0f
    };

    private int taskNum = 0;

    // 使用する分割文字列
    [HideInInspector]
    [SerializeField]
    private string splitString = "/";
    // 分割したメッセージ
    private string[] splitMessage;
    // splitStringで分割したメッセージの何番目か
    private int messageNum;
    // メッセージ中の文字1つ1つの番号
    private int nowTextNum = 0;
    // テキストスピード
    [HideInInspector]
    [SerializeField]
    private float textSpeed = 0.05f;
    // テキストスピードに対し、メッセージの1つ1つの文字を表示する指標となる(経過)時間
    // タスクごとのTransitionTimeに対し、次のメッセージへと切り替える指標となる(経過)時間
    private float elapsedTime = 0f;
    // 1回分のメッセージを表示したかどうか(messageNumが増加あるいは0にリセットされる直前にtrue)
    private bool isOneMessage = false;
    // メッセージをすべて表示したかどうか(nowTaskNumが増加する直前にtrue)
    private bool isEndMessage = false;

    public GameObject player;
    public GameObject astroids;
    public GameObject[] enemys;

    AirFighter_Controller AC;
    float speed;

    public AudioClip messageSound;
    AudioSource audioSource;

    bool test = false;

    // Start is called before the first frame update
    void Start()
    {
        messageText = GetComponentInChildren<Text>();
        messageText.text = "";
        SetMessage(Messages[taskNum], taskNum);
        audioSource = GetComponent<AudioSource>();
        AC = player.GetComponent<AirFighter_Controller>();
        speed = AC.airFighter_speed;
    }

    // Update is called once per frame
    void Update()
    {
        TaskEvent();

        // 全てのチュートリアルタスクを終了したら、シーン遷移
        if (taskNum == Messages.Length - 1)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            elapsedTime = 0f;
            GameManager.instance.GameState = 1;
            SceneManager.LoadScene("Main");
            //return;
        }

        //テスト用 PlayerBaseとAsteroidsの座標
        if(test)
        {
            //Debug.Log(player.transform.position);
            //Debug.Log(astroids.transform.position);
            //test = false;
        }

        // 1回に表示するメッセージを表示した
        if (isOneMessage)
        {
            elapsedTime += Time.deltaTime;

            if(elapsedTime >= t)
            {
                if (!isEndMessage)
                {
                    messageNum++;
                }
                else
                {
                    taskNum++;
                    
                    SetMessage(Messages[taskNum], taskNum);
                }

                nowTextNum = 0;
                messageText.text = "";
                elapsedTime = 0f;
                isOneMessage = false;
            }
        }

        else
        {
            // テキスト表示時間を経過したらメッセージを追加
            if (elapsedTime >= textSpeed)
            {
                messageText.text += splitMessage[messageNum][nowTextNum];
                audioSource.PlayOneShot(messageSound);

                nowTextNum++;
                elapsedTime = 0f;

                // メッセージを全部表示、または行数が最大数表示された
                if (nowTextNum >= splitMessage[messageNum].Length)
                {
                    if (messageNum == splitMessage.Length - 2)
                    {
                        isEndMessage = true;
                    }
                    isOneMessage = true;
                }
            }
            elapsedTime += Time.deltaTime;
        }
    }

    // 新しいメッセージを設定
    void SetMessage(string message, int num)
    {
        this.mainMessage = message;
        // 分割文字列で一回に表示するメッセージを分割する
        splitMessage = Regex.Split(mainMessage, splitString, RegexOptions.IgnorePatternWhitespace);
        if (num == 0)
        {
            nowTextNum = 0;
            isOneMessage = false;
        }
        messageNum = 0;
        isEndMessage = false;
    }

    void TaskEvent()
    {
        switch (taskNum)
        {
            case 2:
                t = TransitionTime[1];
                break;
            case 3:
                t = TransitionTime[2];
                //test = true;
                break;
            case 5:
                t = TransitionTime[3];
                enemys[0].gameObject.SetActive(true);
                break;
            case 6:
                t = TransitionTime[0];
                for (int i = 1; i < enemys.Length; i++)
                {
                    enemys[i].gameObject.SetActive(true);
                }
                break;
            case 7:
                t = TransitionTime[1];
                break;
            default:
                t = TransitionTime[0];
                break;
        }
    }

    // 他のスクリプトから新しいメッセージを設定しUIをアクティブにする
    //public void SetMessagePanel(string message)
    //{
    //SetMessage(message);
    //transform.GetChild(0).gameObject.SetActive(true);
    //}
}
