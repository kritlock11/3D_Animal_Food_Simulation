#pragma warning disable 0649
using System;
using UnityEngine;
using UnityEngine.UI;

public class Ui : MonoBehaviour
{
    [Header("M1")]
    [Space(10)]
    [SerializeField] private RectTransform _menu1;
    [SerializeField] private Button _continue;
    [SerializeField] private Button _new;

    [Header("M2")]
    [Space(10)]
    [SerializeField] private RectTransform _menu2;
    [SerializeField] private Text _n;
    [SerializeField] private Text _m;
    [SerializeField] private Text _v;
    [SerializeField] private Slider _nSlider;
    [SerializeField] private Slider _mSlider;
    [SerializeField] private Slider _vSlider;

    [Header("Start")]
    [Space(10)]
    [SerializeField] private Button _start;

    [Header("Save")]
    [Space(10)]
    [SerializeField] private RectTransform _saveTransform;
    [SerializeField] private Button _save;

    [Header("GameSpeed")]
    [Space(10)]
    [SerializeField] private RectTransform _timeScale;
    [SerializeField] private Text _ts;
    [SerializeField] private Slider _tsSlider;

    [Header("Data")]
    [Space(10)]
    [SerializeField] private float _nMin;
    [SerializeField] private float _nMax;

    [SerializeField] private float _mMin;

    [SerializeField] private float _vMin;
    [SerializeField] private float _vMax;

    [SerializeField] private float _tsMin;
    [SerializeField] private float _tsMax;
    [SerializeField] private float _tsDefault;

    public Action OnStart;
    public Action OnSave;
    public Action OnLoad;

    private int _N;
    private int _M;
    private int _V;

    public int N
    {
        get => _N;
        private set => _N = value;
    }
    public int M
    {
        get => _M;
        private set => _M = value;
    }
    public int V
    {
        get => _V;
        private set => _V = value;
    }


    private void Awake()
    {
        _continue.gameObject.SetActive(SaveLoad.FileExist("data"));

        _continue.onClick.AddListener(LoadGame);
        _new.onClick.AddListener(OpenM2);

        _start.onClick.AddListener(StartGame);
        _save.onClick.AddListener(SaveGame);

        _nSlider.onValueChanged.AddListener(OnNChanged);
        _mSlider.onValueChanged.AddListener(OnMChanged);
        _vSlider.onValueChanged.AddListener(OnVChanged);

        _tsSlider.onValueChanged.AddListener(OnTsChanged);

        SetM2Sliders();
    }

    public void StartGame()
    {
        _menu2.gameObject.SetActive(false);
        _timeScale.gameObject.SetActive(true);
        _saveTransform.gameObject.SetActive(true);
        SetGameSpeedSlider();

        OnStart?.Invoke();
    }

    private void SaveGame()
    {
        OnSave?.Invoke();
    }

    private void LoadGame()
    {
        _menu1.gameObject.SetActive(false);
        _timeScale.gameObject.SetActive(true);
        _saveTransform.gameObject.SetActive(true);
        SetGameSpeedSlider();

        OnLoad?.Invoke();
    }

    private void OnNChanged(float f)
    {
        _N = (int)_nSlider.value;
        _mSlider.maxValue = Mathf.Floor(_nSlider.value * _nSlider.value / 2);
        _n.text = _N.ToString();
    }

    private void OnMChanged(float f)
    {
        _M = (int)_mSlider.value;
        _m.text = _M.ToString();
    }

    private void OnVChanged(float f)
    {
        _V = (int)_vSlider.value;
        _v.text = _V.ToString();
    }

    private void OnTsChanged(float f)
    {
        Time.timeScale = _tsSlider.value;
        _ts.text = Time.timeScale.ToString();
    }

    private void SetM2Sliders()
    {
        SetNSlider();
        SetMSlider();
        SetVSlider();
    }

    private void SetNSlider()
    {
        _nSlider.wholeNumbers = true;
        _nSlider.minValue = _nMin;
        _nSlider.maxValue = _nMax;
    }

    private void SetMSlider()
    {
        _mSlider.wholeNumbers = true;
        _mSlider.minValue = _mMin;
        _mSlider.maxValue = Mathf.Floor(_nSlider.value * _nSlider.value / 2);
    }

    private void SetVSlider()
    {
        _vSlider.wholeNumbers = true;
        _vSlider.minValue = _vMin;
        _vSlider.maxValue = _vMax;
    }

    public void SetGameSpeedSlider()
    {
        _tsSlider.wholeNumbers = true;
        _tsSlider.minValue = _tsMin;
        _tsSlider.maxValue = _tsMax;
        _tsSlider.value = _tsDefault;
    }

    private void OpenM2()
    {
        _menu2.gameObject.SetActive(true);
        _menu1.gameObject.SetActive(false);
    }

}

