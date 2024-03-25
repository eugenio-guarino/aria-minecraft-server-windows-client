using System.Windows.Forms;
using System.Drawing;
using System;
using System.Linq;

public class Client : Form
{
    public void FormLayout()
    {
        this.Name = "Aria Server Launcher";
        this.Text = "Aria Server Launcher";
        this.Size = new System.Drawing.Size(350, 480);

        // Disable Window resizing
        this.StartPosition = FormStartPosition.CenterScreen;
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;
        this.MinimizeBox = false;

        // Title
        System.Windows.Forms.Label titleLabel = new System.Windows.Forms.Label();
        titleLabel.Text = "ARIA";
        titleLabel.Location = new Point(70, 10);
        titleLabel.Font = new Font("Brush Script MT", 18, FontStyle.Regular); // Set the font to "Brush Script MT" and adjust size
        titleLabel.TextAlign = ContentAlignment.MiddleCenter;
        titleLabel.Size = new Size(200, 60);
        this.Controls.Add(titleLabel);
        this.ActiveControl = titleLabel;


        // get a random phrase from database
        string randomPhrase = RandomPhrase(PhrasesList.Phrases);

        // Label with funny phrases
        System.Windows.Forms.Label snarkyPhraseLabel = new System.Windows.Forms.Label();
        snarkyPhraseLabel.Text = randomPhrase;
        snarkyPhraseLabel.Location = new Point(70, 60);
        snarkyPhraseLabel.Font = new Font("Consolas", 8);
        snarkyPhraseLabel.TextAlign = ContentAlignment.MiddleCenter;
        snarkyPhraseLabel.Size = new Size(200, 60);
        this.Controls.Add(snarkyPhraseLabel);

        // TextBox for secret code
        TextBox tokenTextBox = new TextBox();
        tokenTextBox.Text = "insert the code";
        tokenTextBox.Name = "tokenTextBox";
        tokenTextBox.Location = new Point(80, 175);
        tokenTextBox.Size = new Size(180, 50);
        tokenTextBox.Font = new Font("Consolas", 9);
        this.Controls.Add(tokenTextBox);

        // Start Button
        Button startButton = new Button();
        startButton.Text = "START UP SERVER";
        startButton.Location = new Point(70, 235);
        startButton.Font = new Font("Tahoma", 11, FontStyle.Bold);
        startButton.Size = new Size(200, 50);

        // Apply visual enhancements
        startButton.BackColor = Color.FromArgb(224, 240, 255); // Light blue color for a soothing effect
        startButton.FlatAppearance.BorderSize = 0; // Remove border
        startButton.FlatStyle = FlatStyle.Flat; // Flat appearance
        startButton.ForeColor = Color.Black; // Set font color to black for better readability
        startButton.UseCompatibleTextRendering = true;

        // Apply gradient background using background image
        Image buttonBackground = new Bitmap(startButton.Width, startButton.Height);
        using (Graphics g = Graphics.FromImage(buttonBackground))
        {
            using (var brush = new System.Drawing.Drawing2D.LinearGradientBrush(startButton.ClientRectangle, Color.FromArgb(224, 240, 255), Color.FromArgb(191, 219, 255), 90))
            {
                g.FillRectangle(brush, startButton.ClientRectangle);
            }
        }
        startButton.BackgroundImage = buttonBackground;

        this.Controls.Add(startButton);

        // Status label
        System.Windows.Forms.Label statusLabel = new System.Windows.Forms.Label();
        statusLabel.Name = "statusLabel";
        statusLabel.Location = new Point(70, 290);
        statusLabel.Size = new Size(200, 50);
        statusLabel.Font = new Font("Consolas", 9);
        statusLabel.TextAlign = ContentAlignment.MiddleCenter;
        this.Controls.Add(statusLabel);

        Button paypalButton = new Button();
        paypalButton.Text = "Donate via PayPal";
        paypalButton.Size = new Size(150, 30); // Adjust size as needed
        paypalButton.Font = new Font("Tahoma", 10);
        paypalButton.BackColor = Color.Blue;
        paypalButton.ForeColor = Color.White;
        paypalButton.FlatStyle = FlatStyle.Flat; // To remove button border
        paypalButton.FlatAppearance.BorderSize = 0; // To remove button border
        paypalButton.Location = new Point((this.ClientSize.Width - paypalButton.Width) / 2, 350); // Center horizontally
        this.Controls.Add(paypalButton);

        // Event handler for PayPal button click
        paypalButton.Click += new EventHandler((sender, e) =>
        {
            // Open PayPal link
            System.Diagnostics.Process.Start("https://www.paypal.me/ungenio6");
        });

        // Handlers

    }

    void StartServer(object sender, EventArgs ev)
    {
        System.Windows.Forms.Label statusLabel = this.Controls.Find("statusLabel", true).FirstOrDefault() as System.Windows.Forms.Label;
        TextBox tokenBox = this.Controls.Find("tokenTextBox", true).FirstOrDefault() as TextBox;
        string githubToken = tokenBox.Text;
        string username = "eugenio-guarino";
        string repo = "aria-minecraft-server-iac";
        string result = " ";

        if (String.IsNullOrEmpty(githubToken))
        {
            statusLabel.Text = "Code can't be empty.";
            statusLabel.ForeColor = Color.DarkRed;
            return;
        }

        // Couldn't get the http request to work on HTTP Client, so CMD process was used instead.
        System.Diagnostics.Process process = new System.Diagnostics.Process();
        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
        startInfo.RedirectStandardOutput = true;
        startInfo.RedirectStandardError = true;
        startInfo.UseShellExecute = false;
        startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        startInfo.FileName = "cmd.exe";
        startInfo.Arguments = $@"/C curl --request POST --url ""https://api.github.com/repos/{username}/{repo}/dispatches"" --header ""authorization: Bearer {githubToken}""" + @" --data ""{\""event_type\"":\""create-infr\""}""";
        process.StartInfo = startInfo;

        process.OutputDataReceived += (s, e) => result += e.Data;
        process.Start();

        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        process.WaitForExit();


        if (!(result.IndexOf("bad credentials", StringComparison.OrdinalIgnoreCase) >= 0))
        {
            statusLabel.Text = "Success! World will be ready in 5 minutes.";
            statusLabel.ForeColor = Color.DarkGreen;
        }
        else
        {
            statusLabel.Text = "Wrong secret code.";
            statusLabel.ForeColor = Color.DarkRed;
        }
    }

    public string RandomPhrase(string[] phrases)
    {
        string chosen = null;
        int numberSeen = 0;
        var rng = new Random();
        foreach (string line in phrases)
        {
            if (rng.Next(++numberSeen) == 0)
            {
                chosen = line;
            }
        }
        return chosen;
    }
}
