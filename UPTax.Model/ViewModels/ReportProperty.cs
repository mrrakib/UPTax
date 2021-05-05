namespace UPTax.Model
{
    public class ReportProperty<T> where T : class
    {
        public string ReportTitle { get; set; }
        public string ReportPath { get; set; }
        public string ReportViewName { get; set; }
        public long TotalCount { get; set; }
        public T ReportBody { get; set; }
    }
}
