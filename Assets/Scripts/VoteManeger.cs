using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

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


    // 참고 /////////////////////////////////////////////////////////////////

    // 투표 기록 데이터를 투표 이름으로 가져오는 딕셔너리
    // VoteRecord (투표 기록 데이터): Vote(투표 이름, 선택지 데이터), 선택지 선택 누적 수 배열
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