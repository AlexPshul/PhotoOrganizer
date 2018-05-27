using Windows.ApplicationModel.Resources;

namespace PhotoOrganizer.Text
{
    public class StringsReader
    {
        public string this[string key] => Get(key);
        
        public static string Get(string key) => ResourceLoader.GetForViewIndependentUse("Strings").GetString(key);
    }
}