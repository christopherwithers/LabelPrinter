namespace LabelGenerator.Objects.SourceParser
{
    public class Section
    {
        public Section()
        {
            Z13 = new Z13();
            Z30 = new Z30();
        }

        public string Name { get; set; }

        public string FormDate { get; set; }
        public string BibInfo { get; set; }

        public Z13 Z13 { get; set; }
        public Z30 Z30 { get; set; }
    }
}
