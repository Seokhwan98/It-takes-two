using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class SaveLoadPopupUI : MonoBehaviour
{
    [SerializeField] private GameObject root;
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    private Action onYesCallback;

    public void Show(string message, Action onYes)
    {
        root.SetActive(true);
        messageText.text = message;
        onYesCallback = onYes;
    }

    private void Awake()
    {
        yesButton.onClick.AddListener(() =>
        {
            onYesCallback?.Invoke();
            root.SetActive(false);
        });

        noButton.onClick.AddListener(() =>
        {
            root.SetActive(false);
        });
        
        root.SetActive(false); // 시작 시 꺼둠
    }
}