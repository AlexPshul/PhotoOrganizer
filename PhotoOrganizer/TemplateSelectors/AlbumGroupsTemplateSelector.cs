using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace PhotoOrganizer.TemplateSelectors
{
    public class AlbumGroupsTemplateSelector : DataTemplateSelector
    {
        public DataTemplate RegularGroupTemplate { get; set; }
        public DataTemplate AddNewGroupTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            string itemString = item.ToString();
            return string.IsNullOrEmpty(itemString) ? AddNewGroupTemplate : RegularGroupTemplate;
        }
    }
}