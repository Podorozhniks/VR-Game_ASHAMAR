using UnityEngine;
using UnityEngine.VFX;

public class CuttingBoardManager : MonoBehaviour
{
    public GameObject kipCutletPrefab; 
    public Transform spawnPoint; 
    public VisualEffect cuttingBoardEffect;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger has the tag "chicken-fillet"
        if (other.CompareTag("chicken-fillet"))
        {
            Debug.Log("Chicken fillet detected on the cutting board.");

          
            if (cuttingBoardEffect != null)
            {
                cuttingBoardEffect.transform.position = spawnPoint.position;
                cuttingBoardEffect.Play();
                Debug.Log("Cutting board poof effect played.");
            }
            else
            {
                Debug.LogWarning("Cutting board effect is not assigned.");
            }

           
            ReplaceWithKipCutlet(other.gameObject);
        }
    }

    private void ReplaceWithKipCutlet(GameObject originalObject)
    {
        
        if (spawnPoint == null)
        {
            Debug.LogWarning("Spawn point is not assigned. Please assign a spawn point in the inspector.");
            return;
        }

       
        GameObject kipCutlet = Instantiate(kipCutletPrefab, spawnPoint.position, spawnPoint.rotation);

        
        Destroy(originalObject);

        Debug.Log("Replaced chicken fillet with KipCutlet-Raw at the specified spawn point.");
    }
}


