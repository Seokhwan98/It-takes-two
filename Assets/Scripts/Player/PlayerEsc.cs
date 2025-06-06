using System.Runtime.InteropServices.WindowsRuntime;
using Fusion;
using UnityEngine;

public class PlayerEsc : NetworkBehaviour
{
    private GameObject _escUI => InterfaceManager.Instance.EscUI;
    private EscUI _escUIComponent => _escUI.GetComponent<EscUI>();
    private GameObject _pauseUI => InterfaceManager.Instance.PauseUI;
    private bool isPaused;
    
    private void OnEnable()
    {
        EscUI.OnMenuClosedByButton += CloseMenuByEvent;
    }

    private void OnDisable()
    {
        EscUI.OnMenuClosedByButton -= CloseMenuByEvent;
        _pauseUI.SetActive(false);
    }
    
    private void CloseMenuByEvent()
    {
        if (!Object.HasInputAuthority || _escUI == null)
            return;

        if (_escUI.activeSelf)
        {
            RPC_TogglePauseMenus(false);
        }
    }

    private void Update()
    { 
        Debug.Log($"isPaused {isPaused}");
        if (!Object.HasInputAuthority)
            return;
        
        if (Runner != null && Input.GetKeyDown(KeyCode.Escape))
        {
            if (!_escUI.activeSelf && InterfaceManager.Instance.isActive)
                return;
            
            bool shouldOpen = !_escUI.activeSelf;
            RPC_TogglePauseMenus(shouldOpen);
        }
    }
    
    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    private void RPC_TogglePauseMenus(bool open)
    {
        if (Object.HasInputAuthority)
        {
            if (_escUI == null)
                return;

            if (open)
            {
                if (_pauseUI.activeSelf)
                    _pauseUI.SetActive(false);
                
                _escUIComponent.Focus();

            }
            else
            {
                _escUIComponent.Defocus();
                
                if (isPaused)
                    _pauseUI.SetActive(true);
            }
        }
        else
        {
            if (_pauseUI == null)
                return;

            if (open && Runner.GameMode == GameMode.Shared)
                return;

            if (open)
            {
                if (_escUI.activeSelf) return;
                
                _pauseUI.SetActive(true);
            }
            else
            {
                _pauseUI.SetActive(false);
            }
            
            isPaused = open;
        }

        InterfaceManager.Instance.UIActiveCount += open ? 1 : -1;
    }
}
