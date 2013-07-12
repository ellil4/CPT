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
using System.Timers;
using System.Windows.Threading;

namespace CPT
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        bool ENGINEERING = false;

        static int TEST_SPAN_COUNT = 1;
        static int REAL_SPAN_COUNT = 28;

        static int ITEMS_IN_SPAN = 9;
        static int SEMI_SPAN_COUNT = REAL_SPAN_COUNT;

        static int SHOW_TIME = 500;
        static int GAP_TIME = 1000;
        static int TITLE_WIDTH = 404;
        static int TITLE_HEIGHT = 50;

        private int mCurItem = 0;
        private int mCurSpan = 0;

        private List<bool> mCurStiList;
        private List<StAnswer> mAnswers;
        private StAnswer mCurAns;

        private bool mFirstHalf = true;
        private FEITTimer mTimer;

        private ComTarget mComTar;

        private bool begun = false;

        private void next()
        {
            //new span
            if (mCurItem >= ITEMS_IN_SPAN)
            {
                mCurSpan++;
                mCurItem = 0;

                //new half
                if (mCurSpan >= SEMI_SPAN_COUNT)
                {
                    if (mFirstHalf)
                    {
                        mCurSpan = 0;
                        mFirstHalf = false;
                    }
                    else
                    {
                        testEnd();
                    }
                }

                if (mFirstHalf)
                {
                    mCurStiList = genSpan(2, 7, true);
                }
                else
                {
                    mCurStiList = genSpan(7, 2, true);
                }
            }

            showSti(mCurStiList[mCurItem]);
            mCurItem++;

        }

        private void showSti(bool isTarget)
        {
            mTimer.Stop();
            mTimer.Reset();
            mTimer.Start();

            if (isTarget)
            {
                mComTar.SetState(ComState.TARGET);
                mCurAns.Type = ITEM_TYPE.TARGET;
            }
            else
            {
                mComTar.SetState(ComState.NON_TARGET);
                mCurAns.Type = ITEM_TYPE.NON_TARGET;
            }

            Timer t = new Timer();
            t.Interval = SHOW_TIME;
            t.AutoReset = false;
            t.Enabled = true;
            t.Elapsed += new ElapsedEventHandler(t_after_showing);

        }

        private void showBlank()
        {
            mComTar.SetState(ComState.OFF);

            Timer t = new Timer();
            t.Interval = GAP_TIME;
            t.AutoReset = false;
            t.Enabled = true;
            t.Elapsed += new ElapsedEventHandler(t_after_blank);
        }

        public delegate void invokeDelegate();

        void t_after_blank(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(DispatcherPriority.Normal, new invokeDelegate(next));
        }

        void t_after_showing(object sender, ElapsedEventArgs e)
        {
            mAnswers.Add(mCurAns);
            mCurAns = new StAnswer();
            Dispatcher.Invoke(DispatcherPriority.Normal, new invokeDelegate(showBlank));
        }

        private void testEnd()
        {
            //write
            String stampFilename = GetStamp() + ".txt";
            new Writer(stampFilename, mAnswers);
            //Exit
            System.Environment.Exit(0);
        }






        public MainWindow()
        {
            InitializeComponent();
            setFullScreen();
            setCanvas();

            mComTar = new ComTarget();
            mCurStiList = genSpan(2, 7, true);
            mTimer = new FEITTimer();
            mAnswers = new List<StAnswer>();
            mCurAns = new StAnswer();
            this.Cursor = Cursors.None;
        }

        private void setFullScreen()
        {
            this.WindowState = System.Windows.WindowState.Normal;
            if (!ENGINEERING)
            {
                this.WindowStyle = System.Windows.WindowStyle.None;
                this.ResizeMode = System.Windows.ResizeMode.NoResize;
                this.Topmost = true;
            }
            else
            {
                this.WindowStyle = System.Windows.WindowStyle.ToolWindow;
                this.ResizeMode = System.Windows.ResizeMode.CanResize;
                this.Topmost = false;
            }

            this.Left = 0;
            this.Top = 0;
            this.Width = System.Windows.SystemParameters.PrimaryScreenWidth;
            this.Height = System.Windows.SystemParameters.PrimaryScreenHeight;
        }

        private void setCanvas()
        {
            amCanvas.Width = this.Width;
            amCanvas.Height = this.Height;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            amCanvas.Children.Add(mComTar);
            Canvas.SetLeft(mComTar, (this.Width - ComTarget.SIZE) / 2);
            Canvas.SetTop(mComTar, (this.Height - ComTarget.SIZE) / 2);
            mComTar.SetState(ComState.OFF);
            amEllipse.Visibility = Visibility.Hidden;

            Canvas.SetLeft(amTitle, (this.Width - TITLE_WIDTH) / 2);
            Canvas.SetTop(amTitle, (this.Height - TITLE_HEIGHT) / 2);
        }




        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            amEllipse.Visibility = Visibility.Visible;
            
            if(begun)
                mCurAns.PressTime.Add(mTimer.GetElapsedTime());
        }

        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            amEllipse.Visibility = Visibility.Hidden;
        }

        private static List<bool> genSpan(int set1Count, int set2Count, bool set1Flag)
        {
            List<bool> retval = new List<bool>();

            for (int i = 0; i < set1Count; i++)
            {
                retval.Add(set1Flag);
            }

            Random rdm = new Random();

            for (int j = 0; j < set2Count; j++)
            {
                retval.Insert(rdm.Next(0, retval.Count + 1), !set1Flag);
            }

            return retval;
        }

        public static String GetStamp()
        {
            DateTime dt = System.DateTime.Now;
            String str = "";

            str += dt.Year.ToString("0000");
            str += dt.Month.ToString("00");
            str += dt.Day.ToString("00");
            str += dt.Hour.ToString("00");
            str += dt.Minute.ToString("00");
            str += dt.Second.ToString("00");

            return str;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (!begun)
            {
                if (e.Key == Key.Space)
                {
                    begun = true;
                    amTitle.Visibility = Visibility.Collapsed;
                    next();
                }
            }

            if (e.Key == Key.Enter)
            {
                System.Environment.Exit(0);
            }
        }
    }
}
