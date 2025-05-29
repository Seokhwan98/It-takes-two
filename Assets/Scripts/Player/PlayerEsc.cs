using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        
        if (Runner != null
            && SceneManager.GetActiveScene().buildIndex == 3
            && Input.GetKeyDown(KeyCode.Escape))
        {
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
