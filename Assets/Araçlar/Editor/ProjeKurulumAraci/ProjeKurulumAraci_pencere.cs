using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

namespace Araclar
{
    public class ProjeKurulumAraci_pencere : EditorWindow
    {
        #region variables

        static ProjeKurulumAraci_pencere win;

        private string projectName = "test";

        // Oyun türüne göre seçilen klasörler
        private bool createArtFolder = true;
        private bool createCodeFolder = true;
        private bool createResourcesFolder = true;
        private bool createPrefabsFolder = true;
        private bool createScenesFolder = true;

        // Oyun türü seçimi
        private int selectedGameTypeIndex = 0;
        private string[] gameTypes = { "Platformer", "FPS", "RPG", "None" };
        #endregion

        #region main methods
        public static void InitWindow()
        {
            win = EditorWindow.GetWindow<ProjeKurulumAraci_pencere>("Proje Kurulum");
            win.Show();
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();

            // Proje adi girisi
            EditorGUILayout.BeginHorizontal();
            projectName = EditorGUILayout.TextField("Projenizin Ýsmi", projectName);
            EditorGUILayout.EndHorizontal();

            // Oyun türünü seç
            EditorGUILayout.BeginHorizontal();
            selectedGameTypeIndex = EditorGUILayout.Popup("Oyun Türünü Seç", selectedGameTypeIndex, gameTypes);
            EditorGUILayout.EndHorizontal();

            // Seçilen oyun türüne göre klasör seçeneklerini göster
            string selectedGameType = gameTypes[selectedGameTypeIndex]; // Oyun türünü string olarak alýyoruz
            if (selectedGameType != "None")
            {
                EditorGUILayout.LabelField($"Oyun Türü: {selectedGameType}", EditorStyles.boldLabel);
                createArtFolder = EditorGUILayout.Toggle("Art Klasörü", createArtFolder);
                createCodeFolder = EditorGUILayout.Toggle("Code Klasörü", createCodeFolder);
                createResourcesFolder = EditorGUILayout.Toggle("Resources Klasörü", createResourcesFolder);
                createPrefabsFolder = EditorGUILayout.Toggle("Prefabs Klasörü", createPrefabsFolder);
                createScenesFolder = EditorGUILayout.Toggle("Scenes Klasörü", createScenesFolder);
            }

            if (GUILayout.Button("Proje Dosyalarýný Oluþtur", GUILayout.ExpandHeight(true)))
            {
                CreateProjectFolders();
            }

            if (win != null)
            {
                win.Repaint();
            }

            EditorGUILayout.EndVertical();
        }
        #endregion

        #region custom methods
        void CreateProjectFolders()
        {
            if (string.IsNullOrEmpty(projectName))
            {
                if (EditorUtility.DisplayDialog("Proje kurulum Uyarýsý!", "Lütfen projenize bir isim verin", "Tamam"))
                {
                    return;
                }
            }

            string assetPath = Application.dataPath;
            string rootPath = assetPath + "/" + projectName;

            DirectoryInfo rootInfo = Directory.CreateDirectory(rootPath);

            if (!rootInfo.Exists)
            {
                return;
            }

            CreateSpecificFolders(rootPath);

            AssetDatabase.Refresh(); // Unity'yi klasör degisikliklerinden haberdar et

            CloseWindow(); // Pencereyi kapat
        }

        void CreateSpecificFolders(string rootPath)
        {
            List<string> folderNames = new List<string>();

            // Secilen oyun türüne göre klasörler olusturulacak
            if (createArtFolder)
            {
                string artPath = Path.Combine(rootPath, "Art");
                Directory.CreateDirectory(artPath);
                folderNames.Clear();
                folderNames.Add("Characters");
                folderNames.Add("Levels");
                CreateFolder(artPath, folderNames);
            }

            if (createCodeFolder)
            {
                string codePath = Path.Combine(rootPath, "Code");
                Directory.CreateDirectory(codePath);
                folderNames.Clear();
                folderNames.Add("Editor");
                folderNames.Add("Scripts");
                CreateFolder(codePath, folderNames);
            }

            if (createResourcesFolder)
            {
                string resourcesPath = Path.Combine(rootPath, "Resources");
                Directory.CreateDirectory(resourcesPath);
                folderNames.Clear();
                folderNames.Add("Characters");
                folderNames.Add("Props");
                CreateFolder(resourcesPath, folderNames);
            }

            if (createPrefabsFolder)
            {
                string prefabsPath = Path.Combine(rootPath, "Prefabs");
                Directory.CreateDirectory(prefabsPath);
                folderNames.Clear();
                folderNames.Add("Characters");
                folderNames.Add("Enemies");
                CreateFolder(prefabsPath, folderNames);
            }

            if (createScenesFolder)
            {
                string scenesPath = Path.Combine(rootPath, "Scenes");
                Directory.CreateDirectory(scenesPath);
                CreateScene(scenesPath, projectName + "_Main");
                CreateScene(scenesPath, projectName + "_Frontend");
            }
        }

        void CreateFolder(string aPath, List<string> folders)
        {
            foreach (string folder in folders)
            {
                Directory.CreateDirectory(Path.Combine(aPath, folder));
            }
        }

        void CreateScene(string aPath, string aName)
        {
            Scene curScene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
            EditorSceneManager.SaveScene(curScene, Path.Combine(aPath, aName + ".unity"), true); // Sahneyi kaydet
        }

        void CloseWindow()
        {
            if (win)
            {
                win.Close();
            }
        }
        #endregion
    }
}
