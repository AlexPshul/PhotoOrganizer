﻿using System.Reactive;
using PhotoOrganizer.Business.Interfaces;
using ReactiveUI;

namespace PhotoOrganizer.ViewModels
{
    internal class GroupCreatorViewModel : IGroupCreatorViewModel
    {
        public ReactiveCommand<Unit, string> ExecuteGroupLogicCommand { get; }

        public GroupCreatorViewModel(ICurrentAlbumManager currentAlbumManager)
        {
            ExecuteGroupLogicCommand = ReactiveCommand.CreateFromTask(currentAlbumManager.AddAlbumFolder);
        }
    }
}