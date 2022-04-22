using System.Diagnostics;
using ITTV.WPF.Abstractions.Base.Command;
using ITTV.WPF.DTOs;

namespace ITTV.WPF.Commands.Games
{
    public class SelectGameCommand : CommandBase
    {
        private readonly GameDto _gameDto;
        public SelectGameCommand(GameDto gameDto)
        {
            _gameDto = gameDto;
        }
        public override void Execute(object parameter)
        {
            var gameProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = _gameDto.ExecuteFileUri.OriginalString
                }
            };

            gameProcess.Start();
        }
    }
}