using UnityEngine;

public class InteractionUIUpdater : MonoBehaviour
{
    [field: SerializeField] public int TargetPlayerID { get; private set; }
    [field: SerializeField] public Camera camera { get; private set; }

    [Header("UI Elements")] 
    [SerializeField] private GameObject _interactionUI;
    [SerializeField] private GameObject _endInteractionUI;

    public void SetActiveEndInteractionUI(bool value)
    {
        _endInteractionUI.SetActive(value);
    }

    public void SetActiveInteractionUI(bool value)
    {
        _interactionUI.SetActive(value);
    }
    
    public void SetInteractionUIPositionScreen(Vector2 rectPosition)
    {
        var rect = _interactionUI.GetComponent<RectTransform>();
        rect.anchoredPosition = rectPosition;
    }

    public void SetCamera(Camera camera)
    {
        this.camera = camera;
    }
    
    public void SetInteractionUIPositionWorld(Vector3 world)
    {
        var screenPos = camera.WorldToScreenPoint(world);
        Debug.Log(camera.name);
        SetInteractionUIPositionScreen(screenPos);
    }
}
