using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NarrationController : MonoBehaviour
{
    [SerializeField] public GameObject IntroText;

    [SerializeField] public GameObject StepOne;

    public CuttingBoardManager cuttingBoardManager;

    

    public void Start()
    {
        cuttingBoardManager = gameObject.GetComponent<CuttingBoardManager>();

        GameObject otherGameObject = GameObject.Find("CuttingBoardManager");
        if (otherGameObject != null)
        {
            cuttingBoardManager = otherGameObject.GetComponent<CuttingBoardManager>();
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


    private void Update()
    {
      if (cuttingBoardManager.CuttingComplete == true)
      {
            hideIntro();
            showStepOne();
      }
    }
   
    

}
