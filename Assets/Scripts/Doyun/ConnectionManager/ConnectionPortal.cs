using UnityEngine;
using Fusion;

public class ConnectionPortal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<NetworkObject>().HasInputAuthority)
        {
            // 플레이어 멈춤
            
            // UI 활성화
            InterfaceManager.Instance.GateUI.Focus();
            
            // 마우스 활성화
            
        }
    }
}
