using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraGrid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GridControl gridControl1 = new GridControl();
            Controls.Add(gridControl1);
            gridControl1.Dock = DockStyle.Fill;

            //Create a data source
            BindingList<Customer> list = new BindingList<Customer>();
            list.Add(new Customer("Andrew", "Weber"));
            list.Add(new Customer("", "Kovar")); // An invalid first name
            list.Add(new Customer("Kathy", ""));  // An invalid last name

            BindingSource bindingSource1 = new BindingSource(list, "");
            gridControl1.DataSource = bindingSource1;

            // To show data source errors in standalone editors, bind a DXErrorProvider to the data source.
            DXErrorProvider dxErrorProvider1 = new DXErrorProvider(this);
            dxErrorProvider1.DataSource = bindingSource1;

            // Create a TextEdit control and bind it to the First Name field
            TextEdit textEdit1 = new TextEdit();
            textEdit1.DataBindings.Add(new Binding("EditValue", bindingSource1, "FirstName"));

            LabelControl label = new LabelControl();
            label.Padding = new Padding(0, 30, 0, 5);
            label.Text = "TextEdit bound to FirstName field:";

            Controls.Add(label);
            label.Dock = DockStyle.Bottom;

            Controls.Add(textEdit1);
            textEdit1.Dock = DockStyle.Bottom;
        }

        public class Customer : IDataErrorInfo, INotifyPropertyChanged
        {
            public Customer(string firstName, string lastName)
            {
                FirstName = firstName;
                LastName = lastName;

            }
            string firstName;
            public string FirstName
            {
                get
                {
                    return firstName;
                }
                set
                {
                    ValidateValue(value);
                    firstName = value;
                    OnPropertyChanged();
                }
            }

            string lastName;
            public string LastName
            {
                get
                {
                    return lastName;
                }
                set
                {
                    ValidateValue(value);
                    lastName = value;
                    OnPropertyChanged();
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            public virtual void OnPropertyChanged([CallerMemberName] String propertyName = "")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            // Stores errors for all properties of the Customer class.
            Dictionary<string, string> PropertyErrors = new Dictionary<string, string>();
            string IDataErrorInfo.this[string propertyName]
            {
                get
                {
                    string errorText;
                    return PropertyErrors.TryGetValue(propertyName, out errorText) ? errorText : null;
                }
            }
            string IDataErrorInfo.Error => string.Empty;

            // Clears or sets errors for the FirstName and LastName fields.
            void ValidateValue(string value, [CallerMemberName] String propertyName = "")
            {
                bool isValid = !string.IsNullOrEmpty(value);
                if (isValid)
                {
                    //Clear a previous error, if any.
                    PropertyErrors.Remove(propertyName);
                }
                else
                {
                    //Set an error.
                    string errorText = propertyName + " is required";
                    PropertyErrors[propertyName] = errorText;
                }
            }

        }
    }
}
