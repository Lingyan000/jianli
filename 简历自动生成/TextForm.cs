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
        private Boolean textChange;
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
            string MyFileName = StatusLabelMsg.Text;
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
    }
}
