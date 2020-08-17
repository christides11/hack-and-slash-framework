using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDAction.Combat
{
    public class Hitbox : CAF.Combat.Hitbox
    {
        [SerializeField] protected GameObject rectangleVisual;
        [SerializeField] protected GameObject sphereVisual;
        [SerializeField] protected GameObject capsuleVisual;

        protected override void CreateRectangle(Vector3 size)
        {
            base.CreateRectangle(size);
            rectangleVisual.transform.localScale = size;
            rectangleVisual.SetActive(true);
        }

        protected override void CreateSphere(float radius)
        {
            base.CreateSphere(radius);
            sphereVisual.transform.localScale = Vector3.one * radius;
            sphereVisual.SetActive(true);
        }

        protected override void CreateCapsule(float radius, float height)
        {
            base.CreateCapsule(radius, height);
            //capsuleVisual.transform.localScale = new Vector3(radius * 2.0f, height, radius * 2.0f);
            capsuleVisual.SetActive(true);
        }
        public override void Deactivate()
        {
            base.Deactivate();
            rectangleVisual.SetActive(false);
            sphereVisual.SetActive(false);
            capsuleVisual.SetActive(false);
        }
    }
}