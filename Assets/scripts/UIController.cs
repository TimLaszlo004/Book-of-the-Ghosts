using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Gradient healthSliderColor;
    [SerializeField] private Image healthSliderImage;
    [SerializeField] private PlayerInput input;
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private Gradient healthIndicatorGradient;
    [SerializeField] private Image healthIndicator;


    public static UIController Instance { get; private set; }
    
    private void Awake() 
    { 
        Time.timeScale = 0f;
        if (Instance != null && Instance != this) 
        { 
            Destroy(this);
        } 
        else 
        { 
            Instance = this;
        }
    }

    public void setSlider(float value){
        healthSlider.value = value;
        healthSliderImage.color = healthSliderColor.Evaluate(value*0.01f); // bit hard coded!
        setIndicator();
    }

    public void restart(){
        GameplayRegister.Instance.restart();
        winPanel.SetActive(false);
    }


    public void pauseToggle(){
        if(Time.timeScale > 0){
            Time.timeScale = 0f;
            input.enabled = false;
            menuPanel.SetActive(true);
        }
        else{
            Time.timeScale = 1f;
            input.enabled = true;
            menuPanel.SetActive(false);
        }
    }

    public void start(){
        winPanel.SetActive(false);
        input.enabled = true;
        Time.timeScale = 1f;
    }

    public void win(){
        winPanel.SetActive(true);
    }

    public void setIndicator(){
        Color temp = healthIndicatorGradient.Evaluate(PlayerLogic.health * 0.01f);
        temp.a = 1f - (PlayerLogic.health * 0.01f);
        healthIndicator.color = temp;
    }

    public void reload(){
        SceneManager.LoadScene(0);
    }
}
