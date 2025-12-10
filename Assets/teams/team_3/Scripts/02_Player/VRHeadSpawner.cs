using UnityEngine;
using Meta.XR.MRUtilityKit;
using System.Collections.Generic; // MRUK 필수 네임스페이스

[System.Serializable] 
public class SpawnObjectWithDialogSpeaker
{
    public GameObject prefab;
    public ObjectName objectName;
}

public class VRHeadSpawner : SingletonObject<VRHeadSpawner>
{
    [Header("Spawn Settings")]
    [Tooltip("순서대로 생성될 프리팹 리스트")]
    public SpawnObjectWithDialogSpeaker[] spawnPrefabs; 

    [Tooltip("바닥에서 얼마나 띄울지 (Z-fighting 방지용, 예: 0.01)")]
    public float floorHoverHeight = 0.001f;

    [Range(-180f, 180f)]
    [Tooltip("나를 바라보는 시선이 안 맞을 때 이 슬라이더를 조절하세요.")]
    public float yLookCorrection = 45f;

    [Header("거리 유지 설정 (겹침 방지)")]
    [Tooltip("물체끼리 최소한 이만큼은 떨어져야 함 (미터 단위)")]
    public float minObjectDistance = 0.5f; // 예: 50cm

    [Tooltip("빈 자리를 찾기 위해 최대 몇 번 시도할지 (너무 적으면 생성 실패, 너무 많으면 렉 유발)")]
    public int maxSpawnAttempts = 30;
    private List<Vector3> _spawnedPositions = new List<Vector3>();

    private GameObject FindObjectBySpeaker(ObjectName objectName)
    {
        foreach (var item in spawnPrefabs)
        {
            if (item.objectName == objectName)
            {
                return item.prefab;
            }
        }
        return null;
    }

    /// <summary>
    /// 호출할 때마다 리스트의 다음 오브젝트를 방 바닥 랜덤 위치에 생성합니다.
    /// </summary>
    public void SpawnNextOnFloor(ObjectName objectName)
    {
        // 1. 프리팹 리스트 안전 검사
        if (spawnPrefabs == null || spawnPrefabs.Length == 0)
        {
            Debug.LogWarning("[Spawner] 생성할 프리팹 리스트가 비어있습니다.");
            return;
        }

        // 2. MRUK 방 인식 여부 확인
        if (MRUK.Instance == null)
        {
            Debug.LogError("[Spawner] MRUK 인스턴스가 없습니다. 씬에 MRUK 프리팹이 있는지 확인하세요.");
            return;
        }

        var room = MRUK.Instance.GetCurrentRoom();
        if (room == null)
        {
            Debug.LogWarning("[Spawner] 인식된 방(Room)이 없습니다. 방 스캔이 완료되었나요?");
            return;
        }

        // 3. 바닥(Floor) 위 무작위 위치 찾기
        // minRadius: 0.2f (가구 사이 너무 좁은 틈에는 생성 안 함)
        // LabelFilter.Floor: 오직 바닥만 타겟팅
        Vector3 bestPos = Vector3.zero;
        bool validPositionFound = false;
        GameObject prefabToSpawn = FindObjectBySpeaker(objectName);
        if (prefabToSpawn == null) return;

        for (int i = 0; i < maxSpawnAttempts; i++)
        {
            Vector3 randomPos;
            Vector3 normal;

            // MRUK에게 바닥 위 랜덤 위치 요청
            bool found = room.GenerateRandomPositionOnSurface(
                MRUK.SurfaceType.FACING_UP, 
                0.2f, 
                new LabelFilter(MRUKAnchor.SceneLabels.FLOOR),
                out randomPos, 
                out normal
            );

            if (found)
            {
                // 찾은 위치가 기존 물체들과 충분히 떨어져 있는지 검사
                if (IsPositionSafe(randomPos))
                {
                    bestPos = randomPos;
                    validPositionFound = true;
                    break; // 좋은 자리를 찾았으니 반복 종료!
                }
            }
        }

        // ================================================================
        // [생성 로직] 유효한 위치를 찾았을 때만 생성
        // ================================================================
        if (validPositionFound)
        {
            Vector3 spawnPos = bestPos + (Vector3.up * floorHoverHeight);

            // 1. 플레이어 방향 벡터 계산
            Vector3 targetPos = Camera.main.transform.position;
            targetPos.y = spawnPos.y; 
            Vector3 directionToPlayer = (targetPos - spawnPos).normalized;

            // 2. 회전 계산 (나를 보기 + 보정값)
            Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
            Quaternion finalRotation = lookRotation * Quaternion.Euler(0, yLookCorrection, 0);

            // 3. 생성
            Instantiate(prefabToSpawn, spawnPos, finalRotation);
            
            // 4. [중요] 생성된 위치를 리스트에 기록 (다음 생성 때 피하기 위해)
            _spawnedPositions.Add(bestPos);

            Debug.Log($"[Spawner] Success: '{prefabToSpawn.name}' created.");
        }
        else
        {
            Debug.LogWarning($"[Spawner] {maxSpawnAttempts}번 시도했지만 겹치지 않는 빈 공간을 찾지 못했습니다.");
        }
    }

    // 위치가 안전한지(다른 물체와 안 겹치는지) 확인하는 함수
    private bool IsPositionSafe(Vector3 candidatePos)
    {
        foreach (Vector3 existingPos in _spawnedPositions)
        {
            // 거리 계산 (수평 거리만 따지려면 y를 무시해도 됨, 여기선 3D 거리 사용)
            float distance = Vector3.Distance(candidatePos, existingPos);
            
            // 하나라도 너무 가까우면 실패
            if (distance < minObjectDistance)
            {
                return false; 
            }
        }
        return true; // 모든 물체와 거리가 충분함
    }
}