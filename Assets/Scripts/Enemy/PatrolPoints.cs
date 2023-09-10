using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PatrolPoints : MonoBehaviour
{
    [SerializeField] private List<Transform> patrolingPoints;
    private Transform targetPoint;
    private int index;
    private void Start()
    {
        index = 0;
        try 
        {
            if (patrolingPoints.Count > 0) 
            {
                targetPoint = patrolingPoints[index];
            } else {
                new EmptyListException("The list of patroling points can not be empty!");
            } 
        } catch (EmptyListException ex)
        {
            Debug.LogError("An exception has occured: " + ex.Message);
        }
    }

    public bool HasRichedPoint()
    {
        //Debug.Log(Vector3.Distance(this.transform.position, targetPoint.position));
        return (Vector3.Distance(this.transform.position, targetPoint.position) < 0.1f) ? true : false;
    }

    public void SetNextTargetPoint()
    {
        index = (index == patrolingPoints.Count - 1) ? 0 : index + 1;
        targetPoint = patrolingPoints[index];
        //Debug.Log("Target point settled: " + targetPoint);
    }

    public Vector3 GetTargetPointDirection()
    {
        //Debug.Log("Target point direction: " + (targetPoint.position - this.transform.position).normalized);
        return (targetPoint.position - this.transform.position).normalized;
    }
}


public class EmptyListException : Exception
{
    public EmptyListException(string message) : base(message)
    {
    }
}
