using MJC.common.components;
using MJC.forms;
using MJC.forms.sku;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MJC.common
{
    public partial class LookupOptions : BasicModal
    {
        private FInputBox PayPrintInvoice = new FInputBox("1)Search SKU");
        private FInputBox PayNoPrint = new FInputBox("2)Search Cross References");
        private FInputBox UpdateHeldOrder = new FInputBox("3)Non-Inventory Item");
        private FInputBox HeldOrderPrintInvoice = new FInputBox("4)Special Order Item");
        private FInputBox UpdateQuote = new FInputBox("5)Cancel");
        public int lookupOptions = 0;

        public LookupOptions() : base("Lookup Options", false)
        {
            InitializeComponent();
            //_setModalStyle2();
            this.Size = new Size(400, 280);

            InitForms();

            this.KeyDown += LookupOptions_KeyDown;
        }

        private void InitForms()
        {
            int xPos = 30;
            int yPos = 20;
            int yDistance = 40;

            PayPrintInvoice.SetPosition(new Point(xPos, yPos));
            this.Controls.Add(PayPrintInvoice.GetLabel());

            yPos += yDistance;
            PayNoPrint.SetPosition(new Point(xPos, yPos));
            this.Controls.Add(PayNoPrint.GetLabel());

            yPos += yDistance;
            UpdateHeldOrder.SetPosition(new Point(xPos, yPos));
            this.Controls.Add(UpdateHeldOrder.GetLabel());

            yPos += yDistance;
            HeldOrderPrintInvoice.SetPosition(new Point(xPos, yPos));
            this.Controls.Add(HeldOrderPrintInvoice.GetLabel());

            yPos += yDistance;
            UpdateQuote.SetPosition(new Point(xPos, yPos));
            this.Controls.Add(UpdateQuote.GetLabel());
        }

        private void LookupOptions_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    lookupOptions = 0;
                    this.Close();
                    break;
                case Keys.D1:
                case Keys.NumPad1:
                    lookupOptions = 1;
                    this.Close();
                    break;
                case Keys.D2:
                case Keys.NumPad2:
                    this.Close();
                    break;
                case Keys.D3:
                case Keys.NumPad3:
                    this.Close();
                    break;
                case Keys.D4:
                case Keys.NumPad4:
                    this.Close();
                    break;
                case Keys.D5:
                case Keys.NumPad5:
                    this.Close();
                    break;
            }
        }
    }
}
