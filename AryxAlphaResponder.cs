using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Aryx_F22E_StrikeRaptor
{
    public sealed class AryxAlphaResponder : MonoBehaviour
    {
        [SerializeField]
        private Aircraft aircraft;
        [SerializeField]
        private UnitPart criticalPart;
        [SerializeField]
        private float disableThreshold;
        [SerializeField]
        private AnimationCurve alphaResponse;
        [SerializeField]
        private float alphaBoundary = 30;
        [SerializeField]
        private float maxDeflection = 25;
        [SerializeField]
        private float deflectionSpeed = 75;
        [SerializeField]
        private float minimumSpeed = 20;
        [SerializeField]
        private Transform rotator;

        private bool isFunctional = false;
        private float alpha = 0;
        public float deflection;

        private void Awake()
        {
            if (aircraft == null || criticalPart == null || rotator == null || alphaResponse == null)
                return;

            criticalPart.onApplyDamage += Damage;
            criticalPart.onPartDetached += Detach;
            isFunctional = true;
        }

        private void FixedUpdate()
        {
            if (!isFunctional)
                return;

            if (aircraft.speed > minimumSpeed)
                alpha = TargetCalc.GetAngleOnAxis(aircraft.rb.transform.forward, aircraft.rb.velocity, aircraft.rb.transform.right);
            else
                alpha = 0;

                float currentX = rotator.localEulerAngles.x;
            if (currentX > 180f)
                currentX -= 360f;
            deflection = Mathf.MoveTowards(currentX, alphaResponse.Evaluate(Mathf.Clamp(alpha / alphaBoundary, -1f, 1f)) * maxDeflection, deflectionSpeed * Time.fixedDeltaTime);
            rotator.localRotation = Quaternion.Euler(deflection, rotator.localEulerAngles.y, rotator.localEulerAngles.z);

        }
        private void Damage(UnitPart.OnApplyDamage dmg)
        {
            if (criticalPart.hitPoints < disableThreshold)
            {
                Disable();
            }
        }
        private void Detach(UnitPart part)
        {
            Disable();
        }
        private void Disable()
        {
            if (!isFunctional) return;

            isFunctional = false;
            criticalPart.onPartDetached -= Detach;
            criticalPart.onApplyDamage -= Damage;
        }
        private void OnDestroy()
        {
            if (criticalPart == null)
                return;

            criticalPart.onPartDetached -= Detach;
            criticalPart.onApplyDamage -= Damage;
        }
    }
}
