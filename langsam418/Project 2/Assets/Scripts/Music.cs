using UnityEngine;

public class Music: MonoBehaviour
{
    public Transform playerHead; // XR camera (Meta headset)
    public AudioSource audioSource;

    public float maxDistance = 10f;
    public float minDistance = 1f;

    void Update()
    {
        float distance = Vector3.Distance(playerHead.position, transform.position);

        float t = Mathf.InverseLerp(maxDistance, minDistance, distance);
        audioSource.volume = Mathf.Clamp01(t);
    }
}