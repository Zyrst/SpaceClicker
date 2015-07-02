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
        Z = 7,
        Y = 8,
        Yk = 9,
        Ym = 10
    }
    
    public vap()
    {
        _prefix = PREFIX.d;
        _values = new float[11] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f };
    }

    public vap(vap vap_)
    {
        _prefix = vap_._prefix;

        for (int i = 0; i < LENGTH; i++)
        {
            _values[i] = vap_._values[i];
        }
        Checker();
    }

    public PREFIX _prefix = PREFIX.d;
    public float[] _values = new float[11] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f };
    public const int LENGTH = 11;

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
        for (int i = LENGTH - 1; i > 0; i--)
        {   
            while ((int)_prefix <= i && _values[i] < 100f && _values[i] > 0f)
            {
                _prefix = (PREFIX)(i - 1);
                _values[i - 1] += 1000f;
                _values[i] -= 1f;
            }
        }

        for (int i = 0; i < LENGTH; i++)
        {
            if (_values[i] != 0f)
            {
                _prefix = (PREFIX)i;
            }
        }

        for (int i = 0; i < (int)_prefix+1; i++)
        {
            if (i <= (int)_prefix)
            {
                string top = "100000000000000000000000000000";      // vad som subtraheras från denna
                string bot = "100000000000000000000000000";         // vad som adderas till nästa
                while (bot.Length > 2)
                {
                    while (_values[i] >= float.Parse(top))
                    {
                        _prefix = (PREFIX)(i + 1);
                        _values[i + 1] += float.Parse(bot);
                        _values[i] -= float.Parse(top);
                    }
                    top = top.Remove(top.Length - 1);
                    bot = bot.Remove(bot.Length - 1);
                }
            }
            if (i < (int)_prefix)
            {
                // 10k
                while (_values[i] >= 10000f)
                {
                    _prefix = (PREFIX)(i + 1);
                    _values[i + 1] += 10f;
                    _values[i] -= 10000f;
                }
                // 1k
                while (_values[i] >= 1000f)
                {
                    
                    _prefix = (PREFIX)(i + 1);
                    _values[i + 1] += 1f;
                    _values[i] -= 1000f;
                }
            }
        }

        for (int i = 0; i < LENGTH; i++)
        {
            if (_values[i] != 0f)
            {
                _prefix = (PREFIX)i;
            }
        }
    }

    public static vap operator +(vap v1_, vap v2_)
    {
        vap ret = new vap();
        PREFIX highest = ((int)v1_._prefix >= (int)(v2_._prefix) ? v1_._prefix : v2_._prefix);
        ret._prefix = highest;

        for (int i = 0; i < LENGTH; i++)
        {
            ret._values[i] = v1_._values[i] + v2_._values[i];
        }

        ret.Checker();
        return ret;
    }

    public static vap operator -(vap v1_, vap v2_)
    {
        vap ret = new vap();
        PREFIX highest = ((int)v1_._prefix >= (int)(v2_._prefix) ? v1_._prefix : v2_._prefix);
        ret._prefix = highest;

        for (int i = LENGTH - 1; i > -1; i--)
        {
            ret._values[i] = v1_._values[i] - v2_._values[i];
        }

    begin:
        bool exit = true;
    for (int i = 0; i < LENGTH - 1; i++)
        {
            while (ret._values[i] < 0f)
            {
                ret._values[i] += 1000f;
                ret._values[i + 1] -= 1f;
                exit = false;
            }

            if (i+1 == (int)PREFIX.Y && ret._values[i+1] < 0f)
            {
                ret = new vap();
            }
            else if (i + 1 == (int)PREFIX.Y && ret._values[i + 1] > 0f)
            {
                if (!exit)
                    goto begin;
            }
        }

        ret.Checker();
        return ret;
    }

    public static vap operator *(float f_, vap v1_) { return v1_ * f_; }
    public static vap operator *(vap v1_, float f_)
    {
        vap ret = new vap(v1_);
        for (int i = 0; i < (int)ret._prefix + 1; i++)
        {
            ret._values[i] *= f_;
        }
        ret.Checker();
        ret.Checker();
        return ret;
    }

    public static vap operator /(float f_, vap v1_) { return v1_ / f_; }
    public static vap operator /(vap v1_, float f_)
    {
        vap ret = new vap(v1_);
        for (int i = 0; i < (int)ret._prefix + 1; i++)
        {
            ret._values[i] /= f_;
        }
        ret.Checker();
        ret.Checker();
        return ret;
    }
    public static bool operator <(vap v1_, vap v2_)
    {
        bool ret = false;
        // if v1 prefix is smaller than v2 preifx
        if ((int)v1_._prefix < (int)v2_._prefix)
        {
            ret = true;
        }
        // if the prefixes are equal
        else if (v1_._prefix == v2_._prefix)
        {
            // if the value for v1 for the current prefix is lesser 
            // than the value for v2
            if (v1_._values[(int)v1_._prefix] < v2_._values[(int)v1_._prefix])
            {
                ret = true;
            }
        }
        return ret;
    }
    public static bool operator >(vap v1_, vap v2_)
    {
        bool ret = false;
        // if v1 prefix is greater than v2 preifx
        if ((int)v1_._prefix > (int)v2_._prefix)
        {
            ret = true;
        }
        // if the prefixes are equal
        else if (v1_._prefix == v2_._prefix)
        {
            // if the value for v1 for the current prefix is greater 
            // than the value for v2
            if (v1_._values[(int)v1_._prefix] > v2_._values[(int)v1_._prefix])
            {
                ret = true;
            }
        }
        return ret;
    }

    /// <summary>
    /// Calculate with negative numbers, vap1 - vap 2
    /// </summary>
    /// <param name="v1_">Vap with stats</param>
    /// <param name="v2_">vap with stats</param>
    /// <returns>Calculated vap with negative and positive numbers</returns>
    public static vap Minus(vap v1_, vap v2_)
    {
        vap ret = new vap();
        PREFIX highest = ((int)v1_._prefix >= (int)(v2_._prefix) ? v1_._prefix : v2_._prefix);
        ret._prefix = highest;

        for (int i = LENGTH - 1; i > -1; i--)
        {
            ret._values[i] = v1_._values[i] - v2_._values[i];
        }

        return ret;
    }
}
