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
    public RiceCookingManager riceCookingManager;
    public CurryCookingManager curryCookingManager;
    

    public void Start()
    {
        
        GameObject otherGameObject = GameObject.Find("CuttingBoard");
        if (otherGameObject != null)
        {
            cuttingBoardManager = otherGameObject.GetComponent<CuttingBoardManager>();
        }

        GameObject otherGameObject = GameObject.Find("cooking pott! (1)");
        if (otherGameObject != null)
        {
            curryCookingManager = otherGameObject.GetComponent<CurryCookingManager>();
        }

        GameObject otherGameObject = GameObject.Find("Rice Cooker with Rice fbx");
        if (otherGameObject != null)
        {
            riceCookingManager = otherGameObject.GetComponent<RiceCookingManager>();
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
            hideStepOne();
            showStepTwo();
      }



      if (riceCookingManager.RiceDone == true)
        {
            hideStepThree();
            showDonezo();
        }
    }
   
    

}
