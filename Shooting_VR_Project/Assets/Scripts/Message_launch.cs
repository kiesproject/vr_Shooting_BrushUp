using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class Message_launch : MonoBehaviour
{
    // メッセージUI
    private Text messageText;
    // 表示するメッセージ
    [SerializeField]
    [TextArea(1, 5)]
    private string mainMessage;

    private string[] Messages =
    {
        "EMERGENCY 緊急事態、味方補給艦隊が敵戦闘機群を確認\n現在交戦中、目的地はコロニーの残骸付近/",
        "君の任務は敵勢力の掃討及び、敵巨大兵器の討伐だ/",
        "敵巨大兵器の情報は皆無だ、十分に気をつけて出撃せよ/",
        "健闘を祈る/"
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
    private float textSpeed = 4.0f;
    // テキストスピードに対し、メッセージの1つ1つの文字を表示する指標となる(経過)時間
    // タスクごとのTransitionTimeに対し、次のメッセージへと切り替える指標となる(経過)時間
    private float elapsedTime = 6f;
    // 1回分のメッセージを表示したかどうか(messageNumが増加あるいは0にリセットされる直前にtrue)
    private bool isOneMessage = false;
    // メッセージをすべて表示したかどうか(nowTaskNumが増加する直前にtrue)
    private bool isEndMessage = false;


    bool test = false;

    // Start is called before the first frame update
    void Start()
    {
        messageText = GetComponentInChildren<Text>();
        messageText.text = "";
        SetMessage(Messages[taskNum], taskNum);
    }

    // Update is called once per frame
    void Update()
    {

        // 全てのチュートリアルタスクを終了したら、シーン遷移
        if (taskNum == Messages.Length - 1)
        {
            Destroy(this.gameObject);
        }


            //テスト用 PlayerBaseとAsteroidsの座標
            if (test)
        {
            //Debug.Log(player.transform.position);
            //Debug.Log(astroids.transform.position);
            //test = false;
        }

        // 1回に表示するメッセージを表示した
        if (isOneMessage)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= t)
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

   
}
