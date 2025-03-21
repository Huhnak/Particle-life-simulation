using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GeneralSettings", menuName = "Scriptable Objects/GeneralSettings")]
public class GeneralSettings : ScriptableObject
{
    [SerializeField] public int width;
    [SerializeField] public int height;
    [SerializeField] public List<ParticleType> particleTypes;
}
