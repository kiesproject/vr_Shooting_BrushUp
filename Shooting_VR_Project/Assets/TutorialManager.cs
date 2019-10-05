using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class TutorialManager : MonoBehaviour
{
    protected TutorialInterface TI;
    protected List<TutorialInterface> tutorialTasks;

    // メッセージUI
    private Text messageText;
    // 表示するメッセージ
    [SerializeField]
    [TextArea(1, 5)]
    private string mainMessage;
    private string[] Messages =
    {
        ""
    };

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


    private float[] TransitionTime =
    {
        4.0f, 8.0f, 12.0f, 16.0f
    };

    public int taskNum = 1;

    float transitionTime;

    public GameObject astroids;
    public GameObject[] enemys;

    public AudioClip messageSound;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        messageText = GetComponentInChildren<Text>();
        messageText.text = "";
        //TI = tutorialTasks[0];
        //messageText.text += TI.GetText();
        for(int i = 0; i < tutorialTasks.Count; i++)
        {
            Messages[i] += tutorialTasks[i].GetText();
        }
        SetMessage(Messages[0], 0);

        tutorialTasks = new List<TutorialInterface>()
        {
            new TutorialStart(),
            new MovementExplanation(),
            new AvoidMeteor(),
            new WeaponExplanation(),
            new ShotLaser(),
            new ChangeingWeaponExplanation(),
            new ShotMissile(),
            new HPExplanation(),
            new TutorialEnd(),
        };
    }

    // Update is called once per frame
    void Update()
    {
        //TI = tutorialTasks[taskNum];
        //taskNum = TI.GetTaskNum();

        TaskEvent();

        // 1回に表示するメッセージを表示していない
        if (isOneMessage)
        {
            tutorialTime += Time.deltaTime;
            elapsedTime += Time.deltaTime;

            if (tutorialTime >= transitionTime)
            {
                if (!isEndMessage)
                {
                    messageNum++;
                }
                else
                {
                    if (taskNum == Messages.Length)
                    {
                        transform.GetChild(0).gameObject.SetActive(false);
                        tutorialTime = 0;
                        return;
                    }
                    SetMessage(Messages[taskNum], taskNum);
                    taskNum++;
                    
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

    void SetMessage(string message, int num)
    {
        this.mainMessage = message;
        // 分割文字列で一回に表示するメッセージを分割する
        splitMessage = Regex.Split(mainMessage, splitString, RegexOptions.IgnorePatternWhitespace);
        if(num == 0)
        {
            nowTextNum = 0;
            messageText.text = "";
            isOneMessage = false;
        }
        messageNum = 0;
        isEndMessage = false;
    }

    void TaskEvent()
    {
        transitionTime = TI.GetTransitionTime();
        switch (taskNum)
        {
            case 6:
                enemys[0].gameObject.SetActive(true);
                break;
            case 7:
                for (int i = 1; i < enemys.Length; i++)
                {
                    enemys[i].gameObject.SetActive(true);
                }
                break;
        }
    }
}
