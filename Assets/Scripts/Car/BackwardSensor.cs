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
    Vector2 L_SensorOrigin;
    Vector2 R_SensorOrigin;
    public GameObject ReversingAlarm;

    private GameObject ReversingRadar_L;
    private GameObject ReversingRadar_R;

    private void Start() {
        m_car = GameObject.Find("Car").GetComponent<CarEntity> ();
        ReversingRadar_L = gameObject.transform.GetChild(0).gameObject;
        ReversingRadar_R = gameObject.transform.GetChild(1).gameObject;

        ReversingAlarm.SetActive(false);
    }

    private void FixedUpdate(){
        CarRotationZ = m_car.transform.rotation.eulerAngles.z;
        Sensor_NormolVector = new Vector2 (-Mathf.Cos(CarRotationZ * Mathf.Deg2Rad), 
            -Mathf.Sin(CarRotationZ * Mathf.Deg2Rad));
        L_SensorOrigin = ReversingRadar_L.transform.position;
        R_SensorOrigin = ReversingRadar_R.transform.position;

        Debug.DrawRay(L_SensorOrigin, Sensor_NormolVector * RayLenth, Color.red);
        Debug.DrawRay(R_SensorOrigin, Sensor_NormolVector * RayLenth, Color.red);

        if (Physics2D.Raycast(L_SensorOrigin, Sensor_NormolVector, RayLenth) | Physics2D.Raycast(R_SensorOrigin, Sensor_NormolVector, RayLenth))
        {
            ReversingAlarm.SetActive(true);
        }
        if (!Physics2D.Raycast(L_SensorOrigin, Sensor_NormolVector, RayLenth) 
            && !Physics2D.Raycast(R_SensorOrigin, Sensor_NormolVector, RayLenth) 
            && ReversingAlarm) {
            ReversingAlarm.SetActive(false);
        }
    }

}
