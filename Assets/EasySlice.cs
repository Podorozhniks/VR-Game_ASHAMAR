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
            MeshRenderer meshRenderer = slicedHull.GetComponent<MeshRenderer>() ?? slicedHull.AddComponent<MeshRenderer>();
            meshRenderer.materials = new Material[] { surfaceMaterial, crossSectionMaterial };

            MeshCollider meshCollider = slicedHull.AddComponent<MeshCollider>();
            meshCollider.convex = true;

            Rigidbody rb = slicedHull.AddComponent<Rigidbody>();
            rb.mass = 1f;

            slicedHull.tag = objectTag;

            var grabInteractable = slicedHull.AddComponent<XRGrabInteractable>();

            // Create and configure an attach point for precise hand attachment
            GameObject attachPoint = new GameObject("AttachPoint");
            attachPoint.transform.SetParent(slicedHull.transform);
            attachPoint.transform.localPosition = Vector3.zero;
            grabInteractable.attachTransform = attachPoint.transform;

            // Set interaction layers to interact with everything
            grabInteractable.interactionLayers = InteractionLayerMask.GetMask("Everything");

            grabInteractable.attachEaseInTime = 0f; // Instantly attaches to hand
            grabInteractable.throwOnDetach = true;

            Debug.Log($"Configured {slicedHull.name} with close attachment.");
        }
    }
}


















