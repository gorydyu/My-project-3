using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoteCounter : MonoBehaviour
{
    public TMPro.TextMeshProUGUI _voteAll;
    public VoteSaver _voteSaver;
    float voteScene = 0;

    // Start is called before the first frame update
    void Start()
    {
           for(float i = 0; i < _voteSaver.voteSceneNum_1; i++ )
        {

        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
