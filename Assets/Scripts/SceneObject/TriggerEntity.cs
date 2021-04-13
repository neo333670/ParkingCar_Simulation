using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEntity : MonoBehaviour
{

    public AudioSource alarm;
    //GameManeger update score
    //bound
    private PolygonCollider2D m_car_poly;
    private BoxCollider2D m_Trigger;
    private Vector2[] poly_points;

    bool RegionColorChanged = false;

    //BoxCollider2D m_boxCollider;
    GameObject m_inner;
    EdgeCollider2D m_edgeCollider;

    [SerializeField] SpriteRenderer[] BackLevel_List = new SpriteRenderer[5];
    [SerializeField] SpriteRenderer AlertIcon;

    private void Start()
    {
        alarm = GetComponent<AudioSource>();
        //m_boxCollider = GetComponentInChildren<BoxCollider2D>();
        m_inner = gameObject.transform.GetChild(1).gameObject;
        //m_inner.transform.position += new Vector3(-5, -5,0); 
        m_edgeCollider = m_inner.GetComponent<EdgeCollider2D>();

        m_car_poly = GameObject.Find("body").GetComponent<PolygonCollider2D>();
        m_Trigger = GetComponent<BoxCollider2D>();
        poly_points = m_car_poly.points;
    }

    void ChangeColor(Color _color){
        foreach (SpriteRenderer r in BackLevel_List){
            r.color = _color;
        }
    }
    void ResetColor() { ChangeColor(Color.white); }

    public void AlertIconChangeRed() {
        AlertIcon.color = Color.red;    
    }
    public void AlertIconResetColor(){
        AlertIcon.color = Color.white;
    }

    //check whether parking is completed
    private Bounds Get2DBounds(Bounds aBounds)
    {
        var ext = aBounds.extents;
        ext.z = float.PositiveInfinity;
        aBounds.extents = ext;
        return aBounds;
    }

    bool IsParkingCompleted()
    {
        Bounds boundsInTwoD = Get2DBounds(m_Trigger.bounds);

        bool allPointsFit = true;
        foreach (Vector2 p in poly_points)
        {
            if (!boundsInTwoD.Contains(p))
            {
                allPointsFit = false;
                break;
            }
        }
        return allPointsFit;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.GetType() == typeof(PolygonCollider2D))
        {
            m_car_poly = other.GetComponent<PolygonCollider2D>();
            for (int i = 0; i < m_car_poly.points.Length; i++)
            {
                Vector2 GlobalPoints = m_car_poly.transform.TransformPoint(m_car_poly.points[i]);
                poly_points[i] = GlobalPoints;
            }
            if (IsParkingCompleted() && !RegionColorChanged)
            {
                ChangeColor(Color.green);
                RegionColorChanged = true;
            }
            else if (!IsParkingCompleted() && RegionColorChanged) {
                ResetColor();
                RegionColorChanged = false;
            }
        }
    }
    /**
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (m_edgeCollider.IsTouching(other))
        {
            AlertIconChangeRed();
            alarm.Play();
        }                  
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!m_edgeCollider.IsTouching(other))
        {
            AlertIconResetColor();
            alarm.Stop();
        }     
    }
    **/
}
