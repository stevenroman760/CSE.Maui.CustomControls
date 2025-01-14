namespace CSE.Maui.CustomControls.Models
{
    [Serializable]
    public class TreeItem
    {
        public string ItemName { get; set; }
        public int Id { get; set; }
    }

    [Serializable]
    public class TreeFolder
    {
        public List<TreeFolder> Children { get; } = new();
        public List<TreeItem> TreeItems { get; } = new();

        public string FolderName { get; set; }
        public int GroupId { get; set; }
    }
}
