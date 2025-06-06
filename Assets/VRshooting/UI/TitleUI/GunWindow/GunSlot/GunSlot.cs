using UnityEngine;
using UnityEngine.UI;

public class GunSlot : MonoBehaviour
{
    public bool interact;
    [SerializeField] private Text _NameText;
    [SerializeField] private Image _IconImage;

    private GunData _Data;
    public GunData Data { get => _Data; }
    private Button _Button;

    private void Start()
    {
        _Button = GetComponent<Button>();
    }

    private void Update()
    {
        _Button.interactable = interact;
    }

    public void SetData(GunData data)
    {
        _Data = data;
        if(_IconImage) _IconImage.sprite = data.Icon;
        if(_NameText) _NameText.text = data.Name;
    }

    public void SetSelectGunData()
    {
        GunWindow windowCs = TitleUI.instance.GunWindow;

        windowCs.BsetData(Data);
    }
}
