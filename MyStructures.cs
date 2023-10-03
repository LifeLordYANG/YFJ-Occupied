using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class MyStructures
{
    public struct Character_Save
    {
        public int rank;
        public int level;
        public int exp;
        public Character_Save(int r, int l, int e)
        {
            rank = r;
            level = l;
            exp = e;
        }
    }
    // public struct Character_Fight
    // {
    //     public int num;
    //     public int hp;
    //     public int atk;
    //     public int def;
    //     public int spd;
    //     public int sp;
    //     public int move;
    //     public float range;
    //     public int rank;
    //     public Character_Fight(int n, int lv, int rk)
    //     {
    //         num = n;
    //         hp = InitialScene.Count_HpSp(n, lv, true);
    //         atk = InitialScene.Count_AtkDef(n, lv, true);
    //         def = InitialScene.Count_AtkDef(n, lv, false);
    //         spd = InitialScene.Count_Spd(n, lv);
    //         sp = InitialScene.Count_HpSp(n, lv, false);
    //         move = Dictionaries.character_base[n].move_b;
    //         range = Dictionaries.character_base[n].range_b;
    //         rank = rk;
    //     }
    // }
    public struct Character_Fight
    {
        public int num;
        public int level;
        public int rank;
        public Character_Fight(int n, int lv, int rk)
        {
            num = n;
            level = lv;
            rank = rk;
        }
    }
    public struct Character_Formation
    {
        public int[] num_cha;
        public Character_Formation(int[] num)
        {
            num_cha = num;
        }
    }
    public struct Character_Base
    {
        public int hp_b;
        public int atk_b;
        public int def_b;
        public int spd_b;
        public int sp_b;
        public int move_b;
        public int hit_b;
        public float range_b;
        public Character_Base(int hp, int atk, int def, int spd, int sp, int move, int hit, float range)
        {
            hp_b = hp;
            atk_b = atk;
            def_b = def;
            spd_b = spd;
            sp_b = sp;
            move_b = move;
            hit_b = hit;
            range_b = range;
        }
    }
    public struct Character_Time
    {
        public int num_cha;
        public string overTime;
        public Character_Time(int n, DateTime t)
        {
            num_cha = n;
            overTime = t.ToString("MM/dd/yyyy HH:mm:ss");
        }
    }
    public struct Character_Skin
    {
        public int current_skin;
        public List<int> list_skin;
        public Character_Skin(List<int> i)
        {
            current_skin = 0;
            list_skin = i;
        }
    }
    public struct Skill
    {
        public int hp;
        public int atk;
        public int def;
        public int spd;
        public int sp;
        public int range;
        public Skill(int _hp, int _atk, int _def, int _spd, int _sp, int _range)
        {
            hp = _hp;
            atk = _atk;
            def = _def;
            spd = _spd;
            sp = _sp;
            range = _range;
        }

    }
    public struct MapInfo
    {
        public string name;
        public Towns[] townInfo;
        public MapInfo(string n, Towns[] ti)
        {
            name = n;
            townInfo = ti;
        }
        public struct Towns
        {
            public string townName;
            public string townSize; //town, city, capital
            public Dictionary<int, MyStructures.Character_Fight> townFormation;
            public Dictionary<int, int> townReward_first;
            public Dictionary<int, float[]> townReward;
            public Towns(string tn, string ts, Dictionary<int, MyStructures.Character_Fight> tf, Dictionary<int, int> tr_f, Dictionary<int, float[]> tr)
            {
                townName = tn;
                townSize = ts;
                townFormation = tf;
                townReward_first = tr_f;
                townReward = tr;
            }
        }
    }
    public struct Item
    {
        public int num;
        public int amount;
    }
    public struct GachaItem
    {
        //public int inMachine;
        public int num;
        public int amount;
        public GachaItem(/*int m, */int n, int a)
        {
            //inMachine = m;
            num = n;
            amount = a;
        }
    }
    [System.Serializable]
    public struct Skin
    {
        public Sprite[] sp;
    }
}