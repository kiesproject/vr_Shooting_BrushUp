using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningAnimationController : MonoBehaviour
{
    public GameObject playerBase;
    public GameObject gateTop, gateBottom;
    public List<GameObject> light;
    Animator gateTopAnim, gateBottomAnim;
    float time;
    float gateOpenTime = 6.0f;
    float lightUpTime = 8.0f;
    Renderer _renderer;

    // Start is called before the first frame update
    void Start()
    {
        gateTopAnim = gateTop.GetComponent<Animator>();
        gateBottomAnim = gateBottom.GetComponent<Animator>();
        GameManager.instance.Player_Update();
    }

    // Update is called once per frame
    void Update()
    {
        //アニメーションのタイミングを管理
        time += Time.deltaTime;

        //ゲートが開くアニメーション
        if (time >= gateOpenTime)
        {
            gateTopAnim.enabled = true;
            gateBottomAnim.enabled = true;
        }

        //ライトを点灯 コルーチンによる遅延処理
        if (time >= lightUpTime)
        {
            StartCoroutine(LightDelayMethod());
        }

        //チュートリアルシーンへ遷移
        if (time >= 19.0f)
        {
            SceneManager.LoadScene("Tutorial");
        }
    }

    IEnumerator LightDelayMethod()
    {
        for (int i = 0; i < (light.Count / 2); i++)
        {
            //手前から(Light(*)の*の値が小さい)順にライトを点灯させる(Emissionのcolorを設定)
            _renderer = light[i * 2].GetComponent<Renderer>();
            _renderer.material.EnableKeyword("_EMISSION");
            _renderer.material.SetColor("_EmissionColor", new Color(191, 191, 75));

            _renderer = light[i * 2 + 1].GetComponent<Renderer>();
            _renderer.material.EnableKeyword("_EMISSION");
            _renderer.material.SetColor("_EmissionColor", new Color(191, 191, 75));

            //0.1秒の遅延して返す
            yield return new WaitForSeconds(0.1f);
        }
    }
}
