using UnityEngine;
using UnitySimpleLiquid;

public class CookingManager : MonoBehaviour
{
    public LiquidContainer panLiquidContainer;
    public Material cookedChickenMaterial;
    public ParticleSystem smokeParticles;  // Ensure Play on Awake is disabled in the Inspector
    public float requiredOilLevel = 0.1f; // 10% oil required
    public float cookingDuration = 5f; // Time required to cook the chicken

    private bool hasOil = false;
    private bool isCooking = false;
    private float cookingTimer = 0f;
    private Collider panCollider;

    private void Start()
    {
        // Get the pan's collider and set it to active but not trigger initially
        panCollider = GetComponent<Collider>();
        if (panCollider != null)
        {
            panCollider.isTrigger = false; // Disable trigger mode at the start
            Debug.Log("Pan collider found and set to non-trigger mode initially.");
        }

        // Ensure the smoke particle effect is not playing initially
        if (smokeParticles != null)
        {
            smokeParticles.Stop();
            smokeParticles.Clear(); // Ensure it starts from a clean state
            Debug.Log("Smoke particles stopped and cleared at start.");
        }
    }

    private void Update()
    {
        // Check if the oil level in the pan is sufficient
        if (!hasOil && panLiquidContainer.FillAmountPercent >= requiredOilLevel)
        {
            hasOil = true;
            EnablePanTrigger();
            Debug.Log("Oil added to the pan. Pan trigger enabled.");
        }

        // Handle cooking timer if cooking is in progress
        if (isCooking)
        {
            cookingTimer += Time.deltaTime;
            if (cookingTimer >= cookingDuration)
            {
                FinishCooking();
            }
        }
    }

    private void EnablePanTrigger()
    {
        // Set the pan collider to trigger mode for detecting the chicken
        if (panCollider != null)
        {
            panCollider.isTrigger = true;
            Debug.Log("Pan collider set to trigger mode.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the chicken fillet (raw) is placed on the pan
        if (hasOil && !isCooking && other.CompareTag("KipCutlet-Raw"))
        {
            Debug.Log("Chicken detected on the pan. Starting to cook.");
            StartCooking(other.gameObject);
        }
        else
        {
            Debug.Log($"Collider detected with {other.gameObject.name}, but conditions not met for cooking.");
        }
    }

    private void StartCooking(GameObject chicken)
    {
        isCooking = true;
        cookingTimer = 0f;

        // Change the material to indicate cooking has started
        MeshRenderer renderer = chicken.GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            renderer.material = cookedChickenMaterial;
            Debug.Log("Chicken material changed to cooked.");
        }

        // Start the smoke particle effect directly
        if (smokeParticles != null)
        {
            smokeParticles.Stop();  // Ensure it's stopped first in case it was left playing
            smokeParticles.Clear(); // Clear any previous particles
            smokeParticles.Play(true);  // Reinitializing and starting the particle system

            Debug.Log("Smoke particle effect started directly.");
        }
        else
        {
            Debug.LogWarning("Smoke particle effect is not assigned.");
        }
    }


    private void FinishCooking()
    {
        isCooking = false;

        // Stop the smoke particle effect directly
        if (smokeParticles != null)
        {
            smokeParticles.Stop();
            Debug.Log("Cooking completed. Smoke particle effect stopped.");
        }
        else
        {
            Debug.LogWarning("Smoke particle effect is not assigned.");
        }
    }
}








