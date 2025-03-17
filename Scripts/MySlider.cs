using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MySlider : MonoBehaviour
{
    private TextMeshProUGUI _labelText;
    private TextMeshProUGUI _valueText;
    private Slider _slider;
    private PropertyInfo _propertyInfo;
    [SerializeField, TextArea] private string _label;
    [SerializeField] private UnityEvent<float> _onValueChange;
    [SerializeField] private float _maxValue;
    [SerializeField] private float _minValue;
    [SerializeField] private bool _wholeNumber;

    public void Start()
    {
        _slider = gameObject.transform.Find("Slider").GetComponent<Slider>();
        _slider.onValueChanged.AddListener(OnValueChanged);
        _slider.maxValue = _maxValue;
        _slider.minValue = _minValue;
        _slider.wholeNumbers = _wholeNumber;

        _labelText = gameObject.transform.Find("Label").GetComponent<TextMeshProUGUI>();
        _labelText.text = _label;

        _valueText = gameObject.transform.Find("Value").GetComponent<TextMeshProUGUI>();
        _propertyInfo = _onValueChange.GetPersistentTarget(0).GetType().GetProperty(_onValueChange.GetPersistentMethodName(0).Replace("set_", "").Replace("get_", ""));
        _valueText.text = _propertyInfo.GetValue(_onValueChange.GetPersistentTarget(0)).ToString();
        _slider.value = float.Parse(_valueText.text);
    }
    private void OnValueChanged(float value)
    {
        _valueText.text = value.ToString();
        _propertyInfo.SetValue(_onValueChange.GetPersistentTarget(0), value);
        //_onValueChange.Invoke(value);
    }
}
