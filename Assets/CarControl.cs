using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarControl : MonoBehaviour
{
    public WheelCollider[] wheels = new WheelCollider[4];
    public float torque = 200;
    void Start()
    {
        
    }

   private void FixedUpdate(){
       if(Input.GetKey(KeyCode.W)){
           for(int i = 0; i < wheels.Length; i++){
           wheels[i].motorTorque = torque;}
       }
      
   }
}
