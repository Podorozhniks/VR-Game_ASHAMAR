using UnityEngine;

public class CuttingBoardManager : MonoBehaviour
{
    public GameObject kipCutletPrefab; // Assign the KipCutlet-Raw prefab in the Inspector
    public Transform spawnPoint; // Assign an empty GameObject as the spawn point

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger has the tag "chicken-fillet"
        if (other.CompareTag("chicken-fillet"))
        {
            Debug.Log("Chicken fillet detected on the cutting board.");

            // Replace the chicken fillet with KipCutlet-Raw at the specified spawn point
            ReplaceWithKipCutlet(other.gameObject);
        }
    }

    private void ReplaceWithKipCutlet(GameObject originalObject)
    {
        // Check if spawn point is set
        if (spawnPoint == null)
        {
            Debug.LogWarning("Spawn point is not assigned. Please assign a spawn point in the inspector.");
            return;
        }

        // Instantiate the KipCutlet-Raw at the spawn point's position and rotation
        GameObject kipCutlet = Instantiate(kipCutletPrefab, spawnPoint.position, spawnPoint.rotation);

        // Optionally, set any additional properties on the new object (e.g., make it sliceable or grabbable)

        // Destroy the original chicken fillet object
        Destroy(originalObject);

        Debug.Log("Replaced chicken fillet with KipCutlet-Raw at the specified spawn point.");
    }
}


