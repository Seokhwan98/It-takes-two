using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SettingButton : MonoBehaviour
{
    private Button button;
    
    private void Awake()
    {
        button = GetComponent<Button>();
    }
    
    private void OnEnable()
    {
        button.onClick.AddListener(OnClick);
    }

    private void OnDisable()
    {
        button.onClick.RemoveAllListeners();
    }

    private void OnClick()
    {
        var settingUI = InterfaceManager.Instance.SettingUI;
        var canvas = settingUI.GetComponent<Canvas>();
        if (canvas != null)
        {
            canvas.enabled = true;    
        }
    }
}

