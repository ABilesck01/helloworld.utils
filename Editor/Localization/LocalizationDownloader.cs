#if UNITY_EDITOR
using System.Net;
using UnityEditor;
using UnityEngine;

namespace HelloWorld.Utils.Editor
{
    public static class LocalizationDownloader
    {
        private const string GOOGLE_SHEET_URL = "https://docs.google.com/spreadsheets/d/";
        private const string GOOGLE_SHEET_EXPORT = "/export?format=csv";

        [MenuItem("Tools/Localization/Atualizar CSV")]
        public static void DownloadLocalizationCSV()
        {
            // Carrega o arquivo LocalizationSettings da pasta Resources
            LocalizationSettings settings = Resources.Load<LocalizationSettings>("LocalizationSettings");

            if (settings == null)
            {
                Debug.LogError("Arquivo de configurações de localização não encontrado!");
                return;
            }

            // Verifica se a URL do Google Sheets está definida
            if (string.IsNullOrEmpty(settings.googleSheetID))
            {
                Debug.LogError("URL do Google Sheets não definida!");
                return;
            }

            // Verifica se o caminho do arquivo CSV local está definido
            if (string.IsNullOrEmpty(settings.localCsvPath))
            {
                Debug.LogError("Caminho do arquivo CSV local não definido!");
                return;
            }

            // Faz o download do arquivo CSV
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(GOOGLE_SHEET_URL + settings.googleSheetID + GOOGLE_SHEET_EXPORT, settings.localCsvPath);
                    Debug.Log("CSV de localização baixado com sucesso!");
                }

                AssetDatabase.ImportAsset(settings.localCsvPath);
                var csv = AssetDatabase.LoadAssetAtPath<TextAsset>(settings.localCsvPath);
                if (csv == null)
                {
                    Debug.LogError("Erro ao carregar o arquivo CSV após o download.");
                    return;
                }

                // Atualiza o arquivo CSV no projeto
                SerializedObject so = new SerializedObject(settings);
                so.FindProperty("csvFile").objectReferenceValue = csv;
                so.ApplyModifiedProperties();

                EditorUtility.SetDirty(settings);
                AssetDatabase.SaveAssets();

                Debug.Log("Campo csvFile atualizado no LocalizationSettings.");

            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Erro ao baixar o arquivo CSV: {ex.Message}");
                throw;
            }
        }
    }
}
#endif