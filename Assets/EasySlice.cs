using UnityEngine;
using EzySlice;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class EasySlice : MonoBehaviour
{
    public LayerMask sliceableLayer;
    public Collider knifeTipCollider;
    public float minSliceableSize = 0.1f;
    public float sliceCooldown = 0.5f;
    public List<SliceableObjectInfo> sliceableObjectsInfo;

    private float lastSliceTime = 0f;

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

    private void OnTriggerEnter(Collider other)
    {
        if (Time.time - lastSliceTime < sliceCooldown) return;

        if (((1 << other.gameObject.layer) & sliceableLayer) != 0)
        {
            if (IsObjectTooSmall(other.gameObject)) return;

            SliceableObjectInfo sliceableInfo = GetSliceableInfo(other.gameObject);
            if (sliceableInfo.crossSectionMaterial != null)
            {
                SliceObject(other.gameObject, sliceableInfo.surfaceMaterial, sliceableInfo.crossSectionMaterial, sliceableInfo.objectTag);
            }
        }
    }

    private SliceableObjectInfo GetSliceableInfo(GameObject sliceableObject)
    {
        foreach (var info in sliceableObjectsInfo)
        {
            if (sliceableObject.CompareTag(info.objectTag)) return info;
        }
        return new SliceableObjectInfo();
    }

    private bool IsObjectTooSmall(GameObject obj)
    {
        Bounds bounds = obj.GetComponent<Renderer>().bounds;
        return (bounds.size.x < minSliceableSize || bounds.size.y < minSliceableSize || bounds.size.z < minSliceableSize);
    }

    private void SliceObject(GameObject sliceableObject, Material surfaceMaterial, Material crossSectionMaterial, string objectTag)
    {
        Vector3 planePosition = knifeTipCollider.transform.position;
        Vector3 planeNormal = knifeTipCollider.transform.forward;

        SlicedHull slicedObject = sliceableObject.Slice(planePosition, planeNormal, crossSectionMaterial);

        if (slicedObject != null)
        {
            GameObject upperHull = slicedObject.CreateUpperHull(sliceableObject, crossSectionMaterial);
            GameObject lowerHull = slicedObject.CreateLowerHull(sliceableObject, crossSectionMaterial);

            MakeSliceable(upperHull, surfaceMaterial, crossSectionMaterial, objectTag);
            MakeSliceable(lowerHull, surfaceMaterial, crossSectionMaterial, objectTag);

            Destroy(sliceableObject);
            lastSliceTime = Time.time;
        }
    }

    private void MakeSliceable(GameObject slicedHull, Material surfaceMaterial, Material crossSectionMaterial, string objectTag)
    {
        if (slicedHull != null)
        {
            // Set up materials
            MeshRenderer meshRenderer = slicedHull.GetComponent<MeshRenderer>() ?? slicedHull.AddComponent<MeshRenderer>();

            // Retrieve the original materials on the object
            Material[] originalMaterials = meshRenderer.materials;

            // Create a new array with the original materials and the cross-section material
            Material[] newMaterials = new Material[originalMaterials.Length + 1];
            for (int i = 0; i < originalMaterials.Length; i++)
            {
                newMaterials[i] = originalMaterials[i]; // Retain the original materials
            }
            newMaterials[originalMaterials.Length] = crossSectionMaterial; // Add cross-section material at the end

            // Assign the new materials array to the mesh renderer
            meshRenderer.materials = newMaterials;

            // Add physics components
            MeshCollider meshCollider = slicedHull.AddComponent<MeshCollider>();
            meshCollider.convex = true;

            Rigidbody rb = slicedHull.AddComponent<Rigidbody>();
            rb.mass = 1f;

            slicedHull.tag = objectTag;

            // Add XR Grab Interactable component
            var grabInteractable = slicedHull.AddComponent<XRGrabInteractable>();

            // Explicitly set interaction layer mask to Everything
            grabInteractable.interactionLayers = ~0; // Equivalent to "Everything"

            // Create and configure an attach point for close attachment
            GameObject attachPoint = new GameObject("AttachPoint");
            attachPoint.transform.SetParent(slicedHull.transform);
            attachPoint.transform.localPosition = Vector3.zero; // Center of the object
            grabInteractable.attachTransform = attachPoint.transform;

            // Force close interaction by reducing attachEaseInTime
            grabInteractable.attachEaseInTime = 0f; // Ensures instant attachment to hand

            // Optional: allow throwing behavior on detach
            grabInteractable.throwOnDetach = true;

            // Debug log to confirm setup
            Debug.Log($"Configured {slicedHull.name} with close attachment and interaction layers set to Everything.");
        }
    }






}


















