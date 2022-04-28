// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class CarController : MonoBehaviour
// {
//     private float horizontalInput;
//     private float verticalInput;
//     private float steerAngle;
//     private bool isBreaking;

//     public WheelCollider frontLeftWheelCollider;
//     public WheelCollider frontRightWheelCollider;
//     public WheelCollider rearLeftWheelCollider;
//     public WheelCollider rearRightWheelCollider;
//     public Transform frontLeftWheelTransform;
//     public Transform frontRightWheelTransform;
//     public Transform rearLeftWheelTransform;
//     public Transform rearRightWheelTransform;

//     public float maxSteeringAngle = 30f;
//     public float motorForce = 50f;
//     public float brakeForce = 30f;
//     public float DownForceValue = 50;
//     private Rigidbody rigidbody;


//     private void FixedUpdate()
//     {
//         addDownForce();
//         GetInput();
//         HandleMotor();
//         HandleSteering();
//         UpdateWheels();
//     }

//     private void GetInput()
//     {
//         horizontalInput = Input.GetAxis("Horizontal");
//         verticalInput = Input.GetAxis("Vertical");
//         isBreaking = Input.GetKey(KeyCode.Space);
//     }

//     private void HandleSteering()
//     {
//         steerAngle = maxSteeringAngle * horizontalInput;
//         frontLeftWheelCollider.steerAngle = steerAngle;
//         frontRightWheelCollider.steerAngle = steerAngle;
//     }

//     private void HandleMotor()
//     {
//         frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
//         frontRightWheelCollider.motorTorque = verticalInput * motorForce;

//         brakeForce = isBreaking ? 3000f : 0f;
//         frontLeftWheelCollider.brakeTorque = brakeForce;
//         frontRightWheelCollider.brakeTorque = brakeForce;
//         rearLeftWheelCollider.brakeTorque = brakeForce;
//         rearRightWheelCollider.brakeTorque = brakeForce;
//     }

//     private void UpdateWheels()
//     {
//         UpdateWheelPos(frontLeftWheelCollider, frontLeftWheelTransform);
//         UpdateWheelPos(frontRightWheelCollider, frontRightWheelTransform);
//         UpdateWheelPos(rearLeftWheelCollider, rearLeftWheelTransform);
//         UpdateWheelPos(rearRightWheelCollider, rearRightWheelTransform);
//     }

//     private void UpdateWheelPos(WheelCollider wheelCollider, Transform trans)
//     {
//         Vector3 pos;
//         Quaternion rot;
//         wheelCollider.GetWorldPose(out pos, out rot);
//         trans.rotation = rot;
//         trans.position = pos;
//     }
    
//     private void addDownForce(){
//         rigidbody.AddForce(-transform.up * DownForceValue * rigidbody.velocity.magnitude);
//     }
// }