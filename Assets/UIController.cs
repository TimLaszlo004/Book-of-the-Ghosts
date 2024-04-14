using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class UIController : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Gradient healthSliderColor;
    [SerializeField] private Image healthSliderImage;
    [SerializeField] private PlayerInput input;
    [SerializeField] private GameObject panel;


    public static UIController Instance { get; private set; }
    
    private void Awake() 
    { 
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
        healthSliderImage.color = healthSliderColor.Evaluate(value/100f); // bit hard coded!
    }

    public void restart(){
        GameplayRegister.Instance.restart();
    }


    public void pauseToggle(){
        if(Time.timeScale > 0){
            Time.timeScale = 0f;
            input.enabled = false;
            panel.SetActive(true);
        }
        else{
            Time.timeScale = 1f;
            input.enabled = true;
            panel.SetActive(false);
        }
    }

    public void start(){
        input.enabled = true;
    }
}
