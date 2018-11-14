using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 简历自动生成
{
    public partial class TextForm : Form
    {
        public bool textChange { get; set; }
        public TextForm()
        {
            InitializeComponent();
        }
        public TextForm(string iofo)
        {
            InitializeComponent();
            this.richTextBox1.Text = iofo;
        }

        //创建简历
        private void 创建简历ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text != "")
            {
                if (MessageBox.Show("是否保存当前文件？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    saveFile();
            }
            createNewFile();
        }

        //打开简历
        private void 打开简历ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFile();
        }

        //保存简历
        private void 保存简历SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFile();
        }

        //另存为
        private void 另存为AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveAs();
        }

        //返回
        private void 返回XToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //复制
        private void 复制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (richTextBox1.CanSelect) richTextBox1.Copy();
        }

        //剪切
        private void 剪切XToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (richTextBox1.CanSelect) richTextBox1.Cut();
        }

        //粘贴
        private void 粘贴PToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IDataObject iData = Clipboard.GetDataObject();
            if (iData.GetDataPresent(DataFormats.Text))
                this.richTextBox1.SelectedText = (String)iData.GetData(DataFormats.Text);
        }

        //删除
        private void 删除DToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        //撤销
        private void 撤销UToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (richTextBox1.CanUndo) richTextBox1.Undo();
        }

        private void 重做RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (richTextBox1.CanRedo) richTextBox1.Redo();
        }

        private void 自动保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            自动保存ToolStripMenuItem.Checked = !自动保存ToolStripMenuItem.Checked;
            if (自动保存ToolStripMenuItem.Checked)
            {
                timer1.Start();
                timer1.Enabled = true;
            }
            else
            {
                timer1.Stop();
                timer1.Enabled = false;
            }
        }

        private void 字体FToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setFontDialog();
        }

        private void 颜色LToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setFontColcr();
        }

        private void createNewFile()
        {
            richTextBox1.Clear();
        }

        private void openFile()
        {
            openFileDialog1.Title = "请选择一个文件";
            openFileDialog1.Filter = "RTF格式文件(*.rtf)|*.rtf|文本文件(*.txt)|*.txt|所有文件(*.*)|*.*";
            openFileDialog1.RestoreDirectory = true;//关闭前还原当前目录
            //显示对话框，直到用户关闭它。在退出对话框时如果选择了确定按钮表示有效
            StatusLabelMsg.Text = "请选择要编辑的文件";
            string MyFileName;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                MyFileName = openFileDialog1.FileName; //返回选取的文件名

                if (richTextBox1.Text != "")
                {
                    if (MessageBox.Show("是否保存当前文件？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        saveFile();
                }
                if (openFileDialog1.FilterIndex == 1)
                {//如果是*.rtf格式，则用RichText（RTF格式文件）方式打开
                    richTextBox1.LoadFile(MyFileName, RichTextBoxStreamType.RichText);
                }
                else
                {//如果是其它格式，则用PlairText（文本文件）方式打开
                    richTextBox1.LoadFile(MyFileName, RichTextBoxStreamType.PlainText);
                }
                //将文件名显示在状态栏（不含路径）
                StatusLabelFileName.Tag = openFileDialog1.FileName;
            }
            StatusLabelMsg.Text = "";
        }

        private void saveFile()
        {
            string MyFileName = StatusLabelFileName.Text;
            if (MyFileName == "")
            {
                saveAs();
                return;
            }
            MyFileName = (StatusLabelFileName.Tag.ToString()).ToUpper();
            try//文件保存可能出错，需要错误捕获
            {
                StatusLabelMsg.Text = "正在保存文件...";
                if (MyFileName.EndsWith(".RTF"))
                {//如果是*.rtf格式，则用RichText（RTF格式文件）方式保存
                    richTextBox1.SaveFile(MyFileName, RichTextBoxStreamType.RichText);
                }
                else
                {//如果是其它格式，则用PlairText（文本文件）方式保存
                    richTextBox1.SaveFile(MyFileName, RichTextBoxStreamType.PlainText);
                }
                StatusLabelMsg.Text = "";
                textChange = false;
            }
            catch (Exception Err)
            {
                MessageBox.Show("写文本文件发生错误！ \n" + Err.Message, "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void saveAs()
        {
            StatusLabelMsg.Text = "请选择保存位置和文件名";
            saveFileDialog1.Filter = "RTF格式文件(*.rtf)|*.rtf|文本文件(*.txt)|*.txt|所有文件(*.*)|*.*";
            DialogResult dialogResult = saveFileDialog1.ShowDialog();
            string MyFileName = saveFileDialog1.FileName;
            if (dialogResult == DialogResult.OK && MyFileName.Trim() != "")
            {
                StatusLabelMsg.Text = "正在保存文件...";
                try//文件保存可能出错，需要错误捕获
                {
                    if (saveFileDialog1.FilterIndex == 1)
                        richTextBox1.SaveFile(MyFileName, RichTextBoxStreamType.RichText);
                    else
                        richTextBox1.SaveFile(MyFileName, RichTextBoxStreamType.PlainText);
                    StatusLabelFileName.Text = MyFileName.Substring(MyFileName.LastIndexOf("\\") - 1);
                    StatusLabelFileName.Tag = MyFileName;
                    textChange = false;
                }
                catch (Exception Err)
                {
                    MessageBox.Show("写文本文件发生错误！ \n" + Err.Message, "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            StatusLabelMsg.Text = "";
        }

        //设置字体
        private void setFontDialog()
        {
            fontDialog1.ShowEffects = true;
            fontDialog1.Font = richTextBox1.SelectionFont; //设置初始状态
            if (fontDialog1.ShowDialog() == DialogResult.OK)
                richTextBox1.SelectionFont = fontDialog1.Font;
        }

        //设置颜色
        private void setFontColcr()
        {
            colorDialog1.SolidColorOnly = true;//只选择纯色
            colorDialog1.Color = richTextBox1.SelectionColor;// 设置初始值为当前颜色
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.SelectionColor = colorDialog1.Color;
                StatusLabelColor.BackColor = colorDialog1.Color;
            }
        }

        private void 剪切ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (richTextBox1.CanSelect) richTextBox1.Cut();
        }

        private void 复制ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (richTextBox1.CanSelect) richTextBox1.Copy();
        }

        private void 粘贴ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IDataObject iData = Clipboard.GetDataObject();
            if (iData.GetDataPresent(DataFormats.Text))
                this.richTextBox1.SelectedText = (String)iData.GetData(DataFormats.Text);
        }

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFile();
        }

        private void printDocument1_BeginPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            StatusLabelMsg.Text = "正在打印当前文档...";
        }

        private void printDocument1_EndPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            MessageBox.Show("打印完成。");
            StatusLabelMsg.Text = "";
        }

        private void printSetup()
        {
            if (richTextBox1.TextLength < 1)
                return;
            //设置打印对话框属性
            printDialog1.Document = printDocument1;
            printDialog1.AllowCurrentPage = true;//显示“当前页”按钮
            printDialog1.AllowSelection = true;//显示“选择”按钮
            printDialog1.AllowPrintToFile = true;//启动“页”选项按钮
            printDialog1.ShowNetwork = true;//显示“网络”按钮
            printDialog1.UseEXDialog = true;//以 windows xp样式显示
            //显示打印窗口
            if (printDialog1.ShowDialog() == DialogResult.OK)
            {
                print(); //打印文件
            }
        }

        private void print()
        {
            try
            {
                printDocument1.Print();
            }
            catch (Exception Err)
            {
                MessageBox.Show("打印文件发生错误: \n" + Err.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //文档打印程序
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            //打印信息处理
            Graphics g = e.Graphics;//获得绘制的对象
            float LinePages = 0;//一页中的行数
            PointF drawPoint = new PointF(0, 0); //待绘文本的起点坐标
            Font drawFont = new Font("宋体", 16);
            float lineHeight = drawFont.GetHeight(g);
            int count = 0; //行计数
            int printLine = 0; //文本行计数器
            float LeftMargin = e.MarginBounds.Left; //页面左边距
            float topMargin = e.MarginBounds.Top; //页面顶边距
            String line = null; //每行字符串流
            //根据页面的高度和字体的高度计算一页中可以打印的行数
            LinePages = (int)(e.MarginBounds.Height / lineHeight);
            //每次从文件中读取一行并打印
            drawPoint.X = LeftMargin;
            drawPoint.Y = topMargin;
            while (count++ < LinePages)
            {
                line = this.richTextBox1.Lines[printLine++];
                //若文本为空或打印行标记达到文本最大行数，则打印结束
                if (line == null || printLine > richTextBox1.Lines.Length - 1)
                    break;
                //计算这一行的起点显示位置
                drawPoint.Y += drawFont.GetHeight(g);
                //绘制文本
                g.DrawString(line, drawFont, Brushes.Black, drawPoint);
            }
        }

        private void printPreview() //显示打印预览
        {
            //设置Document属性
            printPreviewDialog1.Document = printDocument1;
            try
            {
                //显示打印预览窗口
                this.printPreviewDialog1.ShowDialog();
            }
            catch (Exception excep)
            {
                //显示打印出错消息
                MessageBox.Show(excep.Message, "打印出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pageSetup() //页面设置对话框
        {
            if (richTextBox1.Text.Length < 1)
                return;
            //设置Document属性
            pageSetupDialog1.Document = printDocument1;
            pageSetupDialog1.AllowMargins = true;
            pageSetupDialog1.AllowOrientation = true;
            pageSetupDialog1.AllowPaper = true;

            try
            {//显示打印页面设置窗口
                pageSetupDialog1.ShowDialog();
            }
            catch (Exception excep)
            {//显示打印错误信息
                MessageBox.Show(excep.Message, "打印出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void 打印预览ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            printPreview();
        }

        private void 打印ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            printSetup();
        }

        private void 页面设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pageSetup();
        }

        //工具栏：打开
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            openFile();
        }

        //工具栏：保存
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            saveFile();
        }

        //工具栏：打印
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            printSetup();
        }

        //工具栏：剪切
        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            if (richTextBox1.CanSelect) richTextBox1.Cut();
        }

        //工具栏：复制
        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            if (richTextBox1.CanSelect) richTextBox1.Copy();
        }

        //工具栏：粘贴
        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            IDataObject iData = Clipboard.GetDataObject();
            if (iData.GetDataPresent(DataFormats.Text))
                this.richTextBox1.SelectedText = (String)iData.GetData(DataFormats.Text);
        }


        private void TextForm_Load(object sender, EventArgs e)
        {
            //用系统字体填充字体组合框
            toolStripComboZt.Items.Clear();

            foreach (FontFamily ff in FontFamily.Families)
            {
                //检查字体是否支持相应字体样式
                if (ff.IsStyleAvailable(FontStyle.Regular & FontStyle.Underline & FontStyle.Bold & FontStyle.Italic & FontStyle.Strikeout))
                {
                    toolStripComboZt.Items.Add(ff.Name);
                }
            }

            toolStripComboZt.Text = richTextBox1.Font.Name;
            // 填充字体大小组合框
            for (int i = 5; i <= 20; i++)
                toolStripComboSize.Items.Add(i);

            for (int i = 22; i < 72; i += 2)
                toolStripComboSize.Items.Add(i);

            //设置字体组合框和字体大小组合框的初值
            toolStripComboZt.Text = richTextBox1.Font.Name;
            toolStripComboSize.Text = ((int)(richTextBox1.Font.Size)).ToString();
        }

        private void toolStripComboSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (toolStripComboSize.Text == "")
                return;
            Font currentFont = richTextBox1.SelectionFont;
            float fontSize;
            try
            {
                fontSize = float.Parse(toolStripComboSize.Text);
                if (currentFont == null)
                {
                    richTextBox1.SelectionFont = new Font(toolStripComboZt.Text, fontSize);
                }
                else
                {
                    richTextBox1.SelectionFont = new Font(currentFont.FontFamily, fontSize);
                }
            }
            catch (System.Exception mse)
            {
                MessageBox.Show("字体设置出错！\n" + mse);
            }
        }

        private void toolStripComboZt_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (toolStripComboZt.Text == "")
                return;
            Font currenteFont = richTextBox1.SelectionFont; //获取当前字体
            float fontSize;
            if (currenteFont == null) //如果当前字体不止一样则为空
                fontSize = float.Parse(toolStripComboZt.Text);
            else
                fontSize = currenteFont.Size;
            richTextBox1.SelectionFont = new Font(toolStripComboZt.Text, fontSize);
        }

        private void StyleChanges(object sender, EventArgs e)
        {
            Font currentFont = richTextBox1.SelectionFont;
            FontStyle style = FontStyle.Regular;
            //字型标志是位标志（以比特位为单位）
            if (toolStripButton9.Checked) style |= FontStyle.Bold;
            if (toolStripButton10.Checked) style |= FontStyle.Italic;
            if (toolStripButton11.Checked) style |= FontStyle.Underline;
            if (toolStripButton12.Checked) style |= FontStyle.Strikeout;
            if (currentFont != null)
            {
                richTextBox1.SelectionFont = new Font(currentFont.FontFamily, currentFont.Size, style);
            }
            else
            {
                float fontSize = float.Parse(toolStripComboSize.Text);
                richTextBox1.SelectionFont = new Font(toolStripComboZt.Text, fontSize, style);
            }
        }

        private void toolStrip2_Click(object sender, EventArgs e)
        {
            Font currentFont = richTextBox1.SelectionFont;
            FontStyle style = FontStyle.Regular;
            //字型标志是位标志（以比特位为单位）
            if (toolStripButton9.Checked) style |= FontStyle.Bold;
            if (toolStripButton10.Checked) style |= FontStyle.Italic;
            if (toolStripButton11.Checked) style |= FontStyle.Underline;
            if (toolStripButton12.Checked) style |= FontStyle.Strikeout;
            if (currentFont != null)
            {
                richTextBox1.SelectionFont = new Font(currentFont.FontFamily, currentFont.Size, style);
            }
            else
            {
                float fontSize = float.Parse(toolStripComboSize.Text);
                richTextBox1.SelectionFont = new Font(toolStripComboZt.Text, fontSize, style);
            }
        }

        private void toolStripLabel2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripButton13_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("好像没有这个功能耶！汗。。。");
        }
    }
}
