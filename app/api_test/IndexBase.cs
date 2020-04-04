using Microsoft.AspNetCore.Components;

namespace api_test
{
    public class IndexBase : LayoutComponentBase
    {
        public string GetTextFromMethodInClass()
        {
            return "The source for this text was external C# code in a .cs file";
        }
    }
}