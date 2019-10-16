using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionUI : MonoBehaviour
{
    public static MissionUI instance;

    [SerializeField]
    Image startUI;

    [SerializeField]
    Image failedUI;

    [SerializeField]
    Image warningUI;

    private void Awake()
    {
        if (instance == null){
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        startUI.color =  new Color(startUI.color.r, startUI.color.g, startUI.color.b, 0);
        failedUI.color = new Color(failedUI.color.r, failedUI.color.g, failedUI.color.b, 0);
        warningUI.color = new Color(warningUI.color.r, warningUI.color.g, warningUI.color.b, 0);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator StartUI()
    {
        for (int i = 0; i < 30; i++)
        {
            startUI.color = new Color(startUI.color.r, startUI.color.g, startUI.color.b, (float)i/30);
            yield return null;
        }

        yield return new WaitForSeconds(3);

        for (int i = 0; i < 30; i++)
        {
            startUI.color = new Color(startUI.color.r, startUI.color.g, startUI.color.b, 1f - (float)i / 30);
            yield return null;
        }
        startUI.color = new Color(startUI.color.r, startUI.color.g, startUI.color.b, 0);

    }

    public IEnumerator FailedUI()
    {
        for (int i = 0; i < 30; i++)
        {
            failedUI.color = new Color(failedUI.color.r, failedUI.color.g, failedUI.color.b, (float)i / 30);
            yield return null;
        }
    }

    public IEnumerator WarnigUI()
    {

        for (int i = 0; i < 60 * 5; i++)
        {
            warningUI.color = new Color(warningUI.color.r, warningUI.color.g, warningUI.color.b, Mathf.Abs(Mathf.Sin(Time.time * 3)));
            yield return null;
        }

        while(warningUI.color.a > 0)
        {
            warningUI.color = new Color(warningUI.color.r, warningUI.color.g, warningUI.color.b, warningUI.color.a - 0.1f);
            yield return null;
        }

    }

    public void Put_StartUI()
    {
        StartCoroutine(StartUI());
    }

    public void Put_FailedUI()
    {
        StartCoroutine(FailedUI());
    }

    public void Put_WarningUI()
    {
        StartCoroutine(WarnigUI());
    }

}
