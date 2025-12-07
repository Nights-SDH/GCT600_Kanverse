using System.Collections;
using UnityEngine;
using Meta.XR.MRUtilityKit;

public class CeilngSquareSpawner : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public float normalOffset = 1f;
    public MRUKAnchor.SceneLabels ceilingLabel = MRUKAnchor.SceneLabels.CEILING;

    IEnumerator Start()
    {
        while (MRUK.Instance == null || !MRUK.Instance.IsInitialized)
            yield return null;

        SpawnCeilingSquare();
    }

    private void SpawnCeilingSquare()
    {
        MRUKRoom room = MRUK.Instance.GetCurrentRoom();
        if (room == null)
        {
            Debug.LogWarning("MRUKRoom not found.");
            return;
        }

        MRUKAnchor ceilingAnchor = null;

        // CEILING 라벨 가진 Anchor *직접 찾기*
        foreach (var anchor in room.Anchors)
        {
            if (anchor != null && anchor.HasLabel(ceilingLabel.ToString()))
            {
                ceilingAnchor = anchor;
                break;
            }
        }

        if (ceilingAnchor == null)
        {
            Debug.LogWarning("No CEILING anchor found.");
            return;
        }

        // Anchor 기준 위치 계산
        Vector3 spawnPos = ceilingAnchor.transform.position
                           - ceilingAnchor.transform.up * normalOffset;

        Quaternion rot = Quaternion.LookRotation(
            ceilingAnchor.transform.forward,
            -ceilingAnchor.transform.up
        );

        Instantiate(prefabToSpawn, spawnPos, rot);
    }
}
