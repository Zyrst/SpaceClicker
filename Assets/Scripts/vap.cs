using UnityEngine;
using System.Collections;

[System.Serializable]
public class vap {

    public enum PREFIX : int
    {
        d = 0,
        k = 1,
        M = 2,
        G = 3,
        T = 4,
        P = 5,
        E = 6,
        Y = 7
    }
    
    public vap()
    {
    }
    public vap(float value_, PREFIX prefix_)
    {
        _values[(int)prefix_] = value_;
    }
    public vap(vap vap_)
    {
        _prefix = vap_._prefix;
        for (int i = 0; i < _values.Length; i++)
        {
            _values[i] = vap_._values[i];
        }
    }

    public PREFIX _prefix = 0;
    private float[] _values = new float[8];

    public string GetString()
    {
        string _str = _values[(int)_prefix].ToString();
        if (_prefix != 0)
        {
            _str += _prefix.ToString();
        }
        return _str;
    }
    public float GetFloat()
    {
        return _values[(int)_prefix];
    }

    public static vap operator +(vap v1_, vap v2_)
    {
        vap ret = new vap();
        PREFIX highest = ((int)v1_._prefix >= (int)(v2_._prefix) ? v1_._prefix : v2_._prefix);

        for (int i = 0; i < ret._values.Length; i++)
        {
            ret._values[i] = v1_._values[i] + v2_._values[i];
        }

        for (int i = 0; i < ret._values.Length; i++)
        {
            if (ret._values[i] >= 100f)
            {
                ret._prefix = (PREFIX)(i);
            }
            while (ret._values[i] >= 1000f)
            {
                if ((int)highest != i)
                {
                    ret._values[i + 1]++;
                    ret._values[i] -= 1000f;
                }
            }
        }

        return ret;
    }

    public static vap operator -(vap v1_, vap v2_)
    {
        vap ret = new vap();

        // börja nedifrån
        // minska första
            // om den är < 0
                // minska nästa med en, sätt denna till max
                // om nästa är 0 innan, kolla nästa efter det, etc.
                // om alla är 0 så är det slut

        for (int i = 0; i < ret._values.Length; i++)
        {
            ret._values[i] = v1_._values[i] - v2_._values[i];
            if (ret._values[i] < 0f)
            {
                for (int i2 = i; i2 < ret._values.Length; i2++)
                {
                    if (v1_._values[i2] > 0)
                    {
                        v1_._values[i2]--;
                        ret._values[i] += 100000f;
                        break;
                    }
                }
            }
        }

        for (int i = 0; i < ret._values[i]; i++)
        {
            if (ret._values[i] >= 100f)
            {
                ret._prefix = (PREFIX)i;
            }
        }

        return ret;
    }

    public static vap operator *(float f_, vap v1_) { return v1_ * f_; }
    public static vap operator *(vap v1_, float f_)
    {
        for (int i = 0; i < v1_._values.Length; i++)
        {
            v1_._values[i] *= f_;
        }

        for (int i = 0; i < v1_._values[i]; i++)
        {
            if (v1_._values[i] >= 100f)
            {
                v1_._prefix = (PREFIX)i;
            }
        }

        return v1_;
    }

    public static vap operator /(float f_, vap v1_) { return v1_ / f_; }
    public static vap operator /(vap v1_, float f_)
    {
        for (int i = 0; i < v1_._values.Length; i++)
        {
            v1_._values[i] /= f_;
        }

        for (int i = 0; i < v1_._values[i]; i++)
        {
            if (v1_._values[i] >= 100f)
            {
                v1_._prefix = (PREFIX)i;
            }
        }

        return v1_;
    }
}
