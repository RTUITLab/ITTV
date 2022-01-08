using System.Collections.ObjectModel;
using System.Windows.Input;
using ITTV.WPF.Abstractions.Base.ViewModel;
using ITTV.WPF.Core.Stores;
using ITTV.WPF.MVVM.Commands.News;
using ITTV.WPF.MVVM.DTOs;

namespace ITTV.WPF.MVVM.ViewModels
{
    public class NewsElementViewModel : ViewModelBase
    {
        public ICommand SelectNewsCommand { get; }
        public ICommand SelectNextImageCommand { get; }
        public ICommand SelectBackImageCommand { get; }

        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                if (Equals(_title, value))
                    return;

                _title = value;
                OnPropertyChanged(Title);
            }
        }

        private string _description;

        public string Description
        {
            get => _description;
            set
            {
                if (Equals(_description, value))
                    return;

                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }
        
        public byte[] CurrentImage => _images[SelectedImageIndex];

        private int SelectedImageIndex
        {
            get => _selectedImageIndex;
            set
            {
                if (Equals(_selectedImageIndex, value))
                    return;
                
                _selectedImageIndex = value;
                
                OnPropertyChanged(nameof(CurrentImage));
                OnPropertyChanged(nameof(CanSelectBackImage));
                OnPropertyChanged(nameof(CanSelectNextImage));
            }
        }

        private int _selectedImageIndex;

        public bool CanSelectBackImage => _selectedImageIndex > 0;
        public bool CanSelectNextImage => _selectedImageIndex + 1 < _images.Count;

        private readonly ObservableCollection<byte[]> _images;

        public NewsElementViewModel(NewsDto newsDto, NavigationStore navigationStore)
        {
            SelectBackImageCommand = new SelectBackNewsImageCommand(this);
            SelectNextImageCommand = new SelectNextNewsImageCommand(this);
            SelectNewsCommand = new SelectNewsCommand(this, navigationStore);
            
            Title = newsDto.Title;
            Description = newsDto.Description;

            _images = new ObservableCollection<byte[]>(newsDto.SourceImages);
        }

        public void SelectNextImage()
        {
            if (CanSelectNextImage)
                SelectedImageIndex++;
        }

        public void SelectBackImage()
        {
            if (CanSelectBackImage)
                SelectedImageIndex--;
        }
    }
}