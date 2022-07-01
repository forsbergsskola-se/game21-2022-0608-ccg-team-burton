using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamBurtonCore.Core
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] Transform target = null;

        void LateUpdate()
        {
            transform.position = target.position;
        }
    }
}