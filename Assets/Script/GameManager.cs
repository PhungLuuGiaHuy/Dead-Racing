using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Control RR;
    public GameObject needle;
    private float startPosiziton = -140f ,endPosition = -405f ;
    private float desiredPosition;
    public float vehicleSpeed;
    public GameObject startPosition;
    public Text kph;
    public Text currentPosition;
    public GameObject neeedle;
    

    // Start is called before the first frame update
    void Awake()
    {
      RR = GameObject.FindGameObjectWithTag ("Player").GetComponent<Control> ();
        
    }

    
   private void FixedUpdate(){
       
       // kph.text = RR.KPH.ToString ("0");
        // updateNeedle();
        
    }
    public void updateNeedle () {
        desiredPosition = startPosiziton - endPosition;
        float temp = RR.engineRPM / 10000;
        neeedle.transform.eulerAngles = new Vector3 (0, 0, (startPosiziton - temp * desiredPosition));

    }
}
