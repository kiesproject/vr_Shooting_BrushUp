using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamaover_Controller : MonoBehaviour
{
    [SerializeField]
    private Light Light;

    [SerializeField]
    private TextMesh text;

    [SerializeField]
    private GlitchFx GlitchFx;

    private bool alert = false;
    private int alertI = 0;

    // Start is called before the first frame update
    void Start()
    {
        GlitchFx.intensity = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.GameState == 3 && !alert)
        {

            alert = true;
            StartCoroutine(UI_controll());
            GlitchFx.intensity = 0.3f;
        }

        if (alert)
        {
            Light.intensity = 9 * Mathf.Sin(Time.time * 5) + 1;
        }

    }

    private IEnumerator UI_controll()
    {
        int mi = 20;
        for (int i=0; i < mi; i++)
        {
            Debug.Log("[GC] 表示: " + (float)i/mi);
            text.color = new Color(1, 0f, 0f,(float)(i + 1) / mi);

            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(15.0f);
        alert = false;
        SeaneController.sceanController.GameReset();


    }

}
