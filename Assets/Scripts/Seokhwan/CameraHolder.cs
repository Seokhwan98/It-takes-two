using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraHolder : MonoBehaviour
{
    public static CameraHolder Instance { get; private set; }

    [SerializeField] private List<CameraSet> cameraSets;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public CameraSet GetCameraSet(int channel)
    {
        return cameraSets.FirstOrDefault(set => set.Channel == channel);
    }
}