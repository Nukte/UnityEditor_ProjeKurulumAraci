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

        // Oyun t�r�ne g�re se�ilen klas�rler
        private bool createArtFolder = true;
        private bool createCodeFolder = true;
        private bool createResourcesFolder = true;
        private bool createPrefabsFolder = true;
        private bool createScenesFolder = true;

        // Oyun t�r� se�imi
        private int selectedGameTypeIndex = 0;  // De�i�keni int olarak tan�mlad�k
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

            // Proje ad� giri�i
            EditorGUILayout.BeginHorizontal();
            projectName = EditorGUILayout.TextField("Projenizin �smi", projectName);
            EditorGUILayout.EndHorizontal();

            // Oyun t�r�n� se�
            EditorGUILayout.BeginHorizontal();
            selectedGameTypeIndex = EditorGUILayout.Popup("Oyun T�r�n� Se�", selectedGameTypeIndex, gameTypes);
            EditorGUILayout.EndHorizontal();

            // Se�ilen oyun t�r�ne g�re klas�r se�eneklerini g�ster
            string selectedGameType = gameTypes[selectedGameTypeIndex]; // Oyun t�r�n� string olarak al�yoruz
            if (selectedGameType != "None")
            {
                EditorGUILayout.LabelField($"Oyun T�r�: {selectedGameType}", EditorStyles.boldLabel);
                createArtFolder = EditorGUILayout.Toggle("Art Klas�r�", createArtFolder);
                createCodeFolder = EditorGUILayout.Toggle("Code Klas�r�", createCodeFolder);
                createResourcesFolder = EditorGUILayout.Toggle("Resources Klas�r�", createResourcesFolder);
                createPrefabsFolder = EditorGUILayout.Toggle("Prefabs Klas�r�", createPrefabsFolder);
                createScenesFolder = EditorGUILayout.Toggle("Scenes Klas�r�", createScenesFolder);
            }

            if (GUILayout.Button("Proje Dosyalar�n� Olu�tur", GUILayout.ExpandHeight(true)))
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
                if (EditorUtility.DisplayDialog("Proje kurulum Uyar�s�!", "L�tfen projenize bir isim verin", "Tamam"))
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

            AssetDatabase.Refresh(); // Unity'yi klas�r de�i�ikliklerinden haberdar et

            CloseWindow(); // Pencereyi kapat
        }

        void CreateSpecificFolders(string rootPath)
        {
            List<string> folderNames = new List<string>();

            // Se�ilen oyun t�r�ne g�re klas�rler olu�turulacak
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
            Scene curScene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single); // Do�ru metod burada kullan�ld�
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
