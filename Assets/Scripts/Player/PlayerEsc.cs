using Fusion;
using UnityEngine;

public class PlayerEsc : NetworkBehaviour
{
    private GameObject _escUI;
    
    private EscUI _escUIComponent => _escUI.GetComponent<EscUI>();

    private void Awake()
    {
        _escUI = InterfaceManager.Instance.EscUI;
    }

    private void Update()
    {   
        if (!Object.HasInputAuthority)
            return;
        
        if (Runner != null && Input.GetKeyDown(KeyCode.Escape))
        {
            if (!_escUI.activeSelf && InterfaceManager.Instance.isActive)
                return;
            
            ToggleEscMenu();
        }
    }
    
    private void ToggleEscMenu()
    {
        if (_escUI.activeSelf)
        {
            _escUIComponent.Defocus();
        }
        else
        {
            _escUIComponent.Focus();
        }
    }
}
