using System;
using System.Collections.Generic;
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
namespace BuildUserControls
{
	/// <summary>
	/// Interaction logic for CountDown.xaml
	/// </summary>
	public partial class CountDown : UserControl
	{
		public event EventHandler TimerEnded;
		private int time= 5;
		public DispatcherTimer timer;
		public CountDown()
		{
			this.InitializeComponent();
			timer = new DispatcherTimer();
			timer.Interval = new TimeSpan(0,0,1);
			timer.Tick+= Timer_Tick;
			//timer.Start();
		}
		void Timer_Tick(object sender, EventArgs e)
		{
			if(time>0)
			{
				textBoxTimer.Text=string.Format("Next In 00:0{0}",time);
				time--;
			}
			else
			{
				timer.Stop();
				OnTimerEnded(null);
				this.Visibility = Visibility.Hidden;
			}
		}
		public void start()
		{
			time=4;
			textBoxTimer.Text="Next In 00:05";
			timer.Start();
			this.Visibility = Visibility.Visible;
		}
		 protected virtual void OnTimerEnded(EventArgs e)
        {
            EventHandler handler = TimerEnded;
            if (handler != null)
            {
                handler(this, e);
            }
        }
	}
}