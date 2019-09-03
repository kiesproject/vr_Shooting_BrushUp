using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningSoundController : MonoBehaviour
{
    float time;
    float gateOpenTime = 6.0f;
    float lightUpTime = 8.0f;
    float engineRunningTime = 10.0f;
    float playerLaunchedTime = 14.0f;

    public AudioClip[] audioClips;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //タイミングを管理
        time += Time.deltaTime;

        //エレベーター上昇音
        if (time >= 1.0f && time <= 1.1f)
        {
            audioSource.PlayOneShot(audioClips[0]);
        }

        //ゲートが開く音
        if (time >= gateOpenTime && time <= gateOpenTime + 0.1f)
        {
            audioSource.PlayOneShot(audioClips[1]);
        }

        //ライト点灯
        if (time >= lightUpTime)
        {
            audioSource.PlayOneShot(audioClips[2]);
        }

        //エンジンブースト音
        if (time >= engineRunningTime && time <= engineRunningTime + 0.1f)
        {
            audioSource.PlayOneShot(audioClips[3]);
        }

        //出撃する際の音
        if (time >= playerLaunchedTime && time <= playerLaunchedTime + 0.1f)
        {
            audioSource.PlayOneShot(audioClips[4]);
        }
    }
}
