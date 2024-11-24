using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class VoteManeger : MonoBehaviour
{

    // 투표 기록 데이터를 투표 이름으로 가져오는 딕셔너리
    // VoteRecord (투표 기록 데이터): Vote(투표 이름, 선택지 데이터), 선택지 선택 누적 수 배열
    public Dictionary<string, VoteRecord> voteRecordDict = new Dictionary<string, VoteRecord>();

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
            int Count = options.Count;

            for (int i = 0; i < Count; i++)
            {
                storage.SetValue($"$selection{i}", options[i].OptionName);
                storage.SetValue($"$selectCount{i}", vote.GetOptionCount(i));
                Debug.Log($"Vote Selection {i}: Option Name = {options[i].OptionName}, Count = {vote.GetOptionCount(i)}");
            }
        }
    

    [YarnCommand]
    public void CheckTieAndSetType3(string voteName, int optionIndex1, int optionIndex2, int optionIndex3 )
    {
        int countA = voteRecordDict[voteName].GetOptionCount(optionIndex1);
        int countB = voteRecordDict[voteName].GetOptionCount(optionIndex2);
        int countC = voteRecordDict[voteName].GetOptionCount(optionIndex3);

        int tieType = 0;

        if (countA == countB && countA > countC) tieType = 1;
        else if (countA == countC && countA > countB) tieType = 2;
        else if (countB == countC && countB > countA) tieType = 3;
        else if (countA == countB && countA == countC) tieType = 4;

        storage.SetValue("$tieType", tieType);
    }
    public void CheckTieAndSetType2(string voteName, int optionIndex1, int optionIndex2)
    {
        // Get the vote counts for the specified options
        int count1 = voteRecordDict[voteName].GetOptionCount(optionIndex1);
        int count2 = voteRecordDict[voteName].GetOptionCount(optionIndex2);

        // Determine if there is a tie
        int tieType = 0; // 0: No tie, 1: Tie
        
        if (count1 == count2)
        {
            tieType = 1; // Tie detected
        }

        // Save the result to Yarn Spinner storage
        storage.SetValue("$tieType", tieType);

    // Log the result for debugging
        Debug.Log($"Tie check between options {optionIndex1} and {optionIndex2}: {(tieType == 1 ? "Tie" : "No tie")}");
    }


    [YarnCommand]
    public void InreaseSelectionCount(string voteName, int index)
    {
        voteRecordDict[voteName].IncreaseOptionCount(index);        
    }

    [YarnCommand]
    public void GetLargestSelect3(string voteName, int optionIndex1, int optionIndex2, int optionIndex3)
    {
        var voteRecord = voteRecordDict[voteName];
        
        int count1 = voteRecord.GetOptionCount(optionIndex1);
        int count2 = voteRecord.GetOptionCount(optionIndex2);
        int count3 = voteRecord.GetOptionCount(optionIndex3);
        int largestSelect = optionIndex1;
        int maxCount = count1;

        if (count2 > maxCount)
        {
            largestSelect = optionIndex2;
            maxCount = count2;
        }
        if (count3 > maxCount)
        {
            largestSelect = optionIndex3;
            maxCount = count3;
        }

        // Save the result
        storage.SetValue("$largestSelect", largestSelect);
    }

    [YarnCommand]
    public void GetLargestSelect2(string voteName, int optionIndex1, int optionIndex2)
    {
        // Retrieve the vote record
        var voteRecord = voteRecordDict[voteName];

        // Retrieve the vote counts for the two options
        int count1 = voteRecord.GetOptionCount(optionIndex1);
        int count2 = voteRecord.GetOptionCount(optionIndex2);

         // Determine the largest selection
        int largestSelect = (count1 >= count2) ? optionIndex1 : optionIndex2;

         // Save the result
         storage.SetValue("$largestSelect", largestSelect);

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



