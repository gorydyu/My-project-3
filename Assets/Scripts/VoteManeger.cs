using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

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


    // ���� /////////////////////////////////////////////////////////////////

    // ��ǥ ��� �����͸� ��ǥ �̸����� �������� ��ųʸ�
    // VoteRecord (��ǥ ��� ������): Vote(��ǥ �̸�, ������ ������), ������ ���� ���� �� �迭
    private Dictionary<string, VoteRecord> voteRecordDict = new Dictionary<string, VoteRecord>();

    [SerializeField] private InMemoryVariableStorage storage;

    void Start()
    {
        CreateVoteRecordDict();
    }

    private void CreateVoteRecordDict()
    {
        var voteDataArr = Resources.LoadAll<Vote>("VoteData/");
        
        foreach (Vote data in voteDataArr)
        {
            VoteRecord record = new VoteRecord(data);
            voteRecordDict[data.VoteName] = record;
        }
    }

    [YarnCommand]
    public void GetVoteSelections(string voteName)
    {
        List<VoteOption> options = voteRecordDict[voteName].VoteData.Options;
        for (int i = 0; i < options.Count; i++)
        {
            storage.SetValue($"$selection{i}", options[i].OptionName);
            storage.SetValue($"$selectCount{i}", voteRecordDict[voteName].GetOptionCount(i));
        }
    }

    [YarnCommand]
    public void InreaseSelectionCount(string voteName, int index)
    {
        voteRecordDict[voteName].IncreaseOptionCount(index);
    }

    [YarnCommand]
    public void GetLargestSelect(string voteName)
    {
        List<VoteOption> options = voteRecordDict[voteName].VoteData.Options;
        int largestSelect = 0;
        int maxCount = -1;

        for (int i = 0; i < options.Count; i++)
        {
            int count = voteRecordDict[voteName].GetOptionCount(i);
            if (maxCount < count)
            {
                maxCount = count;
                largestSelect = i;
            }
        }
        storage.SetValue("$largestSelect", largestSelect);
    }

    [YarnCommand]
    public void GetSelectProbability(string voteName, int index)
    {
        int probability = voteRecordDict[voteName].VoteData.Options[index].Probability;
        storage.SetValue("$largestSelectProbability", probability);
    }

    /////////////////////////////////////////////////////////////////////////
}