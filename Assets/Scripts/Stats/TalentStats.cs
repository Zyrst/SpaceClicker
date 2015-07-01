using UnityEngine;
using System.Collections;

[System.Serializable]
public class TalentStats {
    [System.Serializable]
    public class Percent
    {
        [SerializeField]        // syns i editorn
        private float _decimal = 0f;
        [SerializeField]
        private string _percent = "0%";

        public string PerCent
        {
            get
            {
                return _percent;
            }
            set
            {
                _percent = value;

                if (_percent[_percent.Length-1] != '%')
                {
                    _percent = _percent + "%";
                }

                _decimal = (float.Parse(_percent.Substring(_percent.Length - 1)) / 100f) + 1f;
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

                _percent = (_decimal - 1f) * 100f + "%";
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
