using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behaviour : MonoBehaviour
{
    [ReadOnly]
    public Transform target;

    public float attackDistance = 10f;

    public void TargetSelection ()
    {
        // get closest player
        float minSqrDistance = Mathf.Infinity;

        foreach (PlayerController p in PlayerController.players)
        {
            float sqrDistance = Vector3.SqrMagnitude(p.transform.position - transform.position);

            if (sqrDistance <= minSqrDistance)
            {
                target = p.transform;
            }
        }
    }

    public float TargetSqrDistance ()
    {
        return Vector3.SqrMagnitude(target.position - transform.position);
    }

    public bool LineOfSight ()
    {
        // raycast
        return true;
    }

    public void SimpleApproach ()
    {
        // walk towards the target
    }

    public void PathfindingApproach ()
    {

    }

    virtual public void Attack ()
    {
        //
    }

    private void Update ()
    {
        TargetSelection();

        if (LineOfSight())
        {
            if (TargetSqrDistance() > attackDistance)
            {
                SimpleApproach();
            }
            else
            {
                Attack();
            }
        }
        else
        {
            PathfindingApproach();
        }
    }
}
