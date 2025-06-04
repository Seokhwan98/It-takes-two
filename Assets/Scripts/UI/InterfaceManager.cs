using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InterfaceManager : MonoBehaviour
{
    public static InterfaceManager Instance { get; private set; }
    
    public GateUI GateUI;
    public GameObject EscUI;

    public bool isActive;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        MouseEnable();
    }

    public void MouseEnable()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        isActive = true;
    }
    
    public void MouseDisable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isActive = false;
    }
    
    public void ClearInterface()
    {
        UIScreen.activeScreen.Defocus();
    }
}
