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
    
    // [변경 1] 프리팹은 1개, 이미지는 여러 개
    public GameObject cardPrefab; 
    public List<Sprite> cardSprites; 

    public int totalCards => cardSprites.Count;
    public int rows = 2;
    public bool useRandomOrder = false;

    [Header("Layout Settings")]
    public float cardSpacingX = 0.2f;
    public float cardSpacingY = 0.3f;
    public float wallOffset = 0.02f;

    public void CheckUpdate()
    {
        if (OVRInput.GetDown(spawnButton, controller))
        {
            TrySpawnCanvas();
        }
    }

    void TrySpawnCanvas()
    {
        // Sprite 리스트 확인
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
            SpawnAndArrange(hit.point, hit.normal);
        }
    }

    void SpawnAndArrange(Vector3 hitPoint, Vector3 hitNormal)
    {
        Vector3 spawnPos = hitPoint + (hitNormal * wallOffset);
        Quaternion spawnRot = Quaternion.LookRotation(hitNormal);

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

            // [변경 2] 단일 프리팹 생성
            GameObject newCard = Instantiate(cardPrefab);
            
            // 계층 및 위치 설정
            newCard.transform.SetParent(newCanvas.transform, false);
            newCard.transform.localPosition = new Vector3(posX, posY, 0);
            newCard.transform.localRotation = Quaternion.identity;

            // [변경 3] Sprite 교체 로직
            Sprite selectedSprite = SelectSprite(i);
            SpriteRenderer sr = newCard.GetComponent<SpriteRenderer>();
            
            if (sr != null)
            {
                sr.sprite = selectedSprite;
            }
            else
            {
                // 혹시 SpriteRenderer가 자식에 있는 경우 대비
                var childSr = newCard.GetComponentInChildren<SpriteRenderer>();
                if (childSr != null) childSr.sprite = selectedSprite;
            }
        }

        Debug.Log($"Canvas 생성 및 이미지 교체 완료.");
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