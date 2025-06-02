using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChatUI : UIScreen
{
    public TMP_InputField inputField;
    [SerializeField] private RectTransform content;
    [SerializeField] private GameObject messagePrefab;
    [SerializeField] private ScrollRect sv;
    
    private Coroutine _defocusCoroutine;

    private void Awake()
    {
        Canvas.ForceUpdateCanvases();
    }

    public void AddMessage(string text, bool isLocal)
    {
        var go = Instantiate(messagePrefab, content);
        var tmp = go.GetComponentInChildren<TMP_Text>();
        tmp.text = text;
        
        Canvas.ForceUpdateCanvases();
        Focus();
        
        bool isRecent = sv.verticalNormalizedPosition < 3f;
        if (isRecent)
        {
            sv.verticalNormalizedPosition = 0f;
        }
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