using UnityEngine;
using Meta.XR.Samples;
using PassthroughCameraSamples;
using UnityEngine.UI;
using System.Collections;

public class CameraViewerManager : MonoBehaviour
{
    [SerializeField] private WebCamTextureManager m_webCamTextureManager;

    [SerializeField] private RawImage m_image;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private IEnumerator Start()
    {
        while (m_webCamTextureManager.WebCamTexture == null)
        {
            yield return null;
        }
        m_image.texture = m_webCamTextureManager.WebCamTexture;
    }
}