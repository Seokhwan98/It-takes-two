using Unity.Cinemachine;
using UnityEngine;

public class MixingCameraFOVScaler : MonoBehaviour
{
    [SerializeField] private CinemachineCamera camA;
    [SerializeField] private CinemachineCamera camB;
    
    private float minFOV = 60f;
    private float maxFOV = 90f;
    
    private float xWeight = 1.0f;
    private float yWeight = 2.0f;

    private float xThreshold = 20f;
    private float yThreshold = 5f;
    
    private float startDistance = 0f;   // 변화 시작 거리
    private float endDistance = 60f;    // 변화 끝 거리

    
    void LateUpdate()
    {
        // 거리 계산 (플레이어 or 카메라 기준)
        Vector3 posA = camA.transform.position;
        Vector3 posB = camB.transform.position;

        float dx = Mathf.Abs(posA.x - posB.x);
        float dy = Mathf.Abs(posA.y - posB.y);

        float xFactor = Mathf.Max(0f, dx - xThreshold);
        float yFactor = Mathf.Max(0f, dy - yThreshold);

        float weightedDistance = (xFactor * xWeight) + (yFactor * yWeight);
        
        // 0 ~ 1 사이 보간 비율 계산
        float t = Mathf.InverseLerp(startDistance, endDistance, weightedDistance);
        
        float targetFOV = Mathf.Lerp(minFOV, maxFOV, t);
        
        // Debug.Log($"dx: {dx}, dy: {dy}, weightedDistance: {weightedDistance}, targetFOV: {targetFOV}");
        
        // 둘 다 같은 FOV로 맞춰줌
        camA.Lens.FieldOfView = Mathf.Lerp(camA.Lens.FieldOfView, targetFOV, Time.deltaTime * 5f);
        camB.Lens.FieldOfView = Mathf.Lerp(camB.Lens.FieldOfView, targetFOV, Time.deltaTime * 5f);
    }
}
