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
        "宇宙戦闘機spica搭乗者に向けた\nチュートリアルを開始します。/あなたのミッションは敵艦隊を\n撃墜することです。\nチュートリアルの指示に従い、\n機体を操作してください。/",
        "まずは当機体の操縦方法を\n説明します。/",
        "右のコントローラーを立て、左右に傾けることで旋回します。前に傾けると下降し、手前に傾けると上昇します。/",
        "小惑星帯が接近しています。\nコントローラーを操作し、衝突を回避して\nください。/",
        "次は搭載武器の使用方法について\n説明します。/右のコントローラーで照準を定め、\nトリガーを引くことでレーザー光線を発射します。/",
        "敵の戦闘ユニットが出現しました。\n先程のレーザー光線で迎撃してください。/",
        "戦闘ユニットを複数確認。\nミサイルを使用し、敵の艦隊を\n一掃してください。/左コントローラーのトリガーで\n仕様武器を切り替えます。\n左モニターの表示がMISSILEと\n表記されたことを確認してください。/",
        "右コントローラーのトリガーを引いて発射します。/",
        "正面に表示されている\n緑色のゲージは当機のエネルギーです。\nゲージがなくなると当機は推進力を失い、\nミッションの続行が不可能となります。/",
        "以上でチュートリアルは終了です。/60秒後目標地点に到達します。\n敵艦隊の制圧に向け準備してください。/",
        ""
    };

    private float[] TransitionTime =
    {
        4.0f, 8.0f, 12.0f, 16.0f
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

    // テキストスピード
    [HideInInspector]
    [SerializeField]
    private float textSpeed = 0.05f;

    // テキストスピードに対し、メッセージの1つ1つの文字を表示する指標となる(経過)時間
    // タスクごとのTransitionTimeに対し、次のメッセージへと切り替える指標となる(経過)時間
    private float elapsedTime = 0f;

    // メッセージ中の文字1つ1つの番号
    private int nowTextNum = 0;

    // 1回分のメッセージを表示したかどうか(messageNumが増加あるいは0にリセットされる直前にtrue)
    private bool isOneMessage = false;

    // メッセージをすべて表示したかどうか(nowTaskNumが増加する直前にtrue)
    private bool isEndMessage = false;


    float j;

    public GameObject astroids;
    public GameObject[] enemys;

    public AudioClip messageSound;
    AudioSource audioSource;

    public GameObject player;
    AirFighter_Controller AC;
    float speed;

    // Start is called before the first frame update
    void Start()
    {
        messageText = GetComponentInChildren<Text>();
        messageText.text = "";
        SetMessage(Messages[taskNum]);
        audioSource = GetComponent<AudioSource>();
        AC = player.GetComponent<AirFighter_Controller>();
        speed = AC.airFighter_speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (taskNum == 3)
        {
            j = TransitionTime[2];
        }
        else if(taskNum == 5)
        {
            j = TransitionTime[3];
            enemys[0].gameObject.SetActive(true);
        }
        else if (taskNum == 6)
        {
            j = TransitionTime[0];

            for (int i = 1; i < enemys.Length; i++)
            {
                enemys[i].gameObject.SetActive(true);
            }
        }
        else if (taskNum == 7)
        {
            j = TransitionTime[1];
        }
        else if(taskNum == 10)
        {
            speed = 1.0f;
            //SceneManager.LoadScene("Main");
        }
        else
        {
            j = TransitionTime[0];
        }

        // 1回に表示するメッセージを表示していない
        if (isOneMessage)
        {
            elapsedTime += Time.deltaTime;

            if(elapsedTime >= j)
            {
                if (!isEndMessage)
                {
                    messageNum++;
                }
                else
                {
                    //Debug.Log(Messages.Length);
                    if (taskNum == Messages.Length)
                    {
                        transform.GetChild(0).gameObject.SetActive(false);
                        elapsedTime = 0f;
                        return;
                    }
                    taskNum++;
                    SetNextMessage(Messages[taskNum]);
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
                    //Debug.Log(splitMessage[messageNum].Length);
                    if (messageNum == splitMessage.Length - 2)
                    {
                        isEndMessage = true;
                    }
                    isOneMessage = true;
                    //Debug.Log(splitMessage.Length);
                }
            }
            elapsedTime += Time.deltaTime;
        }
    }

    // 新しいメッセージを設定
    void SetMessage(string message)
    {
        this.mainMessage = message;
        // 分割文字列で一回に表示するメッセージを分割する
        splitMessage = Regex.Split(mainMessage, splitString, RegexOptions.IgnorePatternWhitespace);
        nowTextNum = 0;
        messageNum = 0;
        isOneMessage = false;
        isEndMessage = false;
    }

    void SetNextMessage(string message)
    {
        this.mainMessage = message;
        splitMessage = Regex.Split(mainMessage, splitString, RegexOptions.IgnorePatternWhitespace);
        messageNum = 0;
        isEndMessage = false;
    }

    // 他のスクリプトから新しいメッセージを設定しUIをアクティブにする
    public void SetMessagePanel(string message)
    {
        SetMessage(message);
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
