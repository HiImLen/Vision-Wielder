using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowParent : MonoBehaviour
{
    public Transform parent;
    public Vector3 offset;
    void Start()
    {
        transform.position = parent.position + offset;
    }
}
