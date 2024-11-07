using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoteManeger : MonoBehaviour
{
    public Dictionary<int, int> voteOptions = new Dictionary<int, int>();

    public Dictionary<string, int> sceneOptions = new Dictionary<string, int>();

    //Dictionary로 선거 옵션을 저장하고 보관하는 식
    //이 식은 옵션수를 저장 하는 식 이제 만들어야하는 건 옵션 수로 옵션을 생성하고 Dic에 추가하는 식
    public void AddNewVoteOp(int voteSceneID, int thisOp)
    {
        voteOptions.Add(voteSceneID, thisOp);
        foreach (var entry in voteOptions)
        {
            Debug.Log($"VoteSceneID: {entry.Key}, Option Value: {entry.Value}");
        }
    }


    public void generateVoteOp(string voteOpKey, int zero)
    {
        sceneOptions.Add(voteOpKey, zero);
        foreach (var entry in sceneOptions)
        {
            Debug.Log($"VoteSceneOp: {entry.Key}, Option Value: {entry.Value}");
        }
    }
}

