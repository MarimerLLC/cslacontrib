namespace CslaContrib.Mvc
{
    public class PatternMap
    {
        public string ActionPattern { get; set; }
        public string[] MethodPatterns { get; set; }

        public PatternMap() { }
        public PatternMap(string actionPattern, string[] methodPatterns)
        {
            ActionPattern = actionPattern;
            MethodPatterns = methodPatterns;
        }
    }
}