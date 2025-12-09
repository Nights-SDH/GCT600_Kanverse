using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class SpawnCardOnCanvas : SingletonObject<SpawnCardOnCanvas>
{
    [Header("Spawn Settings")]
    public GameObject cardPrefab; 
    public List<NetworkCard> spawnedCards = new List<NetworkCard>();

    public int rows = 2;
    public bool useRandomOrder = false;

    [Header("Layout Settings")]
    public float cardSpacingX = 0.2f;
    public float cardSpacingY = 0.3f;

    public void SpawnCardsOnCanvas()
    {
        CardSet cardSet = CardDeck.Instance.GetCurrentCardSet();
        List<Sprite> cardSprites = new List<Sprite>(cardSet.cardSprites);
        int totalCards = cardSprites.Count;

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
            newCard.transform.SetParent(RatioAlignedCanvas.Instance.transform, true);

            // [핵심 변경] 부모의 크기 영향을 없애기 위한 스케일 역보정
            // 공식: 자식의 LocalScale = (원하는 WorldScale) / (부모의 WorldScale)
            Vector3 parentScale = RatioAlignedCanvas.Instance.transform.lossyScale;
            Vector3 originalScale = cardPrefab.transform.localScale;

            newCard.transform.localScale = new Vector3(
                originalScale.x / parentScale.x,
                originalScale.y / parentScale.y,
                originalScale.z / parentScale.z
            );

            // 2. 위치 및 회전 설정 (Local 기준)
            newCard.transform.localPosition = new Vector3(posX, posY, GameManagerUX.Instance.intervalCardAndCanvas);
            newCard.transform.localRotation = Quaternion.identity;

            // 3. 스프라이트 설정
            Sprite selectedSprite = SelectSprite(i, cardSprites);
            SpriteRenderer sr = newCard.GetComponent<SpriteRenderer>();
            NetworkCard networkCard = newCard.GetComponent<NetworkCard>();
            if (networkCard != null)
            {
                networkCard.SetCardID(i); // 카드 ID 할당
                spawnedCards.Add(networkCard); // 리스트에 추가
            }
            
            
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

    public NetworkCard FindCardByID(int id)
    {
        return spawnedCards.Find(card => card.cardID == id);
    }

    Sprite SelectSprite(int currentIndex, List<Sprite> cardSprites)
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