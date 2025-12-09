using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class SpawnCanvasOnWall : SingletonObject<SpawnCanvasOnWall>
{
    [Header("Raycast Settings")]
    public float maxDistance = 100.0f;
    public LayerMask wallLayer;
    public Transform rayOrigin;

    [Header("Spawn Settings")]
    public GameObject canvasPrefab;

    [Header("Layout Settings")]
    public float wallOffset = 0.001f;

    public void TrySpawnCanvas()
    {
        Vector3 startPos = rayOrigin ? rayOrigin.position : transform.position;
        Vector3 direction = rayOrigin ? rayOrigin.forward : transform.forward;

        RaycastHit hit;
        
        if (Physics.Raycast(startPos, direction, out hit, maxDistance, wallLayer))
        {
            Debug.Log($"[SDH] Wall detected {hit.collider.gameObject.name}");
            // hit.point와 hit.normal 외에 'startPos'(내 위치)도 함께 넘김
            SpawnCanvas(hit.point, hit.normal, startPos);
        }
    }

    // [변경점] startPos(플레이어/컨트롤러 위치)를 인자로 추가
    public void SpawnCanvas(Vector3 hitPoint, Vector3 hitNormal, Vector3 playerPos)
    {
        // 1. 벽에서 플레이어 쪽을 향하는 벡터 계산
        Vector3 toPlayerDir = (playerPos - hitPoint).normalized;

        // 2. 내적(Dot Product)을 통해 방향 판별
        // hitNormal과 toPlayerDir의 각도가 90도 이내면 양수, 벗어나면 음수
        float dot = Vector3.Dot(hitNormal, toPlayerDir);

        // 3. 최종 앞쪽 방향(Forward) 결정
        // 내적이 0보다 크면 벽이 이미 나를 보고 있음. 
        // 0보다 작으면 벽이 반대편이므로 법선(Normal)을 뒤집어줌.
        Vector3 finalForward = (dot < 0) ? hitNormal : -hitNormal;

        // 4. 보정된 방향(finalForward)을 기준으로 위치와 회전 설정
        // 이렇게 하면 항상 플레이어 쪽으로 튀어나오고(Offset), 플레이어를 바라봄(LookRotation)
        Vector3 spawnPos = hitPoint + (finalForward * wallOffset);
        Quaternion spawnRot = Quaternion.LookRotation(finalForward);

        // --- 이하 생성 로직 동일 ---
        GameObject newCanvas = Instantiate(canvasPrefab, spawnPos, spawnRot);
        RatioAlignedCanvas customizableCanvas = newCanvas.GetComponent<RatioAlignedCanvas>();
        customizableCanvas.SetScales(CanvasSizePool.Instance.GetCurrentCanvasSizeSet());
    }
}