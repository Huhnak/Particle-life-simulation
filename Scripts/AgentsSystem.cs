using System.Collections.Generic;
using UnityEngine;

public class AgentsSystem : MonoBehaviour
{
    [SerializeField] private int particlesCount = 0;
    [SerializeField] private List<ParticleType> particleTypes;
    [SerializeField] private float attractionScale = 2;
    [SerializeField] private float r_max = 1;
    [SerializeField][Range(0, 1)] private float friction = 0.05f;
    [SerializeField] private GeneralSettings settings;
    [SerializeField] private ParticleSystem _particleSystem;
    private ParticleSystem.Particle[] _particles;
    private float[] positionsX;
    private float[] positionsY;
    private float[] velocitiesX;
    private float[] velocitiesY;
    private int[] colors;
    private float[,] interactionMaxtix;

    private float force(float x, float t)
    {
        const float beta = 0.3f;
        if (x < beta)
            return x / beta - 1;
        if (beta < x && x < 1)
        {
            float _ = 2 * x - 1 - beta;
            return t * (1 - (_ < 0 ? -_ : _) / (1 - beta));
        }
        return 0;
    }
    void Start()
    {
        _particles = new ParticleSystem.Particle[particlesCount];
        colors = new int[particlesCount];
        positionsX = new float[particlesCount];
        positionsY = new float[particlesCount];
        velocitiesX = new float[particlesCount];
        velocitiesY = new float[particlesCount];
        for (int i = 0; i < particlesCount; i++)
        {
            positionsX[i] = Random.value * 10 - 5;
            positionsY[i] = Random.value * 10 - 5;
            velocitiesX[i] = 0;
            velocitiesY[i] = 0;
            colors[i] = Random.Range(0, 4);
        }
        interactionMaxtix = new float[4, 4] // left - origin, top - target
        {
            {1,0.2f,0,0 },
            {0,1,0.2f,0 },
            {0,0,1,0.2f },
            {0,0,0,1 },
        };
        _particleSystem.Emit(particlesCount);
        for (int i = 0; i < particlesCount; i++)
        {
            _particles[i].startSize = 0.3f;
            _particles[i].startColor = particleTypes[colors[i]].Color;

        }

    }

    void FixedUpdate()
    {
        for (int i = 0; i < particlesCount; i++)
        {
            float phantomPositionX;
            float phantomPositionY;
            float totalForceX = 0;
            float totalForceY = 0;
            float rx = 0;
            float ry = 0;
            float r = 0;
            float f = 0;
            for (int j = 0; j < particlesCount; j++)
            {
                phantomPositionX = positionsX[i];
                phantomPositionY = positionsY[i];
                float _x = positionsX[i] - positionsX[j];
                float _y = positionsY[i] - positionsY[j];
                if (i == j) continue;
                if ((_x < 0 ? -_x : _x) > settings.width / 2)
                    phantomPositionX = positionsX[i] - settings.width * Mathf.Sign(positionsX[i] - positionsX[j]);
                if ((_y < 0 ? -_y : _y) > settings.height / 2)
                    phantomPositionY = positionsY[i] - settings.height * Mathf.Sign(positionsY[i] - positionsY[j]);
                rx = positionsX[j] - phantomPositionX;
                ry = positionsY[j] - phantomPositionY;
                r = Mathf.Sqrt(rx * rx + ry * ry);
                if (r > 0 && r < r_max)
                {
                    f = force(r / r_max, interactionMaxtix[colors[i], colors[j]]);
                    totalForceX += rx / r * f;
                    totalForceY += ry / r * f;
                }
            }
            totalForceX *= r_max * attractionScale;
            totalForceY *= r_max * attractionScale;

            velocitiesX[i] *= friction;
            velocitiesY[i] *= friction;

            velocitiesX[i] += totalForceX * Time.deltaTime;
            velocitiesY[i] += totalForceY * Time.deltaTime;

            positionsX[i] += velocitiesX[i] * Time.deltaTime;
            positionsY[i] += velocitiesY[i] * Time.deltaTime;

            // Board teleporting.
            if (positionsX[i] < 0 || positionsX[i] > settings.width)
                positionsX[i] = (positionsX[i] + settings.width) % settings.width;
            if (positionsY[i] < 0 || positionsY[i] > settings.height)
                positionsY[i] = (positionsY[i] + settings.height) % settings.height;

            _particles[i].position = new Vector3(positionsX[i], positionsY[i], 0);




        }
        _particleSystem.SetParticles(_particles, particlesCount);





    }
}
