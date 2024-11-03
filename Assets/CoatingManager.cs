using System.Collections.Generic;
using UnityEngine;

public class CoatingManager : MonoBehaviour
{
    public Material eggCoatedMaterial;       // Material after coating with eggs
    public Material flourCoatedMaterial;     // Material after coating with flour
    public Material pankoCoatedMaterial;     // Material after coating with panko (breadcrumbs)

    private int coatingStage = 0;            // Tracks the coating stage (0 = none, 1 = egg, 2 = flour, 3 = panko)
    private Dictionary<string, Material> bowlMaterials; // Dictionary to associate bowl names with materials

    private void Start()
    {
        // Initialize the dictionary with bowl names and corresponding materials
        bowlMaterials = new Dictionary<string, Material>
        {
            { "Bowl - Eggs", eggCoatedMaterial },
            { "Bowl - Flour", flourCoatedMaterial },
            { "Bowl - Panko", pankoCoatedMaterial }
        };
    }

    private void OnTriggerEnter(Collider other)
    {
        // Debug log to show what object the chicken fillet collided with
        Debug.Log($"Chicken fillet triggered with: {other.gameObject.name}");

        // Check if the object we collided with is one of the coating bowls
        if (bowlMaterials.ContainsKey(other.gameObject.name))
        {
            Debug.Log($"Trigger detected with a valid coating bowl: {other.gameObject.name}");

            // Get the material for this bowl
            Material nextCoatingMaterial = bowlMaterials[other.gameObject.name];

            // Only apply the material if it matches the next stage in the coating process
            if (IsNextCoatingStage(other.gameObject.name))
            {
                ApplyCoating(nextCoatingMaterial);
                Debug.Log($"Chicken coated with {other.gameObject.name}.");
            }
            else
            {
                Debug.Log("Not the correct order for coating. Ignoring this trigger.");
            }
        }
        else
        {
            Debug.Log("Trigger with a non-coating object.");
        }
    }

    private bool IsNextCoatingStage(string bowlName)
    {
        // Define the order of the coating stages: Eggs -> Flour -> Panko
        if (bowlName == "Bowl - Eggs" && coatingStage == 0)
        {
            coatingStage = 1;
            return true;
        }
        else if (bowlName == "Bowl - Flour" && coatingStage == 1)
        {
            coatingStage = 2;
            return true;
        }
        else if (bowlName == "Bowl - Panko" && coatingStage == 2)
        {
            coatingStage = 3;
            return true;
        }

        // If not in the correct order, return false
        return false;
    }

    private void ApplyCoating(Material coatingMaterial)
    {
        // Get the MeshRenderer and apply the new material for coating
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            meshRenderer.material = coatingMaterial;
            Debug.Log("Coating applied successfully.");
        }
        else
        {
            Debug.LogWarning("No MeshRenderer found on the object to apply the coating material.");
        }
    }
}
