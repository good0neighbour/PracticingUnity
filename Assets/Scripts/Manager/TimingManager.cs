using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingManager : MonoBehaviour
{
    public List<GameObject> boxNoteList = new List<GameObject>();

    int[] judgementRecord = new int[5];

    [SerializeField] Transform Center = null;
    [SerializeField] RectTransform[] timingRect = null;
    Vector2[] timingBoxes = null;

    EffectManager theEffect;
    ScoreManager theScoreManager;
    ComboManager theComboManager;
    StageManager theStageManager;
    PlayerController thePlayer;
    StatusManager theStatusManager;
    AudioManger theAudioManger;

    // Start is called before the first frame update
    void Start()
    {
        theAudioManger = AudioManger.instance;
        theEffect = FindObjectOfType<EffectManager>();
        theScoreManager = FindObjectOfType<ScoreManager>();
        theComboManager = FindObjectOfType<ComboManager>();
        theStageManager = FindObjectOfType<StageManager>();
        thePlayer = FindObjectOfType<PlayerController>();
        theStatusManager = FindObjectOfType<StatusManager>();

        //타이밍 박스 설정
        timingBoxes = new Vector2[timingRect.Length];
        for(int i = 0; i < timingRect.Length; i++)
        {
            timingBoxes[i].Set(Center.localPosition.x - timingRect[i].rect.width / 2,
                Center.localPosition.y + timingRect[i].rect.width / 2);
        }
    }

    public bool CheckTiming()
    {
        for(int i = 0; i < boxNoteList.Count; i++)
        {
            float t_notePosX = boxNoteList[i].transform.localPosition.x;
            for(int x=0; x<timingBoxes.Length; x++)
            {
                if (timingBoxes[x].x <= t_notePosX && t_notePosX <= timingBoxes[x].y)
                {
                    //노트 제거
                    boxNoteList[i].GetComponent<Note>().HideNote();
                    if(x<timingBoxes.Length-1)
                        theEffect.NoteHitEffect();
                    //이펙트 연출
                    boxNoteList.RemoveAt(i);
                    theEffect.JudgementEffect(x);

                    if (CheckCanNextPlate())
                    {
                        theScoreManager.IncreaseScore(x); //점수 증가
                        theStageManager.ShowNextPlate(); //판데기 등장
                        theEffect.JudgementEffect(x); //판정 연출
                        judgementRecord[x]++; //판정 기록
                        theStatusManager.CheckShield(); //쉴드 체크
                    }
                    else
                    {
                        theEffect.JudgementEffect(5);
                    }
                    theAudioManger.PlaySFX("Clap");
                    return true;
                }
            }
        }
        theComboManager.ResetCombo();
        theEffect.JudgementEffect(timingBoxes.Length);
        MissRecord();
        return false;
    }
    bool CheckCanNextPlate()
    {
        if(Physics.Raycast(thePlayer.destPos,Vector3.down,out RaycastHit t_hitInfo, 1.1f))
        {
            if (t_hitInfo.transform.CompareTag("BasicPlate"))
            {
                BasicPlate t_plate=t_hitInfo.transform.GetComponent<BasicPlate>();
                if (t_plate.flage)
                {
                    t_plate.flage=false;
                    return true;
                }
            }
        }
        return false;
    }
    public int[] GetJudgementRecord()
    {
        return judgementRecord;
    }
    public void MissRecord()
    {
        judgementRecord[4]++;
        theStatusManager.ResetShieldCombo();
    }
    public void Initialized()
    {
        judgementRecord[0] = 0;
        judgementRecord[1] = 0;
        judgementRecord[2] = 0;
        judgementRecord[3] = 0;
        judgementRecord[4] = 0;
    }
}
