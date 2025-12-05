using UnityEngine;

public class Card : MonoBehaviour
{
    // 이 카드가 현재 들어가 있는 슬롯
    public SlotManager CurrentSlot { get; private set; }

    private MeshRenderer meshRenderer;

    [Header("Card World Size")]
    [Tooltip("카드의 세로 길이(m 단위). 가로는 비율에 맞춰 자동 조정됨")]
    public float worldHeight = 0.14f; // 14cm 정도, 필요하면 인스펙터에서 조절

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer == null)
        {
            Debug.LogError("[Card] MeshRenderer를 찾을 수 없습니다!");
        }
        transform.localScale = Vector3.one;
    }

    /// <summary>
    /// PNG 텍스처를 받아서 카드 비주얼과 크기를 세팅
    /// DeckManager에서 카드 생성 직후 호출
    /// </summary>
    public void Initialize(Texture2D texture)
    {
        if (texture == null)
        {
            Debug.LogError("[Card] Initialize texture == null");
            return;
        }

        if (meshRenderer == null)
        {
            meshRenderer = GetComponent<MeshRenderer>();
            if (meshRenderer == null)
            {
                Debug.LogError("[Card] MeshRenderer가 없어서 텍스처 적용 불가");
                return;
            }
        }

        // 조금 더 선명하게
        texture.filterMode = FilterMode.Bilinear;

        // Unlit/Texture 머티리얼 생성해서 텍스처 적용
        Material mat = new Material(Shader.Find("Unlit/Texture"));
        mat.mainTexture = texture;
        mat.renderQueue = 3001;
        meshRenderer.material = mat;

        // 가로/세로 비율 유지해서 카드 스케일 설정
        float aspect = (float)texture.width / texture.height;
        float worldWidth = worldHeight * aspect;

        transform.localScale = new Vector3(worldWidth, worldHeight, 1f);
        transform.localRotation = Quaternion.identity;

        if (transform.parent != null)
            transform.localScale = new Vector3(worldWidth, worldHeight, 1f);

    }

    /// <summary>
    /// 슬롯에 들어갈 때 SlotManager가 호출
    /// </summary>
    public void AssignSlot(SlotManager slot)
    {
        CurrentSlot = slot;
    }

    /// <summary>
    /// 컨트롤러로 집어서 빼갈 때 호출하면 슬롯을 비워주고 연결 끊기
    /// (나중에 카드 움직임 구현할 때 사용)
    /// </summary>
    public void DetachFromSlot()
    {
        if (CurrentSlot != null)
        {
            CurrentSlot.OnCardRemoved();
            CurrentSlot = null;
        }
    }
}
