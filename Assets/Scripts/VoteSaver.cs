using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn;
using Yarn.Unity;

public class VoteSaver : MonoBehaviour
{
    
    public InMemoryVariableStorage variableStorage;
    public DialogueRunner dialogueRunner;
    public float voteSceneNum_1;
    public float[] selectionValues;
    public int voteSceneInt_1;

    private void Start()
    {
    
        variableStorage = FindObjectOfType<InMemoryVariableStorage>();
        dialogueRunner = FindObjectOfType<DialogueRunner>();
        variableStorage.TryGetValue("$voteSceneNum_1", out voteSceneNum_1);
        voteSceneInt_1 = Mathf.RoundToInt(voteSceneNum_1);
        Debug.Log(voteSceneNum_1);

        selectionValues = new float[voteSceneInt_1];
        for (int i = 1; i <= voteSceneInt_1; i++)
        {
            string variableName = $"$selection1_{i}";
            float retrievedValue;

            if (variableStorage.TryGetValue(variableName, out retrievedValue))
            {
                selectionValues[i - 1] = retrievedValue; // Store the value in the array
                Debug.Log($"Retrieved {variableName}: {retrievedValue}");
            }
            Debug.Log(selectionValues[1]);
        }
        

    }


    //[YarnCommand]
    //public void voteSceneNum_1(int voteSceneNum_1)
    //{
    //    Debug.Log(voteSceneNum_1);
    //}

    //[YarnCommand] 
    //public void SaveVote1_1(int selection1_1)
    //{
    //    Debug.Log(selection1_1);
    //}

    //[YarnCommand]
    //public void SaveVote1_2(int selection1_2)
    //{
    //    Debug.Log(selection1_2);
    //}
    //[YarnCommand] 
    //public void SaveVote1_3(int selection1_3)
    //{
    //    Debug.Log(selection1_3);
    //}
    //List<int> stringList = new List<int>();



}


