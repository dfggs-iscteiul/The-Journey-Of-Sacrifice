﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovment : MonoBehaviour
{
    public Transform target;
    public float smoothing;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float y = Mathf.Clamp(target.position.y, (float)(-11.59), (float)(14.36));
        float x = Mathf.Clamp(target.position.x, (float)(-12.49), (float)(11.53));

        if (transform.position!=target.position)
        {
            Vector3 targetPosition = new Vector3(x, y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing);
        }
    }
}
