using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PartRendererEntry
{
    public PartType partType;
    public SkinnedMeshRenderer renderer;
}

public class CustomizationManager : MonoBehaviour
{
    public static CustomizationManager Instance { get; private set; }
    
    public Dictionary<PartType, List<MeshPartOption>> PartOptions { get; private set; }
    public Dictionary<PartType, SkinnedMeshRenderer> PartRenderers;
    public Dictionary<PartType, MeshPartOption> CurrentSelections;
    
    [SerializeField] private List<PartRendererEntry> partRendererEntries;
    
    private ICustomizationStorage storage;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        storage = new LocalCustomizationStorage();
        
        InitializeRendererMap();
        LoadAllOptions();
    }

    private void Start()
    {
        if (HasSavedData()) return;
        ApplyDefaultAll();
    }

    private void InitializeRendererMap()
    {
        PartRenderers = new Dictionary<PartType, SkinnedMeshRenderer>();
        foreach (var entry in partRendererEntries)
        {
            if (!PartRenderers.ContainsKey(entry.partType))
            {
                PartRenderers.Add(entry.partType, entry.renderer);
            }
        }
    }

    private void LoadAllOptions()
    {
        PartOptions = new Dictionary<PartType, List<MeshPartOption>>();
        CurrentSelections = new Dictionary<PartType, MeshPartOption>();

        var allGroups = Resources.LoadAll<PartOptionsGroup>("CustomizationData");
        foreach (var group in allGroups)
        {
            PartOptions[group.partType] = new List<MeshPartOption>(group.options);
        }

        Debug.Log($"[Customization] Loaded {PartOptions.Count} parts.");
    }
    
    public void ApplyDefaultAll()
    {
        foreach (var kvp in PartOptions)
        {
            var partType = kvp.Key;
            var options = kvp.Value;

            if (options.Count == 0) continue;

            var defaultOption = options[0]; // 항상 첫 번째 옵션을 기본값으로 사용
            ApplyOption(partType, defaultOption.id);
        }

        Debug.Log("[Customization] 기본 캐릭터 적용 완료");
    }

    public void ApplyOption(PartType partType, string optionId)
    {
        if (!PartOptions.TryGetValue(partType, out var options)) return;
        var option = options.Find(o => o.id == optionId);
        if (option == null) return;

        if (!PartRenderers.TryGetValue(partType, out var renderer)) return;

        if (option.IsEmpty)
        {
            renderer.gameObject.SetActive(false);
            CurrentSelections.Remove(partType);
        }
        else
        {
            renderer.sharedMesh = option.mesh;
            renderer.gameObject.SetActive(true);
            CurrentSelections[partType] = option;
        }
    }
    
    public void SaveCustomization()
    {
        var data = new CustomizationData
        {
            partSelections = new Dictionary<string, string>()
        };

        foreach (var kvp in CurrentSelections)
        {
            data.partSelections[kvp.Key.ToString()] = kvp.Value.id;
        }

        storage.Save(data);
    }
    
    public void LoadCustomization()
    {
        var data = storage.Load();
        foreach (var kvp in data.partSelections)
        {
            if (Enum.TryParse(kvp.Key, out PartType partType))
            {
                Debug.Log($"[Load] {partType}, {kvp.Value}");
                ApplyOption(partType, kvp.Value);
            }
            else
            {
                Debug.LogWarning($"[Customization] 알 수 없는 파츠 타입: {kvp.Key}");
            }
        }

        Debug.Log("[Customization] 저장된 데이터 로드 완료");
    }

    public bool HasSavedData()
    {
        return storage.HasData();
    }


    public void ClearPart(PartType partType)
    {
        if (PartRenderers.TryGetValue(partType, out var renderer))
        {
            renderer.gameObject.SetActive(false);
        }
        CurrentSelections.Remove(partType);
    }
}