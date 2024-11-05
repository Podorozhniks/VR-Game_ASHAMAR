using UnityEngine;
using UnityEngine.VFX; // Import VisualEffect namespace

public class RiceCookingManager : MonoBehaviour
{
    [Header("Rice Pouring")]
    public ParticleSystem ricePouringParticles;
    public Transform bowlTransform;
    public float pouringAngleThreshold = 45f;

    [Header("Rice Cooking Setup")]
    public GameObject riceModelEmpty;
    public GameObject riceModelWithRicePrefab;
    public ParticleSystem smokeParticles;
    public VisualEffect poofEffect; // VisualEffect component for poof effect
    public float cookingDuration = 5f;
    public Collider lidCollider;
    public Transform lidTargetPosition;
    public float snapDistance = 0.1f;

    private bool isPouring = false;
    private bool riceAdded = false;
    private bool isCooking = false;
    private bool lidPlaced = false;
    private float cookingTimer = 0f;
    private GameObject instantiatedRiceModel;

    private void Start()
    {
        riceModelEmpty.SetActive(true);

        if (smokeParticles != null && smokeParticles.isPlaying)
        {
            smokeParticles.Stop();
            Debug.Log("Smoke particles stopped at start.");
        }

        if (ricePouringParticles != null && ricePouringParticles.isPlaying)
        {
            ricePouringParticles.Stop();
            Debug.Log("Rice pouring particles stopped at start.");
        }
    }

    private void Update()
    {
        float tiltAngle = Vector3.Angle(bowlTransform.up, Vector3.up);
        if (tiltAngle > pouringAngleThreshold && !isPouring && !riceAdded)
        {
            StartPouring();
        }
        else if (tiltAngle <= pouringAngleThreshold && isPouring)
        {
            StopPouring();
        }

        if (isCooking)
        {
            cookingTimer += Time.deltaTime;
            if (cookingTimer >= cookingDuration)
            {
                FinishCooking();
            }
        }
    }

    private void StartPouring()
    {
        if (ricePouringParticles != null)
        {
            ricePouringParticles.Play();
            isPouring = true;
            Debug.Log("Rice pouring started.");
        }
    }

    private void StopPouring()
    {
        if (ricePouringParticles != null)
        {
            ricePouringParticles.Stop();
            isPouring = false;
            Debug.Log("Rice pouring stopped.");

            if (!riceAdded)
            {
                AddRiceToCooker();
            }
        }
    }

    private void AddRiceToCooker()
    {
        riceAdded = true;
        riceModelEmpty.SetActive(false);

        // Instantiate the new rice model
        instantiatedRiceModel = Instantiate(riceModelWithRicePrefab, riceModelEmpty.transform.position, riceModelEmpty.transform.rotation);

        // Play the poof effect for adding rice to the cooker
        if (poofEffect != null)
        {
            poofEffect.transform.position = riceModelEmpty.transform.position;
            poofEffect.Play();
            Debug.Log("Poof effect played for adding rice to the cooker.");
        }

        // Reparent lidCollider and smokeParticles if necessary
        if (lidCollider != null && instantiatedRiceModel != null)
        {
            lidCollider.transform.SetParent(instantiatedRiceModel.transform, true);
        }

        if (smokeParticles != null && instantiatedRiceModel != null)
        {
            smokeParticles.transform.SetParent(instantiatedRiceModel.transform, true);
        }

        Debug.Log("Rice added to the cooker.");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == lidCollider && riceAdded && !lidPlaced)
        {
            float distanceToTarget = Vector3.Distance(lidCollider.transform.position, lidTargetPosition.position);
            if (distanceToTarget <= snapDistance)
            {
                SnapLidInPlace();
                StartCooking();
            }
            else
            {
                Debug.Log("Lid is in the trigger area but not close enough to snap.");
            }
        }
    }

    private void SnapLidInPlace()
    {
        lidCollider.transform.position = lidTargetPosition.position;
        lidCollider.transform.rotation = lidTargetPosition.rotation;

        lidPlaced = true;

        Debug.Log("Lid snapped in place.");
    }

    private void StartCooking()
    {
        if (lidPlaced && riceAdded)
        {
            isCooking = true;
            cookingTimer = 0f;

            if (smokeParticles != null)
            {
                smokeParticles.Play();
                Debug.Log("Smoke particle effect started.");
            }
        }
    }

    private void FinishCooking()
    {
        isCooking = false;

        if (smokeParticles != null && smokeParticles.isPlaying)
        {
            smokeParticles.Stop();
            Debug.Log("Rice cooking completed. Smoke particle effect stopped.");
        }

        // Play the poof effect to signify cooking completed
        if (poofEffect != null)
        {
            poofEffect.transform.position = instantiatedRiceModel.transform.position;
            poofEffect.Play();
            Debug.Log("Poof effect played for finished cooking.");
        }

        Debug.Log("Rice is fully cooked!");
    }
}



