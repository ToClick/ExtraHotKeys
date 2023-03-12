using ExtraHotKeys.Utils;
using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ToggleSwitch;
using ExtraHotKeys.Utils;


namespace ExtraHotKeys
{
    public partial class Main : Form
    {
        private Thread GotFocusThread;
        private Config Config;
        private Autorun Autorun;

        public Main()
        {
            InitializeComponent();
            
            textBoxKey1.GotFocus += GotFocusEvent;
            textBoxKey2.GotFocus += GotFocusEvent;
            textBoxKey3.GotFocus += GotFocusEvent;
            textBoxKey4.GotFocus += GotFocusEvent;
            textBoxKey5.GotFocus += GotFocusEvent;
            textBoxKey6.GotFocus += GotFocusEvent;

            textBoxKey1.LostFocus += LostFocusEvent;
            textBoxKey2.LostFocus += LostFocusEvent;
            textBoxKey3.LostFocus += LostFocusEvent;
            textBoxKey4.LostFocus += LostFocusEvent;
            textBoxKey5.LostFocus += LostFocusEvent;
            textBoxKey6.LostFocus += LostFocusEvent;

            textBoxKey1.TextChanged += OnSelectKey;
            textBoxKey2.TextChanged += OnSelectKey;
            textBoxKey3.TextChanged += OnSelectKey;
            textBoxKey4.TextChanged += OnSelectKey;
            textBoxKey5.TextChanged += OnSelectKey;
            textBoxKey6.TextChanged += OnSelectKey;

            btnKey1.Click += OnSelectApp;
            btnKey2.Click += OnSelectApp;
            btnKey3.Click += OnSelectApp;
            btnKey4.Click += OnSelectApp;
            btnKey5.Click += OnSelectApp;
            btnKey6.Click += OnSelectApp;

            toggleSwitch1.CheckedChanged += OnSelectEnable;
            toggleSwitch2.CheckedChanged += OnSelectEnable;
            toggleSwitch3.CheckedChanged += OnSelectEnable;
            toggleSwitch4.CheckedChanged += OnSelectEnable;
            toggleSwitch5.CheckedChanged += OnSelectEnable;
            toggleSwitch6.CheckedChanged += OnSelectEnable;


            this.Resize += delegate(object sender, EventArgs args)
            {
                this.ShowInTaskbar = WindowState != FormWindowState.Minimized;
            };
        }

        private void Main_Load(object sender, EventArgs e)
        {
            Autorun = new Autorun("ExtraHotkey", Application.ExecutablePath);
            toggleAutorun.Checked = Autorun.IsInstalled();

            Config = new Config();

            var controls = tableLayoutPanel1.Controls;
            
   
            foreach (var action in Config.GetAll())
            {
                if (action.keys != null)
                {
                    TextBox text = controls.OfType<TextBox>().Single(x1 => x1.TagInt() == action.id);
                    VKeys[] xkeys = action.keys.Select(xkey => (VKeys)xkey).ToArray();
                    text.Text = xkeys.ToActionString();
                }

                
                if (!string.IsNullOrEmpty(action.app))
                {
                    var button = controls.OfType<Button>().Single(x1 => x1.TagInt() == action.id);
                    button.Width = 144;
                    button.Text = new FileInfo(action.app).Name;
                }


                var toggle = controls.OfType<JCS.ToggleSwitch>().Single(x1 => x1.TagInt() == action.id);
                toggle.Checked = action.enable;


                Hook.RegisterAction(action);

            }

            Hook.Attach();

#if DEBUG
            new Thread(debugCycle) { IsBackground = true }.Start();
#endif

        }

        private void GotFocusEvent(object sender, EventArgs e)
        {
            Hook.Enable = false;
            GotFocusThread = new Thread(()=> CatchFocus(sender as TextBox)).StartBackground();
        }

        private void LostFocusEvent(object sender, EventArgs e)
        {
            GotFocusThread.AbortSafe();
            Hook.Enable = true;
        }

        private void CatchFocus(TextBox textBox)
        {
            int count = 0;

            try
            {
                while (true)
                {
                    Thread.Sleep(10);

                    var keys = Hook.GetPressed();
                    int nowCount = keys.Count;

                    if (nowCount == 0)
                    {
                        count = 0;
                        continue;
                    }

                    if (nowCount < count)
                    {
                        continue;
                    }

                    count = nowCount;

                    textBox.Invoke(new MethodInvoker(() => textBox.Text = keys.ToArray().ToActionString()));
                    
                }

            }
            catch (ThreadAbortException e)
            {
                Thread.ResetAbort();
            }
            catch (Exception e)
            {
                MessageBox.Show("Catch Error: \n" + e.Message);
            }
        }

        private void OnSelectApp(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            var Id = btn.TagInt();

            OpenFileDialog fileOpen = new OpenFileDialog();

            if (fileOpen.ShowDialog() == DialogResult.OK)
            {
                string AppPath = fileOpen.FileName;
                string Name = new FileInfo(AppPath).Name;

                btn.Text = Name;
                btn.Width = 144;

                Config.SetApp(btn.TagInt(), AppPath);
            }
            else
            {
                btn.Text = "...";
                btn.Width = 39;
                Config.SetApp(btn.TagInt(), "...");
            }
        }

        private void OnSelectEnable(object sender, EventArgs e)
        {
            var toggle = (sender as JCS.ToggleSwitch);

            Config.SetEnable(toggle.TagInt(), toggle.Checked);
        }

        private void OnSelectKey(object sender, EventArgs e)
        {
            var box = (sender as TextBox);
            var keys = box?.Text.ParseFromAction();

            Config.SetKeys(box.TagInt(), keys);
        }

        private void debugCycle()
        {
            try
            {
                string text = "";

                while (true)
                {
                    Thread.Sleep(1);

                    if (Hook.DebugPop(out text))
                    {
                        textBox1.Invoke(new MethodInvoker(() => textBox1.Text = text));
                        richTextBox1.Invoke(new MethodInvoker(() => richTextBox1.Text = (text + richTextBox1.Text)));
                    }

                    textBox2.Invoke(new MethodInvoker(() => textBox2.Text = Hook.DebugState()));
                }
            }
            catch
            {

            }

        }

        private void btnAddColumn_Click(object sender, EventArgs e)
        {
            tableLayoutPanel1.RowCount++;
        }

        private void toggleSwitch1_CheckedChanged(object sender, EventArgs e)
        {
         
        }

        private void systemTray_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            this.Activate();
            this.Focus();
        }

        private void toggleAutorun_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (toggleAutorun.Checked)
                {
                    Autorun.Install();
                }
                else
                {
                    Autorun.Remove();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Autorun", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }
    }
}
