using UnityEngine;
using UnityEngine.VFX;
using UnitySimpleLiquid;

public class CookingManager : MonoBehaviour
{
    public LiquidContainer panLiquidContainer;
    public Material cookedChickenMaterial;
    public ParticleSystem smokeParticles;   // Particle effect for cooking smoke
    public VisualEffect poofEffect;         // Visual effect for the poof effect
    public float requiredOilLevel = 0.1f;   // 10% oil required
    public float cookingDuration = 5f;      // Time required to cook the chicken

    private bool hasOil = false;
    private bool isCooking = false;
    private float cookingTimer = 0f;
    private Collider panCollider;
    private GameObject chickenBeingCooked;

    public bool FryingDone = false;

    private void Start()
    {
        panCollider = GetComponent<Collider>();
        if (panCollider != null)
        {
            panCollider.isTrigger = false;
            Debug.Log("Pan collider found and set to non-trigger mode initially.");
        }

        // Ensure smoke particle effect is ready but not playing at start
        if (smokeParticles != null)
        {
            smokeParticles.Stop();
            smokeParticles.Clear();
            Debug.Log("Smoke particles stopped and cleared at start.");
        }

        if (poofEffect != null)
        {
            poofEffect.Stop();
            Debug.Log("Poof effect ready but stopped at start.");
        }
    }

    private void Update()
    {
        if (!hasOil && panLiquidContainer.FillAmountPercent >= requiredOilLevel)
        {
            hasOil = true;
            EnablePanTrigger();
            Debug.Log("Oil added to the pan. Pan trigger enabled.");
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

    private void EnablePanTrigger()
    {
        if (panCollider != null)
        {
            panCollider.isTrigger = true;
            Debug.Log("Pan collider set to trigger mode.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasOil && !isCooking && other.CompareTag("KipCutlet-Raw"))
        {
            Debug.Log("Chicken fillet detected on the pan. Starting to cook.");
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
        chickenBeingCooked = chicken;

        // Start the smoke particle effect with additional diagnostic checks
        if (smokeParticles != null)
        {
            Debug.Log("Attempting to start smoke particles...");
            smokeParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            smokeParticles.Clear(); // Ensure it's cleared
            smokeParticles.Play();  // Attempt to start playing
            Debug.Log($"Smoke particles state after play attempt - isPlaying: {smokeParticles.isPlaying}");
        }
        else
        {
            Debug.LogWarning("Smoke particle effect is not assigned.");
        }
    }
    public bool isCookingComplete { get; private set; } = false;
    private void FinishCooking()
    {
        isCooking = false;
        isCooking = false;
        isCookingComplete = true;

        if (smokeParticles != null && smokeParticles.isPlaying)
        {
            smokeParticles.Stop();
            Debug.Log("Cooking completed. Smoke particle effect stopped.");
        }

        if (poofEffect != null && chickenBeingCooked != null)
        {
            poofEffect.transform.position = chickenBeingCooked.transform.position;
            poofEffect.Play();
            Debug.Log("Poof effect played at chicken position.");
        }

        if (chickenBeingCooked != null)
        {
            MeshRenderer renderer = chickenBeingCooked.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                renderer.material = cookedChickenMaterial;
                Debug.Log("Chicken material changed to cooked.");

                FryingDone = true;
            }
        }
    }
}








