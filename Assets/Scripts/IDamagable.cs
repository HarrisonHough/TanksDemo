using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable <T>
{
    void Damage(T damageTaken, GameObject attacker);
}
