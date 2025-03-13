using Assets.Scripts;
using UnityEngine;


struct Particle
{
    public Vector3 position;
    public Vector3 velocity;
    public int color;
};
public class ParicleShaderScript : MonoBehaviour
{
    [SerializeField] private ComputeShader _computeShader;
    [SerializeField] private int _particleCount;
    [SerializeField] private GameObject particlePrefab;
    [SerializeField] private float _attractionScale;
    [SerializeField] private float _friction;
    [SerializeField] private float _r_max;
    [SerializeField] private float resolution;
    [SerializeField] private ParticleInteractionHandler _particleInteractionHandler;
    [SerializeField] private GeneralSettings _generalSettings;
    [SerializeField] private float[,] InteractionMaxtix { get { return _particleInteractionHandler.Matrix; } }
    private float[] _interactionMaxtix1d;

    private Particle[] data;
    private GameObject[] particlesGameObjects;

    void Start()
    {
        data = new Particle[_particleCount];
        for (int i = 0; i < _particleCount; i++)
        {
            data[i] = new Particle();
            data[i].position = new Vector3(Random.Range(0, resolution), Random.Range(0, resolution), 0);
            data[i].color = Random.Range(0, _generalSettings.particleTypes.Count);
            data[i].velocity = Vector3.zero;
        }
        _interactionMaxtix1d = new float[_generalSettings.particleTypes.Count * _generalSettings.particleTypes.Count];
        CreateParticles();
    }
    private void CreateParticles()
    {
        particlesGameObjects = new GameObject[_particleCount];
        var parent = gameObject.transform;
        for (int i = 0; i < _particleCount; i++)
        {
            particlesGameObjects[i] = Instantiate(particlePrefab, parent);
            particlesGameObjects[i].GetComponent<SpriteRenderer>().color = _generalSettings.particleTypes[data[i].color].Color;
        }
    }

    void Update()
    {
        System.Buffer.BlockCopy(InteractionMaxtix, 0, _interactionMaxtix1d, 0, sizeof(float) * _generalSettings.particleTypes.Count * _generalSettings.particleTypes.Count);
        int bufferSize = sizeof(float) * 3 + sizeof(float) * 3 + sizeof(int);
        ComputeBuffer particlesBuffer = new ComputeBuffer(data.Length, bufferSize);
        particlesBuffer.SetData(data);
        ComputeBuffer interactionMatrixBuffer = new ComputeBuffer(_interactionMaxtix1d.Length, sizeof(float));
        interactionMatrixBuffer.SetData(_interactionMaxtix1d);

        _computeShader.SetBuffer(0, "particles", particlesBuffer);
        _computeShader.SetFloat("resolution", resolution);
        _computeShader.SetInt("particleCount", _particleCount);
        _computeShader.SetFloat("attractionScale", _attractionScale);
        _computeShader.SetFloat("friction", _friction);
        _computeShader.SetFloat("r_max", _r_max);
        _computeShader.SetFloat("deltaTime", Time.deltaTime);
        _computeShader.SetInt("particleTypesCount", _generalSettings.particleTypes.Count);
        //_interactionMaxtix1d = new float[_generalSettings.particleTypes.Count * _generalSettings.particleTypes.Count];
        _computeShader.SetBuffer(0, "particleInteractions", interactionMatrixBuffer);

        _computeShader.Dispatch(0, data.Length, 1, 1);

        particlesBuffer.GetData(data);

        for (int i = 0; i < _particleCount; i++)
        {
            particlesGameObjects[i].transform.position = data[i].position;
            //particlesGameObjects[i].GetComponent<MeshRenderer>().material.SetColor("_Color", data[i].color);
        }
        particlesBuffer.Dispose();
        interactionMatrixBuffer.Dispose();
    }
}
