using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camerafollow : MonoBehaviour
{
    public GameObject Player;
    public Control HH;
    private GameObject cameraConstarint;
    private GameObject cameralookAt;
    private float speed = 0;
    public float defaultFOV = 0,deisiredFOV = 0;
    [Range(0,5)]public float smoothTime = 0;

   private void Awake(){
       Player = GameObject.FindGameObjectWithTag("Player");
       cameraConstarint = Player.transform.Find("camera constraint").gameObject;
       cameralookAt = Player.transform.Find("camera lookat").gameObject;
       HH = Player.GetComponent<Control>();
       defaultFOV = Camera.main.fieldOfView;
   }

  private void FixedUpdate()
  {
      follow();
      boostFOV();
  }
  private void follow(){
      if(speed <= 23)
        speed = Mathf.Lerp(speed, HH.KPH / 4, Time.deltaTime);
      else
        speed = 23;
        gameObject.transform.position = Vector3.Lerp(transform.position,cameraConstarint.transform.position,Time.deltaTime * speed);
        gameObject.transform.LookAt(cameralookAt.gameObject.transform.position);
  }
 private void boostFOV(){
     if(Input.GetKey(KeyCode.LeftShift)){
         Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView,deisiredFOV,Time.deltaTime * smoothTime);
     }else
     Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView,defaultFOV,Time.deltaTime * smoothTime);
 }
}
