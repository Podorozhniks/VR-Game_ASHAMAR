using UnityEngine;
using UnityEngine.VFX;

public class FinalDishManager : MonoBehaviour
{
    [Header("Cooking Manager References")]
    public CurryCookingManager curryCookingManager;
    public RiceCookingManager riceCookingManager;
    public CookingManager chickenCookingManager;

    [Header("Final Dish Setup")]
    public GameObject finalCurryDish;       // The curry dish to display when all cooking is complete
    public VisualEffect finalPoofEffect;    // Poof effect to play before showing the final dish

    private bool isFinalDishReady = false;  // Track if the final dish has been revealed

    private void Start()
    {
        // Ensure final dish and poof effect are hidden at start
        if (finalCurryDish != null)
            finalCurryDish.SetActive(false);

        if (finalPoofEffect != null)
            finalPoofEffect.Stop();
    }

    private void Update()
    {
        // Check if all cooking processes are done and final dish has not yet been revealed
        if (curryCookingManager != null && riceCookingManager != null && chickenCookingManager != null)
        {
            if (curryCookingManager.isCookingComplete && riceCookingManager.isCookingComplete && chickenCookingManager.isCookingComplete && !isFinalDishReady)
            {
                RevealFinalDish();
            }
        }
    }

    private void RevealFinalDish()
    {
        isFinalDishReady = true;

        // Play the poof effect
        if (finalPoofEffect != null)
        {
            finalPoofEffect.Play();
            Debug.Log("Final poof effect played for the curry dish.");
        }

        // Enable the final curry dish after a short delay to sync with poof effect
        Invoke(nameof(ShowFinalDish), 1.0f); // Adjust delay as needed
    }

    private void ShowFinalDish()
    {
        if (finalCurryDish != null)
        {
            finalCurryDish.SetActive(true);
            Debug.Log("Final curry dish displayed.");
        }
    }
}


