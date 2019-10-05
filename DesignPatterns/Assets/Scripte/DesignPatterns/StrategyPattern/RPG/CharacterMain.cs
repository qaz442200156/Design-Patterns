using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMain : MonoBehaviour
{
    CharacterInfo NpcA;
    CharacterInfo NpcB;
    // Start is called before the first frame update
    void Start()
    {
        NpcA = new CharacterInfo();
        NpcA.Init(new farmer(),new Hand());
        NpcA.CheckInfo();
        NpcB = new CharacterInfo();
        NpcB.Init(new fighter(),new Swort());
        NpcB.CheckInfo();

        
    }

    private void Update()
    {
        if (NpcA.Hp > 0 && NpcB.Hp > 0)
        {
            NpcA.fight(NpcB, true);
            NpcB.fight(NpcA, true);
        }
        else
        {
            enabled = false;
        }
    }
}
