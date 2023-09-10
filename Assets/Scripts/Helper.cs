using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper
{
    public static float GenerateRandomFloat(float minValue, float maxValue)
    {
        float randomValue = Random.Range(minValue, maxValue);
        float roundedValue = Mathf.Round(randomValue * 10.0f) / 10.0f; // Округляем до одного знака после запятой
        return roundedValue;
    }

    public static int GenerateRandomInt(int minValue, int maxValue)
    {
        int randomValue = Random.Range(minValue, maxValue);
        return randomValue;
    }

    /*private void OnDrawGizmos()
    {
        float detectionDistance = 0.3f;
        float radius = 0.2f;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + (Vector3)GetLastMoveInput(), radius);

        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position + (Vector3)GetLastMoveInput(), radius, GetLastMoveInput().normalized, detectionDistance);
        foreach (RaycastHit2D hit in hits)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position + (Vector3)GetLastMoveInput(), hit.point);
        }
    } */
}
