using UnityEngine;

public class VRHeadSpawner : SingletonObject<VRHeadSpawner>
{
    [Header("1~8번 버튼에 매핑할 프리팹 리스트")]
    public GameObject[] spawnPrefabs; 

    [Header("플레이어 눈 기준 떨어질 거리 (x, y, z)")]
    public Vector3 spawnOffset = new Vector3(0, 0, 1.0f); // 예: 정면 1m

    // 기존 생성된 오브젝트를 관리하려면 변수 추가 (중복 생성 방지용)
    private GameObject currentObject;

    public void SpawnObject(int index)
    {
        // 인덱스 유효성 검사
        if (index < 0 || index >= spawnPrefabs.Length) return;

        // 기존 오브젝트가 있다면 제거 (필요 없으면 삭제하세요)
        if (currentObject != null) Destroy(currentObject);

        // 1. 오브젝트 생성
        currentObject = Instantiate(spawnPrefabs[index]);

        // 2. Main Camera(HMD) 찾기
        Transform cameraTransform = Camera.main.transform;

        // 3. 카메라를 부모로 설정하여 같이 움직이게 함 (거리 유지 핵심)
        currentObject.transform.SetParent(cameraTransform);

        // 4. 로컬 좌표를 설정하여 눈 기준으로 위치 고정
        currentObject.transform.localPosition = spawnOffset;
        
        // 5. 회전 초기화 (항상 카메라와 같은 방향을 보게 함)
        currentObject.transform.localRotation = Quaternion.identity;
    }
}