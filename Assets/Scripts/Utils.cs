using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utils {

    struct Item
    {
        string name;
    }

    public struct Stats
    {
        public int health;
        public int attack;
        public int defense;
        public int speed;
    }

    [Serializable]
    class PlayerData
    {
        public Item [] equippedItems;
        public Item[] items;
        public ResourceRequest [] resources;
        public int currentHealth;
        public Stats playterStats;
        public int money;

    }

    [Serializable]
    class UserData
    {
        public string id;
        public string charName;
    }
}
