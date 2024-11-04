using UnityEngine;
using UnityEngine.VFX;

public class CuttingBoardManager : MonoBehaviour
{
    public GameObject kipCutletPrefab; // Assign the KipCutlet-Raw prefab in the Inspector
    public Transform spawnPoint; // Assign an empty GameObject as the spawn point
    public VisualEffect cuttingBoardEffect; // Assign the poof effect here in the Inspector

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger has the tag "chicken-fillet"
        if (other.CompareTag("chicken-fillet"))
        {
            Debug.Log("Chicken fillet detected on the cutting board.");

            // Play the cutting board poof effect before replacing the chicken
            if (cuttingBoardEffect != null)
            {
                cuttingBoardEffect.transform.position = spawnPoint.position; // Ensure the effect is positioned at the spawn point
                cuttingBoardEffect.Play();
                Debug.Log("Cutting board poof effect played.");
            }
            else
            {
                Debug.LogWarning("Cutting board effect is not assigned.");
            }

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

        // Destroy the original chicken fillet object
        Destroy(originalObject);

        Debug.Log("Replaced chicken fillet with KipCutlet-Raw at the specified spawn point.");
    }
}


