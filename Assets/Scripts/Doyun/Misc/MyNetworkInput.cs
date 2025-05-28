using Fusion;
using UnityEngine;

public struct MyNetworkInput : INetworkInput
{
    public const int BUTTON_FORWARD = 0;
    public const int BUTTON_BACKWARD = 1;
    public const int BUTTON_LEFT = 2;
    public const int BUTTON_RIGHT = 3;
    public const int BUTTON_INTERACT = 4;
    public const int BUTTON_END_INTERACT = 5;
    
    public NetworkButtons Buttons;

    public Vector3 forward;
    public Vector3 right;

    public bool IsUp(int button) {
        return Buttons.IsSet(button) == false;
    }

    public bool IsDown(int button) {
        return Buttons.IsSet(button);
    }
}
