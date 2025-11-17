using UnityEngine;

public class PauseController : SingletonObject<PauseController>
{
    private float defaultGameSpeed = TimeScaleConstant.Default;
    public bool isPauseActive => Time.timeScale == 0;
    public bool isSlowActive => Time.timeScale < defaultGameSpeed;

    private void SetGameSpeed(float speed)
    {
        Time.timeScale = speed;
    }

    public void ChangeDefaultGameSpeed(float speed)
    {
        defaultGameSpeed = speed;
        SetGameSpeed(defaultGameSpeed);
    }

    public void ResetSpeed() { SetGameSpeed(defaultGameSpeed); }

    public void SetZoomInSlow() { SetGameSpeed(TimeScaleConstant.ZoomInSlowSpeed); }

    public void TryPauseGame()
    {
        if (!isPauseActive)
        {
            SetGameSpeed(0);
        }
    }

    public void TryResumeGame()
    {
        if (isPauseActive)
        {
            ResetSpeed();
        }
    }

    public bool IsPause() { return isPauseActive; }
}