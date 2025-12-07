using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class SpawnCanvasOnWall : SingletonObject<SpawnCanvasOnWall>
{
    [Header("Input Settings")]
    public OVRInput.Button spawnButton = OVRInput.Button.Two;
    public OVRInput.Controller controller = OVRInput.Controller.RTouch;

    [Header("Raycast Settings")]
    public float maxDistance = 100.0f;
    public LayerMask wallLayer;
    public Transform rayOrigin;

    [Header("Spawn Settings")]
    public GameObject canvasPrefab;
    public GameObject cardPrefab; 
    public List<Sprite> cardSprites; 

    public int totalCards => cardSprites != null ? cardSprites.Count : 0;
    public int rows = 2;
    public bool useRandomOrder = false;

    [Header("Layout Settings")]
    public float cardSpacingX = 0.2f;
    public float cardSpacingY = 0.3f;
    public float wallOffset = 0.001f;

    // 외부에서 Update 호출 (Manager 등에서)
    public void CheckUpdate()
    {
        if (OVRInput.GetDown(spawnButton, controller))
        {
            TrySpawnCanvas();
        }
    }

    void TrySpawnCanvas()
    {
        if (cardSprites == null || cardSprites.Count == 0)
        {
            Debug.LogWarning("Card Sprites 리스트가 비어있습니다!");
            return;
        }

        Vector3 startPos = rayOrigin ? rayOrigin.position : transform.position;
        Vector3 direction = rayOrigin ? rayOrigin.forward : transform.forward;

        RaycastHit hit;
        
        if (Physics.Raycast(startPos, direction, out hit, maxDistance, wallLayer))
        {
            Debug.Log($"[SDH] Wall detected {hit.collider.gameObject.name}");
            // hit.point와 hit.normal 외에 'startPos'(내 위치)도 함께 넘김
            SpawnAndArrange(hit.point, hit.normal, startPos);
        }
    }

    // [변경점] startPos(플레이어/컨트롤러 위치)를 인자로 추가
    void SpawnAndArrange(Vector3 hitPoint, Vector3 hitNormal, Vector3 playerPos)
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

        int columns = Mathf.CeilToInt((float)totalCards / rows);
        float startX = -((columns - 1) * cardSpacingX) / 2;
        float startY = ((rows - 1) * cardSpacingY) / 2;

        for (int i = 0; i < totalCards; i++)
        {
            int currentRow = i / columns;
            int currentCol = i % columns;

            float posX = startX + (currentCol * cardSpacingX);
            float posY = startY - (currentRow * cardSpacingY);

            GameObject newCard = Instantiate(cardPrefab);
            
            // 1. 일단 부모 설정
            newCard.transform.SetParent(newCanvas.transform, true);

            // [핵심 변경] 부모의 크기 영향을 없애기 위한 스케일 역보정
            // 공식: 자식의 LocalScale = (원하는 WorldScale) / (부모의 WorldScale)
            Vector3 parentScale = newCanvas.transform.lossyScale;
            Vector3 originalScale = cardPrefab.transform.localScale;

            newCard.transform.localScale = new Vector3(
                originalScale.x / parentScale.x,
                originalScale.y / parentScale.y,
                originalScale.z / parentScale.z
            );

            // 2. 위치 및 회전 설정 (Local 기준)
            newCard.transform.localPosition = new Vector3(posX, posY, -0.3f);
            newCard.transform.localRotation = Quaternion.identity;

            // 3. 스프라이트 설정
            Sprite selectedSprite = SelectSprite(i);
            SpriteRenderer sr = newCard.GetComponent<SpriteRenderer>();
            
            if (sr != null)
            {
                sr.sprite = selectedSprite;
            }
            else
            {
                var childSr = newCard.GetComponentInChildren<SpriteRenderer>();
                if (childSr != null) childSr.sprite = selectedSprite;
            }
        }

        Debug.Log($"Canvas 생성 완료. (방향 보정됨, 카드 스케일 유지됨)");
    }

    Sprite SelectSprite(int currentIndex)
    {
        if (useRandomOrder)
        {
            int randomIdx = Random.Range(0, cardSprites.Count);
            return cardSprites[randomIdx];
        }
        else
        {
            int seqIdx = currentIndex % cardSprites.Count;
            return cardSprites[seqIdx];
        }
    }
}