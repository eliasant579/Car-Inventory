using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace CarInventory
{
    public partial class Form1 : Form
    {
        List<Car> inventory = new List<Car>();

        public Form1()
        {
            InitializeComponent();
            loadDB();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            string year, make, colour, mileage;

            year = yearInput.Text;
            make = makeInput.Text;
            colour = colourInput.Text;
            mileage = mileageInput.Text;

            Car c = new Car(year, make, colour, mileage);
            inventory.Add(c);

            outputLabel.Text = yearInput.Text = makeInput.Text = colourInput.Text = mileageInput.Text = "";
            yearInput.Focus();
            displayItems();
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < inventory.Count; i++)
            {
                if (inventory[i].make == makeInput.Text)
                {
                    inventory.RemoveAt(i);
                }
            }

            int index = inventory.FindIndex(car => car.make == makeInput.Text);

            if (index >-1)
            {
                inventory.RemoveAt(index);
            }

            displayItems();
        }

        public void displayItems()
        {
            outputLabel.Text = "";

            foreach (Car c in inventory)
            {
                outputLabel.Text += c.year + " "
                     + c.make + " "
                     + c.colour + " "
                     + c.mileage + "\n";
            }
        }

        public void loadDB()
        {
            string newYear, newMake, newColour, newMileage;
            newYear = newMake = newColour = newMileage = "";

            XmlReader reader = XmlReader.Create("employeeData.xml");

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Text)
                {
                    newYear = reader.ReadString();

                    reader.ReadToNextSibling("make");
                    newMake = reader.ReadString();

                    reader.ReadToNextSibling("colour");
                    newColour = reader.ReadString();

                    reader.ReadToNextSibling("mileage");
                    newMileage = reader.ReadString();

                    Car s = new Car(newYear, newMake, newColour, newMileage);
                    inventory.Add(s);
                }
            }

            reader.Close();
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            XmlWriter writer = XmlWriter.Create("employeeData.xml", null);

            writer.WriteStartElement("Cars");

            foreach (Car emp in inventory)
            {
                writer.WriteStartElement("Car");

                writer.WriteElementString("year", emp.year);
                writer.WriteElementString("make", emp.make);
                writer.WriteElementString("colour", emp.colour);
                writer.WriteElementString("mileage", emp.mileage);

                writer.WriteEndElement();
            }

            writer.WriteEndElement();

            writer.Close();
        }
    }
}
