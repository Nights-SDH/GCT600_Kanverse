using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class OCRClient : MonoBehaviour
{
    private string serverUrl = "http://localhost:8000/ocr";
    string filePath = Path.Combine(Application.streamingAssetsPath, "Frame 14.png"); 

    // Start에서 테스트 (예시)
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(UploadFile(filePath));
        }
    }

    IEnumerator UploadFile(string filePath)
    {
        // 파일이 실제로 존재하는지 확인
        if (!File.Exists(filePath))
        {
            Debug.LogError("파일을 찾을 수 없습니다: " + filePath);
            yield break;
        }

        byte[] imageBytes = File.ReadAllBytes(filePath);

        WWWForm form = new WWWForm();
        form.AddBinaryData("file", imageBytes, "image.png", "image/png");

        using (UnityWebRequest www = UnityWebRequest.Post(serverUrl, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("에러: " + www.error);
            }
            else
            {
                string jsonResult = www.downloadHandler.text;
                Debug.Log("결과: " + jsonResult);
            }
        }
    }
}