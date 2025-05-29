using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Fusion;

public class ConnectionPortal : MonoBehaviour
{
    [SerializeField] private ConnectionData _connectionData;
    
    public static event Action<ConnectionData> OnPortalTriggered;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<NetworkObject>().HasInputAuthority)
        {
            // 플레이어 멈춤
            
            // UI 활성화
            InterfaceManager.Instance.GateUI.Focus();
            OnPortalTriggered?.Invoke(_connectionData);

            // 마우스 활성화

        }
    }
}
