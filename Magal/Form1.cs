using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Magal
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadCombos();
        }

        private async void LoadCombos()
        {
            fromCombo.Text = toCombo.Text = "Loading";
            fromCombo.DataSource = (await APIHelper.GetCountriesAsync()).ToList();
            toCombo.DataSource = (await APIHelper.GetCountriesAsync()).ToList();
            fromCombo.DisplayMember = toCombo.DisplayMember = "currencyName";
            fromCombo.ValueMember = toCombo.ValueMember = "currencyId";
            fromCombo.SelectedValue = "USD";
            toCombo.SelectedValue = "ILS";

            fromCombo.SelectionChangeCommitted+=SelectedIndexChanged;
            toCombo.SelectionChangeCommitted += SelectedIndexChanged;

            ShowConvertion();
        }

        private void SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowConvertion();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ShowConvertion();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            object tempKey = fromCombo.SelectedValue;
            fromCombo.SelectedValue = toCombo.SelectedValue;
            toCombo.SelectedValue = tempKey;
            ShowConvertion();
        }

        private async void ShowConvertion()
        {
            double number;
            if (amountTxt.Text.Equals("") || amountTxt.Text == null)
                amountTxt.Text = "1";
            if (double.TryParse(amountTxt.Text, out number))
            {
                double unit = await APIHelper.getConvertUnit(fromCombo.SelectedValue.ToString(), toCombo.SelectedValue.ToString());
                resultLbl.Text = String.Format("{0:0.##}", number * unit) + APIHelper.getSignForID(toCombo.SelectedValue.ToString());
            }
            else
            {
                MessageBox.Show("Amount filed must be number!", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                amountTxt.Text = "";
            }
        }

        private void amountTxt_TextChanged(object sender, EventArgs e)
        {
            if(amountTxt.Text!=""&&amountTxt.Text!=null)
            ShowConvertion();
        }
    }
}
