using System;
using UnityEngine;

namespace FloxyDev.RadialMenu
{
    public class HandTransform : MonoBehaviour
    {
        public void VectorInput(Vector3 vector)
        {
            while (true)
            {
                transform.position = vector;
            }
        }
    }
}