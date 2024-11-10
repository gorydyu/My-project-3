using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class VoteManeger : MonoBehaviour
{
//    public Dictionary<int, int> voteOptions = new Dictionary<int, int>();

//    public Dictionary<string, int> sceneOptions = new Dictionary<string, int>();

    //Dictionary로 선거 옵션을 저장하고 보관하는 식
    //이 식은 옵션수를 저장 하는 식 이제 만들어야하는 건 옵션 수로 옵션을 생성하고 Dic에 추가하는 식
//    public void AddNewVoteOp(int voteSceneID, int thisOp)
//    {
//        voteOptions.Add(voteSceneID, thisOp);
//        foreach (var entry in voteOptions)
//        {
//            Debug.Log($"VoteSceneID: {entry.Key}, Option Value: {entry.Value}");
//        }
//    }


//    public void generateVoteOp(string voteOpKey, int zero)
//    {
//        sceneOptions.Add(voteOpKey, zero);
//        foreach (var entry in sceneOptions)
//        {
//            Debug.Log($"VoteSceneOp: {entry.Key}, Option Value: {entry.Value}");
//        }
//    }


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
        var vote = voteRecordDict[voteName];
        List<VoteOption> options = vote.VoteData.Options;
        int halfCount = options.Count / 2;

        for (int i = 0; i < halfCount; i++)
        {
            storage.SetValue($"$selection{i}", options[i].OptionName);
            storage.SetValue($"$selectCount{i}", vote.GetOptionCount(i));
            Debug.Log($"Vote Selection {i}: Option Name = {options[i].OptionName}, Count = {vote.GetOptionCount(i)}");
        }
    }
    
    [YarnCommand]
        public void GetReVoteSelections(string voteName)
    {
        List<VoteOption> options = voteRecordDict[voteName].VoteData.Options;
        int halfCount = options.Count / 2;
        for (int i = halfCount; i < options.Count; i++)
        {
            storage.SetValue($"$reVoteSelection{i - halfCount}", options[i].OptionName);
            storage.SetValue($"$reVoteSelectCount{i - halfCount}", voteRecordDict[voteName].GetOptionCount(i));
            Debug.Log($"Re-Vote Selection {i - halfCount}: Option Name = {options[i].OptionName}, Count = {voteRecordDict[voteName].GetOptionCount(i)}");
        }
    }

        [YarnCommand]
    public void CheckTieAndSetType(string voteName)
    {
        int countA = voteRecordDict[voteName].GetOptionCount(0);
        int countB = voteRecordDict[voteName].GetOptionCount(1);
        int countC = voteRecordDict[voteName].GetOptionCount(2);

        int tieType = 0;

        if (countA == countB && countA > countC) tieType = 1;
        else if (countA == countC && countA > countB) tieType = 2;
        else if (countB == countC && countB > countA) tieType = 3;
        else if (countA == countB && countA == countC) tieType = 4;

        storage.SetValue("$tieType", tieType);
    }


    [YarnCommand]
    public void InreaseSelectionCount(string voteName, int index)
    {
        voteRecordDict[voteName].IncreaseOptionCount(index);
        
    }

 [YarnCommand]
public void GetLargestSelect(string voteName)
{
    // Ensure the vote record exists
    if (!voteRecordDict.ContainsKey(voteName))
    {
        Debug.LogWarning($"Vote '{voteName}' not found.");
        return;
    }

    List<VoteOption> options = voteRecordDict[voteName].VoteData.Options;

    // Get the counts for each option
    int countA = voteRecordDict[voteName].GetOptionCount(0);
    int countB = voteRecordDict[voteName].GetOptionCount(1);
    int countC = voteRecordDict[voteName].GetOptionCount(2);

    int largestSelect = 0;
    int maxCount = -1;
    int tieType = 0;

    // Determine if there's a tie and set the tieType
    if (countA == countB && countA > countC) tieType = 1;  // A == B > C
    else if (countA == countC && countA > countB) tieType = 2;  // A == C > B
    else if (countB == countC && countB > countA) tieType = 3;  // B == C > A
    else if (countA == countB && countA == countC) tieType = 4;  // A == B == C

    // If no tie, find the option with the highest count
    if (tieType == 0)
    {
        for (int i = 0; i < options.Count; i++)
        {
            int count = voteRecordDict[voteName].GetOptionCount(i);
            if (count > maxCount)
            {
                maxCount = count;
                largestSelect = i;
            }
        }
        storage.SetValue("$largestSelect", largestSelect);
    }

    // Save the tieType in storage for Yarn to handle re-vote if necessary
    storage.SetValue("$tieType", tieType);
    Debug.Log($"TieType set to {tieType}, largestSelect set to {largestSelect}");
}

    [YarnCommand]
    public void GetSelectProbability(string voteName, int index)
    {
        int probability = voteRecordDict[voteName].VoteData.Options[index].Probability;
        storage.SetValue("$largestSelectProbability", probability);
    }

    [YarnCommand]
    public void ShowVoteSelections(string voteName)
    {
        var vote = voteRecordDict[voteName];
        List<VoteOption> options = vote.VoteData.Options;
        int Count = options.Count;

        for (int i = 0; i < Count; i++)
        {
            Debug.Log($"Vote Selection {i}: Option Name = {options[i].OptionName}, Count = {vote.GetOptionCount(i)}");
        }

    }

}


