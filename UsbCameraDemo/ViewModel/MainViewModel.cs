using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WPFMediaKit.DirectShow.Controls;
using WPFMediaKit.DirectShow.Interop;
using System.Reflection;

namespace UsbCameraDemo.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
            ///

            Title = $"USB…„œÒÕ∑≤‚ ‘π§æﬂ V{Assembly.GetExecutingAssembly().GetName().Version.ToString()}";
           
            foreach (var item in MultimediaUtil.VideoInputDevices)
            {
                CameraList.Add(item);
            }
        

            SelectionChangedCmd = new RelayCommand<SelectionChangedEventArgs>(args => 
            {
                VideoSource = CurrentDevice?.Name;
            });


            ScreenShotCmd = new RelayCommand<FrameworkElement>(element =>
            {
                string fileName = $"ΩÿÕº {DateTime.Now:yyyy-MM-dd HHmmss}.jpg";
                System.IO.FileStream fs = new System.IO.FileStream(fileName, System.IO.FileMode.Create);
                RenderTargetBitmap bmp = new RenderTargetBitmap((int)element.ActualWidth, (int)element.ActualHeight, 0, 0, PixelFormats.Pbgra32);
                bmp.Render(element);
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bmp));
                encoder.Save(fs);
                fs.Close();
            });



            if (!IsInDesignMode)
            {
                if (CameraList.Count > 0)
                {
                    CurrentDevice = CameraList[0];
                    VideoSource = CurrentDevice.Name;
                }
            }


        }

        #region  ”Õº Ù–‘

        public ObservableCollection<DsDevice> CameraList { get; set; } = new ObservableCollection<DsDevice>();



        private string _title;

        public string Title
        {
            get { return _title; }
            set { Set(ref _title, value); }
        }



        private string _videoSource;

        public string VideoSource
        {
            get => _videoSource;
            set => Set(ref _videoSource, value);
        }

        private DsDevice _currentDevice;
        public DsDevice CurrentDevice
        {
            get => _currentDevice;
            set => Set(ref _currentDevice, value);
        }

        public RelayCommand<SelectionChangedEventArgs> SelectionChangedCmd { get; set; }

        public RelayCommand<FrameworkElement> ScreenShotCmd { get; set; }


        #endregion
    }
}