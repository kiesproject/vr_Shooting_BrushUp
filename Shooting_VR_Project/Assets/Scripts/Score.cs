using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;



public class Score : MonoBehaviour
{
    public GameObject Score_Board;
    // List型(動的配列)の変数textとscore
    public List<Text> text;
    public List<int> score;

    static public int clearScore;

    string filePath;
    SaveData save;

    // Start is called before the first frame update
    // 起動時の処理

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // ランキングをソートするための処理　テキスト及びスコアの数に合わせ、処理を追加する必要がある
        // キーボードのtキーを押した時

        if (Input.GetKey(KeyCode.T) || GameManager.instance.GameState == 2)
        {
            Sort();
        }
    }

    public void Sort()
    {
        filePath = Application.dataPath + "/" + ".savedata.json";
        save = new SaveData();

        Load();

        score = save.score1;

        clearScore = GameManager.instance.downCount;
        score[0] = clearScore; //クリア時のスコアを代入する

        
        // スコアが1位以上
        if (score[0] >= score[1])
        {
            score[3] = score[2];
            score[2] = score[1];
            score[1] = score[0];
            EditRanking();
        }
        // スコアが1位未満2位以上
        else if (score[0] < score[1] && score[0] >= score[2])
        {
            score[3] = score[2];
            score[2] = score[0];
            EditRanking();
        }
        // スコアが2位未満3位以上
        else if (score[0] < score[2] && score[0] >= score[3])
        {
            score[3] = score[0];
            EditRanking();
        }
        // スコアが3位未満
        else
        {
            EditRanking();
        }

        Score_Board.SetActive(true);
        
    }

    [System.Serializable]
    public class SaveData
    {
        //テスト用の文字列
        public List<int> score1 = new List<int>
        {
            0,0,0,0
        };

    }

    // 新規スコアに対し、ランキングを更新するための処理
    void EditRanking()
    {
        for (int i = 0; i < score.Count; i++)
        {
            text[i].text = score[i].ToString();
        }

        Save();

    }

    public void Save()
    {
        string json = JsonUtility.ToJson(save);

        StreamWriter streamWriter = new StreamWriter(filePath);
        streamWriter.Write(json);
        streamWriter.Flush();
        streamWriter.Close();
    }

    public void Load()
    {
        if (File.Exists(filePath))
        {
            StreamReader streamReader;
            streamReader = new StreamReader(filePath);
            string data = streamReader.ReadToEnd();
            streamReader.Close();

            save = JsonUtility.FromJson<SaveData>(data);
        }
    }
}