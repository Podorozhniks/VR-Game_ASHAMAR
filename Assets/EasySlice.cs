using UnityEngine;
using EzySlice;
using System.Collections.Generic;

public class EasySlice : MonoBehaviour
{
    public LayerMask sliceableLayer;         // Layer mask for sliceable objects
    public Collider knifeTipCollider;        // Reference to the knife's tip or blade collider
    public float minSliceableSize = 0.1f;    // Minimum size before the object can be sliced
    public float sliceCooldown = 0.5f;       // Cooldown to prevent repeated slicing
    public List<SliceableObjectInfo> sliceableObjectsInfo;  // List of sliceable objects with their materials

    private float lastSliceTime = 0f;        // Timestamp of the last slice

    // Struct to hold sliceable object type and cross-section material
    [System.Serializable]
    public struct SliceableObjectInfo
    {
        public string objectTag;              // Tag to identify sliceable objects
        public Material surfaceMaterial;      // Material for the main surface of the object
        public Material crossSectionMaterial; // Material to apply to sliced cross-sections
    }

    private void Start()
    {
        
        if (knifeTipCollider != null)
        {
            knifeTipCollider.isTrigger = true;
        }
    }

    // Detect when the knife tip enters a sliceable object
    private void OnTriggerEnter(Collider other)
    {
        // Prevent slicing if the cooldown has not elapsed
        if (Time.time - lastSliceTime < sliceCooldown)
        {
            return;
        }

        Debug.Log("Knife Tip Triggered with: " + other.gameObject.name);  // Log any trigger event

        // Check if the object is on the sliceable layer
        if (((1 << other.gameObject.layer) & sliceableLayer) != 0)
        {
            // Check the object's size before slicing
            if (IsObjectTooSmall(other.gameObject))
            {
                Debug.Log("Object is too small to slice: " + other.gameObject.name);
                return;
            }

            Debug.Log("Slicing object: " + other.gameObject.name);  // Log when slicing starts
            SliceableObjectInfo sliceableInfo = GetSliceableInfo(other.gameObject);
            if (sliceableInfo.crossSectionMaterial != null)
            {
                SliceObject(other.gameObject, sliceableInfo.surfaceMaterial, sliceableInfo.crossSectionMaterial, sliceableInfo.objectTag);
            }
        }
        else
        {
            Debug.Log("Object is not sliceable: " + other.gameObject.name);  // Log non-sliceable objects
        }
    }

    // Retrieve the sliceable object info (including materials) based on object tag
    private SliceableObjectInfo GetSliceableInfo(GameObject sliceableObject)
    {
        foreach (var info in sliceableObjectsInfo)
        {
            if (sliceableObject.CompareTag(info.objectTag))
            {
                return info;
            }
        }

        // Return default SliceableObjectInfo if no match is found
        Debug.LogWarning("No matching sliceable object found for object: " + sliceableObject.name);
        return new SliceableObjectInfo();
    }

    private bool IsObjectTooSmall(GameObject obj)
    {
        // Check if the object's bounding box is smaller than the minimum sliceable size
        Bounds bounds = obj.GetComponent<Renderer>().bounds;
        return (bounds.size.x < minSliceableSize || bounds.size.y < minSliceableSize || bounds.size.z < minSliceableSize);
    }

    private void SliceObject(GameObject sliceableObject, Material surfaceMaterial, Material crossSectionMaterial, string objectTag)
    {
        // Set the plane position to the exact position of the knife tip collider inside the object
        Vector3 planePosition = knifeTipCollider.transform.position;

        // Set the plane normal to the forward direction of the knife blade )
        Vector3 planeNormal = knifeTipCollider.transform.forward;

        // Perform the slice using EzySlice with the specific cross-section material
        SlicedHull slicedObject = sliceableObject.Slice(planePosition, planeNormal, crossSectionMaterial);

        if (slicedObject != null)
        {
            Debug.Log("Object successfully sliced: " + sliceableObject.name);  // Log successful slicing

            // Create upper and lower hulls after slicing
            GameObject upperHull = slicedObject.CreateUpperHull(sliceableObject, crossSectionMaterial);
            GameObject lowerHull = slicedObject.CreateLowerHull(sliceableObject, crossSectionMaterial);

            // Add colliders, rigidbody, and make sure the sliced pieces are sliceable
            MakeSliceable(upperHull, surfaceMaterial, crossSectionMaterial, objectTag);
            MakeSliceable(lowerHull, surfaceMaterial, crossSectionMaterial, objectTag);

            // Destroy the original object
            Destroy(sliceableObject);

            // Update the last slice time to implement cooldown
            lastSliceTime = Time.time;
        }
        else
        {
            Debug.Log("Slicing failed for object: " + sliceableObject.name);  // Log slicing failure
        }
    }

    // Apply surface and cross-section materials explicitly to sliced hulls
    private void MakeSliceable(GameObject slicedHull, Material surfaceMaterial, Material crossSectionMaterial, string objectTag)
    {
        if (slicedHull != null)
        {
            
            MeshRenderer meshRenderer = slicedHull.GetComponent<MeshRenderer>();
            if (meshRenderer == null)
            {
                meshRenderer = slicedHull.AddComponent<MeshRenderer>();
            }

            // Create a material array with surface material and cross-section material
            Material[] newMaterials = new Material[2];
            newMaterials[0] = surfaceMaterial;      
            newMaterials[1] = crossSectionMaterial; 

            // Ensure the sliced object has two materials: one for surface and one for cross-section
            meshRenderer.materials = newMaterials;

            // Add a convex MeshCollider and Rigidbody to allow the object to be sliced again
            MeshCollider meshCollider = slicedHull.AddComponent<MeshCollider>();
            meshCollider.convex = true;

            Rigidbody rb = slicedHull.AddComponent<Rigidbody>();
            rb.mass = 1f;  // You can adjust the mass if necessary

            // Set the sliced object to the sliceable layer to allow it to be sliced again
            slicedHull.layer = LayerMask.NameToLayer("Sliceable");

            // Assign the correct tag to ensure it remains sliceable
            slicedHull.tag = objectTag;

            Debug.Log("Made object sliceable again: " + slicedHull.name);
        }
    }
}




