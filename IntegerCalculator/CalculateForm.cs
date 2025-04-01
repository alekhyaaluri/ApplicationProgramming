using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace IntegerCalculator
{
    public partial class CalculateForm : Form
    {
        public CalculateForm()
        {
            InitializeComponent();
            InitializeComboBox();
        }
        private void InitializeComboBox()
        {
            comboBox1.Items.AddRange(new string[] { "+", "-", "x", "/" });
            comboBox1.SelectedIndex = 0; // Set default selection
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string num1 = textBox1.Text.Trim();
                string num2 = textBox2.Text.Trim();
                char operation = comboBox1.SelectedItem.ToString()[0];

                if (!IsValidNumber(num1) || !IsValidNumber(num2))
                {
                    MessageBox.Show("Invalid input. Please enter only positive integer values.");
                    return;
                }

                int[] op1 = ConvertToArray(num1);
                int[] op2 = ConvertToArray(num2);
                int[] result;

                switch (operation)
                {
                    case '+': result = Add(op1, op2); break;
                    case '-': result = Subtract(op1, op2); break;
                    case 'x': result = Multiply(op1, op2); break;
                    case '/': result = Divide(op1, op2); break;
                    default: throw new Exception("Invalid operation selected.");
                }

                txtResult.Text = ConvertToString(result);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private bool IsValidNumber(string num)
        {
            foreach (char c in num)
            {
                if (!char.IsDigit(c)) return false;
            }
            return true;
        }
        private int[] ConvertToArray(string num)
        {
            int[] result = new int[num.Length];
            for (int i = 0; i < num.Length; i++)
                result[i] = num[i] - '0';
            return result;
        }
        private string ConvertToString(int[] num)
        {
            string result = string.Join("", num).TrimStart('0');
            if(string.IsNullOrEmpty(result))
            {
                result = "0";
            }
            return result;
        }

        private int[] Add(int[] op1, int[] op2)
        {
            List<int> result = new List<int>();
            int carry = 0, sum;
            int i = op1.Length - 1, j = op2.Length - 1;

            while (i >= 0 || j >= 0 || carry > 0)
            {
                sum = carry;
                if (i >= 0) sum += op1[i--];
                if (j >= 0) sum += op2[j--];
                carry = sum / 10;
                result.Insert(0, sum % 10);
            }
            return result.ToArray();
        }

        private int[] Subtract(int[] op1, int[] op2)
        {
            if (CompareArrays(op1, op2) < 0)
                throw new InvalidOperationException("Subtraction would result in a negative number, which is not allowed for unsigned integers.");

            List<int> result = new List<int>();
            int borrow = 0, diff;
            int i = op1.Length - 1, j = op2.Length - 1;

            while (i >= 0)
            {
                int digit1 = op1[i];
                int digit2 = (j >= 0) ? op2[j] : 0;

                diff = digit1 - digit2 - borrow;

                if (diff < 0)
                {
                    diff += 10;
                    borrow = 1;
                }
                else
                {
                    borrow = 0;
                }

                result.Insert(0, diff);
                i--;
                j--;
            }

            // Remove leading zeros
            while (result.Count > 1 && result[0] == 0)
                result.RemoveAt(0);

            return result.ToArray();
        }


        private int[] Multiply(int[] op1, int[] op2)
        {
            int[] result = new int[op1.Length + op2.Length];
            for (int i = op1.Length - 1; i >= 0; i--)
            {
                for (int j = op2.Length - 1; j >= 0; j--)
                {
                    int mul = op1[i] * op2[j] + result[i + j + 1];
                    result[i + j + 1] = mul % 10;
                    result[i + j] += mul / 10;
                }
            }
            return result;
        }
        private int[] Divide(int[] op1, int[] op2)
        {
            if (op2.Length == 1 && op2[0] == 0)
                throw new DivideByZeroException("Cannot divide by zero.");
            
            List<int> quotient = new List<int>();
            List<int> dividend = new List<int>();

            foreach (int digit in op1)
            {
                dividend.Add(digit);
                while (dividend.Count > 1 && dividend[0] == 0)
                    dividend.RemoveAt(0); // Remove leading zeros

                int count = 0;
                while (CompareArrays(dividend.ToArray(), op2) >= 0)
                {
                    int[] tempResult = Subtract(dividend.ToArray(), op2);
                    dividend = new List<int>(tempResult);
                    count++;
                }
                quotient.Add(count);
            }

            // Handle case where quotient is empty (result is zero)
            return quotient.Count == 0 ? new int[] { 0 } : quotient.ToArray();
        }



        private int CompareArrays(int[] a, int[] b)
        {
            if (a.Length > b.Length) return 1;
            if (a.Length < b.Length) return -1;
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] > b[i]) return 1;
                if (a[i] < b[i]) return -1;
            }
            return 0;
        }
    }
}