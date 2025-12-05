using UnityEngine;

public class Icon : MonoBehaviour
{
    [Header("UP/DOWN")]
    public float floatAmplitude = 0.1f; // 위아래로 얼마나 움직일지 (높이)
    public float floatSpeed = 1.0f;     // 위아래 움직이는 속도

    [Header("ROTATE")]
    public bool rotate = true;
    public float rotateSpeed = 50f;     // 초당 회전 속도 (도 단위)

    private Vector3 _startPos;

    void Start()
    {
        _startPos = transform.position;
    }

    void Update()
    {
        // 위아래 둥둥
        float newY = _startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = new Vector3(_startPos.x, newY, _startPos.z);

        // 살짝 회전(선택)
        if (rotate)
        {
            transform.Rotate(0f, rotateSpeed * Time.deltaTime, 0f, Space.World);
        }
    }
}
