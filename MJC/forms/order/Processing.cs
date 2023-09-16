using MJC.common.components;
using MJC.common;
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
    public partial class Processing : BasicModal
    {
        private FInputBox ProcessedBy = new FInputBox("Processed by:");
        private FInputBox InvoiceDesc = new FInputBox("Invoice Desc:");
        private FDateTime DateShiped = new FDateTime("Date Shipped:");
        private FInputBox InvoiceNumber = new FInputBox("Invoice #:");

        private ModalButton MBResume = new ModalButton("F1 Resume", Keys.F1);
        private Button MBResume_button;
        private ModalButton MBCancelOrder = new ModalButton("F5 Cancel Order", Keys.F5);
        private Button MBCancelOrder_button;
        private ModalButton MBNext = new ModalButton("F2 Next", Keys.F2);
        private Button MBNext_button;

        private int saveFlage = 0;
        public int orderId = 0;

        public Processing() : base("Processing")
        {
            InitializeComponent();
            this.Size = new Size(600, 350);
            InitForms();
            InitModalButtons();
            setDetails();
        }

        public void setDetails()
        {
            this.ProcessedBy.GetTextBox().Text = ProcessOrder.sProcessedBy;
            this.InvoiceDesc.GetTextBox().Text = ProcessOrder.sInvoiceDesc;
            if (ProcessOrder.sDateShiped >= new DateTime(1900, 1, 1) && ProcessOrder.sDateShiped <= new DateTime(2100, 12, 31)) this.DateShiped.GetDateTimePicker().Value = ProcessOrder.sDateShiped;
            this.InvoiceNumber.GetTextBox().Text = ProcessOrder.sInvoiceNumber;
        }

        private void InitModalButtons()
        {
        }
        private void resume_button_Click(object sender, EventArgs e)
        {
            saveFlage = 0;
            this.Close();
        }
        private void cancelOrder_button_Click(object sender, EventArgs e)
        {
            saveFlage = 1;
            this.Close();
        }
        private void next_button_Click(object sender, EventArgs e)
        {
            ProcessOrder.sProcessedBy = this.ProcessedBy.GetTextBox().Text;
            ProcessOrder.sInvoiceDesc = this.InvoiceDesc.GetTextBox().Text;
            ProcessOrder.sDateShiped = this.DateShiped.GetDateTimePicker().Value;
            ProcessOrder.sInvoiceNumber = this.InvoiceNumber.GetTextBox().Text;
            //            Session.OrderModelObj.UpdateProcessingDatabyId(processedBy, invoiceDesc, dateShiped, invoiceNumber, orderId);

            saveFlage = 2;
            this.Close();
        }

        public int GetFlag()
        {
            return saveFlage;
        }
        private void InitForms()
        {
            int xPos = 30;
            int yPos = 20;
            int yDistance = 50;

            ProcessedBy.SetPosition(new Point(xPos, yPos));
            this.Controls.Add(ProcessedBy.GetLabel());
            this.Controls.Add(ProcessedBy.GetTextBox());

            yPos += yDistance;
            InvoiceDesc.SetPosition(new Point(xPos, yPos));
            this.Controls.Add(InvoiceDesc.GetLabel());
            this.Controls.Add(InvoiceDesc.GetTextBox());

            yPos += yDistance;
            DateShiped.SetPosition(new Point(xPos, yPos));
            this.Controls.Add(DateShiped.GetLabel());
            this.Controls.Add(DateShiped.GetDateTimePicker());

            yPos += yDistance;
            InvoiceNumber.SetPosition(new Point(xPos, yPos));
            this.Controls.Add(InvoiceNumber.GetLabel());
            this.Controls.Add(InvoiceNumber.GetTextBox());

            yPos += 70;

            ModalButton_HotKeyDown(MBResume);
            MBResume_button = MBResume.GetButton();
            MBResume_button.Click += (sender, e) => resume_button_Click(sender, e);
            MBResume.SetPosition(new Point(xPos, yPos));
            this.Controls.Add(MBResume_button);

            ModalButton_HotKeyDown(MBCancelOrder);
            MBCancelOrder_button = MBCancelOrder.GetButton();
            MBCancelOrder_button.Click += (sender, e) => cancelOrder_button_Click(sender, e);
            MBCancelOrder.SetPosition(new Point(220, yPos));
            this.Controls.Add(MBCancelOrder_button);

            ModalButton_HotKeyDown(MBNext);
            MBNext_button = MBNext.GetButton();
            MBNext_button.Click += (sender, e) => next_button_Click(sender, e);

            MBNext.SetPosition(new Point(450, yPos));
            this.Controls.Add(MBNext_button);
        }

    }
}
