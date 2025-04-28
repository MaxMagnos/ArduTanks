using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TankController : MonoBehaviour
{
    [Header("Public Variables")] 
    public int bodyRotSpeed;
    public int headRotSpeed;
    public int maxHeadRot;
    public float maxSpeed;
    public AnimationCurve speedCurve;
    public float fireCooldown;
    
    [Header("Component References")]
    public GameObject head;
    public GameObject body;
    public GameObject projectilePrefab;
    private Rigidbody rb;
    
    [Header("Faux Input")] 
    public int rotInput = 0;                    //Rotary Encoder for body rotation
    [Range(0, 1024)] public int potInput;       //Potentiometer for head rotation
    [Range(0, 128)] public int throttleInput;   //Slide-Potentiometer for forward/backward movement
    [Range(0, 128)] public int barrelInput;     //Slide-Potentiometer for determining barrel-height
    public bool fireButton;                     //Simulates whether the fire-button is pressed or not pressed
    
    //Member Variables
    private bool isFiring = false;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

     void Update()
    {
        
    }

    private void FixedUpdate()
    {
        RotateTank();
        RotateHead();
        MoveTank();

        if (fireButton && !isFiring)
        {
            StartCoroutine(Fire());
        }
    }

    private void RotateTank()
    {
        var desiredAngle = -rotInput * 18;
        Quaternion targetRotation = Quaternion.Euler(0, 0, desiredAngle);
        Quaternion rotation = Quaternion.RotateTowards(rb.rotation, targetRotation, bodyRotSpeed);
        rb.MoveRotation(rotation);
    }
    
    private void RotateHead()
    {
        var desiredAngle = Mathf.Lerp(-maxHeadRot, maxHeadRot, Mathf.InverseLerp(0, 1024, potInput));
        var currentAngle = Mathf.Lerp(maxHeadRot, -maxHeadRot, Mathf.InverseLerp(-maxHeadRot, maxHeadRot, NormalizeAngle(head.transform.localEulerAngles.z)));
        var angleDiff = desiredAngle - currentAngle;
        //Debug.Log("Desired angle: " + desiredAngle + "\nangleDiff: " + angleDiff + "\nCurrent angle: " + currentAngle);
        if (Mathf.Abs(angleDiff) <= headRotSpeed)
        {
            head.transform.localEulerAngles = new Vector3(0, 0, -desiredAngle);
        }
        else
        {
            var rotationStep = Mathf.Sign(angleDiff) * headRotSpeed;    //
            head.transform.localRotation *= Quaternion.Euler(0,0,-rotationStep);
        }

    }

    private void MoveTank()
    {
        var t = throttleInput / 128f;
        var speed = speedCurve.Evaluate(t);
        Vector3 move = transform.up * speed * maxSpeed;
        rb.MovePosition(rb.position + move * Time.fixedDeltaTime);
        rb.linearVelocity = Vector3.zero;
    }

    private IEnumerator Fire()
    {
        isFiring = true;
        
        var projectile = Instantiate(projectilePrefab, transform.position, head.transform.rotation);
        projectile.GetComponent<Projectile>().Initialize(barrelInput);
            
        yield return new WaitForSeconds(fireCooldown);
        isFiring = false;
    }
    
    float NormalizeAngle(float angle)
    {
        angle = Mathf.Repeat(angle + 180f, 360f) - 180f;
        return angle;
    }
}
