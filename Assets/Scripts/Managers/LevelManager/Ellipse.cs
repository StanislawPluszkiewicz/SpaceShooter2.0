using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Ellipse
{
    public float xAxis;
    public float yAxis;
	public float z;
    
    public Ellipse(float xAxis, float yAxis, float zValue){
        this.xAxis = xAxis;
        this.yAxis = yAxis;
        this.z = zValue;
    }

    public Vector3 Evaluate(float t){
        float angle = t * 360 * Mathf.Deg2Rad;
        float x = Mathf.Sin(angle) * xAxis;
        float y = Mathf.Cos(angle) * yAxis;
		float z = this.z;
        return new Vector3(x,y,z);
    }
}
