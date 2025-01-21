using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    private float SPEED = 5f;

    public void Move(int horizontalAxis, int verticalAxis)
    {
        float newX = this.transform.position.x + SPEED * horizontalAxis * Time.deltaTime;
        float newY = this.transform.position.y + SPEED * verticalAxis * Time.deltaTime;
        this.transform.position = new Vector3(newX, newY, 0);
    }
}
