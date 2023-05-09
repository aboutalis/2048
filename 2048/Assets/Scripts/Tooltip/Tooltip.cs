using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[ExecuteInEditMode]
public class Tooltip : MonoBehaviour
{
    public TextMeshProUGUI content;

    public LayoutElement element;

    public RectTransform rectTransform;

    public int charLimit;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetText(string text)
    {
        content.text = text;
    }

    private void Update()
    {
        if (Application.isEditor)
        {
            int contentLength = content.text.Length;

            element.enabled = (contentLength > charLimit) ? true : false;
        }

        Vector2 pos = Input.mousePosition;

        float pivotX = pos.x / Screen.width - 0.5f;
        float pivotY = pos.y / Screen.height + 1.5f;

        rectTransform.pivot = new Vector2(pivotX, pivotY);

        transform.position = pos;
    }
}
