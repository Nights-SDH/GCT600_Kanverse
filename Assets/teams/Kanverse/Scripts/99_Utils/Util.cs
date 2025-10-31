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
}