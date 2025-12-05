using UnityEngine;

public class CardInitializer : MonoBehaviour
{
    public void Initialize(Texture2D texture)
    {
        // CardPrefab에 있는 MeshRenderer 직접 가져오기
        MeshRenderer mr = GetComponent<MeshRenderer>();
        if (mr == null)
        {
            Debug.LogError("❌ MeshRenderer가 CardPrefab에 없습니다!");
            return;
        }

        // 텍스처 필터링 선명하게
        texture.filterMode = FilterMode.Point;

        // 머티리얼 생성 & 적용
        Material mat = new Material(Shader.Find("Unlit/Texture"));
        mat.mainTexture = texture;
        mr.material = mat;

        // 비율 유지한 크기 조정
        float aspect = (float)texture.width / texture.height;

    // 카드의 실제 높이(월드) — 원하는 크기만 조절하면 됨
        float worldHeight = 0.1f;  // 20cm
        float worldWidth = worldHeight * aspect;

        // 실제 월드 크기에 맞게 스케일 적용
        transform.localScale = new Vector3(worldWidth, worldHeight, 1f);

        transform.localRotation = Quaternion.identity;
    }
}
