using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HelloWorld.Utils
{
    [CreateAssetMenu(fileName = "LocalizationSettings", menuName = "Localization/Settings")]
    public class LocalizationSettings : ScriptableObject
    {
        [Header("Google Sheets CSV")]
        [Tooltip("URL direta para download do CSV do Google Sheets")]
        public string googleSheetID;

        [Header("Destino do CSV")]
        [Tooltip("Caminho onde o arquivo CSV ser� salvo (ex: Assets/Localization/auto_localization.csv)")]
        public string localCsvPath = "Assets/Localization/auto_localization.csv";

        [Header("Arquivo de Localiza��o")]
        [Tooltip("Arquivo CSV que cont�m as tradu��es")]
        public TextAsset csvFile;
    }
}