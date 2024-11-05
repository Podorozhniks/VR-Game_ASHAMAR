using System.Collections.Generic;
using UnityEngine;

public class CoatingManager : MonoBehaviour
{
    public Material eggCoatedMaterial;      
    public Material flourCoatedMaterial;    
    public Material pankoCoatedMaterial;     

    private int coatingStage = 0;           
    private Dictionary<string, Material> bowlMaterials; 

    private void Start()
    {
        
        bowlMaterials = new Dictionary<string, Material>
        {
            { "Bowl - Eggs", eggCoatedMaterial },
            { "Bowl - Flour", flourCoatedMaterial },
            { "Bowl - Panko", pankoCoatedMaterial }
        };
    }

    private void OnTriggerEnter(Collider other)
    {
       
        Debug.Log($"Chicken fillet triggered with: {other.gameObject.name}");

       
        if (bowlMaterials.ContainsKey(other.gameObject.name))
        {
            Debug.Log($"Trigger detected with a valid coating bowl: {other.gameObject.name}");

           
            Material nextCoatingMaterial = bowlMaterials[other.gameObject.name];

            
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
