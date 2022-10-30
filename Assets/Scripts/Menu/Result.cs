using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    [SerializeField] GameObject goUI = null;
    [SerializeField] Text[] txtCount = null;
    [SerializeField] Text txtCoin = null;
    [SerializeField] Text txtScore = null;
    [SerializeField] Text txtMaxCombo = null;

    int currentSong = 0; public void SetCurrentSong(int p_songNum) { currentSong = p_songNum; }

    ScoreManager theScore;
    ComboManager theCombo;
    TimingManager theTiming;
    DataBaseManager theDataBase;
    // Start is called before the first frame update
    void Start()
    {
        theScore = FindObjectOfType<ScoreManager>();
        theCombo = FindObjectOfType<ComboManager>();
        theTiming = FindObjectOfType<TimingManager>();
        theDataBase = FindObjectOfType<DataBaseManager>();
    }
    public void ShowResult()
    {
        FindObjectOfType<CenterFrame>().ResetMusic();
        AudioManger.instance.StopBGM();
        goUI.SetActive(true);
        for (int i = 0; i < txtCount.Length; i++)
            txtCount[i].text = "0";
        txtCoin.text = "0";
        txtScore.text = "0";
        txtMaxCombo.text = "0";

        int[] t_judgement = theTiming.GetJudgementRecord();
        int t_currentScore = theScore.GetCurrentScore();
        int t_maxCombo = theCombo.GetMaxCombo();
        int t_coin = (t_currentScore / 50);
        for(int i = 0; i < txtCount.Length; i++)
        {
            txtCount[i].text = string.Format("{0:#,##0}", t_judgement[i]);
        }
        txtScore.text = string.Format("{0:#,##0}", t_currentScore);
        txtMaxCombo.text = string.Format("{0:#,##0}", t_maxCombo);
        txtCoin.text = string.Format("{0:#,##0}", t_coin);

        if (t_currentScore > theDataBase.score[currentSong])
        {
            theDataBase.score[currentSong] = t_currentScore;
            theDataBase.SaveScore();
        }
    }
    public void BtnMainMenu()
    {
        goUI.SetActive(false);
        GameManager.instance.MainMenu();
        theCombo.ResetCombo();
    }
}
