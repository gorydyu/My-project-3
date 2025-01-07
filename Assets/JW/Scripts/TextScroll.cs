using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextScroll : MonoBehaviour
{
    [SerializeField] private RectTransform content, firstLine;
    [SerializeField] TextMeshProUGUI lineText;
    [SerializeField] private TextMeshProUGUI textLinePrefab;
    private List<TextMeshProUGUI> textLines = new();
    private float heightPerText;
    private int textIndex => textLines.Count - 1;

    void Awake()
    {
        heightPerText = firstLine.rect.height;
    }

    void OnEnable()
    {
        content.anchoredPosition = new Vector2();
    }

    public void AddLine()
    {
        textLines.Add(Instantiate(textLinePrefab, content));
        textLines[textLines.Count - 1].text = lineText.text;

        float totalTextHeight = heightPerText * (textIndex + 1);

        if (totalTextHeight > content.rect.height)
        {
            float offset = totalTextHeight - content.rect.height;

            ScaleContent(offset);
            RepositionContent(offset);
        }

        RepositionLines();
    }

    private void ScaleContent(float offset)
    {
        content.sizeDelta = new Vector2(content.sizeDelta.x, content.sizeDelta.y + offset);
    }

    private void RepositionContent(float offset)
    {
        content.anchoredPosition = new Vector2(content.anchoredPosition.x, content.anchoredPosition.y + offset);
    }

    private void RepositionLines()
    {
        float yPos = GetFirstLineY(content) + heightPerText / 2f;

        for (int i = textLines.Count - 1; i >= 0; i--)
        {
            textLines[i].transform.position = new Vector2(
                firstLine.position.x,
                yPos + heightPerText * (textLines.Count - 1 - i)
            );
        }
    }

    public float GetFirstLineY(RectTransform rect)
    {
        Vector3[] corners = new Vector3[4];
        rect.GetWorldCorners(corners);
        return corners[0].y;
    }
}