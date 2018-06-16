namespace PhotoOrganizer.ViewModels
{
    public interface IGroupFolderViewModelFactory
    {
        IGroupFolderViewModel Create(string title);
    }
}