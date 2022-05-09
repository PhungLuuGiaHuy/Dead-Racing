using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEffect : MonoBehaviour
{
    public ParticleSystem[] smoke;
    private Control control;
    public AudioSource skidClip;
    public TrailRenderer[] tireMarks;
    private InputManager IM;
     private bool smokeFlag  = false  , tireMarksFlag;
    // Start is called before the first frame update
    void Start()
    {
        control = GetComponent<Control>();
        IM = GetComponent<InputManager>();
        
    }

    // Update is called once per frame
     private void FixedUpdate() {
       

        chectDrift();
        activateSmoke();
        
    }
    private void activateSmoke(){
        if (control.playPauseSmoke) startSmoke();
        else stopSmoke();

        if (smokeFlag)
        {
            for (int i = 0; i < smoke.Length; i++)
            {
                var emission = smoke[i].emission;
                emission.rateOverTime = ((int)control.KPH * 10 <= 2000) ? (int)control.KPH * 10 : 2000;
            }
        }
    }

    public void startSmoke(){
        if(smokeFlag)return;
        for (int i = 0; i < smoke.Length; i++){
            var emission = smoke[i].emission;
            emission.rateOverTime = ((int) control.KPH *2 >= 2000) ? (int) control.KPH * 2 : 2000;
            smoke[i].Play();
        }
        smokeFlag = true;

    }

    public void stopSmoke(){
        if(!smokeFlag) return;
        for (int i = 0; i < smoke.Length; i++){
            smoke[i].Stop();
        }
        smokeFlag = false;
    }
private void chectDrift() {
        if (IM.handbrake) startEmitter();
        else stopEmitter();

    }

    private void startEmitter() {
        if (tireMarksFlag) return;
        foreach (TrailRenderer T in tireMarks) {
            T.emitting = true;
        }
        skidClip.Play();
        tireMarksFlag = true;
    }   
    private void stopEmitter() {
        if (!tireMarksFlag) return;
        foreach (TrailRenderer T in tireMarks)
        {
            T.emitting = false;
        }
        skidClip.Stop();
        tireMarksFlag = false;
    }
}
