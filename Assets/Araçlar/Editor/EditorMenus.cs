using UnityEngine;
using UnityEditor;
using Araclar;

public class EditorMenus
{
    [MenuItem("Ara�lar/Proje/Proje olu�turucu")]
    public static void InitProjeOlusturucuArac()
    {
        ProjeKurulumAraci_pencere.InitWindow();
    }
}
