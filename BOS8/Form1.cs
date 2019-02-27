using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace BOS8
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Файлы конфигурации MySQL|*.ini";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    IniController controller = new IniController();
                    Dictionary<String, Dictionary<String, String>> ini = controller.LoadIni(ofd.FileName);
                    String localVars = @"C:\ProgramData\MySQL\MySQL Server 5.5\my.ini";
                //    String localVars = @"C:\Users\vladi\Desktop\Самотуга\BOS8\Local configuration file.ini";
                    Dictionary<String, Dictionary<String, String>> local = controller.LoadLocalConfiguration(localVars);
                    foreach (String blockName in local.Keys)
                    {
                        Label blockNameLabel = new Label();
                        blockNameLabel.Text = blockName;
                        panel1.Controls.Add(blockNameLabel);
                        int i = 0;
                        Point currentLocation ;
                        foreach (String paramName in local[blockName].Keys)
                        {
                            i += 30;
                            CheckBox enableParamBox = new CheckBox();
                            enableParamBox.Text = paramName;
                            enableParamBox.Enabled = true;
                            enableParamBox.Size = new Size(200,25);
                            enableParamBox.Parent = this;
                            enableParamBox.Visible = true;
                            currentLocation = new Point(0,95+i);
                            enableParamBox.Location = currentLocation;
                            if (local[blockName][paramName] != null)
                            {
                                TextBox paramVarBox = new TextBox();
                                paramVarBox.Text = local[blockName][paramName];
                                paramVarBox.Size = new Size(200, 25);
                                paramVarBox.Location = new Point(enableParamBox.Location.X+200,enableParamBox.Location.Y);
                                panel1.Controls.Add(paramVarBox);
                            }
                            
                            panel1.Controls.Add(enableParamBox);
                        }
                    }
                    // Dictionary<String, Dictionary<String, String>> result = controller.SetUpdedParams(local, ini);
                    //  IniLoader.saveFile(result);


                }
                catch (Exception ex)
                {
                    MessageBox.Show("Невозможно открыть выбранный файл " + ex.Message + ex.GetType(), "Ошибка",
                     MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }





    }
}
