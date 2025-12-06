using UnityEngine;
using Meta.XR.MRUtilityKit;

public class WallRaycastLeft : MonoBehaviour
{
    // public Transform rayStartPoint;     // LeftControllerAnchor
    // public float rayLength = 5f;
    // public GameObject canvasPrefab;
    // public Transform hmd;

    // private GameObject spawnedCanvasInstance;

    //  public enum AlignmentMode { Absolute, Ratio }
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
    //             if (room == null)
    //                 return;

    //     // Ray 방향 보정 (Right와 동일)
    //     Vector3 dir = rayStartPoint.forward;
    //     dir.y = 0;
    //     dir.Normalize();

    //     Ray ray = new Ray(rayStartPoint.position, dir);
        
    //     bool hasHit = room.Raycast(ray, rayLength, new LabelFilter(),
    //                 out RaycastHit hit, out MRUKAnchor anchor);
    //     // 벽 캐스트
    //     if (!hasHit)
    //         return;

    //     if (anchor.Label != MRUKAnchor.SceneLabels.WALL_FACE &&
    //         anchor.Label != MRUKAnchor.SceneLabels.WALL_ART)
    //         return;

    //     // 왼손 트리거 입력!!
    //     if (!OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
    //         return;

    //     Debug.Log("Right Trigger → Spawn Canvas");

    //     Vector3 spawnPos = hit.point;

    //     // 시선 기반 높이
    //     Vector3 gazePoint = hmd.position + hmd.forward * 2f;
    //     spawnPos.y = gazePoint.y;

    //     spawnPos += hit.normal * 0.02f;

    //     Quaternion rotation = Quaternion.LookRotation(-hit.normal);

    //     // 중복 생성 방지
    //     if (spawnedCanvasInstance != null)
    //     {
    //         Debug.Log("Canvas already exists.");
    //         return;
    //     }


    //     spawnedCanvasInstance = Instantiate(canvasPrefab, spawnPos, rotation);

    //     var headLock = spawnedCanvasInstance.GetComponent<HeadLockedCanvas>();
    //     if (headLock != null)
    //         Destroy(headLock);

    //     SetupCanvas(spawnedCanvasInstance);
    // }


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
