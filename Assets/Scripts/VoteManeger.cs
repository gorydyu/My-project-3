using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoteManeger : MonoBehaviour
{
    public Dictionary<int, int> voteOptions = new Dictionary<int, int>();

    public Dictionary<string, int> sceneOptions = new Dictionary<string, int>();

    //Dictionary�� ���� �ɼ��� �����ϰ� �����ϴ� ��
    //�� ���� �ɼǼ��� ���� �ϴ� �� ���� �������ϴ� �� �ɼ� ���� �ɼ��� �����ϰ� Dic�� �߰��ϴ� ��
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

