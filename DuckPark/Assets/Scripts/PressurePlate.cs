using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private GameObject objectToUnhide;
    [SerializeField] private Collider2D plateCollider;
    private int playersOnPlate = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (plateCollider == null)
            plateCollider = GetComponent<Collider2D>();
        if (plateCollider == null)
            Debug.LogWarning("PressurePlate: No Collider2D found or assigned!");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && plateCollider != null)
        {
            if (plateCollider.IsTouching(other))
            {
                playersOnPlate++;
                Debug.Log($"Player entered pressure plate. Players on plate: {playersOnPlate}");
                if (objectToUnhide != null)
                    objectToUnhide.SetActive(true);
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && plateCollider != null)
        {
            if (!plateCollider.OverlapPoint(other.transform.position))
            {
                playersOnPlate = Mathf.Max(0, playersOnPlate - 1);
                Debug.Log($"Player exited pressure plate. Players on plate: {playersOnPlate}");
                if (objectToUnhide != null && playersOnPlate == 0)
                    objectToUnhide.SetActive(false);
            }
        }
    }
}
