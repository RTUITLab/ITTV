using System.Diagnostics;
using ITTV.WPF.Abstractions.Base.Command;
using ITTV.WPF.MVVM.DTOs;

namespace ITTV.WPF.MVVM.Commands.Games
{
    public class SelectGameCommand : CommandBase
    {
        private readonly GameDto gameDto;
        public SelectGameCommand(GameDto gameDto)
        {
            this.gameDto = gameDto;
        }
        public override void Execute(object parameter)
        {
            var gameProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = gameDto.ExecuteFileUri.OriginalString
                }
            };

            gameProcess.Start();
        }
    }
}