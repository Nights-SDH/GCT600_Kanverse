using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public static class Util
{
    public static bool IsRootGameObject(GameObject gameObject)
    {
        return gameObject.transform.parent == null;
    }

    public static IEnumerator SetActiveDuringTime(GameObject gameObject, float duration)
    {
        SetActive(gameObject, true);
        yield return new WaitForSeconds(duration);
        SetActive(gameObject, false);
    }

    public static void SetActive(MonoBehaviour gameObject, bool isActive)
    {
        SetActive(gameObject?.gameObject, isActive);
    }

    public static void SetActive(Button gameObject, bool isActive)
    {
        SetActive(gameObject.gameObject, isActive);
    }

    public static bool SetActive(GameObject gameObject, bool isActive)
    {
        if (gameObject == null || gameObject.activeSelf == isActive) return false;
        gameObject.SetActive(isActive);
        return true;
    }

    public static IEnumerator PlayFunctionAfterDelay(float delaySeconds, System.Action action)
    {
        yield return new WaitForSeconds(delaySeconds);
        action?.Invoke();
    }

    public static Stack<T> RemoveSpecificElementInStack<T>(Stack<T> stack, T itemToRemove)
    {
        // 1. 스택을 리스트로 변환 (LIFO 순서)
        List<T> tempList = stack.ToArray<T>().ToList();

        // 2. 리스트에서 모든 일치 항목 제거
        tempList.RemoveAll(item => EqualityComparer<T>.Default.Equals(item, itemToRemove));

        // 3. 순서를 뒤집어 원래 순서로 복원
        tempList.Reverse();

        // 4. 리스트를 다시 스택으로 만들어 반환
        return new Stack<T>(tempList);
    }

    public static IEnumerator StartCallBackAfterDelay(Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action();
    }

    public static T ParseEnumFromString<T>(string value)
    {
        return (T)Enum.Parse(typeof(T), value);
    }

    public static void FlipDirectionX(GameObject gameObject)
    {
        if (gameObject.GetComponent<PolygonCollider2D>() != null)
            FlipLocalScaleX(gameObject.GetComponent<PolygonCollider2D>());
        else
        {
            FlipDirectionX(gameObject.transform);
            FlipLocalScaleX(gameObject.transform);
        }
    }

    public static void FlipDirectionX(Transform transform)
    {
        Vector3 pos = transform.localPosition;
        transform.localPosition = new Vector3(-pos.x, pos.y, pos.z);
    }

    public static void FlipLocalScaleX(GameObject gameObject)
    {
        if (gameObject.GetComponent<PolygonCollider2D>() != null)
            FlipLocalScaleX(gameObject.GetComponent<PolygonCollider2D>());
        else
            FlipLocalScaleX(gameObject.transform);
    }

    public static void FlipLocalScaleX(PolygonCollider2D polygonCollider)
    {
        if (polygonCollider != null)
        {
            Vector2[] points = polygonCollider.points;
            for (int i = 0; i < points.Length; i++)
            {
                points[i].x = -points[i].x;
            }
            polygonCollider.points = points;
        }
    }

    public static void FlipLocalScaleX(Transform transform)
    {
        Vector3 scale = transform.localScale;
        scale.x = -scale.x; // x 값의 부호 반전
        transform.localScale = scale;
    }

    public static T FindComponentInHierarchy<T>(GameObject root) where T : Component
    {
        // 현재 GameObject에서 컴포넌트 찾기
        T component = root.GetComponent<T>();
        if (component != null)
        {
            return component;
        }

        // 자식 오브젝트에서 재귀적으로 탐색
        foreach (Transform child in root.transform)
        {
            component = FindComponentInHierarchy<T>(child.gameObject);
            if (component != null)
            {
                return component;
            }
        }

        return null; // 컴포넌트를 찾지 못한 경우
    }

    public static Vector2 GetLocalSize(SpriteRenderer spriteRenderer)
    {
        if (spriteRenderer != null && spriteRenderer.sprite != null)
        {
            Vector2 pixelSize = spriteRenderer.sprite.rect.size; // 픽셀 크기
            float pixelsPerUnit = spriteRenderer.sprite.pixelsPerUnit; // PPU 값

            return pixelSize / pixelsPerUnit; // 로컬 크기 반환
        }

        Debug.LogError("SpriteRenderer or Sprite is null!");
        return Vector2.zero;
    }

    public static GameObject GetMonsterGameObject(Collider2D collision)
    {
        // TODO: 보스때문에 임시로 수정해놨는데 나중에 다시 수정해야함 - KMJ
        if (collision.gameObject.transform.parent == null) return collision.gameObject;
        else return collision.gameObject.transform.parent?.gameObject;
    }

    public static Vector3 GetMousePointWithPerspectiveCamera()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Mathf.Abs(Camera.main.transform.position.z);
        return Camera.main.ScreenToWorldPoint(mousePosition); ;
    }

    public static bool IsEditor => Application.isEditor;

    public static IEnumerator PlayInstantEffect(GameObject effect, float duration)
    {
        Timer timer = new Timer(duration);
        if (effect != null)
        {
            SetActive(effect, true);
        }
        while (timer.Tick())
        {
            yield return null;
        }
        SetActive(effect, false);
    }

    public static void SetRotationZ(GameObject gameObject, float rotationZRatio)
    {
        if (rotationZRatio > 1.0f) rotationZRatio = 1.0f;
        else if (rotationZRatio < 0.0f) rotationZRatio = 0.0f;
        if (gameObject != null) gameObject.transform.rotation = Quaternion.Euler(0, 0, rotationZRatio * 360);
    }

    public static void ResetRotationZ(GameObject gameObject)
    {
        if (gameObject != null) gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public static void RotateObjectForwardingDirection(GameObject gameObject, Vector3 direction, bool hasTopDownStructure)
    {
        if (gameObject == null) return;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (hasTopDownStructure)
        {
            var localScale = gameObject.transform.localScale;
            if (direction.x < 0) // 위아래가 뒤집히면 안되는 경우 왼쪽으로 날아갈때 위 아래를 뒤집어 주어야 함.
                gameObject.transform.localScale = new Vector3(localScale.x, -localScale.y, localScale.z);
            else
                gameObject.transform.localScale = new Vector3(localScale.x, localScale.y, localScale.z);
        }
        gameObject.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}