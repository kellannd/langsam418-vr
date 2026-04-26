using UnityEngine;

public class Laptop : MonoBehaviour
{
    public GameObject openLaptop;
    public GameObject closedLaptop;

    public Transform playerHead;
    public float triggerDistance = 1.0f;

    private bool playerNearby = false;

    void Update()
    {
        float distance = Vector3.Distance(playerHead.position, transform.position);

        // Player entered range
        if (distance < triggerDistance && !playerNearby)
        {
            playerNearby = true;
            OpenLaptop();
        }

        // Player left range
        if (distance >= triggerDistance && playerNearby)
        {
            playerNearby = false;
            CloseLaptop();
        }
    }

    void OpenLaptop()
    {
        openLaptop.SetActive(true);
        closedLaptop.SetActive(false);
    }

    void CloseLaptop()
    {
        openLaptop.SetActive(false);
        closedLaptop.SetActive(true);
    }
}
