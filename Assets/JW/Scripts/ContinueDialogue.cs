using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;

public class ContinueDialogue : MonoBehaviour
{
    [SerializeField] private UnityEvent onContinueEvent;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            NextDialogue();
    }

    private void NextDialogue()
    {
        onContinueEvent?.Invoke();
    }
}
