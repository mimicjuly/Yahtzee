using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.ComponentModel;

namespace BuildUserControls
{
    /// <summary>
    /// Interaction logic for Dice.xaml
    /// </summary>
    public partial class Dice : UserControl
    { 
        // Collection of possible Dice Faces
        private static List<BitmapImage> Images;
        private static int number = 0;
        public int id = 0;
        static Dice()
        {
            //Load Images
            Images = new List<BitmapImage>();
            BitmapImage img = null;
            for (int i = 1; i <= 6; i++)
            {
                img = new BitmapImage();
                img.BeginInit();
                img.UriSource = new Uri(@"/BuildUserControls;component/Images/dice-" + i.ToString() + ".png", UriKind.RelativeOrAbsolute);
                img.CacheOption = BitmapCacheOption.OnDemand;
                img.EndInit();
                Images.Add(img);
            }

            // פקודת התחלת זריקה
            StartCommand = new RoutedCommand("Start", typeof(Dice));
        }
        public Dice()
        {
            InitializeComponent();
            // Timer setup - Dispatcher Timer Will not block The Main Thread
            id = ++number;
            Timer = new DispatcherTimer();
            Timer.Tick += new EventHandler(Timer_Tick);
            Timer.Interval = new TimeSpan(0, 0, 0, 0, 50);
        }
        static Random random = new Random(); 
        private DispatcherTimer Timer;
        private int CountRolls = 0;
        private int MaxRoll = 20;        
    void Timer_Tick(object sender, EventArgs e)
        {
            if (++CountRolls > MaxRoll)
            {
                Timer.Stop();
                CountRolls = 0;
                // הקפצת אירוע סיום זריקה
                RoutedPropertyChangedEventArgs<int> newEventArgs = new RoutedPropertyChangedEventArgs<int>(id, State, Dice.StopRollEvent);
                RaiseEvent(newEventArgs); 
            }
            else
            {
                 this.State=CountRolls;             
            }
        }
    public int State
    {
        get { return (int)GetValue(StateProperty); }
        set { SetValue(StateProperty, value); }
    }
     public static readonly DependencyProperty StateProperty =
                DependencyProperty.Register("State", typeof(int), typeof(Dice),
                new FrameworkPropertyMetadata(3, FrameworkPropertyMetadataOptions.AffectsRender,
                                                new PropertyChangedCallback(StateChangedCallBack),
                                                new CoerceValueCallback(FixValueCallBack))
                                           );
         // הזדמנות להגיב לשינוי בתכונה
         public static void StateChangedCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
         {
             Dice temp = d as Dice;
             if (temp != null)
             {
                 temp.DiceImage.Source = Images[Convert.ToInt32(e.NewValue)];
             }          
         }
         // בדיקת הערכים שהושמו והזדמנות לתקנם
         public static object FixValueCallBack(DependencyObject d, object baseValue)
         {
             int val=0;
             try
             {
                 val = (Convert.ToInt32(baseValue)%6);              
             }
             catch
             {
                 throw new ArgumentOutOfRangeException("הערך State אינו מספר המתאים לקוביה , רק שלם חיובי");
             }
             return val;
         }
        #region אירוע סיום זריקה
         public static readonly RoutedEvent StopRollEvent = EventManager.RegisterRoutedEvent(
         "StopRoll", RoutingStrategy.Tunnel, typeof(RoutedPropertyChangedEventHandler<int>), typeof(Dice));

         // Provide CLR accessors for the event
         public event RoutedPropertyChangedEventHandler<int> StopRoll
         {
             add { AddHandler(StopRollEvent, value); }
             remove { RemoveHandler(StopRollEvent, value); }
         }
        #endregion
         #region פקודת אתחול זריקה
         private static readonly RoutedCommand StartCommand;

         public static RoutedCommand Start
         {
             get { return Dice.StartCommand; }
         }
         #endregion
         public void StartRoll()
         {
             int min = random.Next(0, 10);
             int max = random.Next(26, 52);
             MaxRoll = random.Next(min, max);
             Timer.Start();          
         }
    }
}
