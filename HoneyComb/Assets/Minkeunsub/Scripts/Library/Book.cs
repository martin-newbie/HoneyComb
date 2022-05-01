using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "Book", menuName = "Book", order = int.MinValue)]
public class Book : ScriptableObject
{
    [SerializeField] string path;

    [Header("Datas")]
    public string BookName;
    public string BookDesc1;
    public string BookDesc2;

    public Book Init()
    {
        TextAsset asset = Resources.Load("Texts/Book/" + path) as TextAsset;
        string[] temp = asset.text.Split('\n');
        BookName = temp[0];
        BookDesc1 = temp[1] + temp[2] + temp[3] + temp[4];

        StringBuilder SB = new StringBuilder();
        for (int i = 5; i < temp.Length; i++)
        {
            SB.Append(temp[i]);
        }
        BookDesc2 = SB.ToString();
        return this;
    }
}
