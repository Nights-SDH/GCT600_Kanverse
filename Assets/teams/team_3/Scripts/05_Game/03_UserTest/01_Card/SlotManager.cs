using UnityEngine;

public class SlotManager : MonoBehaviour
{
    public int slotIndex;

    [HideInInspector] public GameObject currentCard;

    private MeshRenderer renderer;
    private BoxCollider boxCollider;

    [Header("Slot Settings")]
    public float padding = 1.05f;     // 카드보다 약간 크게
    public float depthOffset = 0.001f; // 카드가 슬롯보다 조금 앞에 나오게

    void Awake()
    {
        renderer = GetComponent<MeshRenderer>();
        boxCollider = GetComponent<BoxCollider>();

        SetEmptyVisual();
    }

    // DeckManager에서 카드 배치 시 호출
    public void AssignCard(GameObject card)
    {
        currentCard = card;
        transform.localScale = Vector3.one;
        // 카드 크기에 맞게 슬롯 크기 조정
        Vector3 cardSize = card.transform.localScale;

        transform.localScale = new Vector3(
            cardSize.x * padding,
            cardSize.y * padding,
            1f
        );

        // 카드가 슬롯 앞에 살짝 튀어나오게
        card.transform.position =
            transform.position - transform.forward * depthOffset;

        card.transform.rotation = transform.rotation;

        // 카드에게 "넌 지금 이 슬롯에 있다" 라고 알려주기
        Card cardComp = card.GetComponent<Card>();
        if (cardComp != null)
        {
            cardComp.AssignSlot(this);
        }

        SetFilledVisual();
    }

    // 카드가 슬롯에서 제거될 때
    public void OnCardRemoved()
    {
        currentCard = null;
        SetEmptyVisual();
    }

    private void SetEmptyVisual()
    {
        if (renderer != null)
            renderer.material.color = new Color(0.8f, 0.8f, 0.8f); // 연한 회색 (비어있음)
    }

    private void SetFilledVisual()
    {
        if (renderer != null)
            renderer.material.color = Color.white; // 채워졌을 때 흰색
    }
}
