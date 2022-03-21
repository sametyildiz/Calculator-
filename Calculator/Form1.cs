using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator
{
    public partial class Form1 : Form
    { private string tmpNum = "";
        private List<double> numList = new List<double>();
        private List<char> operators = new List<char>();
        Calculate calculate = new Calculate();
        private bool hasAResult = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            charListener(e.KeyChar);
        }

        private void makeNegatif()
        {
            if (tmpNum == null)
                return;
            double tmp = 0;

            if (tmpNum.Length > 0)
            {
                rtbResultScreen.Text = rtbResultScreen.Text.Remove(rtbResultScreen.Text.LastIndexOf(tmpNum));
                
                if (tmpNum.Length == 1 && "-".Contains(tmpNum))
                {
                    tmpNum = "";
                } 
                else
                {                   
                    tmp = Convert.ToDouble(tmpNum);
                    tmp *= -1;
                    tmpNum = tmp.ToString();
                }
            }
            else
                tmpNum += "-";
            
            rtbResultScreen.Text += tmpNum;

        }

        private void charListener(char ch)
        {
            
            switch (ch)
            {
                case char c when ("0123456789".Contains(c)):
                    if (hasAResult)
                    {
                        tmpNum = "";
                        hasAResult = false;
                    }

                    tmpNum += c;
                    rtbResultScreen.Text += c;
                    break;
                case char c when (".,").Contains(c) && tmpNum != "":
                    if (!tmpNum.Contains(c))
                    {
                        tmpNum += '.';
                        rtbResultScreen.Text += '.';
                    }
                    break;
                case char c when "+-/*".Contains(c):
                    if (tmpNum == "" && numList.Count == 0 && c != '-')
                    {
                        MessageBox.Show("Önce Rakam Girmelisiniz.");
                        break;
                    }
                    if (tmpNum == "")
                    {
                        tmpNum += c;
                        rtbResultScreen.Text += c;
                        break;
                    }
                    if (cbAvg.Checked)
                        c = '+';
                    else
                        operators.Add(c);

                    numList.Add(Convert.ToDouble(tmpNum));
                    rtbResultScreen.Text += String.Concat(' ', c, ' ');
                    tmpNum = "";

                    break;
                case '\b'://backspace
                    tmpNum = backSpace(tmpNum);
                    rtbResultScreen.Text = backSpace(rtbResultScreen.Text);


                    break;
                case '\r'://enter
                case '=':
                    double result = 0.0;

                    if (tmpNum == "")
                        operators.RemoveAt(operators.Count - 1);
                    numList.Add(Convert.ToDouble(tmpNum));

                    result = cbAvg.Checked ? 
                        calculate.avg(numList) :
                        calculate.Calculation(numList, operators);

                    rtbResultScreen.Clear();
                    rtbResultScreen.Text += ("= " + result + '\n');

                    clearMemory();
                    hasAResult = true;
                    tmpNum = result.ToString();
                    break;
                case '\u001b'://esc
                    clearMemory();
                    rtbResultScreen.Clear();
                    break;
                default:
                    break;
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            

        }
        private void clearMemory()
        {
            tmpNum = "";
            numList.Clear();
            operators.Clear();
            hasAResult = false;
        }

        private string backSpace(string txt)
        {   if(txt != null && txt != "")
            {
                txt = txt.Trim();
                if(txt != "" && !"*/+".Contains(txt[txt.Length - 1]))
                    txt = txt.Remove(txt.Length - 1) + ' ';
                
            }
             return txt;
        }
        private void btnNum1_Click(object sender, EventArgs e)
        {
            charListener('1');
        }
        private void btnNum2_Click(object sender, EventArgs e)
        {
            charListener('2');
        }

        private void btnNum3_Click(object sender, EventArgs e)
        {
            charListener('3');
        }

        private void btnNum0_Click(object sender, EventArgs e)
        {
            charListener('0');
        }

        private void btnDot_Click(object sender, EventArgs e)
        {
            charListener(',');
        }

        private void btnNum6_Click(object sender, EventArgs e)
        {
            charListener('6');
        }

        private void btnNum5_Click(object sender, EventArgs e)
        {
            charListener('5');
        }

        private void btnNum4_Click(object sender, EventArgs e)
        {
            charListener('4');
        }

        private void btnNum7_Click(object sender, EventArgs e)
        {
            charListener('7');
        }

        private void btnNum8_Click(object sender, EventArgs e)
        {
            charListener('8');
        }

        private void btnNum9_Click(object sender, EventArgs e)
        {
            charListener('9');
        }

        private void btnEnter_Click(object sender, EventArgs e)
        {
            charListener('\r');
        }

        private void btnSum_Click(object sender, EventArgs e)
        {
         //   simpleSound.Play();
            charListener('+');
        }

        private void btnDiff_Click(object sender, EventArgs e)
        {
            charListener('-');
        }

        private void btnMult_Click(object sender, EventArgs e)
        {
            charListener('*');
        }

        private void btnDiv_Click(object sender, EventArgs e)
        {
            charListener('/');
        }

        private void btnBspace_Click(object sender, EventArgs e)
        {
            charListener('\b');
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            charListener('\u001b');
            
        }

        private void btnNeg_Click(object sender, EventArgs e)
        {
            makeNegatif();
        }

        private void cbAvg_CheckedChanged(object sender, EventArgs e)
        {
            
        }
    }

    internal class Calculate
    {
        internal double Calculation(List<double> numbers, List<char> operators)
        {
            bool[] isUsed = new bool[numbers.Count];
            double tmp = 0;
            double sum = 0;
            bool lastTurn = false;
            int i = 0;
            while (i < operators.Count)
            {
                tmp = 0;
                char c = operators[i];
                if (c == '*' || c == '/')
                {
                    switch (operators[i])
                    {
                        case '*':
                            tmp = numbers[i] * numbers[i + 1];
                            break;
                        case '/':
                            tmp = numbers[i] / numbers[i + 1];
                            break;
                        default:
                            continue;
                    }
                    isUsed[i] = true;
                    numbers[i + 1] = tmp;
                }
                i++;
            }
            i = 0; int nextIndex = passtheUsed(isUsed, i + 1);
            while (nextIndex < numbers.Count)
            {
                tmp = 0;

                switch (operators[i])
                {
                    case '+':
                        tmp = numbers[i] + numbers[nextIndex];
                        break;
                    case '-':
                        tmp = numbers[i] - numbers[nextIndex];
                        break;
                    default:
                        i = passtheUsed(isUsed, i + 1);
                        nextIndex++;
                        continue;
                }
                isUsed[i] = true;
                numbers[nextIndex] = tmp;
                i = nextIndex;
                nextIndex = passtheUsed(isUsed, i + 1);
            }

            return numbers[numbers.Count() - 1];
        }

        private int passtheUsed(bool[] isUsed, int i)
        {
            while (i < isUsed.Length && isUsed[i])
                i++;
            return i;
        }

        internal double avg(List<double> numbers)
        {   double avg = 0;
            foreach(double d in numbers) {
                avg += d;
            }
            return avg /(double) numbers.Count;
        }
    }
}
