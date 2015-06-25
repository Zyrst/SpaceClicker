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
    public float[] _values = new float[8] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f };

    public string GetString()
    {
        string _str = System.Math.Floor(_values[(int)_prefix]+0.5f).ToString();
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

    /// <summary>
    /// returns a values between 0 and 1
    /// </summary>
    /// <param name="v1_">must be lesser than v2_</param>
    /// <param name="v2_">must be greater than v1_</param>
    /// <returns></returns>
    public static float GetScale(vap v1_ , vap v2_)
    {
        float ret = 0f;
        PREFIX prefix = v2_._prefix;

        ret = v1_._values[(int)prefix] / v2_._values[(int)prefix];
        if ((int)prefix != 0)
            ret += (v1_._values[((int)prefix) - 1] * 0.001f) / v2_._values[(int)prefix];

        return ret;
    }

    public void Checker()
    {
        vap ret = new vap(this);

        for (int i = 0; i < ret._values.Length; i++)
        {
            if (i > (int)ret._prefix)
            {
                while (ret._values[i] >= 1000f)
                {
                    ret._values[i + 1] += 1f;
                    ret._values[i] -= 1000f;
                }
            }
            else if (i == (int)ret._prefix)
            {
                while (ret._values[i] >= 100000f)
                {
                    ret._prefix = (PREFIX)(i + 1);
                    ret._values[i + 1] += 100f;
                    ret._values[i] -= 100000f;
                }
            }
        }

        _values = ret._values;
        _prefix = ret._prefix;
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
                else
                {
                    break;
                }
            }
        }
        ret.Checker();
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
                    if (v1_._values[i2] > 0f)
                    {
                        v1_._values[i2]--;
                        ret._values[i] += 1000f;
                        break;
                    }
                }
            }
        }

        ret.Checker();
        return ret;
    }

    public static vap operator *(float f_, vap v1_) { return v1_ * f_; }
    public static vap operator *(vap v1_, float f_)
    {
        vap ret = new vap(v1_);
        for (int i = 0; i < ret._values.Length; i++)
        {
            ret._values[i] *= f_;
        }

        ret.Checker();
        return ret;
    }

    public static vap operator /(float f_, vap v1_) { return v1_ / f_; }
    public static vap operator /(vap v1_, float f_)
    {
        vap ret = new vap(v1_);
        for (int i = 0; i < ret._values.Length; i++)
        {
            ret._values[i] /= f_;
        }
        ret.Checker();
        return ret;
    }
    public static bool operator <(vap v1_, vap v2_)
    {
        if ((int)v1_._prefix < (int)v2_._prefix)
        {
            return true;
        }
        if (v1_._prefix == v2_._prefix)
        {
            if (v1_._values[(int)v1_._prefix] < v2_._values[(int)v1_._prefix])
            {
                return true;
            }
        }
        return false;
    }
    public static bool operator >(vap v1_, vap v2_)
    {
        if ((int)v1_._prefix > (int)v2_._prefix)
        {
            return true;
        }
        if (v1_._prefix == v2_._prefix)
        {
            if (v1_._values[(int)v1_._prefix] > v2_._values[(int)v1_._prefix])
            {
                return true;
            }
        }
        return false;
    }
}
