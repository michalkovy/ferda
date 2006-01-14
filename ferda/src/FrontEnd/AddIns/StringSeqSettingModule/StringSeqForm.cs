
using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

namespace Ferda.FrontEnd
{
	public class StringSeqForm : Form
	{
		public StringSeqForm()
		{
			// We use our own scaling
			AutoScale = false;

			// Title
			Text = GetLocalizedText(TITLE_TEXT);

			// Initialize controls
			Initialize();

			// Add controls
			panel1.Dock = DockStyle.Fill;
			Controls.Add(panel1);

			// Set form size
			Size minSize = PreferredSize;
			Size screenSize = Screen.PrimaryScreen.Bounds.Size;
			Size newSize = windowSize;
			if (newSize.Height < minSize.Height)
				newSize.Height = minSize.Height;
			if (newSize.Width < minSize.Width)
				newSize.Width = minSize.Width;
			if (newSize.Height > screenSize.Height)
				newSize.Height = screenSize.Height;
			if (newSize.Width > screenSize.Width)
				newSize.Width = screenSize.Width;
			MinimumSize = minSize;
			Size = newSize;

			// TODO: add your own initialization code & event handlers here

			textfield.TextChanged += delegate(Object sender, EventArgs e)
			{
				if(textfield.Text.Trim() == "")
				{
					buttonAdd.Enabled = false;
				}
				else
				{
					buttonAdd.Enabled = true;
				}
			};

			cancelButton.Click += delegate(Object sender, EventArgs e)
			{
				DialogResult = DialogResult.Cancel;
			};

			okButton.Click += delegate(Object sender, EventArgs e)
			{
				DialogResult = DialogResult.OK;
			};

			button1.Click += delegate(Object sender, EventArgs e)
			{
				int selectedIndex = listview.SelectedIndex;
				if(selectedIndex >= 0)
					listview.Items.RemoveAt(selectedIndex);
			};

			buttonAdd.Click += delegate(Object sender, EventArgs e)
			{
				listview.Items.Add(textfield.Text);
				textfield.Text = "";
			};
		}

		public String GetLocalizedText(String text)
		{
			return text;
		}

		public string[] Items
		{
			get
			{
				ListBox.ObjectCollection objects = this.listview.Items;
				int number = objects.Count;
				string[] result = new string[number];
				int i = 0;
				foreach(Object objectString in objects)
				{
					result[i] = objectString.ToString();
					i++;
				}
				return result;
			}

			set
			{
				foreach(string val in value)
				{
					this.listview.Items.Add(val);
				}
			}
		}

		#region Components (X-develop WindowsForms 2.0 designer code)

		private Button button1; // Button,Remove,4
		private TextBox textfield; // Textbox,"",2,,,1,,,,,Yes
		private Button buttonAdd; // Button,Add,2,,1,1
		private Button okButton; // Button,OK,6,,1
		private Button cancelButton; // Button,Cancel,6,,2

		#endregion

		#region Panels (X-develop WindowsForms 2.0 designer code)

		private TableLayoutPanel panel1; // Panel,Setting of sequence of strings
		private TableLayoutPanel panel5; // Panel,,1,,,,4,4,4,4
		private ListBox listview; // List,,2
		private TableLayoutPanel panel6; // Panel,,2,,1
		private Panel panel7; // Panel,,4,,,1
		private TableLayoutPanel panel2; // Panel,,1,,,1,4,4,4,4
		private Panel panel3; // Horizontal Space,,6,,,,,,,,,No
		private Panel panel4; // Horizontal Space,,6,,3,,,,,,,No

		#endregion

		#region Implementation (X-develop WindowsForms 2.0 designer code)

		private const String TITLE_TEXT = "Setting of sequence of strings";

		private const double ASPECT_RATIO = 1.2317073170731707; // Aspect Ratio
		private const double SCREEN_AREA_RATIO = 0.07108497619628906; // Screen Area Ratio

		private void Initialize()
		{
			CalculateScreenSize();
			CalculateWindowSize();
			CreateComponents();
			InitComponents();
			LayoutComponents();
		}

		private void CreateComponents()
		{
			panel1 = new TableLayoutPanel();
			panel5 = new TableLayoutPanel();
			listview = new ListBox();
			panel6 = new TableLayoutPanel();
			button1 = new Button();
			panel7 = new Panel();
			textfield = new TextBox();
			buttonAdd = new Button();
			panel2 = new TableLayoutPanel();
			panel3 = new Panel();
			okButton = new Button();
			cancelButton = new Button();
			panel4 = new Panel();
		}

