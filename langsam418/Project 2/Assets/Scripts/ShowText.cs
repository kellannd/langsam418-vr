using UnityEngine;

public class ShowText : MonoBehaviour
{
    public Transform playerHead;
    public GameObject textObject;

    public float minDistance = 2f;
    public float maxDistance = 5f;

    void Start()
    {
        textObject.SetActive(false);
    }

    void Update()
    {
        float distance = Vector3.Distance(
            playerHead.position,
            transform.position
        );

        if (distance >= minDistance &&
            distance <= maxDistance)
        {
            textObject.SetActive(true);
        }
        else
        {
            textObject.SetActive(false);
        }
    }
}
