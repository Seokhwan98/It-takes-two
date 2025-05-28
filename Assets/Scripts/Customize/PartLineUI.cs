using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PartLineUI : MonoBehaviour
{
    public PartType partType;

    [SerializeField] private TMP_Text partNameText;
    [SerializeField] private TMP_Text indexText;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;

    private List<MeshPartOption> options;
    private int currentIndex = 0;
    private CustomizationManager customizationManager;

    public void Init(PartType type, List<MeshPartOption> options, CustomizationManager manager)
    {
        partType = type;
        partNameText.text = type.ToString();
        customizationManager = manager;

        this.options = options;
        currentIndex = 0;

        UpdateUI();

        leftButton.onClick.AddListener(OnLeft);
        rightButton.onClick.AddListener(OnRight);
    }

    private void OnLeft()
    {
        if (options.Count == 0) return;
        currentIndex = (currentIndex - 1 + options.Count) % options.Count;
        Apply();
    }

    private void OnRight()
    {
        if (options.Count == 0) return;
        currentIndex = (currentIndex + 1) % options.Count;
        Apply();
    }

    private void Apply()
    {
        customizationManager.ApplyOption(partType, options[currentIndex].id);
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (options.Count == 0)
        {
            indexText.text = "0 / 0";
        }
        else
        {
            indexText.text = $"{currentIndex + 1} / {options.Count}";
        }
    }
}