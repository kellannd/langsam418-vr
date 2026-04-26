using UnityEngine;

public class ProximityPinballAudio : MonoBehaviour
{
    public Transform playerHead;   // CenterEyeAnchor
    public AudioSource audioSource;

    public float maxDistance = 8f; // starts fading in
    public float minDistance = 1.5f; // full volume

    public float fadeSpeed = 2f;

    void Update()
    {
        float distance = Vector3.Distance(playerHead.position, transform.position);

        // 0 = far, 1 = close
        float targetVolume = Mathf.InverseLerp(maxDistance, minDistance, distance);

        targetVolume = Mathf.Clamp01(targetVolume);

        // smooth fade
        audioSource.volume = Mathf.Lerp(audioSource.volume, targetVolume, Time.deltaTime * fadeSpeed);

        // optional: stop completely when very far
        if (targetVolume <= 0.01f && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        else if (targetVolume > 0.01f && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}
