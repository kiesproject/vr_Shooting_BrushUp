using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningAnimationController : MonoBehaviour
{
    public GameObject playerBase;
    public GameObject gateTop, gateBottom;
    public List<GameObject> light;
    Animator gateTopAnim, gateBottomAnim;
    float time;
    float gateOpenTime = 6.0f;
    float lightUpTime = 8.0f;
    float engineRunningTime = 9.0f;
    float playerLaunchedTime = 12.0f;
    Renderer _renderer;

    // Start is called before the first frame update
    void Start()
    {
        gateTopAnim = gateTop.GetComponent<Animator>();
        gateBottomAnim = gateBottom.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time >= gateOpenTime)
        {
            gateTopAnim.enabled = true;
            gateBottomAnim.enabled = true;
        }
        if (time >= lightUpTime)
        {
            StartCoroutine(LightDelayMethod());
        }
        if (time >= engineRunningTime)
        {

        }
        if (time >= 8.0f)
        {
            //PB.enabled = true;
        }
    }

    IEnumerator LightDelayMethod()
    {
        for (int i = 0; i < (light.Count / 2); i++)
        {
            _renderer = light[i * 2].GetComponent<Renderer>();
            _renderer.material.EnableKeyword("_EMISSION");
            _renderer.material.SetColor("_EmissionColor", new Color(191, 191, 75));
            _renderer = light[i * 2 + 1].GetComponent<Renderer>();
            _renderer.material.EnableKeyword("_EMISSION");
            _renderer.material.SetColor("_EmissionColor", new Color(191, 191, 75));
            yield return new WaitForSeconds(0.1f);
        }
    }
}
