using System.Collections.ObjectModel;

namespace LabelGenerator.Objects.SourceParser
{
    public class LabelItem
    {
        public string FormName { get; set; }
        public string FormLanguage { get; set; }
        public string FormFormat { get; set; }
        public string Subject { get; set; }

        public Collection<Section> Sections { get; set; } 
    }
}
