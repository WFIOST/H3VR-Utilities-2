using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace H3VRUtilities
{
    public class Laser : MonoBehaviour
    {
        public GameObject endPosition;

        private LineRenderer lineRenderer;
        void Start()
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        void Update()
        {
            lineRenderer.SetPosition(0, transform.position);
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit))
            {
                if (hit.collider)
                {
                    lineRenderer.SetPosition(1, hit.point);
                }
            }
            else
            {
                lineRenderer.SetPosition(1, endPosition.transform.position);
            }
        }
    }
}