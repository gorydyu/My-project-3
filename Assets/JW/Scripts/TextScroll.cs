using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class TextScroll : MonoBehaviour
{
    #region VARIABLE_EXPLAIN
    /*
        left: First index of current window (current view, which has the size of textLines.Count)
        right: Last index of current window
    */
    #endregion

    public enum RectCorner
    {
        BOTTOMLEFT,
        TOPLEFT,
        TOPRIGHT,
        BOTTOMRIGHT
    };

    [SerializeField] private RectTransform content;
    [SerializeField] private TextMeshProUGUI lineDisplay, nameDisplay, textLinePrefab;
    private LinkedList<TextMeshProUGUI> textLines = new();
    private List<string> textList = new();
    private float heightPerText, viewBottomY, viewTopY;
    private int left = 0, right = 0;
    private int currentScrollIndex = 0;
    private int textIndex => textList.Count - 1;

    void Start()
    {
        Init();
    }

    void OnEnable()
    {
        content.anchoredPosition = new Vector2();
    }

    private void Init()
    {
        Transform firstLine = content.GetChild(0);

        heightPerText = firstLine.GetComponent<RectTransform>().rect.height;
        textLines.AddLast(firstLine.GetComponent<TextMeshProUGUI>());

        int lineCount = (int)(content.rect.height / heightPerText) + 4;
        float yPos = GetCorner(content, RectCorner.BOTTOMLEFT).y + heightPerText / 2f;

        for (int i = 1; i < lineCount; i++)
        {
            TextMeshProUGUI line = Instantiate(textLinePrefab, content);
            textLines.AddLast(line);
            
            line.transform.position = new Vector2(
                firstLine.position.x,
                yPos + heightPerText * i
            );
        }

        viewBottomY = GetCorner(content, RectCorner.BOTTOMLEFT).y;
        viewTopY    = GetCorner(content, RectCorner.TOPLEFT).y;

        left = 0;
        right = textLines.Count - 1;
    }

    public void AddLine()
    {
        AddText(lineDisplay.text);

        if (textList.Count > textLines.Count)
        {
            left++;
            right++;
        }

        float totalTextHeight = heightPerText * (textIndex + 1);

        if (totalTextHeight >= content.rect.height)
        {
            float offset = totalTextHeight - content.rect.height;

            ScaleContent(offset);
            RepositionContent(offset);
            RepositionLines(offset / 2);
        }

        SetLines();
    }

    private void ScaleContent(float offset)
    {
        content.sizeDelta = new Vector2(content.sizeDelta.x, content.sizeDelta.y + offset);
    }

    private void RepositionContent(float offset)
    {
        content.anchoredPosition = new Vector2(content.anchoredPosition.x, content.anchoredPosition.y + offset);
    }

    private void RepositionLines(float offset)
    {
        foreach (var line in textLines)
        {
            line.transform.position = new Vector2(
                line.transform.position.x,
                line.transform.position.y - offset
            );
        }
    }

    private void AddText(string lineText)
    {
        if (nameDisplay.gameObject.activeSelf)
            textList.Add($"{nameDisplay.text}:\n{lineText}");
        else
            textList.Add($"{lineText}");
    }

    private void SetLines()
    {
        int i = Mathf.Min(right, textList.Count - 1);

        foreach (var line in textLines)
        {
            if (i < left)
                break;
            
            line.text = textList[i--];
        }
    }

    private void WrapLines(bool scrollUp)
    {
        if (scrollUp && left > 0 && textLines.Last.Value.transform.position.y - heightPerText / 2 <= viewTopY)
        {
            TextMeshProUGUI firstLine = textLines.First.Value;
            textLines.RemoveFirst();

            float lastLineYPos = textLines.Last.Value.transform.position.y;

            firstLine.transform.position = new Vector2(
                firstLine.transform.position.x,
                lastLineYPos + heightPerText
            );

            textLines.AddLast(firstLine);
            left--;
            right--;
        }

        else if (right < textList.Count - 1 && textLines.First.Value.transform.position.y + heightPerText / 2 >= viewBottomY)
        {
            TextMeshProUGUI lastLine = textLines.Last.Value;
            textLines.RemoveLast();

            float firstLineYPos = textLines.First.Value.transform.position.y;

            lastLine.transform.position = new Vector2(
                lastLine.transform.position.x,
                firstLineYPos - heightPerText
            );

            textLines.AddFirst(lastLine);
            left++;
            right++;
        }
        
        SetLines();
    }

    public void SetScrollIndex(float normPos)
    {
        int index = Mathf.CeilToInt(normPos * textList.Count);
        index = Mathf.Clamp(index, 0, textList.Count - 1);

        if (currentScrollIndex == index)
            return;

        WrapLines(currentScrollIndex < index);
        currentScrollIndex = index;
    }

    public static Vector3 GetCorner(RectTransform rect, RectCorner type)
    {
        Vector3[] corners = new Vector3[4];
        rect.GetWorldCorners(corners);
        return corners[(int)type];
    }
}