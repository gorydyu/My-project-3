using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;

public class ContinueDialogue : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform viewRect;
    [SerializeField] private GameObject continueButton;
    [SerializeField] private UnityEvent onContinueEvent;
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canContinue)
            NextDialogue();
    }

    private void NextDialogue()
    {
        onContinueEvent?.Invoke();
    }

    private bool canContinue
    {
        get
        {
            if (!continueButton.activeSelf)
                return false;

            if (!canvasGroup.blocksRaycasts)
                return false;

            return IsValidClick();
        }
    }

    private bool IsValidClick()
    {
        return !RectTransformUtility.RectangleContainsScreenPoint(viewRect, Input.mousePosition);
    }
}
