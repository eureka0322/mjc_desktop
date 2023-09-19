using MJC.common;
using MJC.common.components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MJC.forms.order
{
    public partial class ShippingInformation : BasicModal
    {
        private FCheckBox enableShipping = new FCheckBox("Enable Shipping");
        private FInputBox shipVia = new FInputBox("Ship Via:");
        private FCheckBox FOB = new FCheckBox("FOB");
        private FInputBox salesman = new FInputBox("Salesman:");
        private FLabel shipTo = new FLabel("Ship to:");
        private ModalButton selectShippingAddress = new ModalButton("F2 Select Shipping Address", Keys.F2);
        private FLabel shippingAddressContent = new FLabel("[display selected shipping address here]");

        private ModalButton closeShippingInformation = new ModalButton("ESC Close Shipping Information", Keys.Escape);

        private int customerId = 0;
        public bool sEnableShipping { get; set; }
        public string sVia { get; set; }
        public bool sFob { get; set; }
        public string sSalesman { get; set; }
        public string sShipTo { get; set; }
        public int sShipID { get; set; }

        public ShippingInformation(int cID = 0) : base("Shipping Information")
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Width = 600;
            InitForms();
            AddHotKeyEvents();

            this.customerId = cID;
            this.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.F2) {
                    this.selectShippingAddress.GetButton().PerformClick();
                }
            };

            setDetails();
        }
        public void setDetails()
        {
            this.enableShipping.GetCheckBox().Checked = sEnableShipping;
            this.shipVia.GetTextBox().Text = sVia;
            this.FOB.GetCheckBox().Checked = sFob;
            this.shipVia.GetTextBox().Text = sSalesman;
            this.shippingAddressContent.GetLabel().Text = sShipTo;
        }

        public void AddHotKeyEvents()
        {
            selectShippingAddress.GetButton().Click += (s, e) =>
            {
                ShipInformation ShipInfoModal = new ShipInformation(this.customerId, true);
                ShipInfoModal._prevForm = this;
                this.Enabled = false;
                ShipInfoModal.Show();
                ShipInfoModal.VisibleChanged += (ss, sargs) =>
                {
                    if (!ShipInfoModal.Visible)
                    {
                        shippingAddressContent.GetLabel().Text = ShipInfoModal.shippingAddress;
                        sShipID = ShipInfoModal.selectedID;
                        ShipInfoModal.Close();
                        this.Enabled = true;
                    }
                };
            };

            closeShippingInformation.GetButton().Click += (s, e) =>
            {
                this.Hide();
            };

            this.FormClosed += (s, e) =>
            {
                this.sEnableShipping = enableShipping.GetCheckBox().Checked;
                this.sVia = shipVia.GetTextBox().Text;
                this.sFob = FOB.GetCheckBox().Checked;
                this.sSalesman = salesman.GetTextBox().Text;
                this.sShipTo = shippingAddressContent.GetLabel().Text;
            };
        }

        public void InitForms()
        {
            int xPos = 30;
            int yPos = 20;
            int yDistance = 50;

            enableShipping.SetPosition(new Point(xPos, yPos));
            this.Controls.Add(enableShipping.GetCheckBox());

            yPos += yDistance;

            shipVia.SetPosition(new Point(xPos, yPos));
            this.Controls.Add(shipVia.GetLabel());
            this.Controls.Add(shipVia.GetTextBox());

            yPos += yDistance;

            FOB.SetPosition(new Point(xPos, yPos));
            this.Controls.Add(FOB.GetCheckBox());

            yPos += yDistance;

            salesman.SetPosition(new Point(xPos, yPos));
            this.Controls.Add(salesman.GetLabel());
            this.Controls.Add(salesman.GetTextBox());

            yPos += yDistance;

            shipTo.SetPosition(new Point(xPos, yPos));
            this.Controls.Add(shipTo.GetLabel());

            selectShippingAddress.GetButton().Location = new Point(xPos + 200, yPos);
            selectShippingAddress.GetButton().Width = 300;
            this.Controls.Add(selectShippingAddress.GetButton());

            yPos += yDistance;

            shippingAddressContent.SetPosition(new Point(xPos, yPos));
            this.Controls.Add(shippingAddressContent.GetLabel());

            yPos += yDistance*2;

            closeShippingInformation.GetButton().Location = new Point(xPos, yPos);
            this.Controls.Add(closeShippingInformation.GetButton());
        }
    }
}
