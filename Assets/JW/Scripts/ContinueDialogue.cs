using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;

public class ContinueDialogue : MonoBehaviour
{
    [SerializeField] private GameObject continueButton;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private UnityEvent onContinueEvent;
    void Update()
    {
        if (continueButton.activeSelf && canvasGroup.blocksRaycasts && Input.GetMouseButtonDown(0))
            NextDialogue();
    }

    private void NextDialogue()
    {
        onContinueEvent?.Invoke();
    }
}
