using UnityEngine;

public class GizmosDrawer : MonoBehaviour
{
    [SerializeField] private GeneralSettings _settings;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnDrawGizmos()
    {
        var a = new Vector3[4];
        a[0] = new Vector3(0, 0, 0);
        a[1] = new Vector3(0, _settings.height, 0);
        a[2] = new Vector3(_settings.width, _settings.height, 0);
        a[3] = new Vector3(_settings.width, 0, 0);
        Gizmos.DrawLineStrip(new System.ReadOnlySpan<Vector3>(a), true);
    }
}
