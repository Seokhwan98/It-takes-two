using UnityEngine;

public class InputBuffer : MonoBehaviour
{
    public static InputBuffer Instance { get; private set; }

    public float LookYaw { get; private set; }
    public float LookPitch { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject); // 중복 방지
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // 씬 이동해도 유지
    }

    private void Update()
    {
        LookYaw = Input.GetAxis("Mouse Y");
        LookPitch = Input.GetAxis("Mouse X");
    }
}