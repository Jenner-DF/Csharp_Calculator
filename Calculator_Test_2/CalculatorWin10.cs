using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator_Test_2
{
    public partial class Form1 : Form
    {
        bool optr_clicked, num_clicked, equal_clicked, symbols_clicked, m_clicked, error_clicked = false;
        double first_num, second_num,symbol_base;
        byte char_symbols, M_count = 0;
        char optr_selected, initial_optr;
        readonly char[] optr_check = { '+', '-', 'x', '÷' };
        readonly char[] symbol_check = {'^', 'v', '/' };

        public Form1()
        {
            InitializeComponent();
        }
        private void change_FontSize(object sender, EventArgs e)
        {
            Label control = sender as Label;

            if (control.Text.Length <= 11) { control.Font = new Font(Font.FontFamily, 33, FontStyle.Bold); }
            else if (control.Text.Length == 12) { control.Font = new Font(Font.FontFamily, 30, FontStyle.Bold); }
            else if (control.Text.Length == 13) { control.Font = new Font(Font.FontFamily, 27, FontStyle.Bold); }
            else { control.Font = new Font(Font.FontFamily, 24, FontStyle.Bold); }

        } //change fontsize to fit
        private void reset_ALL()
        {
            display_2.Text = "";
            display_1.Text = "0";
            optr_clicked = false;
            equal_clicked = false;
            num_clicked = false;
            symbols_clicked = false;
            char_symbols = 0;
        }
        private void error_enabled_but() //enables disabled button
        {
            reset_ALL();
            error_clicked = false;
            foreach (Control control in but_group1.Controls)

                if (control.Name.StartsWith("_"))
                {
                    control.Enabled = true;
                    if (control.Name == "_Sign" || control.Name == "_decimal")
                    {
                        control.BackColor = Color.FromArgb(242, 242, 242);
                    }
                    else if (control.Name == "_equals")
                    {
                        control.BackColor = Color.FromArgb(144, 163, 202);
                    }
                    else
                    {
                        control.BackColor = Color.FromArgb(229, 229, 229);
                    }
                }
        }
        private void _decimal_Click(object sender, EventArgs e)
        {
            if (!display_1.Text.Contains(".")) { display_1.Text = display_1.Text.Insert(display_1.Text.Length, "."); }

        } //add decimal(.) display1
        private void _Sign_Click(object sender, EventArgs e)
        {
            if (display_1.Text.Contains("0"))      { }
            else if (display_1.Text.Contains("-")) { display_1.Text = display_1.Text.Trim('-'); }
            else                                   { display_1.Text = display_1.Text.Insert(0, "-"); }

        } //change sign display1
        private void _clearEntry_Click(object sender, EventArgs e)
        {
            if (error_clicked) { error_enabled_but(); }
            display_1.Text = "0";
        } //clear display1
        private void _clearAll_Click(object sender, EventArgs e)
        {
            if (error_clicked)
            {
                error_enabled_but();
            }
            reset_ALL();
        } //reset to 0
        private void _backspace_Click(object sender, EventArgs e)
        {
            if (error_clicked) { error_enabled_but(); }

            if (display_1.Text.Length == 1) { display_1.Text = "0"; }
            else { display_1.Text = display_1.Text.Remove(display_1.Text.Length - 1); }
        }//remove newest digit
        private void _reciprocal_Click(object sender, EventArgs e)
        {
            var reciprocal_char = "1/( ";

            //adding characters to display_2
            if (symbols_clicked && symbol_check.Any(display_2.Text.Contains))
            {
                var left_side_index = symbol_base.ToString().Length + (char_symbols);
                display_2.Text = display_2.Text.Insert(display_2.Text.Length - left_side_index, reciprocal_char);
                display_2.Text = display_2.Text.Insert(display_2.Text.Length, " )");
            }
            else
            {
                if (equal_clicked) { display_2.Text = ""; }

                symbol_base = Convert.ToDouble(display_1.Text);
                var reciprocal_name = reciprocal_char + symbol_base.ToString() + " )";
                display_2.Text = display_2.Text.Insert(display_2.Text.Length, " " + reciprocal_name);
               
            }
            symbols_clicked = true;
            char_symbols += 6; //length of added characters

            //solving
            display_1.Text = solve(Convert.ToDouble(display_1.Text), optr_selected: '/');
        }//symbols
        private void _sqr_Click(object sender, EventArgs e)
        {
            var sqr_char = "sqr( ";

            //adding characters to display_2
            if (symbols_clicked && symbol_check.Any(display_2.Text.Contains))
            {
                var left_side_index = symbol_base.ToString().Length + (char_symbols);
                display_2.Text = display_2.Text.Insert(display_2.Text.Length - left_side_index, sqr_char);
                display_2.Text = display_2.Text.Insert(display_2.Text.Length, " )");
            }
            else
            {
                if (equal_clicked) { display_2.Text = ""; }

                symbol_base = Convert.ToDouble(display_1.Text);
                var sqr_name = sqr_char + symbol_base.ToString() + " )";
                display_2.Text = display_2.Text.Insert(display_2.Text.Length, " " + sqr_name);
            }
            symbols_clicked = true;
            char_symbols += 7;

            //solving
            display_1.Text = solve(Convert.ToDouble(display_1.Text), optr_selected: '^');
        }//symbols
        private void _sqrroot_Click(object sender, EventArgs e)
        {
            var sqr_root_char = "√( ";

            //adding characters to display_2
            if (symbols_clicked && symbol_check.Any(display_2.Text.Contains))
            {
                var left_side_index = symbol_base.ToString().Length + (char_symbols);
                display_2.Text = display_2.Text.Insert(display_2.Text.Length - left_side_index, sqr_root_char);
                display_2.Text = display_2.Text.Insert(display_2.Text.Length, " )");
            }
            else
            {
                if (equal_clicked) { display_2.Text = ""; }

                symbol_base = Convert.ToDouble(display_1.Text);
                var sqrroot_name = sqr_root_char + symbol_base.ToString() + " )";
                display_2.Text = display_2.Text.Insert(display_2.Text.Length, " " + sqrroot_name);
            }
            symbols_clicked = true;
            char_symbols += 5;

            //solving
            display_1.Text = solve(Convert.ToDouble(display_1.Text), optr_selected: 'v');
        } //symbols
        private void _percent_Click(object sender, EventArgs e)
        {
            string percent_value;
            if (optr_check.Any(display_2.Text.Contains))
            {
                if (equal_clicked)
                {
                    percent_value = solve(Convert.ToDouble(display_1.Text), Convert.ToDouble(display_1.Text), '%');
                    display_1.Text = percent_value;
                    display_2.Text = percent_value;
                }
                else
                {
                    percent_value = solve(Convert.ToDouble(display_1.Text), first_num, '%');
                    display_1.Text = percent_value;
                    display_2.Text = display_2.Text.Insert(display_2.Text.Length, " " + percent_value);
                }
            }
            else
            {
                display_1.Text = 0.ToString();
                display_2.Text = 0.ToString();
            }
            symbols_clicked = true;
        }//symbbols
        private void num_click(object sender, EventArgs e)
        {
            Button nums = sender as Button;
            if (error_clicked)
            {
                error_enabled_but();
            }

            if (equal_clicked || symbols_clicked)
            {
                display_2.Text = "";
                display_1.Text = nums.Text;
                optr_clicked = false;
                equal_clicked = false;
                num_clicked = true;
            }
            else if (optr_clicked || m_clicked) //does not reset display 2,
            {
                display_1.Text = nums.Text;
                optr_clicked = false;
                equal_clicked = false;
                num_clicked = true;
                m_clicked = false;
            }
            else if (display_1.Text == "0")
            {
                display_1.Text = nums.Text;
            }
            else
            {
                display_1.Text = display_1.Text + nums.Text;
            }
            error_clicked = false;
            //reset count symbol
            symbols_clicked = false;
            char_symbols = 0;

        }//numbers 0-9 
        private void Operator(object sender, EventArgs e)
        {
            Button optr = sender as Button;
            optr_selected = char.Parse(optr.Text);

            if (optr_check.Any(display_2.Text.Contains) && num_clicked || optr_check.Any(display_2.Text.Contains) && symbols_clicked)
            {
                second_num = Convert.ToDouble(display_1.Text);

                //solving using method
                var ans = solve(first_num, second_num, initial_optr);
                display_1.Text = ans;
                display_2.Text = ans + " " + optr.Text;
                first_num = Convert.ToDouble(display_1.Text);

                num_clicked = false; // to not increment when operator button is clicked again
            }
            else
            {
                display_2.Text = Convert.ToDouble(display_1.Text) + " " + optr.Text;
                first_num = Convert.ToDouble(display_1.Text);
            }
            //get optr display 2
            initial_optr = char.Parse(display_2.Text.Substring(first_num.ToString().Length + 1, 1));
            //button reset
            symbols_clicked = false;
            char_symbols = 0;
            equal_clicked = false; //if(operator clicked only) second num will be display1
            optr_clicked = true;

        }//optr
        private void _equals_Click(object sender, EventArgs e)
        {
            if (!optr_check.Any(display_2.Text.Contains)  && !symbols_clicked ) //to increment if symbols_clicked after optr
            {
                    display_2.Text = display_1.Text + " =";
            }
            else if (equal_clicked) //equal button clicked second time
            {
                first_num = Convert.ToDouble(display_1.Text);
                display_2.Text = first_num.ToString() + " " + optr_selected.ToString() + " " + second_num.ToString();
                //solving using method
                var ans = solve(first_num, second_num, optr_selected);
                display_1.Text = ans;
                
            }
            else  //if equals button clicked for first time
            {
                //constant
                second_num = Convert.ToDouble(display_1.Text);

                //solving using method
                var ans = solve(first_num, second_num, optr_selected);
                display_1.Text = ans;
                display_2.Text = first_num.ToString() + " " + optr_selected.ToString() + " " + second_num.ToString();

                //equal_clicked = true;
                num_clicked = false; // to not increment if operator is clicked
            }
            //button reset
            equal_clicked = true;
            symbols_clicked = false;
            char_symbols = 0;
        } //equals button
        private string solve(double first = 0, double second = 0, char optr_selected = '+')
        {
            double the_solve = 0;
            switch (optr_selected)
            {
                case '+':
                    the_solve = first + second;
                    break;
                case '-':
                    the_solve = first - second;
                    break;
                case 'x':
                    the_solve = first * second;
                    break;
                case '÷':
                    the_solve = first / second;
                    break;
                //ADDING SYMBOLS
                case '%':
                    if (initial_optr == '+' || initial_optr == '-') { the_solve = Convert.ToDouble(first) / 100 * second; }
                    else { the_solve = Convert.ToDouble(first) / 100; }
                    break;
                case '/':
                    the_solve = 1 / Convert.ToDouble(first);
                    break;
                case '^':
                    the_solve = Math.Pow(first, 2);
                    break;
                case 'v':
                    the_solve = Math.Sqrt(Convert.ToDouble(first));
                    break;
            }
            return error_return(the_solve);

            //check error
            string error_return(double error)
            {
                if (double.IsNaN(error) || double.IsInfinity(error))
                {
                    error_clicked = true;
                    //disables all controls with "_" in name
                    foreach (Control control in but_group1.Controls)
                    {
                        if (control.Name.StartsWith("_"))
                        {
                            control.BackColor = Color.FromKnownColor(KnownColor.Silver);
                            control.Enabled = false;
                        }
                    }
                    string Error = "Invalid input";
                    if (double.IsInfinity(error)) { Error = "Overflow"; } //different name for inf error
                    return Error;
                }
                else { return error.ToString(); } // no error
            }
        }//all operations

        //////M-BUTTONS//////
        private void _mc_Click(object sender, EventArgs e)
        {
            panel_M.Controls.Clear();
            _mc.Enabled = false;
            _mr.Enabled = false;
            _mlist.Enabled = false;
            m_clicked = true; //reset display 1
        }//clear M list
        private void _mr_Click(object sender, EventArgs e)
        {
            display_1.Text = panel_M.Controls[0].Text;
            m_clicked = true;
        }//get first M value at display_1
        private void m_plus_minus(object sender, EventArgs e)
        {
            Control control = sender as Control;
            char sign = '+';
            if (panel_M.Controls.Count < 1)
            {
                _mc.Enabled = true;
                _mr.Enabled = true;
                _mlist.Enabled = true;
                Create_M_button();
            }
            else //determine operator if + or - clicked
            {
                if (!control.Text.Contains("+")) { sign = '-'; }
                Control first_label = panel_M.Controls[0];
                first_label.Text = solve(Convert.ToDouble(first_label.Text), Convert.ToDouble(display_1.Text), sign);
            }
            m_clicked = true;
        }//add or subtract first M value
        private void _mstore_Click(object sender, EventArgs e)
        {
            _mlist.Enabled = true;
            _mc.Enabled = true;
            _mr.Enabled = true;
            m_clicked = true;
            Create_M_button();
        }//add new M value
        private void Create_M_button()
        {
            M_count++;
            Label display_M = new Label
            {
                Name = "display_M" + M_count.ToString(),
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Microsoft YaHei", 12),
                Text = display_1.Text,
                AutoSize = false,
                Size = new Size(262, 29),
            };
            Button button_mc = new Button
            {
                Name = "butC" + M_count.ToString(),
                Text = "MC"
            };
            Button button_mc_add = new Button
            {
                Name = "butA" + M_count.ToString(),
                Text = "M+",
            };
            Button button_mc_minus = new Button
            {
                Name = "butM" + M_count.ToString(),
                Text = "M-"
            };
            //add methods to buttons
            button_mc_add.Click += M_operation;
            button_mc_minus.Click += M_operation;
            button_mc.Click += panel_m_clearbut;

            panel_M.Controls.Add(display_M);
            panel_M.Controls.Add(button_mc_add);
            panel_M.Controls.Add(button_mc_minus);
            panel_M.Controls.Add(button_mc);

            panel_M.Controls.SetChildIndex(display_M, 0);
            panel_M.Controls.SetChildIndex(button_mc, 1);
            panel_M.Controls.SetChildIndex(button_mc_minus, 2);
            panel_M.Controls.SetChildIndex(button_mc_add, 3);


        }//create [m+,m-,mc] buttons at m panel
        private void panel_m_clearbut(object sender, EventArgs e)
        {
            Control control = sender as Control;
            int x = panel_M.Controls.GetChildIndex(control); //get index of panel mc button
            panel_M.Controls.RemoveAt(x - 1); //remove display
            panel_M.Controls.RemoveAt(x); //remove minus but
            panel_M.Controls.RemoveAt(x);//remove plus but
            panel_M.Controls.RemoveAt(x - 1);//remove clear but
        }//remove individual M value
        private void M_operation(object sender, EventArgs e)
        {
            Control but = sender as Control;
            //get plus or minus operator
            char sign;
            if (but.Text.Contains("+")) { sign = '+'; }
            else { sign = '-'; }

            //get specified display_M
            Control m_select = panel_M.Controls["display_M" + but.Name.Last()];
            //solving using method
            m_select.Text = solve(Convert.ToDouble(m_select.Text), Convert.ToDouble(display_1.Text), sign);
            m_clicked = true;
        }//M operation
        private void open_Mtab(object sender, EventArgs e)
        {
            //disable all, except panel_M
            foreach (Control button in but_group1.Controls)
            {
                if (button.Name == "panel_M") { button.Visible = true; }
                else { button.Enabled = false; }
            }
        }//view M list
        private void panel_M_leave(object sender, MouseEventArgs e)
        {
            if (panel_M.Visible)
            {
                foreach (Control button in but_group1.Controls)
                {
                    if (button.Name == "panel_M") { button.Visible = false; }
                    else
                    {
                        if (panel_M.Controls.Count < 1) //if nothing displayed in M list
                        {
                            _mr.Enabled = false;
                            _mc.Enabled = false;
                            _mlist.Enabled = false;
                        }
                        button.Enabled = true;
                    }
                }
            }
        }//go back to calculator
    }
}