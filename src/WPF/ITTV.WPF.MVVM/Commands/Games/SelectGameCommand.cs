using System.Diagnostics;
using ITTV.WPF.Abstractions.Base.Command;
using ITTV.WPF.MVVM.Models;

namespace ITTV.WPF.MVVM.Commands.Games
{
    public class SelectGameCommand : CommandBase
    {
        private readonly Game _game;
        public SelectGameCommand(Game game)
        {
            _game = game;
        }
        public override void Execute(object parameter)
        {
            var gameProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = _game.ExecuteFileUri.AbsoluteUri
                }
            };

            gameProcess.Start();
        }
    }
}