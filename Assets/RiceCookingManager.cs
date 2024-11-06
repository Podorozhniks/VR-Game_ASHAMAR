using UnityEngine;
using UnityEngine.VFX;

public class RiceCookingManager : MonoBehaviour
{
    public bool RiceDone = false;
    [Header("Rice Pouring")]
    public ParticleSystem ricePouringParticles;  
    public Transform bowlTransform;               
    public float pouringAngleThreshold = 45f;     

    [Header("Rice Cooker Setup")]
    public GameObject riceObject;                 
    public ParticleSystem smokeParticles;         
    public VisualEffect poofEffectAddRice;        
    public VisualEffect poofEffectCookingDone;    
    public float cookingDuration = 5f;            
    public Collider lidCollider;                 
    public Transform lidTargetPosition;          
    public float snapDistance = 0.1f;            
    private bool isPouring = false;               
    private bool riceAdded = false;               
    private bool isCooking = false;               
    private bool lidPlaced = false;             
    private float cookingTimer = 0f;             
    private Rigidbody lidRigidbody;              

    private void Start()
    {
       
        if (riceObject != null)
            riceObject.SetActive(false);

       
        if (smokeParticles != null && smokeParticles.isPlaying)
            smokeParticles.Stop();
        if (poofEffectAddRice != null)
            poofEffectAddRice.Stop();
        if (poofEffectCookingDone != null)
            poofEffectCookingDone.Stop();

        
        lidRigidbody = lidCollider.GetComponent<Rigidbody>();
        if (lidRigidbody == null)
            Debug.LogWarning("Lid does not have a Rigidbody. Please add one for proper physics control.");

        Debug.Log("Initial setup complete: Rice object hidden.");
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
       
        if (poofEffectAddRice != null)
        {
            poofEffectAddRice.transform.position = riceObject.transform.position;
            poofEffectAddRice.Play();
            Debug.Log("Poof effect played for adding rice.");
        }

        if (riceObject != null)
            riceObject.SetActive(true);

        riceAdded = true;
        Debug.Log("Rice object enabled inside the cooker.");
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
            RiceDone = true;
        }
    }

    private void SnapLidInPlace()
    {
        
        lidCollider.transform.position = lidTargetPosition.position;
        lidCollider.transform.rotation = lidTargetPosition.rotation;

        
        lidPlaced = true;

      
        Rigidbody lidRigidbody = lidCollider.GetComponent<Rigidbody>();
        if (lidRigidbody != null)
        {
            lidRigidbody.isKinematic = true;      
            lidRigidbody.useGravity = false;      
            lidRigidbody.constraints = RigidbodyConstraints.FreezeAll;  

            Debug.Log("Lid Rigidbody set to kinematic and all constraints applied.");
        }
        else
        {
            Debug.LogWarning("Rigidbody component not found on lid. Make sure the lid has a Rigidbody component.");
        }
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
                Debug.Log("Smoke particle effect started for cooking.");
            }
        }
    }
    public bool isCookingComplete { get; private set; } = false;
    private void FinishCooking()
    {
        isCooking = false;
        isCookingComplete = true;

        if (smokeParticles != null && smokeParticles.isPlaying)
        {
            smokeParticles.Stop();
            Debug.Log("Cooking completed. Smoke particle effect stopped.");
        }

       
        if (poofEffectCookingDone != null)
        {
            poofEffectCookingDone.transform.position = riceObject.transform.position;
            poofEffectCookingDone.Play();
            Debug.Log("Poof effect played to indicate cooking is done.");
        }

        Debug.Log("Rice is fully cooked!");
    }
}




