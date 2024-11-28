using UnityEngine;
using UnityEditor;
using Araclar;

public class EditorMenus
{
    [MenuItem("Araçlar/Proje/Proje oluþturucu")]
    public static void InitProjeOlusturucuArac()
    {
        ProjeKurulumAraci_pencere.InitWindow();
    }
}
