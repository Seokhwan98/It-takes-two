using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : NetworkBehaviour
{
    [SerializeField] private ChatUI _chatUI;

    private TMP_InputField inputField;
    private bool isFocused;

    private void Start()
    {
        inputField = _chatUI.inputField;
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (!isFocused)
            {
                isFocused = true;
                
                _chatUI.Focus();
                inputField.ActivateInputField();
            }
            else
            {
                var msg = inputField.text.Trim();
                
                if (string.IsNullOrEmpty(msg))
                {
                    isFocused = false;
                    inputField.DeactivateInputField();
                    
                    _chatUI.Defocus();
                }
                else
                {
                    SendChat(msg);
                    inputField.text = "";
                    inputField.ActivateInputField();
                }
            }
        }
    }
    
    public void SendChat(string message)
    {
        if (string.IsNullOrEmpty(message) || !Runner.IsRunning)
            return;

        RpcSendChatMessage(message);
    }
    
    [Rpc(RpcSources.All, RpcTargets.All)]
    private void RpcSendChatMessage(string message, RpcInfo info = default)
    {
        string senderId = info.Source.ToString();
        bool isLocal = info.Source == Runner.LocalPlayer;
        
        _chatUI.AddMessage($"{senderId} \n{message}", isLocal);
    }
}