﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;

public class DataBaseManager : MonoBehaviour
{
    public int[] score;
    public void SaveScore()
    {
        BackendAsyncClass.BackendAsync(Backend.GameInfo.GetPrivateContents, "ScoreTable", UserDataBro =>
        {
            if (UserDataBro.IsSuccess())
            {
                Param data = new Param();
                data.Add("Scores", score);
                if (UserDataBro.GetReturnValuetoJSON()["rows"].Count > 0)
                {
                    string t_Indate = UserDataBro.GetReturnValuetoJSON()["rows"][0]["inDate"]["S"].ToString();
                    BackendAsyncClass.BackendAsync(Backend.GameInfo.Update, "ScoreTable", t_Indate, data, (t_callback) =>
                    {

                    });
                }
                else
                {
                    BackendAsyncClass.BackendAsync(Backend.GameInfo.Insert, "ScoreTable", data, (t_callback) =>
                       {

                       });
                }
            }
        });
    }
    public void LoadScore()
    {
        BackendAsyncClass.BackendAsync(Backend.GameInfo.GetPrivateContents, "ScoreTable", UserDataBro =>
        {
            JsonData t_data = UserDataBro.GetReturnValuetoJSON();
            if (t_data.Count > 0)
            {
                JsonData t_List = t_data["rows"][0]["Scores"]["L"];
                for (int i = 0; i < t_List.Count; i++)
                {
                    var t_value = t_List[i]["N"];
                    score[i] = int.Parse(t_value.ToString());
                }
                Debug.Log("로드 완료");
            }
            else
            {
                Debug.Log("로드할 것 없음");
            }
        });
    }
}
