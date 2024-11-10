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
        public void CheckReTieAndSetType(string voteName)
    {
        int countA = voteRecordDict[voteName].GetOptionCount(3);
        int countB = voteRecordDict[voteName].GetOptionCount(4);
        int countC = voteRecordDict[voteName].GetOptionCount(5);

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
    public void GetReLargestSelect(string voteName)
    {
        List<VoteOption> options = voteRecordDict[voteName].VoteData.Options;
        int largestSelect = 0;
        int maxCount = -1;
        int halfcount = options.Count / 2;

        for (int i = halfcount; i < options.Count; i++)
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

    [YarnCommand]
    public void SaveVoteDataToStorage(string voteName)
    {
        // Retrieve the vote data from the dictionary
        if (!voteRecordDict.ContainsKey(voteName))
        {
            Debug.LogWarning($"Vote '{voteName}' not found.");
            return;
        }
        
        var vote = voteRecordDict[voteName];
        List<VoteOption> options = vote.VoteData.Options;

        // Save vote name
        storage.SetValue("$voteName", vote.VoteData.VoteName);
        
        // Save each option's name and count
        for (int i = 0; i < options.Count; i++)
        {
            storage.SetValue($"$option{i + 1}Name", options[i].OptionName);
            storage.SetValue($"$option{i + 1}Count", vote.GetOptionCount(i));
            Debug.Log($"Saved: Option {i + 1} Name = {options[i].OptionName}, Count = {vote.GetOptionCount(i)}");
        }
    }

}



