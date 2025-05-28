using System;
using System.Collections.Generic;
using UnityEngine;

public class CustomUIManager : MonoBehaviour
{
    [SerializeField] private CustomizationManager customizationManager;
    
    [Header("각 PartType별 UI 라인 직접 연결")]
    [SerializeField] private List<PartLineUI> partLines;

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
    }
}
