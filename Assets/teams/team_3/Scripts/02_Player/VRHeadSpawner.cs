using UnityEngine;
using Meta.XR.MRUtilityKit; // MRUK 필수 네임스페이스

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
        Vector3 randomPos;
        Vector3 normal;
        
        bool foundPosition = room.GenerateRandomPositionOnSurface(
            MRUK.SurfaceType.FACING_UP, 
            0.2f, 
            new LabelFilter(MRUKAnchor.SceneLabels.FLOOR),
            out randomPos, 
            out normal
        );

        if (foundPosition)
        {
            GameObject prefabToSpawn = FindObjectBySpeaker(objectName);
            if (prefabToSpawn != null)
            {
                Debug.LogError($"[Spawner] '{objectName}' 프리팹을 찾을 수 없습니다.");
                return;
            }

            Vector3 spawnPos = randomPos + (Vector3.up * floorHoverHeight);

            // ================================================================
            // [핵심 로직] 플레이어를 바라보는 회전값 계산
            // ================================================================
            
            // 1. 플레이어(카메라)의 위치를 가져옵니다.
            Vector3 playerPos = Camera.main.transform.position;

            // 2. "생성 위치"에서 "플레이어"로 향하는 방향 벡터를 구합니다.
            Vector3 directionToPlayer = playerPos - spawnPos;

            // 3. [중요] 높이 차이는 무시합니다 (Y축 0으로 평탄화).
            // 이걸 안 하면 물체가 하늘을 보려고 뒤로 눕거나 앞으로 쏠립니다.
            directionToPlayer.y = 0; 

            // 4. 해당 방향을 바라보는 회전값(Quaternion)을 만듭니다.
            // (만약 프리팹 설정 단계에서 얼굴을 Z축에 안 맞췄다면 여기서 * Quaternion.Euler(0, 90, 0) 등을 해야 해서 복잡해집니다)
            Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);

            // 5. 생성 (계산된 회전값 적용)
            Instantiate(prefabToSpawn, spawnPos, lookRotation);

            Debug.Log($"[Spawner] '{prefabToSpawn.name}' created at {spawnPos}");
        }
        else
        {
            Debug.LogWarning("[Spawner] 공간 부족");
        }
    }
}