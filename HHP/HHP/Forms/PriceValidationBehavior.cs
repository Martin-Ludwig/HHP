using HHP.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Windows.Networking.Connectivity;
using Windows.System.Display;
using Xamarin.Forms;

namespace HHP.Forms
{
    class PriceValidationBehavior : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += OnEntryTextChanged;
            base.OnAttachedTo(entry);
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(entry);
        }

        /// <summary>
        /// This function is triggered everytime the Text is changed.
        /// Checks if text is a number with two optiinal decimal places.
        /// Changes textcolor to red if text entry is incorrect.
        /// </summary>
        void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            String result;
            bool isValid = IsValidPriceFormat(args.NewTextValue, out result);


            if (!isValid) {
                ((Entry)sender).TextColor = HHP_Colors.Error;
            } else
            {
                ((Entry)sender).TextColor = HHP_Colors.Text;
            }

        }


        /// <summary>
        /// Checks if value is a number with two optional decimal places.
        /// Out result is "0" if value is not a number.
        /// Returns True or False
        /// </summary>
        private bool IsValidPriceFormat(String value, out String result)
        {
            result = "0";

            string pattern = @"^(?![.0]*$)\d+(?:.\d{1,2})?$";
            bool isDouble = Regex.IsMatch(value, pattern);

            if (isDouble && (Double.Parse(value) > 0))
            {
                // shorten number to two decimal places
                result = String.Format("{0:0.##}", Double.Parse(value));

                return true;
            }
            return false;
        }

    }
}
