////using Assets.Scripts;
////using System;
////using UnityEngine;



////public class AgentsSystem : MonoBehaviour
////{
////    [SerializeField] private int _particleCount = 0;
////    [SerializeField] private int _maxParticleCount = 5000;
////    [SerializeField] private float attractionScale = 2;
////    [SerializeField] private float r_max = 1;
////    [SerializeField][Range(0, 1)] private float friction = 0.05f;
////    [SerializeField] private GeneralSettings _settings;
////    [SerializeField] private ParticleSystem _particleSystem;
////    [SerializeField] private ParticleInteractionHandler _particleInteractionHandler;
////    private ParticleSystem.Particle[] _particles;
////    [HideInInspector] public float[] positionsX;
////    [HideInInspector] public float[] positionsY;
////    [HideInInspector] public float[] velocitiesX;
////    [HideInInspector] public float[] velocitiesY;
////    [HideInInspector] public int[] colors;
////    private float[,] InteractionMaxtix { get { return _particleInteractionHandler.Matrix; } }
////    public int ParticleCount { get { return _particleCount; } }


////    private float force(float x, float t)
////    {
////        const float beta = 0.3f;
////        if (x < beta)
////            return x / beta - 1;
////        if (beta < x && x < 1)
////        {
////            float _ = 2 * x - 1 - beta;
////            return t * (1 - (_ < 0 ? -_ : _) / (1 - beta));
////        }
////        return 0;
////    }

////    void Start()
////    {
////        if (_maxParticleCount < _particleCount) _maxParticleCount = _particleCount;
////        _particles = new ParticleSystem.Particle[_maxParticleCount];
////        colors = new int[_maxParticleCount];
////        positionsX = new float[_maxParticleCount];
////        positionsY = new float[_maxParticleCount];
////        velocitiesX = new float[_maxParticleCount];
////        velocitiesY = new float[_maxParticleCount];
////        for (int i = 0; i < _maxParticleCount; i++)
////        {
////            positionsX[i] = UnityEngine.Random.value * 10 - 5;
////            positionsY[i] = UnityEngine.Random.value * 10 - 5;
////            velocitiesX[i] = 0;
////            velocitiesY[i] = 0;
////            colors[i] = UnityEngine.Random.Range(0, _settings.particleTypes.Count);
////        }

////        _particleSystem.Emit(_particleCount);
////        for (int i = 0; i < _maxParticleCount; i++)
////        {
////            _particles[i].startSize = 0.05f;
////            _particles[i].startColor = _settings.particleTypes[colors[i]].Color;

////        }

////    }
////    public void AgentCountChange(System.Single value)
////    {
////        if ((int)value > _maxParticleCount) throw new ArgumentException($"value:{(int)value} can not be greater than maxParticleCount: {_maxParticleCount}");
////        this._particleCount = (int)value;
////        _particleSystem.Emit(_particleCount);
////    }
////    void FixedUpdate()
////    {
////        for (int i = 0; i < _particleCount; i++)
////        {


////            float phantomPositionX;
////            float phantomPositionY;
////            float totalForceX = 0;
////            float totalForceY = 0;
////            float rx = 0;
////            float ry = 0;
////            float r = 0;
////            float f = 0;


////            for (int j = 0; j < _particleCount; j++)
////            {
////                phantomPositionX = positionsX[i];
////                phantomPositionY = positionsY[i];
////                float _x = positionsX[j] - positionsX[i];
////                float _y = positionsY[j] - positionsY[i];
////                if (i == j) continue;
////                if (positionsX[i] > positionsX[j] && -_x > _settings.width / 2)
////                    phantomPositionX -= _settings.width;
////                else if (_x > _settings.width / 2)
////                    phantomPositionX += _settings.width;
////                if (positionsY[i] > positionsY[j] && -_y > _settings.height / 2)
////                    phantomPositionY -= _settings.height;
////                else if (_y > _settings.height / 2)
////                    phantomPositionY += _settings.height;
////                rx = positionsX[j] - phantomPositionX;
////                ry = positionsY[j] - phantomPositionY;
////                r = Mathf.Sqrt(rx * rx + ry * ry);
////                if (r > 0 && r < r_max)
////                {
////                    f = force(r / r_max, InteractionMaxtix[colors[i], colors[j]]);
////                    totalForceX += rx / r * f;
////                    totalForceY += ry / r * f;
////                }
////            }

////            totalForceX *= r_max * attractionScale;
////            totalForceY *= r_max * attractionScale;

////            velocitiesX[i] *= friction;
////            velocitiesY[i] *= friction;

////            velocitiesX[i] += totalForceX * Time.deltaTime;
////            velocitiesY[i] += totalForceY * Time.deltaTime;

////            positionsX[i] += velocitiesX[i] * Time.deltaTime;
////            positionsY[i] += velocitiesY[i] * Time.deltaTime;

////            // Board teleporting.
////            if (positionsX[i] < 0 || positionsX[i] > _settings.width)
////                positionsX[i] = (positionsX[i] + _settings.width) % _settings.width;
////            if (positionsY[i] < 0 || positionsY[i] > _settings.height)
////                positionsY[i] = (positionsY[i] + _settings.height) % _settings.height;

////            _particles[i].position = new Vector3(positionsX[i], positionsY[i], 0);




////        }
////        _particleSystem.SetParticles(_particles, _particleCount);


////    }

////}

//using UnityEngine;



//public class AgentsSystem : MonoBehaviour
//{
//    [SerializeField] private int _particleCount = 0;
//    [SerializeField] private int _maxParticleCount = 5000;
//    [SerializeField] private float attractionScale = 2;
//    [SerializeField] private float r_max = 1;
//    [SerializeField][Range(0, 1)] private float friction = 0.05f;
//    [SerializeField] private GeneralSettings _settings;
//    [SerializeField] private ParticleSystem _particleSystem;
//    private ParticleSystem.Particle[] _particles;
//    [HideInInspector] public float[] positionsX;
//    [HideInInspector] public float[] positionsY;
//    [HideInInspector] public float[] velocitiesX;
//    [HideInInspector] public float[] velocitiesY;
//    [HideInInspector] public int[] colors;
//    public int ParticleCount { get { return _particleCount; } }



//}
