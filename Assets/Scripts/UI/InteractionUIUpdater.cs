using UnityEngine;

public class InteractionUIUpdater : MonoBehaviour
{
    [field: SerializeField] public int TargetPlayerID { get; private set; }
    [field: SerializeField] public Camera camera { get; private set; }

    [Header("UI Elements")] 
    [SerializeField] private GameObject _grabInteractionUI;
    [SerializeField] private GameObject _triggerInteractionUI;
    [SerializeField] private GameObject _endInteractionUI;

    public void SetActiveEndInteractionUI(bool value)
    {
        _endInteractionUI.SetActive(value);
    }

    public void SetActiveGrabInteractionUI(bool value)
    {
        _grabInteractionUI.SetActive(value);
    }
    
    public void SetGrabInteractionUIPositionScreen(Vector2 rectPosition)
    {
        var rect = _grabInteractionUI.GetComponent<RectTransform>();
        rect.anchoredPosition = rectPosition;
    }
    
    public void SetGrabInteractionUIPositionWorld(Vector3 world)
    {
        var screenPos = camera.WorldToScreenPoint(world);
        SetGrabInteractionUIPositionScreen(screenPos);
    }
    
    public void SetActiveTriggerInteractionUI(bool value)
    {
        _triggerInteractionUI.SetActive(value);
    }
    
    public void SetTriggerInteractionUIPositionScreen(Vector2 rectPosition)
    {
        var rect = _triggerInteractionUI.GetComponent<RectTransform>();
        rect.anchoredPosition = rectPosition;
    }
    
    public void SetTriggerInteractionUIPositionWorld(Vector3 world)
    {
        var screenPos = camera.WorldToScreenPoint(world);
        // Debug.Log(camera.name);
        SetTriggerInteractionUIPositionScreen(screenPos);
    }

    public void SetCamera(Camera camera)
    {
        this.camera = camera;
    }
    
    
}
