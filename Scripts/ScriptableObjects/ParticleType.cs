using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ParticleType", menuName = "Scriptable Objects/ParticleType")]
public class ParticleType : ScriptableObject
{
    [SerializeField] public Color Color = Color.white;
    [SerializeField] public List<ParticleType> interactionKey;
    [SerializeField] public List<float> interactionValue;
}
