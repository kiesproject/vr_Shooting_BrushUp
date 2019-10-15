using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField]
    AudioClip[] audios;

    [SerializeField]
    string[] filename;

    [SerializeField]
    GameObject soundObject;

    [SerializeField]
    public bool beepAccept = true;

    Dictionary<string, AudioClip> audioDict = new Dictionary<string, AudioClip>();
    AudioSource source;

    string cullentBGMname;

    [SerializeField]
    private float baseVolume = 1;


    //一番最初に実行
    private void Awake()
    {
        //ゲームマネージャーにアクセス出来るようにする。
        if (instance == null)
        { instance = this; }
        else
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        source.volume = baseVolume;

        for (int i=0; i < filename.Length; i++)
        {
            try
            {
                audioDict[filename[i]] = audios[i];
            }
            catch
            {
                Debug.Log("宣言されていないのでは？？");
            }

        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //音を生成
    public void Instance_Sound(Vector3 wroldPoos, string name, float pitch)
    {
        if (!beepAccept) return;
        GameObject s = Instantiate(soundObject, wroldPoos, Quaternion.identity);
        AudioSource audioSource = s.GetComponent<AudioSource>();
        audioSource.pitch = pitch;

        try
        {
            audioSource.clip = audioDict[name];
        }
        catch
        {
            Debug.Log("音楽ファイルが見つかりません");
        }

        if (audioSource.clip != null)
        {
            s.GetComponent<SoundObject>().GoPlay();
        }

    }

    //BGMを再生
    public void PlayBGM(string name)
    {
        try
        {
            source.clip = audioDict[name];
        }
        catch
        {
            Debug.Log("音楽ファイルが見つかりません");
            return;
        }

        if (source.clip != null)
        {
            cullentBGMname = name;
            source.Play();
        }

    }

    public void StopBGM()
    {
        source.Stop();
    }

    public void SmoothPlayBGM(string name)
    {
        StartCoroutine(Smoothing(name));
    }

    private IEnumerator Smoothing(string name)
    {
        Debug.Log("[SM] source: " + source);
        if (source.clip != null || source.isPlaying)
        {
            for (int i = 10; i > 0; i--)
            {
                source.volume = baseVolume * i / 10;
                yield return new WaitForSeconds(0.1f);
            }
        }

        PlayBGM(name);

        for (int i = 0; i < 10; i++)
        {
            source.volume = baseVolume * i / 10;
            yield return new WaitForSeconds(0.1f);
        }

    }

}
