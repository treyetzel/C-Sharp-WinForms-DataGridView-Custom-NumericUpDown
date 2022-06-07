using System;
using System.Windows.Forms;
using System.Globalization;
using System.ComponentModel;

namespace Example
{
    /// <summary>
    /// This is a custom NumericUpDown control that allows for leading/trailing text before/after the
    /// number displayed in the control's textbox
    /// Got from https://stackoverflow.com/questions/18751838/adding-a-sign-to-numericupdown-value-like
    /// </summary>
    public class CustomNumericUpDown : NumericUpDown
    {
        private string _leadingSymbol = "";

        /// <summary>
        /// Leading symbol to go before the text in the NumericUpDown control
        /// </summary>
        [DefaultValue("")]
        public string LeadingSymbol
        {
            get { return _leadingSymbol; }
            set
            {
                _leadingSymbol = value;
                this.UpdateEditText();
            }
        }


        private string _trailingSymbol = "";

        /// <summary>
        /// Trailing symbol to go after the text in the NumericUpDown control
        /// </summary>
        [DefaultValue("")]
        public string TrailingSymbol
        {
            get { return _trailingSymbol; }
            set
            {
                _trailingSymbol = value;
                this.UpdateEditText();
            }
        }

        
        /// <summary>
        /// Sets the text inside the [Custom]NumericUpDown control
        /// </summary>
        protected override void UpdateEditText()
        {
            if(UserEdit)
            {
                ParseEditText();
            }
            ChangingText = true;
            base.Text = _leadingSymbol + GetNumberFromText(this.Value) + _trailingSymbol;
        }

        /// <summary>
        /// Converts decimal into formatted string
        /// </summary>
        /// <param name="num">decimal to be converted into string</param>
        /// <returns></returns>
        private string GetNumberFromText(decimal num)
        {
            string num_text;
            if (Hexadecimal)
            {
                num_text = ((Int64)num).ToString("X", CultureInfo.InvariantCulture);
            } else
            {
                num_text = num.ToString((ThousandsSeparator ? "N" : "F") + DecimalPlaces.ToString(CultureInfo.CurrentCulture), CultureInfo.CurrentCulture);
            }
            return num_text;
        }

        /// <summary>
        /// Validates & Updates the text displayed in the [Custom]NumericUpDown control
        /// </summary>
        protected override void ValidateEditText()
        {
            ParseEditText();
            UpdateEditText();
        }

        /// <summary>
        /// Converts the text in the textbox to a numeric value
        /// </summary>
        protected new void ParseEditText()
        {
            
            try
            {
                string text = base.Text;
                if(!string.IsNullOrEmpty(_leadingSymbol))
                {
                    if(text.StartsWith(_leadingSymbol))
                    {
                        text = text.Substring(_leadingSymbol.Length);
                    }
                }
                if(!string.IsNullOrEmpty(_trailingSymbol))
                {
                    if (text.EndsWith(_trailingSymbol))
                    {
                        text = text.Substring(0, text.Length - _trailingSymbol.Length);
                    }
                }

                if(!string.IsNullOrEmpty(text) && !(text.Length == 1 && text =="-"))
                {
                    if(Hexadecimal)
                    {
                        base.Value = Constrain(Convert.ToDecimal(Convert.ToInt32(text, 16)));
                    } else
                    {
                        base.Value = Constrain(decimal.Parse(text, CultureInfo.CurrentCulture));
                    }
                }
            }
            catch
            {
            }
            finally 
            {
                UserEdit = false;
            }
        }

        /// <summary>
        /// Constrains the minimum and maximum when they are set
        /// </summary>
        /// <param name="value">(decimal) value to check and constrain</param>
        /// <returns></returns>
        private decimal Constrain(decimal value)
        {
            if (value < base.Minimum)
                value = base.Minimum;
            if (value > base.Maximum)
                value = base.Maximum;
            return value;
        }
    }
}
