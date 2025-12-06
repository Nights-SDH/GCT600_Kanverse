using UnityEngine;
using Meta.XR.MRUtilityKit;
using Unity.VisualScripting;
using Photon.Pun; // MRUK 필수 네임스페이스

public class SpawnCanvasOnWall: SingletonObject<SpawnCanvasOnWall>
{
    [Header("Settings")]
    public GameObject canvasPrefab; // 생성할 캔버스 프리팹
    public GameObject cardPrefab; // 생성할 카드 프리팹
    public const float wallOffset = 0.02f; // 벽에서 살짝 띄울 거리 (Z-fighting 방지, 2cm)

    public void SpawnCanvas()
    {
        // 1. 현재 방 정보 가져오기
        if (MRUK.Instance == null) return;
        var room = MRUK.Instance.GetCurrentRoom();

        // 방이 없거나 벽이 하나도 없으면 중단
        if (room == null || room.WallAnchors.Count == 0)
        {
            Debug.LogWarning("방을 찾을 수 없거나 벽이 없습니다.");
            return;
        }

        // 2. 벽 리스트 중 랜덤으로 하나 선택
        int randomIndex = Random.Range(0, room.WallAnchors.Count);
        MRUKAnchor selectedWall = room.WallAnchors[randomIndex];

        // 3. 위치 및 회전 계산
        // 벽의 중심 위치
        Vector3 spawnPos = selectedWall.transform.position;
        
        // 벽이 바라보는 방향(방 안쪽)으로 회전
        Quaternion spawnRot = selectedWall.transform.rotation;

        // 벽에 딱 붙으면 겹쳐서 안 보일 수 있으므로 앞으로 살짝 띄움
        // (MRUK 벽의 forward는 방 안쪽을 향합니다)
        spawnPos += selectedWall.transform.forward * wallOffset;

        // 4. 생성 (이미 생성된 게 있다면 위치만 옮길지, 새로 만들지는 선택)
        GameObject.Instantiate(canvasPrefab, spawnPos, spawnRot);
        spawnPos+= selectedWall.transform.forward * wallOffset;
        GameObject newCardDeck = GameObject.Instantiate(cardPrefab, spawnPos, spawnRot);

        Debug.Log($"Canvas generated on: {selectedWall.name}");
    }
}