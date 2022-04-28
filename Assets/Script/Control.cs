using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour
{  
    internal enum driveType
    {
        frontWheelDrive,
        rearWheelDrive,
        allWheelDrive
    }
   [SerializeField] private driveType drive;
   [Header("Variables")]
    public int motorTorque = 200; 
    public float DownForceValue = 50;
    public float brakepower;
    public float KPH;
    public float radius = 6, brakPower = 10, horizontal , vertical, totalPower;
    public float[] slip = new float[4];
    public float thrust = 1000;
    public InputManager IM;
    public GameObject wheelMeshes,wheelColliders;
    public WheelCollider[] wheels = new WheelCollider[4];
    public GameObject[] wheelMesh = new GameObject[4];
    private GameObject centerOfMass;
    private Rigidbody rigidbody;
    
    void Start()
    {   

        getObjects();   
    }

   private void FixedUpdate(){
       horizontal = IM.horizontal;
       vertical = IM.vertical;
       
       addDownForce();
       animateWheels();
       moveVehicle();
       steerVehicle(); 
       getFriction();
      
   }
      void moveVehicle(){

        // brakeVehicle();

        if (drive == driveType.allWheelDrive){
            for (int i = 0; i < wheels.Length; i++){
                wheels[i].motorTorque = IM.vertical *(motorTorque / 4) ;
                // wheels[i].brakeTorque = brakPower;
            }
        }else if(drive == driveType.rearWheelDrive){
            wheels[2].motorTorque = IM.vertical *(motorTorque / 2);
            wheels[3].motorTorque = IM.vertical *(motorTorque / 2);

            // for (int i = 0; i < wheels.Length; i++)
            // {
            //     wheels[i].brakeTorque = brakPower;
            // }
        }
        else{
            wheels[0].motorTorque = IM.vertical *(motorTorque / 2);
            wheels[1].motorTorque = IM.vertical *(motorTorque / 2);

            // for (int i = 0; i < wheels.Length; i++)
            // {
            //     wheels[i].brakeTorque = brakPower;
            // }
        }
    KPH = rigidbody.velocity.magnitude * 3.6f;  
        
    if(IM.handbrake){
        wheels[3].brakeTorque = wheels[2].brakeTorque = brakepower;
    }else{
        wheels[3].brakeTorque = wheels[2].brakeTorque = 0;
    }
    if(IM.boosting){
        rigidbody.AddForce(Vector3.forward * thrust);
    }
    }

    // private void brakeVehicle(){

    //     if (vertical < 0){
    //         brakPower =(KPH >= 10)? 500 : 0;
    //     }
    //     else if (vertical == 0 &&(KPH <= 10 || KPH >= -10)){
    //         brakPower = 10;
    //     }
    //     else{
    //         brakPower = 0;
    //     }


    
   void steerVehicle(){
    if (horizontal > 0 ) {
				//rear tracks size is set to 1.5f       wheel base has been set to 2.55f
            wheels[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius + (1.5f / 2))) * horizontal;
            wheels[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius - (1.5f / 2))) * horizontal;
        } else if (horizontal < 0 ) {                                                          
            wheels[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius - (1.5f / 2))) * horizontal;
            wheels[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius + (1.5f / 2))) * horizontal;
			//transform.Rotate(Vector3.up * steerHelping);

        } else {
            wheels[0].steerAngle =0;
            wheels[1].steerAngle =0;
        }

    }

    private void animateWheels ()
	{
		Vector3 wheelPosition = Vector3.zero;
		Quaternion wheelRotation = Quaternion.identity;

		for (int i = 0; i < 4; i++) {
			wheels [i].GetWorldPose (out wheelPosition, out wheelRotation);
			wheelMesh [i].transform.position = wheelPosition;
			wheelMesh [i].transform.rotation = wheelRotation;
		}
	}
   
   
     private void addDownForce(){

         rigidbody.AddForce(-transform.up * DownForceValue * rigidbody.velocity.magnitude);

     }
    private  void getObjects(){
        IM = GetComponent<InputManager>();
        rigidbody =GetComponent<Rigidbody>();
        wheelColliders = GameObject.Find("Colliders");
        wheelMeshes = GameObject.Find("Meshes");
        wheelMesh[0] = wheelMeshes.transform.Find("FrontLeftWheel").gameObject;
        wheelMesh[1] = wheelMeshes.transform.Find("FrontRightWheel").gameObject;
        wheelMesh[2] = wheelMeshes.transform.Find("RearLeftWheel").gameObject;
        wheelMesh[3] = wheelMeshes.transform.Find("RearRightWheel").gameObject;
        wheels[0] = wheelColliders.transform.Find("FrontLeftWheel").gameObject.GetComponent<WheelCollider>();
        wheels[1] = wheelColliders.transform.Find("FrontRightWheel").gameObject.GetComponent<WheelCollider>();
        wheels[2] = wheelColliders.transform.Find("RearLeftWheel").gameObject.GetComponent<WheelCollider>();
        wheels[3] = wheelColliders.transform.Find("RearRightWheel").gameObject.GetComponent<WheelCollider>();
        centerOfMass = GameObject.Find("mass");
        rigidbody.centerOfMass = centerOfMass.transform.localPosition;
    }
    private void getFriction(){
        for(int i = 0; i < wheels.Length; i++){
            WheelHit wheelHit;
            wheels[i].GetGroundHit(out wheelHit);
            slip[i] = wheelHit.forwardSlip;
        }
    }
}
