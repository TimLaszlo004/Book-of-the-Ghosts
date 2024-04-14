using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Gradient healthSliderColor;
    [SerializeField] private Image healthSliderImage;

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
}
