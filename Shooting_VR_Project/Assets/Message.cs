using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
        "宇宙戦闘機spica搭乗者に向けた\nチュートリアルを開始します。/あなたのミッションは敵艦隊を\n撃墜することです。\nチュートリアルの指示に従い、\n速やかに習得してください。/",
        "まずは当機体の操縦方法を\n説明します。/",
        "右のコントローラーを立て、\n左右に傾けることで旋回します。\n前に傾けると下降し、\n手前に傾けると上昇します。/",
        "小惑星帯が接近しています。\nコントローラーを操作し、\n衝突を回避してください。/",
        "次は搭載武器の使用方法について\n説明します。/右のコントローラーで照準を定め、\nトリガーを引くことでレーザー光線を発射します。/",
        "敵の戦闘ユニットが出現しました。\n先程のレーザー光線で迎撃してください。/",
        "戦闘ユニットを複数確認。\nミサイルを使用し、敵の艦隊を\n一掃してください。/左コントローラーのトリガーで\n仕様武器を切り替えます。\n左モニターの表示がMISSILEと\n表記されたことを確認してください。/",
        "右コントローラーのトリガーを引いて発射します。/",
        "正面に表示されている\n緑色のゲージは当機のエネルギーです。\nゲージがなくなると当機は推進力を失い、\nミッションの続行が不可能となります。/",
        "以上でチュートリアルは終了です。/60秒後目標地点に到達します。\n敵艦隊の制圧に向け準備してください。/"
    };

    private float[] TransitionTime =
    {
        5.0f, 10.0f, 15.0f
    };

    public int nowTaskNum = 1;

    // 使用する分割文字列
    [SerializeField]
    private string splitString = "/";

    // 分割したメッセージ
    private string[] splitMessage;

    // 分割したメッセージの何番目か
    private int messageNum;

    // テキストスピード
    [SerializeField]
    private float textSpeed = 0.05f;

    // 経過時間
    private float elapsedTime = 0f;

    // 今見ている文字番号
    private int nowTextNum = 0;

    // 1回分のメッセージを表示したかどうか
    private bool isOneMessage = false;

    // メッセージをすべて表示したかどうか
    private bool isEndMessage = false;

    // チュートリアル全体の時間計測
    public float tutorialTime = 0f;

    float j;

    public GameObject astroids;

    // Start is called before the first frame update
    void Start()
    {
        messageText = GetComponentInChildren<Text>();
        messageText.text = "";
        SetMessage(Messages[0]);
    }

    // Update is called once per frame
    void Update()
    {
        if(nowTaskNum == 4 || nowTaskNum == 6)
        {
            j = TransitionTime[2];
            //if(nowTaskNum == 3)
            //{
                //astroids.gameObject.SetActive(true);
            //}
        }
        else if (nowTextNum == 8)
        {
            j = TransitionTime[1];
        }
        else
        {
            j = TransitionTime[0];
        }

        // 1回に表示するメッセージを表示していない
        if (isOneMessage)
        {
            tutorialTime += Time.deltaTime;
            elapsedTime += Time.deltaTime;

            if (tutorialTime >= j)
            {
                if (!isEndMessage)
                {
                    messageNum++;
                }
                else
                {
                    if (nowTaskNum == Messages.Length + 1)
                    {
                        transform.GetChild(0).gameObject.SetActive(false);
                        tutorialTime = 0;
                        return;
                    }
                    SetNextMessage(Messages[nowTaskNum]);
                    nowTaskNum++;
                }

                nowTextNum = 0;
                messageText.text = "";
                elapsedTime = 0f;
                tutorialTime = 0f;
                isOneMessage = false;
            }
        }

        else
        {
            // テキスト表示時間を経過したらメッセージを追加
            if (elapsedTime >= textSpeed)
            {
                messageText.text += splitMessage[messageNum][nowTextNum];

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
    void SetMessage(string message)
    {
        this.mainMessage = message;
        // 分割文字列で一回に表示するメッセージを分割する
        splitMessage = Regex.Split(mainMessage, splitString, RegexOptions.IgnorePatternWhitespace);
        nowTextNum = 0;
        messageNum = 0;
        messageText.text = "";
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
