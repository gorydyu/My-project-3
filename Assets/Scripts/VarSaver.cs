using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;
using Yarn.Unity;

public class VarSaver : MonoBehaviour
{
    //투표 선택지 저장
    public int voteSceneID;//key
    int options;

    //투표 선택지 변수를 읽어오는 식
    [YarnCommand]
    public void readVoteOption(int option)
    {
        options = option;
        Debug.Log(options);
        VoteManeger voteManager = GameObject.FindObjectOfType<VoteManeger>();
        voteManager.AddNewVoteOp(voteSceneID, options);

        //투표 선택지를 만드는 생성하는 코드
        for (int i = 1; i <= options; i++)
        {
            string voteOpKey = $"Scene_{voteSceneID}_{i}";
            voteManager.generateVoteOp(voteOpKey, 0);

        }
    }
    [YarnCommand]
    public void addVotetoVal(int num)
    {

    }



}



