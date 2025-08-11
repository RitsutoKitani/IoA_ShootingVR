using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class KeyBoard : MonoBehaviour
{
    [SerializeField] private int _TextLen = 10;
    [SerializeField, ReadOnly] private string _TextData;
    public string TextData { get => _TextData; }

    [Space(30)]
    [SerializeField] private Text _TextUI;

    #region//文字変換用ディクショナリ
    public static readonly Dictionary<char, char[]> SmallKana = new Dictionary<char, char[]>
    {
        {'ア', new char[] { 'ア', 'ァ' } },
        {'イ', new char[] { 'イ', 'ィ' } },
        {'ウ', new char[] { 'ウ', 'ゥ' } },
        {'エ', new char[] { 'エ', 'ェ' } },
        {'オ', new char[] { 'オ', 'ォ' } },
        {'ヤ', new char[] { 'ヤ', 'ャ' } },
        {'ユ', new char[] { 'ユ', 'ュ' } },
        {'ヨ', new char[] { 'ヨ', 'ョ' } },
        {'ツ', new char[] { 'ツ', 'ッ' } },
        {'ワ', new char[] { 'ワ', 'ヮ' } },
    };

    public static readonly Dictionary<char, char[]> Dakuten = new Dictionary<char, char[]>
    {
        { 'カ', new char[] { 'カ', 'ガ' } },
        { 'キ', new char[] { 'キ', 'ギ' } },
        { 'ク', new char[] { 'ク', 'グ' } },
        { 'ケ', new char[] { 'ケ', 'ゲ' } },
        { 'コ', new char[] { 'コ', 'ゴ' } },
        { 'サ', new char[] { 'サ', 'ザ' } },
        { 'シ', new char[] { 'シ', 'ジ' } },
        { 'ス', new char[] { 'ス', 'ズ' } },
        { 'セ', new char[] { 'セ', 'ゼ' } },
        { 'ソ', new char[] { 'ソ', 'ゾ' } },
        { 'タ', new char[] { 'タ', 'ダ' } },
        { 'チ', new char[] { 'チ', 'ヂ' } },
        { 'ツ', new char[] { 'ツ', 'ヅ' } },
        { 'テ', new char[] { 'テ', 'デ' } },
        { 'ト', new char[] { 'ト', 'ド' } },
        { 'ハ', new char[] { 'ハ', 'バ', 'パ' } },
        { 'ヒ', new char[] { 'ヒ', 'ビ', 'ピ' } },
        { 'フ', new char[] { 'フ', 'ブ', 'プ' } },
        { 'ヘ', new char[] { 'ヘ', 'ベ', 'ペ' } },
        { 'ホ', new char[] { 'ホ', 'ボ', 'ポ' } }
    };
    #endregion

    public void DataReset(int length)
    {
        _TextLen = length;
        _TextData = string.Empty;

        _UpdateText();
    }

    public void AddWord(string text)
    {
        if (_TextData.Length >= _TextLen) return;
        _TextData += text;

        _UpdateText();
    }

    public void DeleteWord(int num = 1)
    {
        if(_TextData.Length == 0 || num <= 0) return;
        _TextData = _TextData.Remove(_TextData.Length - num, num);

        _UpdateText();
    }

    public void ChangeSmallWord()
    {
        if(string.IsNullOrEmpty(_TextData)) return;

        char lastWord = _TextData[^1];
        foreach (var pair in SmallKana)
        {
            var cycle = pair.Value;
            int index = System.Array.IndexOf(cycle, lastWord); //サイクル内の文字の位置を探す
            if (index == -1) continue; //なかったら次へ

            char nextChar = cycle[(index + 1) % cycle.Length]; //次の文字検出
            _TextData = _TextData.Remove(_TextData.Length - 1, 1) + nextChar;
            _UpdateText();
            return;
        }
    }

    public void ChangeDakutenWord()
    {
        if (string.IsNullOrEmpty(_TextData)) return;

        char lastWord = _TextData[^1];
        foreach (var pair in Dakuten)
        {
            var cycle = pair.Value;
            int index = System.Array.IndexOf(cycle, lastWord); //サイクル内の文字の位置を探す
            if (index == -1) continue; //なかったら次へ

            char nextChar = cycle[(index + 1) % cycle.Length]; //次の文字検出
            _TextData = _TextData.Remove(_TextData.Length - 1, 1) + nextChar;
            _UpdateText();
            return;
        }
    }

    private void _UpdateText()
    {
        if(!_TextUI) return;
        _TextUI.text = _TextData;
    }
}
