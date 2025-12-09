using UnityEngine;
using System.Collections.Generic;

public class MRUKPaintInteractor : MonoBehaviour
{
    [Header("Controller Settings")]
    public OVRInput.Controller controllerNode = OVRInput.Controller.RTouch;
    public OVRInput.Button drawButton = OVRInput.Button.PrimaryIndexTrigger;

    [Header("Ray Settings")]
    public float maxDistance = 5.0f;
    public LayerMask drawingSurfaceLayer;

    [Header("Painting Settings")]
    public Color paintColor = Color.black;
    public int brushSize = 5;
    public int textureResolution = 512; // 텍스처 해상도 (512x512)

    [Header("References")]
    public LineRenderer lineRenderer;

    // 변경점: Renderer 대신 SpriteRenderer와 텍스처를 매핑
    private Dictionary<Collider, Texture2D> drawingTextures = new Dictionary<Collider, Texture2D>();

    void Start()
    {
        if (lineRenderer != null)
        {
            lineRenderer.useWorldSpace = false;
            lineRenderer.SetPosition(0, Vector3.zero);
        }
    }

    void Update()
    {
        HandleRaycastingAndPainting();
    }

    void HandleRaycastingAndPainting()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance, drawingSurfaceLayer))
        {
            Debug.Log("동환아 그만하자" + hit.collider.gameObject.name);
            SetLaserLength(hit.distance);

            if (OVRInput.Get(drawButton, controllerNode))
            {
                PaintOnSprite(hit);
            }
        }
        else
        {
            SetLaserLength(maxDistance);
        }
    }

    void PaintOnSprite(RaycastHit hit)
    {
        Collider hitCollider = hit.collider;
        SpriteRenderer spriteRenderer = hitCollider.GetComponent<SpriteRenderer>();
        BoxCollider boxCol = hitCollider as BoxCollider; // BoxCollider 가져오기

        if (spriteRenderer == null || boxCol == null) return;

        // 1. 그릴 텍스처 준비
        Texture2D drawTexture;
        if (!drawingTextures.TryGetValue(hitCollider, out drawTexture))
        {
            drawTexture = InitializeDrawingSprite(spriteRenderer, hitCollider);
            drawingTextures.Add(hitCollider, drawTexture);
        }

        // 2. 월드 좌표 -> 로컬 좌표 변환
        Vector3 localPos = hitCollider.transform.InverseTransformPoint(hit.point);

        // [핵심 수정] Box Collider의 실제 크기와 중심점을 반영하여 UV 계산
        // 로컬 좌표계에서 Collider의 왼쪽 끝은 (center.x - size.x / 2) 입니다.
        
        // 예: size가 10이면, 범위는 -5 ~ +5. 
        // localPos가 -5일 때 -> (-5 / 10) + 0.5 = 0 (UV 시작점)
        // localPos가 +5일 때 -> (+5 / 10) + 0.5 = 1 (UV 끝점)
        
        float uvX = ((localPos.x - boxCol.center.x) / boxCol.size.x) + 0.5f;
        float uvY = ((localPos.y - boxCol.center.y) / boxCol.size.y) + 0.5f;

        // 3. UV -> 픽셀 좌표 변환
        int centerX = (int)(uvX * drawTexture.width);
        int centerY = (int)(uvY * drawTexture.height);

        // 4. 그리기 (브러시)
        bool modified = false;
        for (int x = centerX - brushSize; x < centerX + brushSize; x++)
        {
            for (int y = centerY - brushSize; y < centerY + brushSize; y++)
            {
                // 텍스처 범위 체크
                if (x >= 0 && x < drawTexture.width && y >= 0 && y < drawTexture.height)
                {
                    // 원형 브러시
                    if ((x - centerX) * (x - centerX) + (y - centerY) * (y - centerY) <= brushSize * brushSize)
                    {
                        drawTexture.SetPixel(x, y, paintColor);
                        modified = true;
                    }
                }
            }
        }

        // 5. 적용
        if (modified)
        {
            drawTexture.Apply();
        }
    }

    // SpriteRenderer를 위한 초기화 함수
    Texture2D InitializeDrawingSprite(SpriteRenderer sr, Collider col)
    {
        // 1. 새 하얀색 텍스처 생성
        Texture2D newTexture = new Texture2D(textureResolution, textureResolution, TextureFormat.RGBA32, false);
        
        Color[] fillColors = new Color[textureResolution * textureResolution];
        for (int i = 0; i < fillColors.Length; i++) fillColors[i] = Color.white; // 배경 흰색
        newTexture.SetPixels(fillColors);
        newTexture.Apply();

        // 2. 텍스처를 담을 새 Sprite 생성 (Rect는 전체, Pivot은 중앙)
        Sprite newSprite = Sprite.Create(
            newTexture, 
            new Rect(0, 0, newTexture.width, newTexture.height), 
            new Vector2(0.5f, 0.5f), // Pivot Center
            100.0f // PPU (Pixels Per Unit) - 필요하면 조정
        );

        // 3. SpriteRenderer 교체
        sr.sprite = newSprite;

        // 4. BoxCollider 크기 재조정 (선택 사항)
        // 스프라이트 크기가 바뀌었을 수 있으므로 Collider 사이즈를 스프라이트에 맞춤
        BoxCollider boxCol = col as BoxCollider;
        if (boxCol != null)
        {
            boxCol.size = new Vector3(1, 1, 0.2f); // 1x1 단위 크기로 가정
        }

        Debug.Log($"Created new writable Sprite for: {sr.gameObject.name}");
        return newTexture;
    }

    void SetLaserLength(float distance)
    {
        if (lineRenderer != null)
        {
            lineRenderer.SetPosition(1, new Vector3(0, 0, distance));
        }
    }
}