		private void InitComponents()
		{
			panel1.RowCount = 2;
			panel1.ColumnCount = 1;
			panel1.AutoSize = true;
			panel5.RowCount = 2;
			panel5.ColumnCount = 2;
			panel5.AutoSize = true;
			listview.Font = new Font(listview.Font.FontFamily, listview.Font.Size, listview.Font.Style);
			listview.AutoSize = true;
			panel6.RowCount = 2;
			panel6.ColumnCount = 1;
			panel6.AutoSize = true;
			button1.Text = GetLocalizedText("Remove");
			button1.Font = new Font(button1.Font.FontFamily, button1.Font.Size, button1.Font.Style);
			button1.AutoSize = true;
			panel7.AutoSize = true;
			textfield.Font = new Font(textfield.Font.FontFamily, textfield.Font.Size, textfield.Font.Style);
			textfield.AutoSize = true;
			buttonAdd.Text = GetLocalizedText("Add");
			buttonAdd.Font = new Font(buttonAdd.Font.FontFamily, buttonAdd.Font.Size, buttonAdd.Font.Style);
			buttonAdd.AutoSize = true;
			panel2.RowCount = 1;
			panel2.ColumnCount = 4;
			panel2.AutoSize = true;
			panel3.AutoSize = true;
			okButton.Text = GetLocalizedText("OK");
			okButton.Font = new Font(okButton.Font.FontFamily, okButton.Font.Size, okButton.Font.Style);
			okButton.AutoSize = true;
			cancelButton.Text = GetLocalizedText("Cancel");
			cancelButton.Font = new Font(cancelButton.Font.FontFamily, cancelButton.Font.Size, cancelButton.Font.Style);
			cancelButton.AutoSize = true;
			panel4.AutoSize = true;
		}

