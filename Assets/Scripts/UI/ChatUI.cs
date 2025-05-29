using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChatUI : UIScreen
{
    public TMP_InputField inputField;
    [SerializeField] private RectTransform content;
    [SerializeField] private GameObject messagePrefab;
    
    private Coroutine _defocusCoroutine;

    public void AddMessage(string text, bool isLocal)
    {
        var go = Instantiate(messagePrefab, content);
        var tmp = go.GetComponentInChildren<TMP_Text>();
        tmp.text = text;

        var rt = go.GetComponent<RectTransform>();

        if (isLocal)
        {
            rt.anchorMin = new Vector2(0f, rt.anchorMin.y);
            rt.anchorMax = new Vector2(0f, rt.anchorMax.y);
            rt.pivot     = new Vector2(0f, rt.pivot.y);

            tmp.alignment = TextAlignmentOptions.Left;
            tmp.margin = new Vector4(0f, 0f, 200f, 0f);
        }
        else
        {
            rt.anchorMin = new Vector2(1f, rt.anchorMin.y);
            rt.anchorMax = new Vector2(1f, rt.anchorMax.y);
            rt.pivot     = new Vector2(1f, rt.pivot.y);

            tmp.alignment = TextAlignmentOptions.Right;
            tmp.margin = new Vector4(200f, 0f, 0f, 0f);
        }
        
        Canvas.ForceUpdateCanvases();
        Focus();
    }

    
    public override void Focus()
    {
        base.Focus();
        
        if (_defocusCoroutine != null)
        {
            StopCoroutine(_defocusCoroutine);
        }
        _defocusCoroutine = StartCoroutine(DefocusProcess());
    }
    
    private IEnumerator DefocusProcess()
    {
        yield return new WaitForSeconds(5f);
        Defocus();
    }
}