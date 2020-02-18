using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cone : MonoBehaviour
{
    [SerializeField] GameObject circleIndicator;

    [SerializeField] GameObject leftIndicator;
    [SerializeField] GameObject rightIndicator;

    [SerializeField] GameObject leftPivot;
    [SerializeField] GameObject rightPivot;


    //float myAngle = 0f;

    public void SetTarget(Vector3 targetPos)
    {
        transform.LookAt(targetPos);
    }

    

    public void SetLength(float inputFeet)
    {
        leftIndicator.transform.localScale = new Vector3(0.5f, 0.5f, inputFeet);
        rightIndicator.transform.localScale = new Vector3(0.5f, 0.5f, inputFeet);

        Vector3 endPos = new Vector3(0, 0, inputFeet);
        leftIndicator.transform.localPosition = (Vector3.zero + endPos) / 2;
        rightIndicator.transform.localPosition = (Vector3.zero + endPos) / 2;

        circleIndicator.transform.localScale = new Vector3(inputFeet*2, 0.5f, inputFeet*2);

        //float xL = Mathf.Sin(Mathf.Deg2Rad * -myAngle / 2) * inputFeet;
        //float zL = Mathf.Cos(Mathf.Deg2Rad * -myAngle / 2) * inputFeet;
        //Vector3 endPosL = Vector3.zero + new Vector3(xL, 0, zL);
        //leftIndicator.transform.localPosition = ((Vector3.zero + endPosL) / 2);


        //float xR = Mathf.Cos(Mathf.Deg2Rad * myAngle / 2) * inputFeet;
        //float zR = Mathf.Sin(Mathf.Deg2Rad * myAngle / 2) * inputFeet;
        //Vector3 endPosR = Vector3.zero + new Vector3(xR, 0, zR);
        //rightIndicator.transform.localPosition = ((Vector3.zero + endPosR) / 2);
    }

    public void SetDegree(float degree)
    {
        leftPivot.transform.localRotation = Quaternion.Euler(0, -degree / 2, 0);
        rightPivot.transform.localRotation = Quaternion.Euler(0, degree / 2, 0);
        //myAngle = degree;
    }
}
