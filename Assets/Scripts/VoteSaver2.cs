using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn;
using Yarn.Unity;
using Unity.VisualScripting;

public class VoteSaver2 : MonoBehaviour
{
    private InMemoryVariableStorage storage;
    int selection1;
    int voteOption;

    private void Start() => storage = FindObjectOfType<InMemoryVariableStorage>();


    //yarn spinner안에서 직접 찾는 방식
    private int getVoteOption(string variableName)
    {
        int currentValue = 0;
        storage.TryGetValue(variableName, out currentValue);
        return currentValue;
    }
}