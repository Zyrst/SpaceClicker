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
                return _decimal == 0f ? "0%" : (_decimal * 100f).ToString("N2") + "%";
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

    [System.Serializable]
    public class Element
    {
        public vap damage = new vap();
        public Percent crit;
        public Percent critMultiplier;
        public Percent resistance;
        public float cooldownReduction; //Abilities
        public Percent _damagePercent;

        public Element(Element element_)
        {
            this.damage = new vap(element_.damage);
            this.crit = element_.crit;
            this.critMultiplier = element_.critMultiplier;
            this.resistance = element_.resistance;
            this.cooldownReduction = element_.cooldownReduction;
        }

        public Element()
        {
            damage = new vap();
        }
    }

    public Element _normal = new Element();
    public Element _tech = new Element();
    public Element _kinetic = new Element();
    public Element _psychic = new Element();

    public vap _health;

    public Percent _healtPercent;
    
    public Percent _techPercent;
    public Percent _kineticPercent;
    public Percent _psychicPercent;
}
