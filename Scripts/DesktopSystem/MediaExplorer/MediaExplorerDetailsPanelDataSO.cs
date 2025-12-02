using System;
using UnityEngine;

namespace Proselyte.OldschoolOS
{
    [CreateAssetMenu(fileName = "MediaExplorerDetailsPanelDataSO", menuName = "Proselyte / OldschoolOS / Media Explorer Details Panel Data SO")]
    public class MediaExplorerDetailsPanelDataSO : ScriptableObject
    {
        public CrunchOSFile.CrunchFileData crunch_OS_file_data;
    }
}
