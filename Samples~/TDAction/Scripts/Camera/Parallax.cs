using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDAction
{
    public class Parallax : MonoBehaviour
    {
        private float length, startpos, startY;
        public GameObject cam;
        public float parallaxEffect;

        private void Start()
        {
            startpos = transform.position.x;
            startY = transform.position.y;
            length = GetComponent<SpriteRenderer>().bounds.size.x;
        }

        private void Update()
        {
            float temp = (cam.transform.position.x * (1 - parallaxEffect));
            float dist = (cam.transform.position.x * parallaxEffect);

            transform.position = new Vector3(startpos + dist, startY, transform.position.z);

            if (temp > startpos + length) startpos += length;
            else if (temp < startpos - length) startpos -= length;
        }
    }
}