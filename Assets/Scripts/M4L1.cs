using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M4L1 : MonoBehaviour
{
    public enum Enemies
    {
        None,
        Turret,
        Zombie,
        Wizzard
    }
    public Enemies enemies;



    void Start()
    {

    }

    private void Update()
    {
        switch (enemies)
        {
            case Enemies.None:
                print("No Enemy");
                break;
            case Enemies.Turret:
                print("Enemy Turret");
                break;
            case Enemies.Zombie:
                print("Enemy Zombie");
                break;
            case Enemies.Wizzard:
                print("Enemy Wizard");
                break;
            default:
                print("Unknown Enemy");
                break;
        }
    }

}