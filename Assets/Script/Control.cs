using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Control : MonoBehaviour
{  
    internal enum driveType
    {
        frontWheelDrive,
        rearWheelDrive,
        allWheelDrive
    }
    [HideInInspector]public bool test;
   [SerializeField] private driveType drive;
   [Header("Variables")]
    public int motorTorque = 200; 
    public float[] gears;
    public float[] gearChangeSpeed;
    [HideInInspector]public int gearNum = 1;
    [HideInInspector]public bool reverse = false;
    [HideInInspector]public float engineRPM;
    
    public AnimationCurve enginepower;
    public float DownForceValue = 100f;
    public float brakepower = 50000;
    public float KPH;
    public float radius = 6, brakPower = 10, horizontal , vertical, totalPower, wheelsRPM, lastValue;
    public float[] slip = new float[4];
    public float thrust = 20000f;
    public float maxRPM = 5500 , minRPM = 3000;
    public InputManager IM;
    public GameManager manager;
    public GameObject wheelMeshes,wheelColliders;
    public WheelCollider[] wheels = new WheelCollider[4];
    public GameObject[] wheelMesh = new GameObject[4];
    private GameObject centerOfMass;
    private Rigidbody rigidbody;
     private bool flag=false;
     private float smoothTime = 0.09f;
    
    void Start()
    {   

        getObjects();   
    }

   private void FixedUpdate(){
       horizontal = IM.horizontal;
       vertical = IM.vertical;
       lastValue = engineRPM;
        if(SceneManager.GetActiveScene().name == "awakeScene")return;
       addDownForce();
       animateWheels();
       moveVehicle();
       steerVehicle(); 
       getFriction();
       calculateEnginePower();
      
   }
   
   
      void moveVehicle(){

        // brakeVehicle();

        if (drive == driveType.allWheelDrive){
            for (int i = 0; i < wheels.Length; i++){
                wheels[i].motorTorque = totalPower / 4 ;
                // wheels[i].brakeTorque = brakPower;
            }
        }else if(drive == driveType.rearWheelDrive){
            for (int i = 2; i < wheels.Length; i++)
            {
                wheels[i].motorTorque = (totalPower / 2);}
            
        //     wheels[3].motorTorque = (totalPower / 2);

        //     // for (int i = 0; i < wheels.Length; i++)
        //     // {
        //     //     wheels[i].brakeTorque = brakPower;
        //     // }
         }
         else{
             for (int i = 0; i < wheels.Length - 2; i++){
             wheels[i].motorTorque = (totalPower / 2);}
            //  wheels[1].motorTorque = IM.vertical *(totalPower / 2);

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
  

private void calculateEnginePower(){

        wheelRPM();

            if (vertical != 0 ){
                rigidbody.drag = 0.005f; 
            }
            if (vertical == 0){
                rigidbody.drag = 0.1f;
            }
            totalPower = 3.6f * enginepower.Evaluate(engineRPM) * (vertical) * (gears[gearNum]);

        


        float velocity  = 0.0f;
        
            engineRPM = Mathf.SmoothDamp(engineRPM,1000 + (Mathf.Abs(wheelsRPM) * 3.6f * (gears[gearNum])), ref velocity , smoothTime);
        
        moveVehicle();
    shifter();
    }
 private void wheelRPM(){
        float sum = 0;
        int R = 0;
        for (int i = 0; i < 4; i++)
        {
            sum += wheels[i].rpm;
            R++;
        }
        wheelsRPM = (R != 0) ? sum / R : 0;
 
        if(wheelsRPM < 0 && !reverse ){
            reverse = true;
     
        }
        else if(wheelsRPM > 0 && reverse){
            reverse = false;
          
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
 private bool checkGears(){
        if(KPH >= gearChangeSpeed[gearNum] ) return true;
        else return false;
    }
 private void shifter(){

        if(!isGrounded())return;
            //automatic
        if(engineRPM > maxRPM && gearNum < gears.Length-1 && !reverse && checkGears() ){
            gearNum ++;
       
            return;
        }
        if(engineRPM < minRPM && gearNum > 0){
            gearNum --;
          
        }

    }
 
    private bool isGrounded(){
        if(wheels[0].isGrounded &&wheels[1].isGrounded &&wheels[2].isGrounded &&wheels[3].isGrounded )
            return true;
        else
            return false;
    }

    
  private void steerVehicle(){
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




