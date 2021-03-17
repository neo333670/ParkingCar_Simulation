
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracingCameraEntity : MonoBehaviour
{
    public CarEntity targetObject;
    public float tracingLateFactor = 1;
    public float MOVING_THRESHOLD = 10f;

    Camera m_camera;
    float m_OrthographicSize;

    void Start() {
        m_camera = this.GetComponent <Camera> ();
        m_OrthographicSize = m_camera.orthographicSize;
    }

    void LateUpdate () {
        Vector2 deltaPos = this.transform.position - targetObject.transform.position ;

        m_camera.orthographicSize = m_OrthographicSize + targetObject.Velocity * 0.2f;

        if (deltaPos.magnitude > MOVING_THRESHOLD) {
            deltaPos.Normalize();

            Vector2 newPosition = new Vector2 
                (targetObject.transform.position.x, targetObject.transform.position.y)
                + deltaPos * MOVING_THRESHOLD *tracingLateFactor;

            this.transform.position = new Vector3(newPosition.x, newPosition.y, this.transform.position.z);
        }
    }
}
