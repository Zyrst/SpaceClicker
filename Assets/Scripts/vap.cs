﻿using UnityEngine;
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
        Y = 8
    }
    
    public vap()
    {
        _prefix = PREFIX.d;
        _values = new float[9] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f };
    }

    public vap(vap vap_)
    {
        _prefix = vap_._prefix;

        for (int i = 0; i < LENGTH; i++)
        {
            _values[i] = vap_._values[i];
        }
    }

    public PREFIX _prefix = PREFIX.d;
    public float[] _values = new float[9] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f };
    public const int LENGTH = 9;

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

        for (int i = LENGTH - 1; i > 0; i--)
        {   
            while ((int)ret._prefix <= i && ret._values[i] < 100f && ret._values[i] > 0f)
            {
                ret._prefix = (PREFIX)(i - 1);
                ret._values[i - 1] += 1000f;
                ret._values[i] -= 1f;
            }
        }
        for (int i = 0; i < (int)ret._prefix+1; i++)
        {
            if (i <= (int)ret._prefix)
            {
                string top = "100000000000000000000000000000";      // vad som subtraheras från denna
                string bot = "100000000000000000000000000";         // vad som adderas till nästa
                while (bot.Length > 2)
                {
                    while (ret._values[i] >= float.Parse(top))
                    {
                        ret._prefix = (PREFIX)(i + 1);
                        ret._values[i + 1] += float.Parse(bot);
                        ret._values[i] -= float.Parse(top);
                    }
                    top = top.Remove(top.Length - 1);
                    bot = bot.Remove(bot.Length - 1);
                }
            }
            if (i < (int)ret._prefix)
            {
                // 10k
                while (ret._values[i] >= 10000f)
                {
                    ret._prefix = (PREFIX)(i + 1);
                    ret._values[i + 1] += 10f;
                    ret._values[i] -= 10000f;
                }
                // 1k
                while (ret._values[i] >= 1000f)
                {
                    
                    ret._prefix = (PREFIX)(i + 1);
                    ret._values[i + 1] += 1f;
                    ret._values[i] -= 1000f;
                }
            }
        }

        for (int i = 0; i < LENGTH; i++)
        {
            if (ret._values[i] != 0f)
            {
                ret._prefix = (PREFIX)i;
            }
        }

        _values = ret._values;
        _prefix = ret._prefix;
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
            ret.Checker();
        }
        return ret;
    }

    public static vap operator /(float f_, vap v1_) { return v1_ / f_; }
    public static vap operator /(vap v1_, float f_)
    {
        vap ret = new vap(v1_);
        for (int i = 0; i < (int)ret._prefix + 1; i++)
        {
            ret._values[i] /= f_;
            ret.Checker();
        }
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
}
