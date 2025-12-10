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
    public float floorHoverHeight = 0.01f;

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
            // 4. 생성할 프리팹 가져오기
            GameObject prefabToSpawn = FindObjectBySpeaker(objectName);
            if(prefabToSpawn == null)
            {
                Debug.LogWarning($"[Spawner] 해당 DialogSpeaker에 매칭된 프리팹이 없습니다: {objectName}");
                return;
            }

            // 5. 위치 보정 (바닥에 딱 붙으면 깜빡거리니 살짝 띄움)
            Vector3 spawnPos = randomPos + (Vector3.up * floorHoverHeight);

            // 6. 오브젝트 생성 (회전은 기본값, 필요시 Random.rotation.y 등 적용 가능)
            Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);

            Debug.Log($"[Spawner] '{prefabToSpawn.name}' created at {spawnPos}");
        }
        else
        {
            Debug.LogWarning("[Spawner] 바닥에서 물체를 놓을 충분한 빈 공간을 찾지 못했습니다.");
        }
    }
}