using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Vote_Data", menuName = "Vote Data")]
public class Vote : ScriptableObject
{
    [SerializeField] private string voteName;
    [SerializeField] private List<VoteOption> options;
    public string VoteName => voteName;
    public List<VoteOption> Options => options;
}

[System.Serializable]
public class VoteOption
{
    [SerializeField] private string optionName;
    [SerializeField] private int probability;

    public string OptionName => optionName;
    public int Probability => probability;
}

[System.Serializable]
public class VoteRecord
{
    [SerializeField] private Vote voteData;
    [SerializeField] private int[] optionCounts;

    public VoteRecord(Vote voteData)
    {
        this.voteData = voteData;
        optionCounts = new int[voteData.Options.Count];
    }

    public void IncreaseOptionCount(int index)
    {
        if (index >= optionCounts.Length)
        {
            Debug.LogWarning($"해당 투표에는 {index}번째 선택지가 존재하지 않음.");
            return;
        }
        optionCounts[index]++;
    }

    public int GetOptionCount(int index)
    {
        if (index >= optionCounts.Length)
        {
            Debug.LogWarning($"해당 투표에는 {index}번째 선택지가 존재하지 않음.");
            return -1;
        }
        return optionCounts[index];
    }
    
    public Vote VoteData => voteData;
    public int[] OptionCounts => optionCounts;
    
}