		private void LayoutComponents()
		{
			panel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100.0f));
			panel1.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			panel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100.0f));
			panel5.Margin = TranslateMargin(4, 4, 4, 4);
			panel5.Anchor = AnchorStyles.Top|AnchorStyles.Left|AnchorStyles.Bottom|AnchorStyles.Right;
			panel1.Controls.Add(panel5, 0, 0);
			panel2.Margin = TranslateMargin(4, 4, 4, 4);
			panel2.Anchor = AnchorStyles.Top|AnchorStyles.Left|AnchorStyles.Bottom|AnchorStyles.Right;
			panel1.Controls.Add(panel2, 0, 1);
			panel5.RowStyles.Add(new RowStyle(SizeType.Percent, 100.0f));
			panel5.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			panel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100.0f));
			panel5.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
			listview.Margin = TranslateMargin(2, 2, 2, 2);
			listview.Anchor = AnchorStyles.Top|AnchorStyles.Left|AnchorStyles.Bottom|AnchorStyles.Right;
			panel5.Controls.Add(listview, 0, 0);
			panel6.Margin = TranslateMargin(0, 0, 0, 0);
			panel6.Anchor = AnchorStyles.Top|AnchorStyles.Left|AnchorStyles.Bottom|AnchorStyles.Right;
			panel5.Controls.Add(panel6, 1, 0);
			textfield.Dock = DockStyle.Fill;
			FixedSizeTextBoxPanel textfieldPanel = new FixedSizeTextBoxPanel();
			textfieldPanel.AutoSize = true;
			textfieldPanel.Columns = 0;
			textfieldPanel.Margin = TranslateMargin(2, 2, 2, 2);
			textfieldPanel.Anchor = AnchorStyles.Left|AnchorStyles.Right;
			textfieldPanel.Controls.Add(textfield);
			panel5.Controls.Add(textfieldPanel, 0, 1);
			buttonAdd.Margin = TranslateMargin(2, 2, 2, 2);
			buttonAdd.Anchor = AnchorStyles.Top|AnchorStyles.Left|AnchorStyles.Bottom|AnchorStyles.Right;
			panel5.Controls.Add(buttonAdd, 1, 1);
			panel6.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			panel6.RowStyles.Add(new RowStyle(SizeType.Percent, 100.0f));
			panel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100.0f));
			button1.Margin = TranslateMargin(2, 2, 2, 2);
			button1.Anchor = AnchorStyles.Top|AnchorStyles.Left|AnchorStyles.Bottom|AnchorStyles.Right;
			panel6.Controls.Add(button1, 0, 0);
			panel7.Margin = TranslateMargin(0, 0, 0, 0);
			panel7.Anchor = AnchorStyles.Top|AnchorStyles.Left|AnchorStyles.Bottom|AnchorStyles.Right;
			panel6.Controls.Add(panel7, 0, 1);
			panel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100.0f));
			panel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50.0f));
			panel2.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
			panel2.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
			panel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50.0f));
			panel3.Margin = TranslateMargin(0, 0, 0, 0);
			panel3.Anchor = AnchorStyles.Top|AnchorStyles.Left|AnchorStyles.Bottom|AnchorStyles.Right;
			panel2.Controls.Add(panel3, 0, 0);
			okButton.Margin = TranslateMargin(2, 2, 2, 2);
			okButton.Anchor = AnchorStyles.Top|AnchorStyles.Left|AnchorStyles.Bottom|AnchorStyles.Right;
			panel2.Controls.Add(okButton, 1, 0);
			cancelButton.Margin = TranslateMargin(2, 2, 2, 2);
			cancelButton.Anchor = AnchorStyles.Top|AnchorStyles.Left|AnchorStyles.Bottom|AnchorStyles.Right;
			panel2.Controls.Add(cancelButton, 2, 0);
			panel4.Margin = TranslateMargin(0, 0, 0, 0);
			panel4.Anchor = AnchorStyles.Top|AnchorStyles.Left|AnchorStyles.Bottom|AnchorStyles.Right;
			panel2.Controls.Add(panel4, 3, 0);
		}
		// Helpers

		private int screenSize;
		private double pixelAspectRatio;
		private Size windowSize = new Size(0,0);

		private Padding TranslateMargin(int left, int top, int right, int bottom) {
			return new Padding
			(
				(int) ((screenSize*left+500)/1000/pixelAspectRatio),
				(int) ((screenSize*top+500)/1000*pixelAspectRatio),
				(int) ((screenSize*right+500)/1000/pixelAspectRatio),
				(int) ((screenSize*bottom+500)/1000*pixelAspectRatio)
			);
		}

		private void CalculateScreenSize() {
			screenSize = Math.Min(Screen.PrimaryScreen.Bounds.Height*4/3, Screen.PrimaryScreen.Bounds.Width);
			pixelAspectRatio = 1.0;
			if (Screen.PrimaryScreen.Bounds.Height==1024&&Screen.PrimaryScreen.Bounds.Width==1280) pixelAspectRatio = (double) 1024*4/3/1280;
		}

		private void CalculateWindowSize() {
			windowSize.Width = (int) (screenSize*Math.Sqrt(SCREEN_AREA_RATIO*ASPECT_RATIO)/pixelAspectRatio);
			windowSize.Height = (int) (screenSize*Math.Sqrt(SCREEN_AREA_RATIO/ASPECT_RATIO)*pixelAspectRatio);
		}

		private class XDGroupBox : GroupBox
		{
			private Size addSize;

			public XDGroupBox() {
				this.addSize = DefaultSize-DisplayRectangle.Size;
				this.AutoSize = true;
			}

			public override Size GetPreferredSize(Size proposedSize) {
				if (Controls.Count==0)
					return base.GetPreferredSize(proposedSize);
				else
					return Controls[0].PreferredSize+addSize;
			}
		}

		private class XDSplitPanel : SplitContainer
		{
			private Size prefSize1;
			private Size prefSize2;

			public XDSplitPanel() {
				AutoSize = true;
			}

			public void SetFirstControl(Control control) {
				prefSize1 = getPreferredSize(control);
				Panel1.Controls.Clear();
				TableLayoutPanel panel = new TableLayoutPanel();
				panel.RowCount = 1;
				panel.ColumnCount = 1;
				panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100.0f));
				panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100.0f));
				panel.AutoSize = true;
				panel.Controls.Add(control);
				panel.Dock = DockStyle.Fill;
				Panel1.Controls.Add(panel);
			}

			public void SetSecondControl(Control control) {
				prefSize2 = getPreferredSize(control);
				Panel2.Controls.Clear();
				TableLayoutPanel panel = new TableLayoutPanel();
				panel.RowCount = 1;
				panel.ColumnCount = 1;
				panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100.0f));
				panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100.0f));
				panel.AutoSize = true;
				panel.Controls.Add(control);
				panel.Dock = DockStyle.Fill;
				Panel2.Controls.Add(panel);
			}

			public override Size GetPreferredSize(Size proposedSize) {
				if (Orientation==Orientation.Vertical)
					return new Size(prefSize1.Width+prefSize2.Width, Math.Max(prefSize1.Height, prefSize2.Height));
				else
					return new Size(Math.Max(prefSize1.Width, prefSize2.Width), prefSize1.Height+prefSize2.Height);
			}

			private Size getPreferredSize(Control control) {
				Size rawRes = control.PreferredSize;
				rawRes.Height += control.Margin.Top+control.Margin.Bottom;
				rawRes.Width += control.Margin.Left+control.Margin.Right;
				return rawRes;
			}
		}

		private class HSeparator : Control
		{
			private System.Windows.Forms.VisualStyles.VisualStyleRenderer cachedRenderer;

			public HSeparator() {
				SetStyle(ControlStyles.ResizeRedraw, true);
			}

			public override Size GetPreferredSize(Size proposedSize) {
				return new Size(100, 2);
			}

			protected override void OnPaint(PaintEventArgs e) {
				base.OnPaint(e);
				if (Application.RenderWithVisualStyles) {
					if (cachedRenderer==null) {
						cachedRenderer = new System.Windows.Forms.VisualStyles.VisualStyleRenderer
						(
							System.Windows.Forms.VisualStyles.VisualStyleElement.Button.PushButton.Normal
						);
					}
					cachedRenderer.DrawParentBackground
					(
						e.Graphics,
						new Rectangle(0, 0, ClientSize.Width, ClientSize.Height),
						this
					);
				}
				Rectangle rect = new Rectangle(0, 0, ClientSize.Width, ClientSize.Height/2+1);
				if (rect.Height<2)
					rect.Height = 2;
				ControlPaint.DrawBorder3D
				(
					e.Graphics,
					rect,
					Border3DStyle.Etched,
					Border3DSide.Bottom
				);
			}
		}

		private class VSeparator : Control
		{
			private System.Windows.Forms.VisualStyles.VisualStyleRenderer cachedRenderer;

			public VSeparator() {
				SetStyle(ControlStyles.ResizeRedraw, true);
			}

			public override Size GetPreferredSize(Size proposedSize) {
				return new Size(2, 100);
			}

			protected override void OnPaint(PaintEventArgs e) {
				base.OnPaint(e);
				if (Application.RenderWithVisualStyles) {
					if (cachedRenderer==null) {
						cachedRenderer = new System.Windows.Forms.VisualStyles.VisualStyleRenderer
						(
							System.Windows.Forms.VisualStyles.VisualStyleElement.Button.PushButton.Normal
						);
					}
					cachedRenderer.DrawParentBackground
					(
						e.Graphics,
						new Rectangle(0, 0, ClientSize.Width, ClientSize.Height),
						this
					);
				}
				Rectangle rect = new Rectangle(0, 0, ClientSize.Width/2+1, ClientSize.Height);
				if (rect.Width<2)
					rect.Width = 2;
				ControlPaint.DrawBorder3D
				(
					e.Graphics,
					rect,
					Border3DStyle.Etched,
					Border3DSide.Right
				);
			}
		}

		private class FixedSizeTextBoxPanel : Panel
		{
			private int columns = 1;
			private Label l = new Label();

			public int Columns
			{
				set {
					columns = value;
				}

				get {
					return columns;
				}
			}

			public override Size GetPreferredSize(Size proposedSize) {
				if (Controls.Count==1) {
					int prefHeight = Controls[0].GetPreferredSize(proposedSize).Height;
					char[] arr = new char[columns];
					for (int i = 0; i<arr.Length; i++) {
						arr[i] = 'W';
					}
					l.Text = new String(arr);
					l.Font = Controls[0].Font;
					int prefWidth = l.GetPreferredSize(proposedSize).Width;
					return new Size(prefWidth, prefHeight);
				} else {
					return base.GetPreferredSize(proposedSize);
				}
			}
		}

		private class MultilineLabel : Label
		{
			public override System.Drawing.Size GetPreferredSize(System.Drawing.Size proposedSize) {
				if (proposedSize.Width < 2)
					proposedSize.Width = 50;
				Size res = base.GetPreferredSize(proposedSize);
				return res;
			}
		}

		#endregion Implementation (X-develop WindowsForms 2.0 designer code)
	}
}
