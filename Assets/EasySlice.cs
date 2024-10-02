using EzySlice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasySlice : MonoBehaviour
{
    public Material crossSectionMaterial;  // Material to apply to the sliced surface
    public LayerMask sliceableLayer;       // Layer mask for sliceable objects
    public Collider knifeTipCollider;      // Reference to the knife's tip or blade collider
    public float minSliceableSize = 0.1f;  // Minimum size before the object can be sliced
    public float sliceCooldown = 0.5f;     // Cooldown to prevent repeated slicing

    private float lastSliceTime = 0f;      // Timestamp of the last slice

    private void Start()
    {
        // Ensure that the knife tip collider has "Is Trigger" enabled
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
            SliceObject(other.gameObject);
        }
        else
        {
            Debug.Log("Object is not sliceable: " + other.gameObject.name);  // Log non-sliceable objects
        }
    }

    private bool IsObjectTooSmall(GameObject obj)
    {
        // Check if the object's bounding box is smaller than the minimum sliceable size
        Bounds bounds = obj.GetComponent<Renderer>().bounds;
        if (bounds.size.x < minSliceableSize || bounds.size.y < minSliceableSize || bounds.size.z < minSliceableSize)
        {
            return true;
        }
        return false;
    }

    private void SliceObject(GameObject sliceableObject)
    {
        // Set the plane position to the exact position of the knife tip collider inside the object
        Vector3 planePosition = knifeTipCollider.transform.position;

        // Set the plane normal to the forward direction of the knife blade (adjust as per your knife model)
        Vector3 planeNormal = knifeTipCollider.transform.forward;

        // Log the plane position and normal for debugging purposes
        Debug.Log("Plane Position (Knife Tip): " + planePosition);
        Debug.Log("Plane Normal (Knife Forward): " + planeNormal);

        // Perform the slice using EzySlice
        SlicedHull slicedObject = sliceableObject.Slice(planePosition, planeNormal, crossSectionMaterial);

        if (slicedObject != null)
        {
            Debug.Log("Object successfully sliced: " + sliceableObject.name);  // Log successful slicing

            // Create upper and lower hulls after slicing
            GameObject upperHull = slicedObject.CreateUpperHull(sliceableObject, crossSectionMaterial);
            GameObject lowerHull = slicedObject.CreateLowerHull(sliceableObject, crossSectionMaterial);

            // Add colliders, rigidbody, and make sure the sliced pieces are sliceable
            MakeSliceable(upperHull);
            MakeSliceable(lowerHull);

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

    private void MakeSliceable(GameObject obj)
    {
        // Add MeshCollider and set it to convex for physics interaction
        MeshCollider meshCollider = obj.AddComponent<MeshCollider>();
        meshCollider.convex = true;

        // Add a Rigidbody so the object can react to physics
        Rigidbody rb = obj.AddComponent<Rigidbody>();

        // Set the sliced object to the same layer as the original sliceable object
        obj.layer = LayerMask.NameToLayer("Sliceable");  // Ensure the "Sliceable" layer is correctly set in your project

        // Optionally, log the setup
        Debug.Log("Made object sliceable: " + obj.name);
    }
}