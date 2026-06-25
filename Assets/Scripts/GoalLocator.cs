using UnityEngine;

public static class GoalLocator
{
    public static GameObject FindClosestGoal(Vector3 position)
    {
        GameObject[] objects = Object.FindObjectsOfType<GameObject>();
        float minDistance = float.MaxValue;
        GameObject closestGoal = null;

        foreach (GameObject obj in objects)
        {
            if (obj.name.ToLower().Contains("goal"))
            {
                float distance = Vector3.Distance(position, obj.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestGoal = obj;
                }
            }
        }
        return closestGoal;
    }
}
