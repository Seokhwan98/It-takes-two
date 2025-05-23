using Fusion;
using UnityEngine;

public struct NetworkInputData : INetworkInput
{
	public const byte GrabButton = 1;
	public const byte FreeButton = 2;

    public Vector3 direction;
	public NetworkButtons networkButtons;
}
