using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace FlyWithMe10
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
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

        private void MainPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            CoreWindow.GetForCurrentThread().KeyDown += MyPage_KeyDown;
            this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => Stuff());

        }

        private async void MyPage_KeyDown(CoreWindow sender, KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case Windows.System.VirtualKey.W:
                    {
                        //await drone.Forward();
                        await drone.Move(Direction.Front, 30);
                        break;
                    }
                case Windows.System.VirtualKey.S:
                    {
                        //await drone.Backward();
                        await drone.Move(Direction.Back, 30);
                        break;
                    }
                case Windows.System.VirtualKey.A:
                    {
                        //await drone.Left();
                        await drone.Move(Direction.Left, 30);
                        break;
                    }
                case Windows.System.VirtualKey.D:
                    {
                        //await drone.Right();
                        await drone.Move(Direction.Right, 30);
                        break;
                    }
                case Windows.System.VirtualKey.Space:
                    {
                        //await drone.Up();
                        await drone.Move(Direction.Up, 30);
                        break;
                    }
                case Windows.System.VirtualKey.X:
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
            //drone = new Drone();
            //await drone.Initialize();
            //drone.SomethingChanged += OnHandle_SomethingChanged;
            drone = new Drone_New();
            await drone.Connection.Initialise();
            Console.Text += string.Format("Drone initialized. \n");
        }

        private void OnHandle_SomethingChanged(object sender, CustomEventArgs e)
        {
            this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => Console.Text += string.Format(e.Message + "\n"));
            
        }

        private async void TakeOffButton_OnClick(object sender, RoutedEventArgs e)
        {
            await drone.TakeOff();
            Console.Text += string.Format("Drone takeoff sent. \n");
        }

        private async void LandingButton_OnClick(object sender, RoutedEventArgs e)
        {
            await drone.Land();
            Console.Text += string.Format("Drone landing sent. \n");
        }

        private async void Emergency_OnClick(object sender, RoutedEventArgs e)
        {
            await drone.EmergencyStopp();
            Console.Text += string.Format("Drone emergency stop sent. \n");
        }

        private async void Connect_OnClick(object sender, RoutedEventArgs e)
        {
            await drone.Connect();
            Console.Text += string.Format("Drone connected. \n");
        }

        private async void Forwards_OnClick(object sender, RoutedEventArgs e)
        {
            //await drone.Forward();
            await drone.Move(Direction.Front, 30);
            Console.Text += string.Format("Drone move forwards. \n");
        }

        private async void Backwards_OnClick(object sender, RoutedEventArgs e)
        {
            //await drone.Backward();
            await drone.Move(Direction.Back, 30);
            Console.Text += string.Format("Drone move backwards. \n");
        }

        private async void Left_OnClick(object sender, RoutedEventArgs e)
        {
            //await drone.Left();
            await drone.Move(Direction.Left, 30);
            Console.Text += string.Format("Drone move left. \n");
        }

        private async void FlipLeft_OnCLick(object sender, RoutedEventArgs e)
        {
            //await drone.FlipLeft();
            await drone.Flip(Direction.Left);
            Console.Text += string.Format("Drone flip left. \n");
        }

        private async void FlipForward_OnCLick(object sender, RoutedEventArgs e)
        {
            //await drone.FlipForward();
            await drone.Flip(Direction.Front);
            Console.Text += string.Format("Drone flip forward. \n");
        }

        private async void Right_OnClick(object sender, RoutedEventArgs e)
        {
            //await drone.Right();
            await drone.Move(Direction.Right, 30);
            Console.Text += string.Format("Drone move right. \n");
        }

        private async void Up_OnClick(object sender, RoutedEventArgs e)
        {
            //await drone.Up();
            await drone.Move(Direction.Up, 30);
            Console.Text += string.Format("Drone move up. \n");
        }

        private async void Down_OnClick(object sender, RoutedEventArgs e)
        {
            //await drone.Down();
            await drone.Move(Direction.Down, 30);
            Console.Text += string.Format("Drone move down. \n");
        }
    }
}
