using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Windows.Forms;

namespace To_Do_List
{
	
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}
		int fMove;
		int fMouse_X;
		int fMouse_Y;
		private void Form1_MouseMove(object sender, MouseEventArgs e)
		{
			if (fMove == 1)
			{
				this.SetDesktopLocation(MousePosition.X - fMouse_X, MousePosition.Y - fMouse_Y);
			}
		}

		private void Form1_MouseUp(object sender, MouseEventArgs e)
		{
			fMove = 0;
		}

		private void Form1_MouseDown(object sender, MouseEventArgs e)
		{
			fMove = 1;
			fMouse_X = e.X;
			fMouse_Y = e.Y;
		}
		public int count = 0, ccount = 0;
		private void Form1_Load(object sender, EventArgs e)
		{
			
			XmlDocument doc = new XmlDocument();
			doc.Load("checkboxdata.xml");
			XmlNodeList checkboxNodes = doc.SelectNodes("CheckboxData/Checkbox");
			foreach (XmlNode node in checkboxNodes)
			{
				XmlNode textNode = node.SelectSingleNode("text");
				XmlNode locationNode = node.SelectSingleNode("location");
				XmlNode heightNode = node.SelectSingleNode("height");
				XmlNode statusNode = node.SelectSingleNode("status");

				CheckBox cb = new CheckBox
				{
					Text = textNode.InnerText,
					Location = new Point(Convert.ToInt32(locationNode.InnerText.Split(',')[0].Split('=')[1]), Convert.ToInt32(locationNode.InnerText.Split(',')[1].Split('=')[1].Replace("}", ""))),
					ForeColor = Color.White,
					Checked = Convert.ToBoolean(statusNode.InnerText.ToString()),
					Height = Convert.ToInt32(heightNode.InnerText),
					Font = new Font("Arial Rounded MT Bold", 12),
				};
				if(cb.Checked)
				{
					cb.Font = new Font("Arial Rounded MT Bold", 12,FontStyle.Strikeout);
					ccount++;
				}
				else
				{
					cb.Font = new Font("Arial Rounded MT Bold", 12);
				}
				CheckBoxes.Add(cb);
				this.Controls.AddRange(CheckBoxes.ToArray());
				count = CheckBoxes.Count;
				
				label2.Text = count.ToString() + " of " + ccount + " completed.";
			}
		}
		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			XmlDocument doc = new XmlDocument();
			XmlElement root = doc.CreateElement("CheckboxData");
			doc.AppendChild(root);
			foreach (CheckBox cb in CheckBoxes)
			{
				XmlElement checkboxElement = doc.CreateElement("Checkbox");

				XmlElement textElement = doc.CreateElement("text");
				textElement.InnerText = cb.Text;
				checkboxElement.AppendChild(textElement);

				XmlElement locationElement = doc.CreateElement("location");
				locationElement.InnerText = cb.Location.ToString();
				checkboxElement.AppendChild(locationElement);

				XmlElement heightElement = doc.CreateElement("height");
				heightElement.InnerText = cb.Height.ToString();
				checkboxElement.AppendChild(heightElement);

				XmlElement statusElement = doc.CreateElement("status");
				statusElement.InnerText = cb.Checked.ToString();
				checkboxElement.AppendChild(statusElement);

				root.AppendChild(checkboxElement);
			}
			doc.Save("checkboxdata.xml");
		}

		List<CheckBox> CheckBoxes = new List<CheckBox>();
		
		public void textBox1_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Return)
			{
				if(textBox1.Text == "")
				{
					MessageBox.Show("Test");
				}
				else if(textBox1.Text != "")
				{
					CheckBox Mycheckbox = new CheckBox
					{
						Text = textBox1.Text,
						Height = 50,
						AutoSize = true,
						Width = 100,
						Location = new Point(200, 200),
						ForeColor = Color.White,
						Font = new Font("Arial Rounded MT Bold", 12)
					};
					
					CheckBoxes.Add(Mycheckbox);
					
					int x = 250, y = 20;
					foreach (var checkbox in CheckBoxes)
					{
						checkbox.CheckedChanged += checkbox_CheckedChanged;
						checkbox.GotFocus += Checkbox_GotFocus;
						checkbox.Location = new Point(x, y);
						y += checkbox.Height + 10;
					}
					this.Controls.AddRange(CheckBoxes.ToArray());
					count++;
					label2.Text = count.ToString() + " of " + ccount.ToString() + " completed.";
					textBox1.Clear();
				}
				
			}
		}

		private void Checkbox_GotFocus(object sender, EventArgs e)
		{
			CheckBox checkbox = sender as CheckBox;
			if(checkbox.Focused == true)
			{
				textBox1.Focus();
			}
		}
		private void checkbox_CheckedChanged(object sender, EventArgs e)
		{

			CheckBox checkbox = sender as CheckBox;
			if (checkbox.Checked)
			{
				checkbox.Font = new Font("Arial Rounded MT Bold", 12, FontStyle.Strikeout);
				System.Media.SoundPlayer player = new System.Media.SoundPlayer
				{
					SoundLocation = "C:\\Users\\Kaan UZUNER\\source\\repos\\To-Do-List\\Resources\\blink.wav"
				};
				player.Play();
				XmlDocument doc = new XmlDocument();
				doc.Load("checkboxdata.xml");
				XmlNodeList checkboxNodes = doc.SelectNodes("CheckboxData/Checkbox");
				foreach (XmlNode node in checkboxNodes)
				{
					if (node["status"].InnerText == "True")
					{
						checkbox.Font = new Font("Arial Rounded MT Bold", 12, FontStyle.Strikeout);
					}
				}
				doc.Save("checkboxdata.xml");
				ccount++;
				label2.Text = count.ToString() + " of " + ccount.ToString() + " completed.";
			}
			else if(checkbox.Checked == false)
			{
				checkbox.Font = new Font("Arial Rounded MT Bold", 12);
				XmlDocument doc = new XmlDocument();
				doc.Load("checkboxdata.xml");
				XmlNodeList checkboxNodes = doc.SelectNodes("CheckboxData/Checkbox");
				foreach (XmlNode node in checkboxNodes)
				{
					if (node["status"].InnerText == "False")
					{
						checkbox.Font = new Font("Arial Rounded MT Bold", 12);
					}
				}
				doc.Save("checkboxdata.xml");
				ccount--;
				label2.Text = count.ToString() + " of " + ccount.ToString() + " completed.";
			}
		}

		private void pictureBox1_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void pictureBox2_Click(object sender, EventArgs e)
		{
			this.WindowState = FormWindowState.Minimized;
		}
	}
}