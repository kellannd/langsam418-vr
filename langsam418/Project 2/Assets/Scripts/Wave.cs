using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DanceWaveDistanceController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerHead;

    [Header("Distance Settings")]
    [SerializeField] private float waveDistance = 2.0f;
    [SerializeField] private float stopWaveDistance = 2.4f;

    private Animator animator;
    private static readonly int IsCloseHash = Animator.StringToHash("IsClose");

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (playerHead == null)
        {
            return;
        }

        float distance = Vector3.Distance(playerHead.position, transform.position);

        bool currentlyClose = animator.GetBool(IsCloseHash);

        if (!currentlyClose && distance <= waveDistance)
        {
            animator.SetBool(IsCloseHash, true);
        }
        else if (currentlyClose && distance >= stopWaveDistance)
        {
            animator.SetBool(IsCloseHash, false);
        }
    }
}
