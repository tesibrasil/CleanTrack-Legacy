using Avalonia.Media;
using Avalonia.Threading;
using ReactiveUI;
using System.Threading;

namespace CleanTrackPi.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        #region Properties

        private bool _pageNotFoundVisibility;
        public bool PageNotFoundVisibility
        {
            get => _pageNotFoundVisibility;
            set => this.RaiseAndSetIfChanged(ref _pageNotFoundVisibility, value);
        }


        private bool _barcodeVisibility;
        public bool BarcodeVisibility
        {
            get => _barcodeVisibility;
            set => this.RaiseAndSetIfChanged(ref _barcodeVisibility, value);
        }

        #region PageNotFoundProperties

        private string _firstLine;
        public string FirstLine
        {
            get => _firstLine;
            set => this.RaiseAndSetIfChanged(ref _firstLine, value);
        }


        private string _secondLine;
        public string SecondLine
        {
            get => _secondLine;
            set => this.RaiseAndSetIfChanged(ref _secondLine, value);
        }

        private string _thirdLine;
        public string ThirdLine
        {
            get => _thirdLine;
            set => this.RaiseAndSetIfChanged(ref _thirdLine, value);
        }

        private string _fourthLine;
        public string FourthLine
        {
            get => _fourthLine;
            set => this.RaiseAndSetIfChanged(ref _fourthLine, value);
        }

        private string _fifthLine;
        public string FifthLine
        {
            get => _fifthLine;
            set => this.RaiseAndSetIfChanged(ref _fifthLine, value);
        }

        private IBrush _textbox3Foreground;

        public IBrush Textbox3Foreground
        {
            get { return _textbox3Foreground; }
            set
            {
                this.RaiseAndSetIfChanged(ref _textbox3Foreground, value);
            }
        }
        #endregion

        #region BarcodeProperties

        private CleanTrackPi.Models.Status _nextStatus;
        public CleanTrackPi.Models.Status NextStatus
        {
            get => _nextStatus;
            set => this.RaiseAndSetIfChanged(ref _nextStatus, value);
        }


        private string _operationName;
        public string OperationName
        {
            get => _operationName;
            set => this.RaiseAndSetIfChanged(ref _operationName, value);
        }

        private string _operationLabel;
        public string OperationLabel
        {
            get => _operationLabel;
            set => this.RaiseAndSetIfChanged(ref _operationLabel, value);
        }


        private CleanTrackPi.Models.Device _device;
        public CleanTrackPi.Models.Device Device
        {
            get => _device;
            set => this.RaiseAndSetIfChanged(ref _device, value);
        }


        private string _sondaLabel;
        public string SondaLabel
        {
            get => _sondaLabel;
            set => this.RaiseAndSetIfChanged(ref _sondaLabel, value);
        }

        private string _sondaName;
        public string SondaName
        {
            get => _sondaName;
            set => this.RaiseAndSetIfChanged(ref _sondaName, value);
        }

        private string _stateLabel;
        public string StateLabel
        {
            get => _stateLabel;
            set => this.RaiseAndSetIfChanged(ref _stateLabel, value);
        }

        private string _stateName;
        public string StateName
        {
            get => _stateName;
            set => this.RaiseAndSetIfChanged(ref _stateName, value);
        }

        private string _barcodeAcessory;

        public string BarcodeAcessory
        {
            get => _barcodeAcessory;
            set => this.RaiseAndSetIfChanged(ref _barcodeAcessory, value);
        }


        private CleanTrackPi.Models.Operator _operator;
        public CleanTrackPi.Models.Operator Operator
        {
            get => _operator;
            set => this.RaiseAndSetIfChanged(ref _operator, value);
        }

        private string _operatorLabel;
        public string OperatorLabel
        {
            get => _operatorLabel;
            set => this.RaiseAndSetIfChanged(ref _operatorLabel, value);
        }

        private string _operatorName;
        public string OperatorName
        {
            get => _operatorName;
            set => this.RaiseAndSetIfChanged(ref _operatorName, value);
        }

        private string _barcode;
        public string BarcodeText
        {
            get => _barcode;
            set => this.RaiseAndSetIfChanged(ref _barcode, value);
        }

        private bool operationFlag;
        public bool OperationFlag
        {
            get => operationFlag;
            set => this.RaiseAndSetIfChanged(ref operationFlag, value);
        }

        private bool sondaFlag;
        public bool SondaFlag
        {
            get => sondaFlag;
            set => this.RaiseAndSetIfChanged(ref sondaFlag, value);
        }

        private bool operatorFlag;
        public bool OperatorFlag
        {
            get => operatorFlag;
            set => this.RaiseAndSetIfChanged(ref operatorFlag, value);
        }

        private bool _newBarcodeScanned;
        public bool NewBarcodeScanned
        {
            get => _newBarcodeScanned;
            set => this.RaiseAndSetIfChanged(ref _newBarcodeScanned, value);
        }

        private CancellationTokenSource? _cancellationTokenSource;

        DispatcherTimer timerTimeout = new DispatcherTimer();


        private bool _progress;
        public bool Progress
        {
            get => _progress;
            set => this.RaiseAndSetIfChanged(ref _progress, value);
        }

        private bool _backgroundVisibility;
        public bool BackgroundVisibility
        {
            get => _backgroundVisibility;
            set => this.RaiseAndSetIfChanged(ref _backgroundVisibility, value);
        }

        private IBrush _backgroundColor;

        public IBrush BackgroundColor
        {
            get { return _backgroundColor; }
            set
            {
                this.RaiseAndSetIfChanged(ref _backgroundColor, value);
            }
        }

        private IBrush _headerColor;
        
        public IBrush HeaderColor
        {
            get { return _headerColor; }
            set
            {
                this.RaiseAndSetIfChanged(ref _headerColor, value);
            }
        }

        private string _messageBox;
        public string MessageBox
        {
            get => _messageBox;
            set => this.RaiseAndSetIfChanged(ref _messageBox, value);
        }

        #endregion

        #endregion
    }
}
