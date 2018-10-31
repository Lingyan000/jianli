using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;

namespace 简历自动生成
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textXm_Validating(object sender, CancelEventArgs e)
        {
            if (this.textXm.TextLength < 2)
            {
                MessageBox.Show("长度过短或为空");//提示
                this.textXm.Focus();//重置焦点
            }
        }

        private void textXm_Validated(object sender, EventArgs e)
        {
            this.lalTiltle.Text = this.textXm.Text + "的简历";
            this.lalTjzp.Text = this.textXm.Text + "的照片";
            this.labelXm.Text = this.textXm.Text;
        }


        private void pictureZp_DoubleClick(object sender, EventArgs e)
        {
            String fileToDisplay;
            openFileDialog1.Filter = "All File|*.*|Jpg File|*.jpg|Bmp File|*.bmp|Gif File|*.gif";
            openFileDialog1.FileName = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)//如果在对话框中按了确定按钮
            {
                fileToDisplay = openFileDialog1.FileName;//打开文件对话框返回的文件名
                pictureZp.ImageLocation = fileToDisplay;//设置文件URL
                pictureZp.Load();
            }
            else
            {
                pictureZp.Image = 简历自动生成.Properties.Resources.embed;
                this.BackgroundImage = 简历自动生成.Properties.Resources.C800600;
            }
        }

        private void buttonGbjl_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            loadcomboZzmm();
            //dateTimeCsrq.Value = new data();
        }
        public void loadcomboZzmm()//动态下拉列表
        {
            this.comboZzmm.Items.Add("中共党员");
            this.comboZzmm.Items.Add("共青团员");
            this.comboZzmm.Items.Add("其他党派");
            this.comboZzmm.Items.Add("群众");
        }

        private void comboXw_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboZgxl.Text != "本科" && this.comboZgxl.Text != "研究生")
            {
                if (this.comboXw.Text == "博士" || this.comboXw.Text == "硕士" || this.comboXw.Text == "学士")
                    MessageBox.Show("学位与学历不匹配");
            }
        }

        private void linkWlkj_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string target = this.textWeb.Text.ToLower();
            if (target == "")
                return;
            if (target.StartsWith("www."))
            {
                linkWlkj.Links[0].Visited = true;
                System.Diagnostics.Process.Start(target);
            }
            else
                MessageBox.Show("不是有效的URL链接格式");
        }

        private void buttonScjl_Click(object sender, EventArgs e)
        {
            string infoText = "";
            if (textXm.Text != "")
            {
                infoText = creaneNewFile();
            }
            else
            {
                MessageBox.Show("输入姓名");
                textXm.Focus();
                return;
            }
            //创建窗体 TEXTForm 的实例txtForm，并传入参数
            TextForm txtForm = new TextForm(infoText);
            txtForm.Show();
        }
        public String creaneNewFile()
        {
            if (this.textXm.Text == "")
            {
                MessageBox.Show("能不能写名字了！");
                textXm.Focus();
                return "";
            }
            String infoText = "\n";
            String tmp;
            infoText += "\n\t\t\t" + this.textXm.Text + "的简历";

            infoText += "\n\n" + this.textXm.Text;
            infoText += "，" + (this.radioXb1.Checked ? "男" : "女");
            if (this.comboZzmm.Text != "")
                infoText += "，" + this.comboZzmm.Text;
            if (this.comboMz.Text != "")
                infoText += "，" + this.comboMz.Text + "人，";
            infoText += this.dateTimeCsrq.Value.ToLongDateString() + "出生";
            infoText += "，" + (this.checkHf.Checked ? "已婚" : "未婚");
            if (this.textByyx.Text != "")
                infoText += "/n 毕业于" + this.textByyx.Text;
            if (this.textSxzy.Text != "")
                infoText += "，" + this.textSxzy.Text + "专业";
            if (this.comboZgxl.Text != "")
                infoText += "，" + this.comboZgxl.Text;
            if (this.comboXw.Text != "")
                infoText += "，" + this.comboXw.Text;
            if (this.numericUpGznx.Value > 0)
                infoText += "\n 工作年限" + this.numericUpGznx.Value;
            if (this.textJszc.Text != "")
                infoText += "，" + this.textJszc.Text;
            if (this.maskedTextSfzh.Text != "")
                infoText += "\n 身份证号：" + this.maskedTextSfzh.Text;
            if (this.textJg.Text != "")
                infoText += "\n 籍贯：" + this.textJg.Text;
            if (this.textDz.Text != "")
                infoText += "\n 家庭住址：" + this.textDz.Text;
            infoText += "\n\n 【教育经历】 \n" + this.textJyjl.Text;
            infoText += "\n\n 【工作简历】 \n" + this.textGzjl.Text;
            infoText += "\n\n 【自我鉴定】 \n" + this.richTextGrjd.Text;

            infoText += "\n\n 【求职意向】 \n";

            tmp = "";
            if (this.checkedListHylb.CheckedItems.Count != 0)
            {
                for (int x = 0; x <= this.checkedListHylb.CheckedItems.Count - 1; x++)
                {
                    if (x == this.checkedListHylb.CheckedItems.Count - 1)
                        tmp += this.checkedListHylb.CheckedItems[x].ToString() + "。";  
                    else
                        tmp += this.checkedListHylb.CheckedItems[x].ToString() + "，";
                }
            }
            if (tmp == "") tmp = "不限";
            infoText += "\n 期望行业：" + tmp;

            tmp = CallRecursive(treeGw);
            if (tmp == "") tmp = "任意安排";
            infoText += "\n 期望岗位：" + tmp;

            tmp = "";
            if (this.listGzdd.SelectedItem != null)
            {
                for (int i = 0; i < this.listGzdd.SelectedItems.Count - 1; i++)
                {
                    if (i == this.listGzdd.SelectedItems.Count - 1)
                        tmp += this.listGzdd.SelectedItems[i].ToString() + "。";  
                    else
                        tmp += this.listGzdd.SelectedItems[i].ToString() + "，";
                }
            }
            if (tmp == "") tmp = "不限";
            infoText += "\n 工作地点：" + tmp;

            infoText += "\n 期望工资：" + this.trackQxyq.Value;

            infoText += "\n 其它要求：" + (this.checkXybx.Checked ? "需要保险，" : "") 
                + (this.checkXyzf.Checked ? "需要住房，" : "") 
                + (this.checkZjmt.Checked ? "需要面谈，" : "");

            infoText += "\n\n 【联系方式】 \n";
            if (maskedTextYddh.Text != "")
                infoText += "\n 移动电话：" + this.maskedTextYddh.Text;
            if (maskedTextGddh.Text != "")
                infoText += "\n 固定电话：" + this.maskedTextGddh.Text;
            if (maskedTextQQ.Text != "")
                infoText += "\n QQ / MSN" + this.maskedTextQQ.Text;
            if (textEmail.Text != "")
                infoText += "\n Email：" + this.textEmail.Text;
            if (textWeb.Text != "")
                infoText += "\n 个人主页：" + this.textWeb.Text;
            return infoText;
        }

        private string PrintRecursive(TreeNode treeNode)
        {
            string treetxt = "";
            if (treeNode.Checked)
                treetxt = treeNode.Text + "，";
            //输出每个子节点
            foreach (TreeNode tn in treeNode.Nodes)
            {
                treetxt += PrintRecursive(tn);
            }
            return treetxt;
        }

        private string CallRecursive(TreeView treeView)
        {
            string treeText = "";
            TreeNodeCollection nodes = treeView.Nodes;//节点集合数据类型
            foreach (TreeNode n in nodes)//找到每个根节点
            {
                treeText += PrintRecursive(n);//调用递归方法
            }
            return treeText;
        }
    }
}
