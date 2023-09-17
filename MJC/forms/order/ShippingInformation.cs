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
        private FInputBox shipVia = new FInputBox("Ship Via");
        private FCheckBox fbo = new FCheckBox("FBO");
        private FInputBox salesman = new FInputBox("Salesman");
        private FLabel shipTo = new FLabel("Ship to:");
        private ModalButton selectShippingAddress = new ModalButton("F2 Select Shipping Address", Keys.F2);
        private FLabel shippingAddressContent = new FLabel("[display selected shipping address here]");

        private ModalButton closeShippingInformation = new ModalButton("ESC Close Shipping Information", Keys.Escape);

        private int customerId = 0;
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

            enableShipping.GetCheckBox().Checked = ProcessOrder.enableShipping;
            shipVia.GetTextBox().Text = ProcessOrder.via;
            fbo.GetCheckBox().Checked = ProcessOrder.fbo;
            salesman.GetTextBox().Text = ProcessOrder.salesman;
            shippingAddressContent.GetLabel().Text = ProcessOrder.shipTo;
        }

        public void AddHotKeyEvents()
        {
            selectShippingAddress.GetButton().Click += (s, e) =>
            {
                ShippingModal ShipInfoModal = new ShippingModal(this.customerId, true);
                ShipInfoModal._prevForm = this;
                this.Enabled = false;
                ShipInfoModal.Show();
                ShipInfoModal.FormClosed += (ss, sargs) =>
                {
                    shippingAddressContent.GetLabel().Text = ShipInfoModal.shippingAddress;
                    this.Enabled = true;
                };
            };

            closeShippingInformation.GetButton().Click += (s, e) =>
            {
                this.Hide();
            };

            this.FormClosed += (s, e) =>
            {
                ProcessOrder.enableShipping = enableShipping.GetCheckBox().Checked;
                ProcessOrder.via = shipVia.GetTextBox().Text;
                ProcessOrder.fbo = fbo.GetCheckBox().Checked;
                ProcessOrder.salesman = salesman.GetTextBox().Text;
                ProcessOrder.shipTo = shippingAddressContent.GetLabel().Text;
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

            fbo.SetPosition(new Point(xPos, yPos));
            this.Controls.Add(fbo.GetCheckBox());

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
