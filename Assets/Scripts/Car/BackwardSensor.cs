using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackwardSensor : MonoBehaviour
{
    public float RayLenth = 3;
    //Get carEntity 
    private CarEntity m_car;
    float CarRotationZ;
    Vector2 Sensor_NormolVector;
    Vector2 SensorOrigin;
    public GameObject ReversingAlarm;

    private void Start() {
        m_car = GameObject.Find("Car").GetComponent<CarEntity> ();
        ReversingAlarm.SetActive(false);
    }

    private void FixedUpdate(){
        CarRotationZ = m_car.transform.rotation.eulerAngles.z;
        Sensor_NormolVector = new Vector2 (-Mathf.Cos(CarRotationZ * Mathf.Deg2Rad), 
            -Mathf.Sin(CarRotationZ * Mathf.Deg2Rad));
        SensorOrigin = transform.position;

        Debug.DrawRay(SensorOrigin, Sensor_NormolVector * RayLenth, Color.red);

        if (Physics2D.Raycast(SensorOrigin, Sensor_NormolVector, RayLenth))
        {
            ReversingAlarm.SetActive(true);
        }
        if (!Physics2D.Raycast(SensorOrigin, Sensor_NormolVector, RayLenth)
             && ReversingAlarm) {
            ReversingAlarm.SetActive(false);
        }
    }

}
