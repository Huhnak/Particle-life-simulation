//using UnityEngine;

//public class MouseHandler : MonoBehaviour
//{
//    [SerializeField] private AgentsSystem _agentsSystem;
//    [SerializeField] private float power;
//    [SerializeField][Range(0, 5)] private float radius;
//    [SerializeField] private ParticleSystem _particles;
//    private float force(float distance)
//    {
//        return -(distance * (Mathf.Sqrt(power) / radius)) + power;
//    }
//    private void AttractParticles(Vector3 mousePos)
//    {
//        int particleCount = _agentsSystem.ParticleCount;
//        for (int i = 0; i < particleCount; i++)
//        {
//            float posX = _agentsSystem.positionsX[i];
//            float posY = _agentsSystem.positionsY[i];
//            Vector3 vec = new Vector3(posX - mousePos.x, posY - mousePos.y);
//            if (vec.magnitude > radius) continue;
//            float f = force(vec.magnitude);
//            vec = vec.normalized * f;
//            _agentsSystem.velocitiesX[i] -= vec.x;
//            _agentsSystem.velocitiesY[i] -= vec.y;
//        }
//    }
//    private void RepelParticles(Vector3 mousePos)
//    {
//        int particleCount = _agentsSystem.ParticleCount;
//        for (int i = 0; i < particleCount; i++)
//        {
//            float posX = _agentsSystem.positionsX[i];
//            float posY = _agentsSystem.positionsY[i];
//            Vector3 vec = new Vector3(posX - mousePos.x, posY - mousePos.y);
//            if (vec.magnitude > radius) continue;
//            float f = force(vec.magnitude);
//            vec = vec.normalized * f;
//            _agentsSystem.velocitiesX[i] += vec.x;
//            _agentsSystem.velocitiesY[i] += vec.y;
//        }
//    }
//    private void HandleParticles(Vector3 mousePos)
//    {
//        _particles.transform.position = mousePos;
//        var s = _particles.shape;
//        s.radius = radius;
//    }
//    private void EnableParicles()
//    {
//        _particles.enableEmission = true;
//    }
//    private void DisableParicles()
//    {
//        _particles.enableEmission = false;

//    }
//    public void Update()
//    {
//        if (Input.GetMouseButton(0))
//        {
//            var worldMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//            worldMouse.z = 0;
//            AttractParticles(worldMouse);
//            HandleParticles(worldMouse);
//        }
//        else if (Input.GetMouseButton(1))
//        {
//            var worldMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//            worldMouse.z = 0;
//            RepelParticles(worldMouse);
//            HandleParticles(worldMouse);
//        }
//        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
//            EnableParicles();
//        else if ((Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1)) &&
//                 !(Input.GetMouseButton(0) && Input.GetMouseButton(1)))
//            DisableParicles();
//    }
//}
