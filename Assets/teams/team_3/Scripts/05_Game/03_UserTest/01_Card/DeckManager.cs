using UnityEngine;

public class DeckManager : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject slotPrefab;
    public GameObject cardPrefab;

    [Header("Deck Settings")]
    public int rows = 2;        // 위/아래
    public int columns = 8;     // 카드 개수

    // 카드 worldHeight → PNG 비율 유지 (869 x 1160 기준 세로 길이 정함)
    [Header("Card Size Settings")]
    public float cardHeight = 0.25f; // 카드 세로 (m 단위)
    public float spacingPadding = 0.01f; // 카드 간격

    // 데크와 사용자 위치
    [Header("Deck Position (HMD 기준)")]
    public float deckDistance = 0.01f;
    public float deckHeightOffset = 0.1f;

    private SlotManager[,] slots;
    private float cardHeightWorld;  // 0.14 (Card.worldHeight)
    private float cardWidthWorld;   // PNG 비율 반영된 가로 길이
    void Start()
    {
        // --- 카드 실제 크기(월드) 계산: Card.cs 의 worldHeight 값 사용 ---
        Card cardComp = cardPrefab.GetComponent<Card>();
        if (cardComp == null)
        {
            Debug.LogError("[DeckManager] cardPrefab 에 Card 컴포넌트가 없습니다.");
            return;
        }

        cardHeightWorld = cardComp.worldHeight;       // 0.14m
        float aspect = 869f / 1160f;                  // PNG 비율 그대로
        cardWidthWorld = cardHeightWorld * aspect;    // 0.14 * (869/1160)

        PositionDeck();
        GenerateDeckBases();
        GenerateSlots();
        GenerateCards();
    }
    // -------------------------------------------------------------------
    // 1. 사용자 앞 배치
    // -------------------------------------------------------------------
    void PositionDeck()
    {
        Transform hmd = GameObject.Find("CenterEyeAnchor").transform;

        Vector3 forwardFlat = Vector3.ProjectOnPlane(hmd.forward, Vector3.up).normalized;

        Vector3 pos =
            hmd.position +
            forwardFlat * deckDistance +        // 앞뒤 위치
            Vector3.up * deckHeightOffset;      // 높낮이 위치

        transform.position = pos;

        transform.rotation = Quaternion.LookRotation(forwardFlat, Vector3.up);
    }

    // -------------------------------------------------------------------
    // 2. DeckBase 생성 (카드 줄과 정확히 맞게)
    // -------------------------------------------------------------------
    void GenerateDeckBases()
    {
        // 카드 배열 전체 폭
        float totalCardsWidth =
            cardWidthWorld * columns +
            spacingPadding * (columns - 1);

        // 덱은 카드 배열보다 약간만 크게
        float deckWidth = totalCardsWidth + cardWidthWorld * 0.5f;   // 좌우로 카드 1/4 씩 여유
        float deckHeight = cardHeightWorld * 1.5f;                   // 위/아래 여유 포함

        // 위/아래 줄 사이 간격(센터 기준)
        float rowGap = deckHeight + 0.05f;

        for (int r = 0; r < rows; r++)
        {
            GameObject deckBase = GameObject.CreatePrimitive(PrimitiveType.Quad);
            deckBase.name = (r == 0) ? "Deck_Red" : "Deck_Blue";
            deckBase.transform.SetParent(transform);

            float verticalOffset = (rows == 1)
                ? 0f
                : (r == 0 ? +rowGap * 0.5f : -rowGap * 0.5f);

            deckBase.transform.position =
                transform.position + transform.up * verticalOffset;

            deckBase.transform.rotation = transform.rotation;

            // 🔹 여기서 스케일이 최종 Deck 크기
            deckBase.transform.localScale =
                new Vector3(deckWidth, deckHeight, 1f);

            // 반투명 머티리얼
            Shader shader = Shader.Find("Sprites/Default"); // 알파 지원
            Material mat = new Material(shader);
            mat.color = (r == 0)
                ? new Color(1f, 0f, 0f, 0.25f)     // 빨간 반투명
                : new Color(0f, 0.4f, 1f, 0.25f);  // 파란 반투명

            deckBase.GetComponent<MeshRenderer>().material = mat;
            Destroy(deckBase.GetComponent<BoxCollider>());
        }
    }

    // -------------------------------------------------------------------
    // 3. 슬롯 생성 (DeckBase 표면 위 정확히)
    // -------------------------------------------------------------------
    void GenerateSlots()
    {
        slots = new SlotManager[rows, columns];

        // 🔥 카드 크기 계산을 DeckBase와 동일하게!
        float cardWidth = cardWidthWorld;     // (0.14m * 869/1160)
        float cardHeight = cardHeightWorld;   // (0.14m)

        float totalWidth =
            cardWidth * columns +
            spacingPadding * (columns - 1);

        float startX = -totalWidth / 2f + cardWidth / 2f;

        for (int r = 0; r < rows; r++)
        {
            Transform deckBase =
                transform.Find((r == 0) ? "Deck_Red" : "Deck_Blue");

            Vector3 center = deckBase.position;
            Vector3 right = deckBase.right;
            Vector3 forward = deckBase.forward;

            for (int c = 0; c < columns; c++)
            {
                Vector3 slotPos =
                    center +
                    right * (startX + c * (cardWidth + spacingPadding)) +
                    forward * 0.001f; // z-fighting 회피

                GameObject slotObj =
                    Instantiate(slotPrefab, slotPos, deckBase.rotation, transform);

                slotObj.transform.localScale = Vector3.one;

                slots[r, c] = slotObj.GetComponent<SlotManager>();
                slots[r, c].slotIndex = r * columns + c;
            }
        }
    }


    // -------------------------------------------------------------------
    // 4. 카드 생성
    // -------------------------------------------------------------------
    void GenerateCards()
    {
        Texture2D[] textures = Resources.LoadAll<Texture2D>("affinity card_things");

        int idx = 0;

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                GameObject cardObj = Instantiate(cardPrefab);
                Card cd = cardObj.GetComponent<Card>();
                cd.Initialize(textures[idx]);
                slots[r, c].AssignCard(cardObj);
                cd.AssignSlot(slots[r, c]);

                idx++;
                if (idx >= textures.Length) return;
            }
        }
    }
}
