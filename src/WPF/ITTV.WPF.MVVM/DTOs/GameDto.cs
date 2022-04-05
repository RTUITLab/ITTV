﻿using System;
using System.Windows.Input;
using ITTV.WPF.Core.Helpers;
using ITTV.WPF.MVVM.Commands.Games;

namespace ITTV.WPF.MVVM.DTOs
{
    public sealed class GameDto
    {
        public GameDto()
        { }
        public GameDto(string title)
        {
            Title = title;

            SelectGameCommand = new SelectGameCommand(this);
        }
        
        public string Title { get; }
        public ICommand SelectGameCommand { get; }
        
        public Uri ExecuteFileUri => DirectoryHelper.GetExecuteFilePath(Path);
        private string Path => System.IO.Path.Combine(PathHelper.GetDirectoryGamesPath, Title);

    }
}