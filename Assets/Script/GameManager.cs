using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Control RR;
    public GameObject neeedle;
    private float startPosiziton = -140f ,endPosition = -405f ;
    private float desiredPosition;
    public float vehicleSpeed;
   // public GameObject startPosition;
    public Text kph;
   // public Text currentPosition;
    
    

    // Start is called before the first frame update
    void Awake()
    {
     
        
    }

    
   private void FixedUpdate(){
       
        vehicleSpeed = RR.KPH;
         updateNeedle();
        
    }
    public void updateNeedle () {
        desiredPosition = startPosiziton - endPosition;
        float temp = vehicleSpeed / 180;
        neeedle.transform.eulerAngles = new Vector3 (0, 0, (startPosiziton - temp * desiredPosition));

    }
}
