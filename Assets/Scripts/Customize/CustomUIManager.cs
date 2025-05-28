using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomUIManager : MonoBehaviour
{
    [SerializeField] private CustomizationManager customizationManager;
    
    [Header("각 PartType별 UI 라인 직접 연결")]
    [SerializeField] private List<PartLineUI> partLines;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button loadButton;

    private void Start()
    {
        foreach (var line in partLines)
        {
            if (line == null) continue;

            var partType = line.partType;
            if (!customizationManager.PartOptions.TryGetValue(partType, out var options))
            {
                Debug.LogWarning($"[CustomizationUI] 옵션 없음: {partType}");
                continue;
            }

            line.Init(partType, options, customizationManager);
        }
        
        if (customizationManager.HasSavedData())
        {
            customizationManager.LoadCustomization();
            foreach (var line in partLines)
            {
                line.RefreshUI();
            }
        }
    }
    
    public void OnSaveClicked()
    {
        customizationManager.SaveCustomization();
    }

    public void OnLoadClicked()
    {
        customizationManager.LoadCustomization();
        foreach (var line in partLines)
        {
            line.RefreshUI();
        }

        Debug.Log("[CustomizationUI] 저장된 커스터마이징 로드 및 UI 반영 완료");
    }
}
