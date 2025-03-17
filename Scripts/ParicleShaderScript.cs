using Assets.Scripts;
using UnityEngine;

struct ParticleData
{
    public Vector3 position;
    public Vector3 velocity;
    public int color;
};
public class ParicleShaderScript : MonoBehaviour
{
    [SerializeField] private ComputeShader _computeShader;
    public float ParticleCount { get { return _particleCount; } set { _particleCount = Mathf.FloorToInt(value); } }
    [SerializeField] private int _particleCount;
    public float AttractionScale { get { return _attractionScale; } set { _attractionScale = value; } }
    [SerializeField] private float _attractionScale;
    public float Friction { get { return _friction; } set { _friction = value; } }
    [SerializeField] private float _friction;
    public float RMax { get { return _r_max; } set { _r_max = value; } }
    [SerializeField] private float _r_max;
    public float RMin { get { return _r_min; } set { _r_min = value; } }
    [SerializeField] private float _r_min;
    public float TMin { get { return _t_min; } set { _t_min = value; } }
    [SerializeField] private float _t_min;
    [SerializeField] private float resolution;
    [SerializeField] private ParticleInteractionHandler _particleInteractionHandler;
    [SerializeField] private GeneralSettings _generalSettings;
    [SerializeField] private ParticleSystem _particleSystem;
    private ParticleSystem.Particle[] _particles;
    [SerializeField] private float[,] InteractionMaxtix { get { return _particleInteractionHandler.Matrix; } }
    private float[] _interactionMaxtix1d;

    private ParticleData[] data;
    private GameObject[] particlesGameObjects;

    void Start()
    {
        InitializeParticleData();
        _particles = new ParticleSystem.Particle[_particleCount];
        for (int i = 0; i < _particleCount; i++)
        {
            _particles[i].startSize = 0.1f;
            _particles[i].startColor = _generalSettings.particleTypes[data[i].color].Color;
            _particles[i].startLifetime = 0;
            _particles[i].position = new Vector3((float)(Random.value * 0.5), (float)(Random.value * 0.5), (float)(Random.value * 0.5));
        }

        _particleSystem.SetParticles(_particles, _particleCount);

        _interactionMaxtix1d = new float[_generalSettings.particleTypes.Count * _generalSettings.particleTypes.Count];
    }
    private void InitializeParticleData()
    {
        data = new ParticleData[_particleCount];
        for (int i = 0; i < _particleCount; i++)
        {
            data[i] = new ParticleData();
            data[i].position = new Vector3(Random.Range(0, resolution), Random.Range(0, resolution), 0);
            data[i].color = Random.Range(0, _generalSettings.particleTypes.Count);
            data[i].velocity = Vector3.zero;
        }
    }

    public void Update()
    {
        _particleSystem.SetParticles(_particles, _particleCount);

        UpdateParticles();
    }
    private void UpdateParticles()
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
        _computeShader.SetFloat("r_min", _r_min);
        _computeShader.SetFloat("t_min", _t_min);
        _computeShader.SetFloat("deltaTime", Time.deltaTime);
        _computeShader.SetInt("particleTypesCount", _generalSettings.particleTypes.Count);
        //_interactionMaxtix1d = new float[_generalSettings.particleTypes.Count * _generalSettings.particleTypes.Count];
        _computeShader.SetBuffer(0, "particleInteractions", interactionMatrixBuffer);

        int threadGroupsX = Mathf.CeilToInt((float)data.Length / 64);
        _computeShader.Dispatch(0, threadGroupsX, 1, 1);

        particlesBuffer.GetData(data);

        for (int i = 0; i < _particleCount; i++)
        {
            _particles[i].position = data[i].position;
        }
        particlesBuffer.Dispose();
        interactionMatrixBuffer.Dispose();
    }

}
