using System.Collections;
using UnityEngine;
using UnityEngine.VFX;
using UnitySimpleLiquid;

public class CurryCookingManager : MonoBehaviour
{
    public LiquidContainer potLiquidContainer;
    public Material curryLiquidMaterial;
    public ParticleSystem smokeParticles;
    public VisualEffect poofEffect; // Poof effect to play when curry cube is added
    public float requiredOilLevel = 0.1f; // 10% oil required
    public float cookingDuration = 5f; // Time required to cook the ingredients
    public float curryVolumeIncrease = 0.1f; // Volume increase when adding the curry cube, settable in the Inspector

    private bool hasOil = false;
    private bool isCooking = false;
    private bool hasAddedFeathers = false;
    private bool hasAddedChickenLegs = false;
    private bool hasAddedPotato = false;
    private bool oilLimitEnabled = true; // Track whether the oil level limit is enforced
    private float cookingTimer = 0f;
    private Collider potCollider;

    public bool CurryDone = false;

    private void Start()
    {
        // Get the pot's collider and set it to active but not trigger initially
        potCollider = GetComponent<Collider>();
        if (potCollider != null)
        {
            potCollider.isTrigger = false; // Disable trigger mode at the start
            Debug.Log("Pot collider found and set to non-trigger mode initially.");
        }

        // Ensure the smoke particle effect is not playing initially
        if (smokeParticles != null && smokeParticles.isPlaying)
        {
            smokeParticles.Stop();
            Debug.Log("Smoke particles stopped at start.");
        }

        // Ensure the poof visual effect is not playing initially
        if (poofEffect != null)
        {
            poofEffect.Stop();
            Debug.Log("Poof effect stopped at start.");
        }
    }

    private void Update()
    {
        // Enforce the oil level limit if enabled
        if (oilLimitEnabled && potLiquidContainer.FillAmountPercent > requiredOilLevel)
        {
            potLiquidContainer.FillAmountPercent = requiredOilLevel;
        }

        // Check if the oil level in the pot is sufficient
        if (!hasOil && potLiquidContainer.FillAmountPercent >= requiredOilLevel)
        {
            hasOil = true;
            EnablePotTrigger();
            Debug.Log("Oil added to the pot. Pot trigger enabled.");
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

    private void EnablePotTrigger()
    {
        // Set the pot collider to trigger mode for detecting the ingredients
        if (potCollider != null)
        {
            potCollider.isTrigger = true;
            Debug.Log("Pot collider set to trigger mode.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check for feathers
        if (other.CompareTag("feathers") && !hasAddedFeathers)
        {
            hasAddedFeathers = true;
            Debug.Log("Feathers added to the pot.");
            StartCooking();
        }
        // Check for chicken legs
        else if (other.CompareTag("Chicken_legs") && !hasAddedChickenLegs)
        {
            hasAddedChickenLegs = true;
            Debug.Log("Chicken legs added to the pot.");
            StartCooking();
        }
        // Check for potato
        else if (other.CompareTag("Potato") && !hasAddedPotato)
        {
            hasAddedPotato = true;
            Debug.Log("Potato added to the pot.");
            StartCooking();
        }
        // Check for curry cube after initial ingredients are added
        else if (other.CompareTag("curry_cube") && hasAddedFeathers && hasAddedChickenLegs && hasAddedPotato)
        {
            AddCurryCube();
        }
    }

    private void StartCooking()
    {
        isCooking = true;
        cookingTimer = 0f;

        // Start the smoke particle effect
        if (smokeParticles != null && !smokeParticles.isPlaying)
        {
            smokeParticles.Play();
            Debug.Log("Smoke particle effect started.");
        }
    }
    public bool isCookingComplete { get; private set; } = false;
    private void FinishCooking()
    {
        isCooking = false;
        isCookingComplete = true;

        // Stop the smoke particle effect
        if (smokeParticles != null && smokeParticles.isPlaying)
        {
            smokeParticles.Stop();
            Debug.Log("Cooking completed. Smoke particle effect stopped.");
        }
    }

    private IEnumerator UpdateLiquidVisual()
    {
        // Temporarily disable the oil limit to allow liquid increase
        oilLimitEnabled = false;
        Debug.Log("Oil limit temporarily disabled.");

        // Wait a few frames to ensure the change is applied
        yield return new WaitForSeconds(0.5f); // Adjust delay if necessary

        // Directly set FillAmountPercent and reassign the color
        float targetVolume = Mathf.Min(potLiquidContainer.FillAmountPercent + curryVolumeIncrease, 1.0f);
        Debug.Log($"Before: FillAmountPercent = {potLiquidContainer.FillAmountPercent}");

        potLiquidContainer.FillAmountPercent = targetVolume; // Ensure this value is visually updated
        potLiquidContainer.LiquidColor = curryLiquidMaterial.color; // Set the color for the liquid

        Debug.Log($"After: FillAmountPercent = {potLiquidContainer.FillAmountPercent}");

        // No need to re-enable oil limit, as this is the final liquid addition
        Debug.Log("Oil limit remains disabled after adding curry liquid.");
    }

    private void AddCurryCube()
    {
        Debug.Log("Curry cube added to the pot.");
        StartCoroutine(UpdateLiquidVisual());

        // Play the poof visual effect when the curry cube is added
        if (poofEffect != null)
        {
            poofEffect.Play();
            Debug.Log("Poof effect played to indicate curry cube addition.");

            CurryDone = true;
        }

        
    }
}









