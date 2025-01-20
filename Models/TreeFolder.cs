using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSE.Maui.CustomControls.Models
{
    [Serializable]
    public class TreeFolder
    {
        public List<TreeFolder> Children { get; } = new();
        public List<TreeItem> TreeItems { get; } = new();

        public string FolderName { get; set; }
        public int GroupId { get; set; }
    }
}
