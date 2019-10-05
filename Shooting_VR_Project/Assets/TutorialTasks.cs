using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTasks : MonoBehaviour
{
    /* チュートリアルタスクをまとめる体裁だけのクラス */
}


public class TutorialStart: TutorialInterface
{
    public int GetTaskNum()
    {
        return 0;
    }

    public string GetText()
    {
        return "宇宙戦闘機spicaの発射を確認\nこれより敵艦隊との交戦地点にガイドします\nチュートリアルの指示に従い、機体を操作してください/当機体の操縦方法を説明します/";
    }

    public void GetEvent()
    {

    }

    public float GetTransitionTime()
    {
        return 4.0f;
    }
}

public class MovementExplanation: TutorialInterface
{
    public int GetTaskNum()
    {
        return 1;
    }

    public string GetText()
    {
        return "右のコントローラーを立て、左右に傾けることで旋回します/前に傾けると下降し、手前に傾けると上昇します/";
    }

    public void GetEvent()
    {

    }

    public float GetTransitionTime()
    {
        return 8.0f;
    }
}

public class AvoidMeteor: TutorialInterface
{
    public int GetTaskNum()
    {
        return 2;
    }

    public string GetText()
    {
        return "";
    }

    public void GetEvent()
    {

    }

    public float GetTransitionTime()
    {
        return 12.0f;
    }
}

public class WeaponExplanation: TutorialInterface
{
    public int GetTaskNum()
    {
        return 3;
    }

    public string GetText()
    {
        return "";
    }

    public void GetEvent()
    {

    }

    public float GetTransitionTime()
    {
        return 4.0f;
    }
}

public class ShotLaser: TutorialInterface
{
    public int GetTaskNum()
    {
        return 4;
    }

    public string GetText()
    {
        return "";
    }

    public void GetEvent()
    {

    }

    public float GetTransitionTime()
    {
        return 16.0f;
    }
}

public class ChangeingWeaponExplanation: TutorialInterface
{
    public int GetTaskNum()
    {
        return 5;
    }

    public string GetText()
    {
        return "";
    }

    public void GetEvent()
    {

    }

    public float GetTransitionTime()
    {
        return 4.0f;
    }
}

public class ShotMissile: TutorialInterface
{
    public int GetTaskNum()
    {
        return 6;
    }

    public string GetText()
    {
        return "";
    }

    public void GetEvent()
    {

    }

    public float GetTransitionTime()
    {
        return 12.0f;
    }
}

public class HPExplanation: TutorialInterface
{
    public int GetTaskNum()
    {
        return 7;
    }

    public string GetText()
    {
        return "";
    }

    public void GetEvent()
    {

    }

    public float GetTransitionTime()
    {
        return 4.0f;
    }
}

public class TutorialEnd: TutorialInterface
{
    public int GetTaskNum()
    {
        return 8;
    }

    public string GetText()
    {
        return "";
    }

    public void GetEvent()
    {

    }

    public float GetTransitionTime()
    {
        return 4.0f;
    }
}
