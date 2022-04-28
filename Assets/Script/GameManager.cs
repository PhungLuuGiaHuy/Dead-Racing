using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Control HH;
    public GameObject needle;
    private float starPosition = -140f ,endPosition = -405f ;
    private float desiredPosition;
    public float vehicleSpeed;

    // Start is called before the first frame update
    void Awake()
    {
     
        
    }

    
   private void FixedUpdate(){
       
        //vehicleSpeed = HH.KPH;
       //  updateNeedle();
        
    }
    public void updateNeedle(){
        desiredPosition = starPosition - endPosition;
        float temp = vehicleSpeed / 180;
        needle.transform.eulerAngles = new Vector3(0,0,(starPosition - temp * desiredPosition));
    }
}
