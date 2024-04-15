using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonGenerator : MonoBehaviour
{

    [SerializeField] private GameObject ghost;
    [SerializeField] private float range = 100f;
    [SerializeField] private float rate = 0.2f;
    [SerializeField] private int maxPopulation = 20;
    [SerializeField] private int averageLength = 3;
    [SerializeField] private int randomLength = 2;
    [SerializeField] private float rageChanceInit = 0.15f;
    public static float rageChance = 0.15f;
    public static int count = 0;

    void Start(){
        rageChance = rageChanceInit;
        Spawn(rate);
    }

    void updateVariables(){
        switch(UIController.Instance.mode){
            case 1:
                DemonController.speed = 1.5f;
                range = 100f;
                rate = 2f;
                maxPopulation = 2;
                averageLength = 1;
                randomLength = 0;
                rageChanceInit = 0f;
                rageChance = 0f;
                break;
            case 2:
                DemonController.speed = 2f;
                range = 100f;
                rate = 0.2f;
                maxPopulation = 10;
                averageLength = 2;
                randomLength = 1;
                rageChanceInit = 0.08f;
                rageChance = 0.08f;
                break;
            case 3:
                DemonController.speed = 2.5f;
                range = 100f;
                rate = 0.2f;
                maxPopulation = 20;
                averageLength = 3;
                randomLength = 2;
                rageChanceInit = 0.15f;
                rageChance = 0.15f;
                break;
            case 4:
                DemonController.speed = 3.5f;
                range = 100f;
                rate = 0.2f;
                maxPopulation = 35;
                averageLength = 4;
                randomLength = 2;
                rageChanceInit = 0.25f;
                rageChance = 0.25f;
                break;
            case 5:
                DemonController.speed = 5f;
                range = 100f;
                rate = 0.2f;
                maxPopulation = 50;
                averageLength = 6;
                randomLength = 4;
                rageChanceInit = 0.4f;
                rageChance = 0.4f;
                break;
        }

        CancelInvoke("one");
        Spawn(rate);
        
    }


    public void Spawn(float r){
        InvokeRepeating("one", r, r);
    }

    void one(){
        if(count < maxPopulation){
            Vector3 pos = PlayerLogic.position + new Vector3((Random.value-0.5f)*range, (Random.value-0.25f)*range, (Random.value-0.5f)*range);
            GameObject ob = Instantiate(ghost, pos, Quaternion.identity);
            ob.GetComponent<DemonController>().setLifeStack(genRandList());
            ob.GetComponent<DemonController>().colorUpdate();
            count++;
        }
    }

    List<DemonColor> genRandList(){
        int length = (int)(averageLength + (2f*Random.value-1f)*randomLength);
        if(length <= 0){
            length = 1;
        }
        List<DemonColor> colors = new List<DemonColor>();
        for(int i = 0; i < length; i++){
            float roll = Random.value;
            if(roll > 0.75f){
                colors.Add(DemonColor.Red);
            }
            else if(roll > 0.5f){
                colors.Add(DemonColor.Blue);
            }
            else if(roll > 0.25f){
                colors.Add(DemonColor.Green);
            }
            else{
                colors.Add(DemonColor.White);
            }
        }

        return colors;
    }
}
