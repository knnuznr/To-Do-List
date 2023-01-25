﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Windows.Forms;
using System.Security.Cryptography;

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
		
		private void Form1_Load(object sender, EventArgs e)
		{
			XmlDocument doc = new XmlDocument();
			doc.Load("checkboxdata.xml");
			XmlNodeList checkboxNodes = doc.SelectNodes("CheckboxData/Checkbox");
			foreach (XmlNode node in checkboxNodes)
			{
				CheckBox cb = new CheckBox();
				cb.Text = node.InnerText;
				cb.Left = 10;
				cb.ForeColor = Color.White;
				cb.Font = new Font("Segoe UI", 12);
				cb.Top = 10;
				CheckBoxes.Add(cb);
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
				checkboxElement.InnerText = cb.Text;
				root.AppendChild(checkboxElement);
			}
			doc.Save("checkboxdata.xml");

		}

		List<CheckBox> CheckBoxes = new List<CheckBox>();
		public void textBox1_KeyUp(object sender, KeyEventArgs e)
		{
			int count = 11;
			if(CheckBoxes.Count >= count)
			{
				MessageBox.Show("Test");
			}
			else
			if (e.KeyCode == Keys.Return)
			{
				if(textBox1.Text == "")
				{
					MessageBox.Show("Test");
				}
				else if(textBox1.Text != "")
				{
					CheckBox Mycheckbox = new CheckBox();
					Mycheckbox.Text = textBox1.Text;
					Mycheckbox.Height = 50;
					Mycheckbox.AutoSize = true;
					Mycheckbox.Width = 100;
					Mycheckbox.Location = new Point(200, 200);
					Mycheckbox.ForeColor = Color.White;
					Mycheckbox.Font = new Font("Segoe UI", 12);
					CheckBoxes.Add(Mycheckbox);
					int y = 10;
					int x = 200;
					foreach (var checkbox in CheckBoxes)
					{
						checkbox.CheckedChanged += checkbox_CheckedChanged;
						checkbox.Location = new Point(x, y);
						y += checkbox.Height + 10;
					}
					this.Controls.AddRange(CheckBoxes.ToArray());
					textBox1.Clear();
				}
				
			}
		}
		private void checkbox_CheckedChanged(object sender, EventArgs e)
		{
			CheckBox checkbox = sender as CheckBox;
			if (checkbox.Checked)
			{
				checkbox.Font = new Font("Segoe UI", 12, FontStyle.Strikeout);
			}
			else
			{
				checkbox.Font = new Font("Segoe UI", 12);
			}
		}

		private void pictureBox1_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}