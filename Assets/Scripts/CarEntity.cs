﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CarEntity : MonoBehaviour
{
    public GameObject wheelFrontLeft;
    public GameObject wheelFrontRight;
    public GameObject wheelBackLeft;
    public GameObject wheelBackRight;
    //Car Steering
    float m_FrontWheelAngle = 0;
    const float WHEEL_ANGLE_LIMIT = 20f;
    public float turnAngulurVelocity = 20f;
    //Accelerate and deceleration
    float m_Velocity = 0;
    public float Velocity { get { return m_Velocity; } }
    public float maxVelocity = 40f;
    public float acceleration = 3f;
    public float deceleration = 10f;

    float m_DeltaMovement = 0;
    float CarLength = 0.96f;

    [SerializeField] SpriteRenderer[] m_renders = new SpriteRenderer[5];
    void ChangeColor(Color _color) {
        foreach(SpriteRenderer r in m_renders){
            r.color = _color;
        }
    }

    void ResetColor() {
        ChangeColor(Color.blue);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        ChangeColor(Color.red);
        stop();
    }
    private void OnCollisionStay2D(Collision2D collision) {
            ChangeColor(Color.yellow);
    }
    private void OnCollisionExit2D(Collision2D collision) {
        ResetColor();
    }

    void OnTriggerEnter2D(Collider2D other){
        Checkpoint checkpoint = other.gameObject.GetComponent<Checkpoint> ();
        if (checkpoint != null) {
            ChangeColor(Color.green);
            this.Invoke("ResetColor", 0.5f);
        }
    }
    void stop() {
        m_Velocity = 0;
    }

    void Start()
    {
        //ResetColor();
    }
    void FixedUpdate()
    {
        MoveUpControl();
        TurnControl();
        //Update car transform 
        m_DeltaMovement = m_Velocity * Time.deltaTime;
        
        this.transform.Translate(Vector3.right * m_DeltaMovement);
        this.transform.Rotate(0, 0, 1/CarLength *
            Mathf.Tan(Mathf.Deg2Rad *m_FrontWheelAngle) 
            *m_DeltaMovement *Mathf.Rad2Deg);
    }

    void MoveUpControl() {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            m_Velocity = Mathf.Min(maxVelocity, m_Velocity + Time.fixedDeltaTime * deceleration);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            m_Velocity = Mathf.Max(-5, m_Velocity - Time.fixedDeltaTime * deceleration);
        }
        else { 
            if(m_Velocity > 0) { m_Velocity -= Time.fixedDeltaTime * turnAngulurVelocity; }
            if (m_Velocity < 0) {m_Velocity += Time.fixedDeltaTime * turnAngulurVelocity; }
        }
    }

    void TurnControl() {
        if (Input.GetKey(KeyCode.LeftArrow)) {
            m_FrontWheelAngle = Mathf.Clamp(
                m_FrontWheelAngle + Time.fixedDeltaTime * turnAngulurVelocity,
                -WHEEL_ANGLE_LIMIT,
                WHEEL_ANGLE_LIMIT);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            m_FrontWheelAngle = Mathf.Clamp(
               m_FrontWheelAngle - Time.fixedDeltaTime * turnAngulurVelocity,
               -WHEEL_ANGLE_LIMIT,
               WHEEL_ANGLE_LIMIT);
        }
        else {
            if (m_FrontWheelAngle > 0) { m_FrontWheelAngle -= Time.fixedDeltaTime * deceleration *2; }
            if (m_FrontWheelAngle < 0) { m_FrontWheelAngle += Time.fixedDeltaTime * deceleration *2; }
        }
        UpdateWheels();
    }

    void UpdateWheels() {
        Vector3 localEulerAngles = new Vector3 (0f, 0f, m_FrontWheelAngle);
        wheelFrontLeft.transform.localEulerAngles = localEulerAngles;
        wheelFrontRight.transform.localEulerAngles = localEulerAngles;
    }
}