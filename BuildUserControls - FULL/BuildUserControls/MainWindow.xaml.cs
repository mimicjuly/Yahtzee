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

namespace BuildUserControls
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static int Counter = 0;
        static Random random = new Random(); 
        public string Total
        {
            get { return (string)GetValue(TotalProperty); }
            set { SetValue(TotalProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Total.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TotalProperty =
            DependencyProperty.Register("Total", typeof(string), typeof(MainWindow));

        
        public MainWindow()
        {
            InitializeComponent();

            // Attached event אלטרנטיבה לכתיבה בזמל 
            this.AddHandler(Dice.StopRollEvent, new RoutedPropertyChangedEventHandler<int>((X, Y) => { SumItUp(); }));
            this.DataContext = this;
            
        }

      

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Dice d = e.Source as Dice;
            d.StartRoll();
            Counter++;

        }


        private void SumItUp()
        {

            if (--Counter == 0)
            {
                Total = "YOU WIN!!!!!!!!";
                var Dices = FindVisualChildren<Dice>(this);
                int sum = fruit1.State;
                foreach (var dice in Dices)
                {
                    if (dice.State!=sum)
                        Total = "YOU LOSE!\n :( :'( :(";
                }
            }

        }

        #region Helper Funtion To find all instances of Control
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
       #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void start_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void start_Click_1(object sender, RoutedEventArgs e)
        {
            fruit1.StartRoll();
            fruit2.StartRoll();
            fruit3.StartRoll();
            Counter = 3;
            //scoreSheet.getSuggestion(4, 2, 5, 4, 5);
            
        }

        private void fruit3_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void ScoreSheet_Loaded(object sender, RoutedEventArgs e)
        {

        }
        

    }
}
