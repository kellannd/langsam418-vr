using UnityEngine;

public class LaptopController : MonoBehaviour
{
    public Transform screenPivot;
    public float openAngle = -110f;
    public float speed = 5f;

    private float targetAngle = 0f;

    void Update()
    {
        // Toggle with key (for testing)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            targetAngle = (targetAngle == 0f) ? openAngle : 0f;
        }

        float current = screenPivot.localEulerAngles.x;

        // Fix angle wrapping (Unity uses 0–360)
        if (current > 180) current -= 360;

        float newAngle = Mathf.Lerp(current, targetAngle, Time.deltaTime * speed);

        screenPivot.localEulerAngles = new Vector3(newAngle, 0, 0);
    }
}