using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NarrationController : MonoBehaviour
{
    //objects for the steps game objects in the inspector
    [SerializeField] public GameObject IntroText;
    [SerializeField] public GameObject StepOne;
    [SerializeField] public GameObject StepTwo;
    [SerializeField] public GameObject StepThree;
    [SerializeField] public GameObject Donezo;

    //connecting scripts for cutting, rice, boiling and frying
    public CuttingBoardManager cuttingBoardManager;
    public CurryCookingManager curryCookingManager;
    public CookingManager cookingManager;
    public RiceCookingManager riceCookingManager;

    public void Start()
    {
        
        GameObject otherGameObject = GameObject.Find("CuttingBoard");
        if (otherGameObject != null)
        {
            cuttingBoardManager = otherGameObject.GetComponent<CuttingBoardManager>();
        }

        GameObject otherGameObjectCurry = GameObject.Find("cooking pott! (1)");
        if (otherGameObjectCurry != null)
        {
            curryCookingManager = otherGameObjectCurry.GetComponent<CurryCookingManager>();
        }

        GameObject otherGameObjectFrying = GameObject.Find("FRYING PAN done (1)");
        if (otherGameObjectFrying != null)
        {
            cookingManager = otherGameObjectFrying.GetComponent<CookingManager>();
        }

        GameObject otherGameObjectRice = GameObject.Find("Rice Cooker with Rice fbx");
        if (otherGameObjectRice != null)
        {
            riceCookingManager = otherGameObjectRice.GetComponent<RiceCookingManager>();
        }

    }

    public void hideIntro()
    {
        IntroText.SetActive(false);
    }

    public void showStepOne()
    {
        StepOne.SetActive(true);
    }

    public void hideStepOne()
    {
        StepOne.SetActive(false);
    }

    public void showStepTwo()
    {
        StepTwo.SetActive(true);
    }

    public void hideStepTwo()
    {
        StepTwo.SetActive(false);
    }

    public void showStepThree() 
    {
        StepThree.SetActive(true);
    }

    public void hideStepThree()
    {
        StepThree.SetActive(false);
    }

    public void showDonezo() 
    { 
        Donezo.SetActive(true);
    }


    private void Update()
    {
      if (cuttingBoardManager.CuttingComplete == true)
      {
            hideIntro();
            showStepOne();
      }

      if (curryCookingManager.CurryDone == true)
      {
            cuttingBoardManager.CuttingComplete = false;
            hideStepOne();
            showStepTwo();
      }

      if (cookingManager.FryingDone == true)
      {
            curryCookingManager.CurryDone = false;
            hideStepTwo();
            showStepThree();
      }

      if (riceCookingManager.RiceDone == true)
      {
            cookingManager.FryingDone = false;
            hideStepThree();
            showDonezo();
      }
    }
   
    

}
