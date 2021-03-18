using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleEntity : MonoBehaviour
{
    SpriteRenderer targetRender;

    private void Start(){
        targetRender = this.GetComponent<SpriteRenderer> ();
    }

    private void OnCollisionEnter2D (Collision2D collision) {
        targetRender.color = Color.red;
    }
    private void OnCollisionExit2D(Collision2D collision) {
        targetRender.color = Color.white;
    }
}
