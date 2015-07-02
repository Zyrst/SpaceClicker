using UnityEngine;
using System.Collections;

[System.Serializable]
public class TalentStats {
    [System.Serializable]
    public class Percent
    {
        public Percent()
        {
        }

        public Percent(float value_)
        {
            value = value_;
        }
        [SerializeField]        // syns i editorn
        private float _decimal = 0f;

        public string PerCent
        {
            get
            {
                return _decimal == 0f ? "0%" : ((_decimal - 1f) * 100f).ToString("N2") + "%";
            }
        }
        public float value
        {
            get
            {
                return _decimal;
            }
            set
            {
                _decimal = value;
            }
        }
    }

    public CharacterStats.Element _normal = new CharacterStats.Element();
    public CharacterStats.Element _tech = new CharacterStats.Element();
    public CharacterStats.Element _kinetic = new CharacterStats.Element();
    public CharacterStats.Element _psychic = new CharacterStats.Element();

    public vap _health;

    public Percent _healtPercent;
    
    public Percent _damagePercent;
    public Percent _techPercent;
    public Percent _kineticPercent;
    public Percent _psychicPercent;
}
