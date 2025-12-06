using UnityEngine;
using Meta.XR.MRUtilityKit;

public class WallRaycastRight : MonoBehaviour
{
    // public Transform rayStartPoint;   // RightHandAnchor
    // public float rayLength = 5f;
    // public GameObject canvasPrefab;   // 반드시 "Project 창의 screen.prefab" 넣기!
    // public Transform hmd;             // CenterEyeAnchor 넣기

    // private GameObject spawnedCanvasInstance;

    // public enum AlignmentMode { Absolute, Ratio }
    // public AlignmentMode currentMode = AlignmentMode.Absolute;

    // public enum CanvasSize { P1, P2, P3 }
    // public CanvasSize currentSize = CanvasSize.P1;


    // void Start()
    // {
    //     // 씬 안에 있는 screen 인스턴스만 숨기고, Prefab은 건드리지 않음
    //     foreach (Transform t in FindObjectsOfType<Transform>())
    //     {
    //         if (t.name == "screen" && t.gameObject.scene.IsValid())
    //             t.gameObject.SetActive(false);
    //     }
    // }



    // void Update()
    // {
    //     if (!MRUK.Instance || !MRUK.Instance.IsInitialized)
    //         return;

    //     MRUKRoom room = MRUK.Instance.GetCurrentRoom();
    //     if (room == null)
    //         return;

    //     // ==========================================================
    //     // 1) RightHand ray → 수평 방향으로 보정
    //     // ==========================================================
    //     Vector3 dir = rayStartPoint.forward;
    //     dir.y = 0;            // 아래로 향하는 현상 제거 → 정확히 벽 중앙을 향하게 함
    //     dir.Normalize();

    //     Ray ray = new Ray(rayStartPoint.position, dir);

    //     // 벽 레이캐스트
    //     bool hasHit = room.Raycast(ray, rayLength, new LabelFilter(),
    //         out RaycastHit hit, out MRUKAnchor anchor);

    //     if (!hasHit)
    //         return;

    //     if (anchor.Label != MRUKAnchor.SceneLabels.WALL_FACE &&
    //         anchor.Label != MRUKAnchor.SceneLabels.WALL_ART)
    //         return;

    //     // 오른손 트리거 누를 때 생성
    //     if (!OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
    //         return;

    //     Debug.Log("Right Trigger → Spawn Canvas");



    //     // ==========================================================
    //     // 2) 스크린 생성 위치
    //     //    X, Z는 Ray hit 결과
    //     //    Y는 무조건 사용자 눈높이(HMD.y)
    //     // ==========================================================

    //     Vector3 spawnPos = hit.point;

    //     // 높이는 hit.y 사용 금지! → HMD.y 로 고정
    //     Vector3 gazePoint = hmd.position + hmd.forward * 2f;
    //     spawnPos.y = gazePoint.y;

    //     // 벽에서 조금 띄우기
    //     spawnPos += hit.normal * 0.005f;



    //     // ==========================================================
    //     // 3) 회전 = 벽 방향을 바라보도록
    //     //    (Y 180 보정 금지)
    //     // ==========================================================
    //     Quaternion rotation = Quaternion.LookRotation(-hit.normal);



    //     // ==========================================================
    //     // 4) 중복 생성 방지
    //     // ==========================================================
    //     if (spawnedCanvasInstance != null)
    //     {
    //         Debug.Log("Canvas already exists.");
    //         return;
    //     }



    //     // ==========================================================
    //     // 5) Instantiate
    //     // ==========================================================
    //     spawnedCanvasInstance = Instantiate(canvasPrefab, spawnPos, rotation);

    //     var headLock = spawnedCanvasInstance.GetComponent<HeadLockedCanvas>();
    //     if (headLock != null)
    //         Destroy(headLock);



    //     // ==========================================================
    //     // 6) 정합 & 크기
    //     // ==========================================================
    //     SetupCanvas(spawnedCanvasInstance);
    // }



    // // 정합 설정 -----------------------------------------------
    // private void SetupCanvas(GameObject canvas)
    // {
    //     var ratio = canvas.GetComponent<RatioAlignedCanvas>();
    //     var absolute = canvas.GetComponent<AbsoluteCanvasController>();

    //     if (ratio) ratio.enabled = false;
    //     if (absolute) absolute.enabled = false;

    //     ApplyCanvasSize(canvas.transform);

    //     if (currentMode == AlignmentMode.Absolute)
    //     {
    //         if (absolute) absolute.enabled = true;
    //     }
    //     else
    //     {
    //         if (ratio) ratio.enabled = true;
    //     }
    // }



    // // 스크린 크기 적용 -----------------------------------------
    // private void ApplyCanvasSize(Transform canvas)
    // {
    //     switch (currentSize)
    //     {
    //         case CanvasSize.P1:
    //             canvas.localScale = new Vector3(0.304f, 0.371f, 1f);
    //             break;

    //         case CanvasSize.P2:
    //                 canvas.localScale = new Vector3(0.434f, 0.556f, 1f);
    //                 break;

    //         case CanvasSize.P3:
    //             canvas.localScale = new Vector3(0.434f, 0.695f, 1f);
    //             break;
    //     }

    //     Debug.Log($"Canvas size applied: {currentSize}, scale = {canvas.localScale}");
    // }
}
