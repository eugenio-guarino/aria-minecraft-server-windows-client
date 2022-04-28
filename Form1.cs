using System.Windows.Forms;
using System.Drawing;
using System;

public class Form1 : Form
{
    public void FormLayout()
    {
        this.Name = "Daniland";
        this.Text = "Daniland";
        this.Size = new System.Drawing.Size(350, 350);

        // avoid resizing
        this.StartPosition = FormStartPosition.CenterScreen;
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;
        this.MinimizeBox = false;

        // create snarkyPhrase to
        snarkyPhrase snarkyPhrase = new snarkyPhrase();
        snarkyPhrase.Text = "Snarky phrase.";
        snarkyPhrase.Location = new Point(70, 70);
        snarkyPhrase.Size = new Size(200, 60);
        this.Controls.Add(snarkyPhrase);

        //create button to start the server
        Button startButton = new Button();
        startButton.Text = "AVVIA IL SERVER";
        startButton.Location = new Point(70, 150);
        startButton.Size = new Size(200, 60);
        this.Controls.Add(startButton);

        startButton.Click += new EventHandler(this.CreateServer);

    }

    void CreateServer(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(txtName.Text) && !lstNames.Items.Contains(txtName.Text))
            lstNames.Items.Add(txtName.Text);
    }
}
