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
        //Временный словарь, который затем редактируется и вносится в необходимый файл конфигураций
        public Dictionary<String, Dictionary<String, String>> tempParamBlocks = new Dictionary<string, Dictionary<string, string>>();
        //Массив CheckBox'ов, в котором просто записаны все CheckBox'ы
        public List<CheckBox> checkBoxList = new List<CheckBox>();
        //Массив TextBox'ов в котором просто записаны все TextBox'ы
        public List<TextBox> textBoxList = new List<TextBox>();
        //Словарь в котором хранится пара: CheckBox:Блок к которому он относится(необходимо для четкой записи
        public Dictionary<CheckBox, String> mapCheckBox = new Dictionary<CheckBox, String>();

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
                    //  String localVars = @"C:\ProgramData\MySQL\MySQL Server 5.5\my.ini";
                    String localVars = @"C:\Users\vladi\Desktop\Самотуга\BOS8\Local configuration file.ini";
                    Dictionary<String, Dictionary<String, String>> local = controller.LoadLocalConfiguration(localVars);
                    tempParamBlocks = controller.LoadLocalConfiguration(localVars);
                    int i = 0;
                    Point currentLocationBlockName;

                    foreach (String blockName in local.Keys)
                    {
                        i += 30;
                        currentLocationBlockName = new Point(0, 55 + i);
                        Label blockNameLabel = new Label();
                        blockNameLabel.Location = currentLocationBlockName;
                        blockNameLabel.Text = blockName;
                        panel1.Controls.Add(blockNameLabel);
                        Point currentLocation;
                        foreach (String paramName in local[blockName].Keys)
                        {
                            i += 30;
                            CheckBox enableParamBox = new CheckBox();
                            enableParamBox.Text = paramName;
                            enableParamBox.Enabled = true;
                            enableParamBox.Size = new Size(200, 25);
                            enableParamBox.Parent = this;
                            enableParamBox.Visible = true;
                            currentLocation = new Point(0, 45 + i);
                            enableParamBox.Location = currentLocation;

                            TextBox paramVarBox = new TextBox();
                            paramVarBox.Size = new Size(200, 25);
                            paramVarBox.Location = new Point(enableParamBox.Location.X + 200, enableParamBox.Location.Y);
                            if (local[blockName][paramName] != null)
                            {
                                paramVarBox.Text = local[blockName][paramName];

                            }
                            else
                            {
                                paramVarBox.Visible = false;
                            }

                            checkBoxList.Add(enableParamBox);
                            mapCheckBox.Add(enableParamBox, blockName);

                            textBoxList.Add(paramVarBox);

                            panel1.Controls.Add(paramVarBox);
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


        //Метод, который по нажатию кнопки изменяет словарь (удаляет неотмеченные чекбоксы и перезаписывает значения (если такие имеются) у отмеченых чекбоксов)
        private void button2_Click(object sender, EventArgs e)
        {
            foreach (String blockName in tempParamBlocks.Keys)
            {
                int i = 0;
                // for (int i = 0; i < checkBoxList.Count; i++)
                // {                  
                //       if (!checkBoxList[i].Checked)
                //        {
                //             tempParamBlocks[blockName].Remove(checkBoxList[i].Text);
                //         }
                //         else
                //         {
                //             if(textBoxList[i].Visible)
                //            tempParamBlocks[blockName][checkBoxList[i].Text] = textBoxList[i].Text;
                //       }
                foreach (CheckBox tempCheckBox in mapCheckBox.Keys)
                {
                    if (!tempCheckBox.Checked && blockName.Equals(mapCheckBox[tempCheckBox]))
                    {
                        tempParamBlocks[blockName].Remove(tempCheckBox.Text);
                    }
                    else if (tempCheckBox.Checked && blockName.Equals(mapCheckBox[tempCheckBox]))
                    {
                        if (textBoxList[i].Visible)
                        {
                            tempParamBlocks[blockName][tempCheckBox.Text] = textBoxList[i].Text;
                        }

                    }
                    i += 1;
                }
            }
            //Короче хуй знает как там сделать чтобы он в тот файл записывал, на данном этапе он какой-то новый создаёт, я в твоем коде не зашарил
            IniLoader.saveFile(tempParamBlocks);
            
        }
    }

}
