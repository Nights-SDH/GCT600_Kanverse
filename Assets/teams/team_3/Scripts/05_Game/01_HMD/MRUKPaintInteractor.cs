using UnityEngine;
using System.Collections.Generic;

public class MRUKPaintInteractor : SingletonObject<MRUKPaintInteractor>
{
    [Header("Controller Settings")]
    public OVRInput.Controller controllerNode = OVRInput.Controller.RTouch;
    public OVRInput.Button drawButton = OVRInput.Button.PrimaryIndexTrigger;

    [Header("Ray Settings")]
    public float maxDistance = 100.0f;
    public LayerMask drawingSurfaceLayer;
    public GameObject standard;

    [Header("Painting Settings")]
    public Color paintColor = Color.black;
    public int brushSize = 5;
    public int textureResolution = 1024; // 해상도 높임 (더 정밀하게)

    [Header("References")]
    public LineRenderer lineRenderer;

    private Dictionary<Collider, Texture2D> drawingTextures = new Dictionary<Collider, Texture2D>();

    // [해결 2] 점이 끊기는 현상 방지를 위한 '이전 프레임 좌표' 저장
    private Vector2? lastDrawUV = null; 
    private Collider lastHitCollider = null;

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
        if(RatioAlignedCanvas.InstanceWithoutCreate?.gameObject.activeSelf == true)
        {
            HandleRaycastingAndPainting();
        }
    }

    void HandleRaycastingAndPainting()
    {
        Ray ray = new Ray(transform.position, standard.transform.position);
        RaycastHit hit;

        bool isHit = Physics.Raycast(ray, out hit, maxDistance, drawingSurfaceLayer);

        // 1. 시각적 레이저(LineRenderer) 길이 업데이트
        // 충돌했으면 그 거리까지, 안 했으면 최대 거리까지
        float visualDistance = isHit ? hit.distance : maxDistance;
        
        if (lineRenderer != null)
        {
            lineRenderer.SetPosition(0, Vector3.zero); // 시작점 (컨트롤러 위치)
            lineRenderer.SetPosition(1, new Vector3(0, 0, visualDistance)); // 끝점 (로컬 좌표계 기준 Z방향)
        }

        // 2. 페인팅 로직 수행
        if (isHit)
        {
            // ... 기존 페인팅 로직 ...
            if (OpenXRHandPinchDetector.Instance.IsPinching)
            {
                PaintOnSprite(hit);
            }
            else
            {
                lastDrawUV = null;
                lastHitCollider = null;
            }
        }
        else
        {
            // 허공을 보고 있을 때 초기화
            lastDrawUV = null;
            lastHitCollider = null;
        }
    }

    void PaintOnSprite(RaycastHit hit)
    {
        Collider hitCollider = hit.collider;
        SpriteRenderer spriteRenderer = hitCollider.GetComponent<SpriteRenderer>();
        BoxCollider boxCol = hitCollider as BoxCollider;

        if (spriteRenderer == null || boxCol == null) return;

        // 1. 텍스처 준비 (초기화)
        Texture2D drawTexture;
        if (!drawingTextures.TryGetValue(hitCollider, out drawTexture))
        {
            // [해결 1] 텍스처 생성 시 크기 왜곡 방지 로직 적용
            drawTexture = InitializeDrawingSprite(spriteRenderer, boxCol);
            drawingTextures.Add(hitCollider, drawTexture);
        }

        // 2. UV 좌표 계산 (Collider 크기 기준 정규화)
        Vector3 localPos = hitCollider.transform.InverseTransformPoint(hit.point);
        
        float uvX = ((localPos.x - boxCol.center.x) / boxCol.size.x) + 0.5f;
        float uvY = ((localPos.y - boxCol.center.y) / boxCol.size.y) + 0.5f;

        // UV가 0~1을 벗어나지 않도록 안전장치
        uvX = Mathf.Clamp01(uvX);
        uvY = Mathf.Clamp01(uvY);

        Vector2 currentUV = new Vector2(uvX, uvY);

        // 3. [해결 2] 선형 보간 (Interpolation) - 점과 점 사이 채우기
        // 다른 벽으로 넘어갔거나, 처음 찍는 점이라면 보간 없이 현재 점만 찍음
        if (lastDrawUV == null || lastHitCollider != hitCollider)
        {
            DrawBrush(drawTexture, currentUV);
        }
        else
        {
            // 이전 위치와 현재 위치 사이를 촘촘하게 채움
            float dist = Vector2.Distance(lastDrawUV.Value, currentUV);
            // 텍스처 크기에 비례하여 단계 수 결정 (너무 많으면 렉 걸림)
            int steps = (int)(dist * textureResolution); 
            
            for (int i = 0; i <= steps; i++)
            {
                float t = (float)i / steps;
                Vector2 lerpedUV = Vector2.Lerp(lastDrawUV.Value, currentUV, t);
                DrawBrush(drawTexture, lerpedUV);
            }
        }

        drawTexture.Apply();

        // 현재 위치를 '이전 위치'로 저장
        lastDrawUV = currentUV;
        lastHitCollider = hitCollider;
    }

    // 실제로 픽셀을 찍는 함수
    void DrawBrush(Texture2D texture, Vector2 uv)
    {
        int centerX = (int)(uv.x * texture.width);
        int centerY = (int)(uv.y * texture.height);

        for (int x = centerX - brushSize; x < centerX + brushSize; x++)
        {
            for (int y = centerY - brushSize; y < centerY + brushSize; y++)
            {
                if (x >= 0 && x < texture.width && y >= 0 && y < texture.height)
                {
                    // 원형 브러시
                    if ((x - centerX) * (x - centerX) + (y - centerY) * (y - centerY) <= brushSize * brushSize)
                    {
                        texture.SetPixel(x, y, paintColor);
                    }
                }
            }
        }
    }

    // 모든 캔버스를 하얗게 지우기
    public void ResetAllCanvases()
    {
        foreach (var entry in drawingTextures)
        {
            Texture2D tex = entry.Value;
            
            // 텍스처 전체 픽셀 수만큼 흰색 배열 생성
            Color[] fillColors = new Color[tex.width * tex.height];
            for (int i = 0; i < fillColors.Length; i++) 
            {
                fillColors[i] = Color.white; // 초기 색상과 맞춰주세요
            }

            tex.SetPixels(fillColors);
            tex.Apply();
        }
        
        Debug.Log("모든 캔버스가 리셋되었습니다.");
    }

    Texture2D InitializeDrawingSprite(SpriteRenderer sr, BoxCollider boxCol)
    {
        // 새 텍스처 생성
        Texture2D newTexture = new Texture2D(textureResolution, textureResolution, TextureFormat.RGBA32, false);
        
        // 흰색으로 채우기 (배경)
        Color[] fillColors = new Color[textureResolution * textureResolution];
        for (int i = 0; i < fillColors.Length; i++) fillColors[i] = Color.white;
        newTexture.SetPixels(fillColors);
        newTexture.Apply();

        // [해결 1 핵심] PPU (Pixels Per Unit) 자동 계산
        // 텍스처의 가로 픽셀 수 / 실제 콜라이더의 가로 길이 = 1유닛당 픽셀 수
        // 이렇게 해야 텍스처가 콜라이더 크기에 딱 맞게 늘어납니다.
        float autoPPU = textureResolution / boxCol.size.x;

        // 너무 작거나 0이면 기본값 방어
        if (autoPPU <= 0.01f) autoPPU = 100f;

        // Sprite 생성
        Sprite newSprite = Sprite.Create(
            newTexture, 
            new Rect(0, 0, newTexture.width, newTexture.height), 
            new Vector2(0.5f, 0.5f), // Pivot Center
            autoPPU 
        );

        sr.sprite = newSprite;
        
        // 더 이상 BoxCollider 사이즈를 강제로 바꾸지 않습니다. (기존 크기 유지)

        Debug.Log($"Initialized Sprite. Texture Size: {textureResolution}, Collider Size X: {boxCol.size.x}, Calculated PPU: {autoPPU}");
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