using System;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ParrotMiniDroneControle
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //private NetworkAl network;
        private Drone_New drone;

        public MainPage()
        {
            InitializeComponent();
            //Task.WaitAll(Stuff());
            Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            CoreWindow.GetForCurrentThread().KeyDown += MyPage_KeyDown;
            Log.Instance.MessageLogged += UpdateConsole;
            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => Stuff());
        }

        private void UpdateConsole(object sender, CustomEventArgs customEventArgs)
        {
            Console.Text += customEventArgs.Message;
        }

        private async void MyPage_KeyDown(CoreWindow sender, KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case VirtualKey.W:
                {
                    //await drone.Forward();
                    await drone.Move(Direction.Front, 30);
                    break;
                }
                case VirtualKey.S:
                {
                    //await drone.Backward();
                    await drone.Move(Direction.Back, 30);
                    break;
                }
                case VirtualKey.A:
                {
                    //await drone.Left();
                    await drone.Move(Direction.Left, 30);
                    break;
                }
                case VirtualKey.D:
                {
                    //await drone.Right();
                    await drone.Move(Direction.Right, 30);
                    break;
                }
                case VirtualKey.Space:
                {
                    //await drone.Up();
                    await drone.Move(Direction.Up, 30);
                    break;
                }
                case VirtualKey.X:
                {
                    //await drone.Down();
                    await drone.Move(Direction.Down, 30);
                    break;
                }
                default:
                {
                    break;
                }
            }
        }

        private async void Stuff()
        {
            drone = new Drone_New();
            await drone.Connection.Initialise();
            Console.Text += "Drone initialized. \n";
        }
        
        private async void TakeOffButton_OnClick(object sender, RoutedEventArgs e)
        {
            await drone.TakeOff();
            Console.Text += "Drone takeoff sent. \n";
        }

        private async void LandingButton_OnClick(object sender, RoutedEventArgs e)
        {
            await drone.Land();
            Console.Text += "Drone landing sent. \n";
        }

        private async void Emergency_OnClick(object sender, RoutedEventArgs e)
        {
            await drone.EmergencyStopp();
            Console.Text += "Drone emergency stop sent. \n";
        }

        private async void Connect_OnClick(object sender, RoutedEventArgs e)
        {
            await drone.Connect();
            Console.Text += "Drone connected. \n";
        }

        private async void Forwards_OnClick(object sender, RoutedEventArgs e)
        {
            //await drone.Forward();
            await drone.Move(Direction.Front, 30);
            Console.Text += "Drone move forwards. \n";
        }

        private async void Backwards_OnClick(object sender, RoutedEventArgs e)
        {
            //await drone.Backward();
            await drone.Move(Direction.Back, 30);
            Console.Text += "Drone move backwards. \n";
        }

        private async void Left_OnClick(object sender, RoutedEventArgs e)
        {
            //await drone.Left();
            await drone.Move(Direction.Left, 30);
            Console.Text += "Drone move left. \n";
        }

        private async void FlipLeft_OnCLick(object sender, RoutedEventArgs e)
        {
            //await drone.FlipLeft();
            await drone.Flip(Direction.Left);
            Console.Text += "Drone flip left. \n";
        }

        private async void FlipForward_OnCLick(object sender, RoutedEventArgs e)
        {
            //await drone.FlipForward();
            await drone.Flip(Direction.Front);
            Console.Text += "Drone flip forward. \n";
        }

        private async void Right_OnClick(object sender, RoutedEventArgs e)
        {
            //await drone.Right();
            await drone.Move(Direction.Right, 30);
            Console.Text += "Drone move right. \n";
        }

        private async void Up_OnClick(object sender, RoutedEventArgs e)
        {
            //await drone.Up();
            await drone.Move(Direction.Up, 30);
            Console.Text += "Drone move up. \n";
        }

        private async void Down_OnClick(object sender, RoutedEventArgs e)
        {
            //await drone.Down();
            await drone.Move(Direction.Down, 30);
            Console.Text += "Drone move down. \n";
        }
    }
}