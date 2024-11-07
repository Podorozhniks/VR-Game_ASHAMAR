using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuController : MonoBehaviour
{
    
    public Color ambientLightColor = Color.gray;
    public float directionalLightIntensity = 1.0f;

    public void StartBtn()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; 
        SceneManager.LoadSceneAsync("Cutting");
    }

    public void exitGame()
    {
        Application.Quit();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Cutting")
        {
            SetupLighting(); 
        }
        SceneManager.sceneLoaded -= OnSceneLoaded; 

    }

    private void SetupLighting()
    {
        
        RenderSettings.ambientLight = ambientLightColor;

        
        Light directionalLight = FindObjectOfType<Light>();
        if (directionalLight == null)
        {
            GameObject lightGameObject = new GameObject("Directional Light");
            directionalLight = lightGameObject.AddComponent<Light>();
            directionalLight.type = LightType.Directional;
        }

        
        directionalLight.intensity = directionalLightIntensity;
        directionalLight.color = Color.white;
        directionalLight.transform.rotation = Quaternion.Euler(50, -30, 0);

        Debug.Log("Lighting setup complete");
    }
}


