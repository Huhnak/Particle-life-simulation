using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Assets.Scripts
{
    class ParticleInteractionHandler : MonoBehaviour
    {
        [SerializeField] private GameObject _onePropertyPrefab;
        [SerializeField] private GameObject _onePropertyParticlePrefab;
        [SerializeField] private GeneralSettings _settings;
        public int Size { get; private set; }
        public float[,] Matrix { get; private set; } // left - origin, top - target
        private GameObject[,] _properties;

        UIDocument document;
        public void Start()
        {
            var _ = _settings.particleTypes.Count;
            Size = _;
            Matrix = new float[_, _];
            _properties = new GameObject[_, _];

            CreateUI();
            GenerateSnakeParameters();
            UpdateUI();
        }
        private void CreateUI()
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    int _i = i;
                    int _j = j;
                    _properties[i, j] = Instantiate(_onePropertyPrefab, this.transform);
                    _properties[i, j].GetComponent<OneProperty>().Value = new OneProperty.value(i, j);
                    var _x = _properties[i, j].GetComponent<RectTransform>().sizeDelta.x;
                    var _y = _properties[i, j].GetComponent<RectTransform>().sizeDelta.y;
                    _properties[i, j].transform.localPosition = new Vector3(j * _x + _x * 1.5f, -i * _y - _y * 1.5f);
                    var _ = _properties[i, j].GetComponent<TMP_InputField>();
                    _.onValueChanged.AddListener((string s) => OnValueChanged(s, _i, _j));
                    // Сохн аряет скоуп и помнит значения i=4 и j=4. 
                    if (i == 0)
                    {
                        var particleColorImage = Instantiate(_onePropertyParticlePrefab, transform);
                        particleColorImage.transform.localPosition = new Vector3(j * _x + _x * 1.5f, -_y / 2);
                        particleColorImage.GetComponent<UnityEngine.UI.Image>().color = _settings.particleTypes[j].Color;
                    }
                    if (j == 0)
                    {
                        var particleColorImage = Instantiate(_onePropertyParticlePrefab, transform);
                        particleColorImage.transform.localPosition = new Vector3(j * _x + _x * 1.5f, -_y / 2);
                        particleColorImage.transform.localPosition = new Vector3(_x / 2, -i * _y - _y * 1.5f);
                        particleColorImage.GetComponent<UnityEngine.UI.Image>().color = _settings.particleTypes[i].Color;
                    }
                }
            }
        }
        public void OnValueChanged(string s, int i, int j)
        {
            float val;
            if (float.TryParse(s, out val))
                Matrix[i, j] = val;
            UpdateUI(i, j);


        }
        private void UpdateUI()
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    var _ = _properties[i, j].GetComponent<TMP_InputField>();
                    _.text = Matrix[i, j].ToString();
                    ColorBlock temp = _.colors;
                    Color c = Color.HSVToRGB(interpolate(Matrix[i, j]), 1, 1);
                    temp.normalColor = c;
                    temp.highlightedColor = c;
                    temp.pressedColor = c;
                    temp.selectedColor = Color.HSVToRGB(interpolate(Matrix[i, j]), 0.7f, 1);
                    _.colors = temp;
                }
            }
        }
        private void UpdateUI(int i, int j)
        {
            var _ = _properties[i, j].GetComponent<TMP_InputField>();
            //_.text = Matrix[i, j].ToString();
            ColorBlock temp = _.colors;
            Color c = Color.HSVToRGB(interpolate(Matrix[i, j]), 1, 1);
            temp.normalColor = c;
            temp.highlightedColor = c;
            temp.pressedColor = c;
            temp.selectedColor = Color.HSVToRGB(interpolate(Matrix[i, j]), 0.7f, 1);
            _.colors = temp;
        }
        public void AltGenerateRandomParameters()
        {
            GenerateRandomParameters();
            UpdateUI();
        }
        public void GenerateRandomParameters(float min = -1, float max = 1)
        {

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    Matrix[i, j] = Random.Range(min, max);
                }
            }
        }
        public void AltGenerateSnakeParameters()
        {
            GenerateSnakeParameters();
            UpdateUI();
        }
        public void GenerateSnakeParameters(float self = 1, float next = 0.2f)
        {
            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                    Matrix[i, j] = 0;

            for (int i = 0; i < Size; i++)
            {
                Matrix[i, i] = self;
            }
            for (int i = 0; i < Size - 1; i++)
            {
                Matrix[i, i + 1] = next;
            }
        }
        public void GenerateBlankParameters()
        {
            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                    Matrix[i, j] = 0;
            UpdateUI();
        }
        private float interpolate(float val, float in_min = -1, float in_max = 1, float out_min = 0, float out_max = 0.3f)
        {
            val = Mathf.Max(in_min, val);
            val = Mathf.Min(in_max, val);
            float t = (val - in_min) / (in_max - in_min);
            return out_min + t * (out_max - out_min);
        }
    }

}
