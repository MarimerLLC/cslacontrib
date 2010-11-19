
namespace ProjectTrackerMvc
{
    public class AjaxResult 
    {
        public object Data { get; set; }

        public bool Success { get; set; }
        public string[] Messages { get; set; }

        public AjaxResult()
        {
            Success = true;
        }
    }
}