using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class awakeManager : MonoBehaviour
{
    public GameObject toRotate;
    public GameObject player;
    public float rotateSpeed;
    public vehicleList listOfVehicles;
    public int vehiclePointer = 0;
   
   private void Awake(){

       PlayerPrefs.SetInt("pointer",0);
       vehiclePointer = PlayerPrefs.GetInt("pointer");
       GameObject childObject = Instantiate(listOfVehicles.vehicles[vehiclePointer],Vector3.zero,Quaternion.identity) as GameObject;
       childObject.transform.parent = toRotate.transform;
   }
   
    private void FixedUpdate(){
        player = GameObject.FindGameObjectWithTag("Player");
        toRotate.transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
        player.transform.Rotate(Vector3.up * rotateSpeed *Time.deltaTime);

    }
    public void rightButton(){
        if(vehiclePointer < listOfVehicles.vehicles.Length-1){
            Destroy(GameObject.FindGameObjectWithTag("Player"));
            vehiclePointer++;
            PlayerPrefs.SetInt("pointer",vehiclePointer);
            GameObject childObject = Instantiate(listOfVehicles.vehicles[vehiclePointer],Vector3.zero,toRotate.transform.rotation) as GameObject;
            childObject.transform.parent = toRotate.transform;
            
        }
    }
     public void leftButton(){
        if(vehiclePointer > 0){
            Destroy(GameObject.FindGameObjectWithTag("Player"));
            vehiclePointer--;
            PlayerPrefs.SetInt("pointer",vehiclePointer);
            GameObject childObject = Instantiate(listOfVehicles.vehicles[vehiclePointer],Vector3.zero,toRotate.transform.rotation) as GameObject;
            childObject.transform.parent = toRotate.transform;
           
        }
    }
    public void startGameButton(){
        SceneManager.LoadScene("Map1");
    }
}
