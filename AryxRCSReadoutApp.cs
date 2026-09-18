using NuclearOption.UIStyleSystem;
using TMPro;
using UnityEngine;

namespace Aryx_F22E_StrikeRaptor
{
    public sealed class AryxRCSReadoutApp : HUDApp
    {
        private float lastRCS = float.NaN;
        private Aircraft aircraft;
        private Gradient redToGreenGradient;
        private float currentRcs;

        [SerializeField]
        private TextMeshProUGUI readoutText;

        [SerializeField]
        private float RedThreshold;

        public override void Initialize(Aircraft aircraft)
        {
            if (aircraft == null)
                return;

            this.aircraft = aircraft;
            UpdateReadout();
        }

        public override void Refresh()
        {
            if (aircraft == null || readoutText == null)
                return;

            if (Mathf.Approximately(lastRCS, aircraft.RCS))
                return;

            UpdateReadout();
        }

        private void UpdateReadout()
        {
            if (aircraft == null || readoutText == null)
                return;

            if (redToGreenGradient == null)
                redToGreenGradient = ThemeManager.Active.ColorTheme.Gradient();

            currentRcs = aircraft.RCS;

            readoutText.color = redToGreenGradient.Evaluate(1f - Mathf.InverseLerp(0.0012f, RedThreshold, currentRcs));

            readoutText.text = $"RCS: {currentRcs.ToString("0.0000")}";
            lastRCS = currentRcs;
        }

        private void RCSReadout_OnThemeGroupChanged()
        {
            redToGreenGradient = ThemeManager.Active.ColorTheme.Gradient();
            UpdateReadout();
        }

        private void OnEnable()
        {
            ThemeManager.ThemeGroupChanged += RCSReadout_OnThemeGroupChanged;
            redToGreenGradient = ThemeManager.Active.ColorTheme.Gradient();
        }

        private void OnDisable()
        {
            ThemeManager.ThemeGroupChanged -= RCSReadout_OnThemeGroupChanged;
        }
    }
}