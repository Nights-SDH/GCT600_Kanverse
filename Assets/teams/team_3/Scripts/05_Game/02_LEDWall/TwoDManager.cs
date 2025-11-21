using UnityEngine;
using UnityEngine.XR.Management;

public class TwoDManager : SingletonObject<TwoDManager>
{
    void Start()
    {
        // XR 시스템을 중지하고 비활성화합니다.
        if (XRGeneralSettings.Instance != null && XRGeneralSettings.Instance.Manager != null)
        {
            XRGeneralSettings.Instance.Manager.StopSubsystems();
            XRGeneralSettings.Instance.Manager.DeinitializeLoader();
        }
        
        // XR 비활성화 후, 마우스가 정상 작동하도록 커서를 보이게 합니다.
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
