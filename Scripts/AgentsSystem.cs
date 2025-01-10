using System.Collections.Generic;
using UnityEngine;

public class AgentsSystem : MonoBehaviour
{
    [SerializeField] private int agentCount = 0;
    private List<GameObject> agents = new List<GameObject>();
    [SerializeField] private List<ParticleType> particleTypes;
    [SerializeField] private GameObject go_Agent;
    [SerializeField] private Transform parent;
    [SerializeField] private float attractionScale = 2;
    [SerializeField] private float r_max = 1;
    [SerializeField][Range(0, 1)] private float friction = 0.05f;
    [SerializeField] private GeneralSettings settings;
    private float[] positionsX;
    private float[] positionsY;
    private float[] velocitiesX;
    private float[] velocitiesY;
    private int[] colors;
    private float[,] interactionMaxtix;
    void Start()
    {
        colors = new int[agentCount];
        positionsX = new float[agentCount];
        positionsY = new float[agentCount];
        velocitiesX = new float[agentCount];
        velocitiesY = new float[agentCount];
        for (int i = 0; i < agentCount; i++)
        {
            agents.Add(Instantiate(go_Agent, parent));
            positionsX[i] = Random.value * 10 - 5;
            positionsY[i] = Random.value * 10 - 5;
            velocitiesX[i] = 0;
            velocitiesY[i] = 0;
            colors[i] = Random.Range(0, 2);
            agents[i].GetComponent<Agent>().Type = particleTypes[colors[i]];
        }
        interactionMaxtix = new float[2, 2];
        interactionMaxtix[0, 0] = 1;
        interactionMaxtix[0, 1] = -1;
        interactionMaxtix[1, 0] = 1;
        interactionMaxtix[1, 1] = 1;
    }

    void FixedUpdate()
    {
        for (int i = 0; i < agents.Count; i++)
        {
            float phantomPositionX;
            float phantomPositionY;
            float totalForceX = 0;
            float totalForceY = 0;
            float rx = 0;
            float ry = 0;
            float r = 0;
            float f = 0;
            for (int j = 0; j < agents.Count; j++)
            {
                phantomPositionX = positionsX[i];
                phantomPositionY = positionsY[i];
                if (i == j) continue;
                if (Mathf.Abs(positionsX[i] - positionsX[j]) > settings.width / 2)
                    phantomPositionX = positionsX[i] - settings.width * Mathf.Sign(positionsX[i] - positionsX[j]);
                if (Mathf.Abs(positionsY[i] - positionsY[j]) > settings.height / 2)
                    phantomPositionY = positionsY[i] - settings.height * Mathf.Sign(positionsY[i] - positionsY[j]);
                rx = positionsX[j] - phantomPositionX;
                ry = positionsY[j] - phantomPositionY;
                r = Mathf.Sqrt(rx * rx + ry * ry);
                if (r > 0 && r < r_max)
                {
                    f = Helper.Particle(r / r_max, interactionMaxtix[colors[i], colors[j]]);
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

            agents[i].transform.position = new Vector3(positionsX[i], positionsY[i], 0);
        }

    }
}
