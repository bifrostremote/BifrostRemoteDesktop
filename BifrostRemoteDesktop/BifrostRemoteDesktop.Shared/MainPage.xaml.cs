using BifrostRemoteDesktop.BusinessLogic;
using BifrostRemoteDesktop.BusinessLogic.Controls;
using BifrostRemoteDesktop.BusinessLogic.UIControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace BifrostRemoteDesktop
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        private double _mouseX;
        private double _mouseY;
        private bool _mouseRightButton = false;
        private bool _mouseLeftButton = false;

        public MainPage()
        {
            this.InitializeComponent();
        }

        public bool MouseRightButton
        {
            get => _mouseRightButton;
            set
            {
                _mouseRightButton = value;
                NotifyPropertyChanged(nameof(MouseRightButton));
            }
        }
        public bool MouseLeftButton
        {
            get => _mouseLeftButton;
            set
            {
                _mouseLeftButton = value;
                NotifyPropertyChanged(nameof(MouseLeftButton));
            }
        }

        public double MouseX
        {
            get => _mouseX;
            set
            {
                _mouseX = value;
                NotifyPropertyChanged(nameof(MouseX));
            }
        }
        public double MouseY
        {
            get => _mouseY;
            set
            {
                _mouseY = value;
                NotifyPropertyChanged(nameof(MouseY));
            }
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void Canvas_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            var point = e.GetCurrentPoint((UIElement)sender);
            MouseX = point.Position.X;
            MouseY = point.Position.Y;
            //throw new Exception("YOU DON GOOFED!");
        }

        private void Canvas_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var point = e.GetCurrentPoint((UIElement)sender);
            MouseRightButton = point.Properties.IsRightButtonPressed;
            MouseLeftButton = point.Properties.IsLeftButtonPressed;
        }

        private void Canvas_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            var point = e.GetCurrentPoint((UIElement)sender);
            MouseRightButton = point.Properties.IsRightButtonPressed;
            MouseLeftButton = point.Properties.IsLeftButtonPressed;
        
        }
    }
}
