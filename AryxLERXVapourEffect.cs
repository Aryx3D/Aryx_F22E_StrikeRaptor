using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Aryx_F22E_StrikeRaptor
{
    public class AryxLERXVapourEffect : MonoBehaviour
    {
        [SerializeField]
        private Aircraft aircraft;

        [SerializeField]
        private ParticleSystem[] vaporSystems;

        [SerializeField]
        private float minimumAirspeed = 70f;

        [SerializeField]
        private float fullAirspeed = 140f;

        [SerializeField]
        private float minimumAlpha = 6f;

        [SerializeField]
        private float fullAlpha = 16f;


        [SerializeField]
        private AnimationCurve alphaCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

        [SerializeField]
        private AnimationCurve airspeedCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

        private ParticleSystem.EmissionModule[] emissions;
        private float[] baseEmissionRates;
        public float speedFactor;
        public float alphaFactor;
        private float alpha = 0;
        private void Awake()
        {
            emissions = new ParticleSystem.EmissionModule[vaporSystems.Length];
            baseEmissionRates = new float[vaporSystems.Length];

            for (int i = 0; i < vaporSystems.Length; i++)
            {
                ParticleSystem.MainModule main = vaporSystems[i].main;

                emissions[i] = vaporSystems[i].emission;

                baseEmissionRates[i] = emissions[i].rateOverTimeMultiplier;
                emissions[i].rateOverTimeMultiplier = 0f;
            }
        }

        private void FixedUpdate()
        {
            if (aircraft == null || aircraft.rb == null)
                return;

            alpha = TargetCalc.GetAngleOnAxis(transform.forward, aircraft.rb.velocity, transform.right);
            if (alpha <= 0f)
                return;

            speedFactor = Mathf.InverseLerp(minimumAirspeed, fullAirspeed, aircraft.speed);
            alphaFactor = Mathf.InverseLerp(minimumAlpha, fullAlpha, alpha);

            speedFactor = airspeedCurve.Evaluate(speedFactor);
            alphaFactor = alphaCurve.Evaluate(alphaFactor);

            for (int i = 0; i < emissions.Length; i++)
            {
                emissions[i].rateOverTimeMultiplier = baseEmissionRates[i] * speedFactor * alphaFactor;
                vaporSystems[i].Play();
            }
        }
    }
}
